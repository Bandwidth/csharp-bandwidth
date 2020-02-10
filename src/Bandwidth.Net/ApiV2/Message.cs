using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Bandwidth.Net.ApiV2
{
  /// <summary>
  ///   Access to Message Api
  /// </summary>
  public interface IMessage
  {
    /// <summary>
    ///   Create messaging application on Bandwidth Dashboard (its id is required to send messages)
    /// </summary>
    /// <param name="authData">Bandwidth Dashboard auth data</param>
    /// <param name="data">Options of new application</param>
    /// <param name="cancellationToken">Optional token to cancel async operation</param>
    /// <returns>Created application data</returns>
    /// <example>
    ///   <code>
    /// var application = await client.V2.Message.CreateMessagingApplicationAsync(authData, new CreateMessagingApplicationData {
    ///   Name = "My Messaging App",
    ///   CallbackUrl = "http://your/callback/handler",
    ///   PearName = "Current",
    ///   SmsOptions = new SmsOptions {TollFreeEnabled = true},
    ///   MmsOptions = new MmsOptions {Enabled = true}
    /// });
    /// </code>
    /// </example>
    Task<MessagingApplication> CreateMessagingApplicationAsync(IrisAuthData authData,
      CreateMessagingApplicationData data, CancellationToken? cancellationToken = null);

    /// <summary>
    ///   Look for and reserve phone numbers on Bandwidth Dashboard
    /// </summary>
    /// <param name="authData">Bandwidth Dashboard auth data</param>
    /// <param name="application">Messaging application data (result of CreateMessagingApplicationAsync())</param>
    /// <param name="query">Search query</param>
    /// <param name="cancellationToken">Optional token to cancel async operation</param>
    /// <returns>Array of reserved phone numbers</returns>
    /// <example>
    ///   <code>
    /// var numbers = await client.V2.Message.SearchAndOrderNumbersAsync(authData, application, new AreaCodeSearchAndOrderNumbersQuery {AreaCode = "910"});
    /// </code>
    /// </example>
    Task<string[]> SearchAndOrderNumbersAsync(IrisAuthData authData, MessagingApplication application,
      SearchAndOrderNumbersQuery query, CancellationToken? cancellationToken = null);

    /// <summary>
    ///   Send a message.
    /// </summary>
    /// <param name="data">Parameters of new message</param>
    /// <param name="cancellationToken">Optional token to cancel async operation</param>
    /// <returns>Created message</returns>
    /// <example>
    ///   <code>
    /// var message = await client.V2.Message.SendAsync(new MessageData{ From = "from", To = new[] {"to"}, Text = "Hello", ApplicationId = "appId"});
    /// </code>
    /// </example>
    Task<Message> SendAsync(MessageData data, CancellationToken? cancellationToken = null);
  }

  internal class MessageApi : ApiBase, IMessage
  {
    public async Task<MessagingApplication> CreateMessagingApplicationAsync(IrisAuthData authData,
      CreateMessagingApplicationData data, CancellationToken? cancellationToken = null)
    {
      var app = new MessagingApplication
      {
        ApplicationId = await CreateApplication(authData, data, cancellationToken),
        LocationId = await CreateLocation(authData, data, cancellationToken)
      };
      await EnableSms(authData, data.SmsOptions, app, cancellationToken);
      await EnableMms(authData, data.MmsOptions, app, cancellationToken);
      await AssignApplicationToLocation(authData, app, cancellationToken);
      return app;
    }

    public async Task<string[]> SearchAndOrderNumbersAsync(IrisAuthData authData, MessagingApplication application,
      SearchAndOrderNumbersQuery query, CancellationToken? cancellationToken = null)
    {
      var xml = new XDocument(
        new XElement("Order", query.ToXElement(),
          new XElement("SiteId", authData.SubaccountId),
          new XElement("PeerId", application.LocationId))
      );
      var (doc, _) = await MakeRequestAsync(authData, HttpMethod.Post, "/orders", xml, true, cancellationToken);
      var orderId = doc.Descendants("Order").First().Element("id").Value;
      var successStatuses = new[] {"COMPLETE", "PARTIAL"};
      var stopwatch = Stopwatch.StartNew();
      try
      {
        while (true)
        {
          await Task.Delay(500, cancellationToken ?? default(CancellationToken));
          if (stopwatch.Elapsed >= query.Timeout)
            throw new TimeoutException();
          var (result, _) = await MakeRequestAsync(authData, HttpMethod.Get, $"/orders/{orderId}", null, true,
            cancellationToken);
          var status = (result.Descendants("OrderStatus").FirstOrDefault() ?? new XElement("OrderStatus")).Value;
          if (successStatuses.Contains(status))
            return (result.Descendants("CompletedNumbers").FirstOrDefault() ?? new XElement("CompletedNumbers"))
              .Descendants("FullNumber").Select(n => n.Value).ToArray();
          if (status == "FAILED")
            throw new BandwidthException("Error on reserving phone numbers", HttpStatusCode.BadRequest);
        }
      }
      finally
      {
        stopwatch.Stop();
      }
    }

    public Task<Message> SendAsync(MessageData data,
      CancellationToken? cancellationToken = null)
    {
      return Client.MakeJsonRequestAsync<Message>(HttpMethod.Post, $"/users/{Client.UserId}/messages",
        cancellationToken, null, data, "v2");
    }

    private HttpRequestMessage CreateIrisRequest(IrisAuthData authData, HttpMethod method, string path,
      object query = null)
    {
      var url = new UriBuilder("https://dashboard.bandwidth.com")
      {
        Path = $"/v1.0/api/accounts/{authData.AccountId}{path}",
        Query = Client.BuildQueryString(query)
      };
      var message = new HttpRequestMessage(method, url.Uri);
      message.Headers.UserAgent.Add(Client.UserAgent);
      message.Headers.Authorization = new AuthenticationHeaderValue("Basic",
        Convert.ToBase64String(Encoding.UTF8.GetBytes($"{authData.UserName}:{authData.Password}")));
      return message;
    }

    private async Task<XDocument> CheckResponse(HttpResponseMessage response)
    {
      try
      {
        var xml = await response.Content.ReadAsStringAsync();
        if (xml.Length > 0)
        {
          var doc = XDocument.Parse(xml);
          var code = doc.Descendants("ErrorCode").FirstOrDefault();
          var description = doc.Descendants("Description").FirstOrDefault();
          if (code == null)
          {
            var error = doc.Descendants("Error").FirstOrDefault();
            if (error == null)
            {
              var exceptions =
              (from item in doc.Descendants("Errors")
                select
                  (Exception) new BandwidthIrisException(item.Element("Code").Value,
                    item.Element("Description").Value, response.StatusCode)).ToArray();
              if (exceptions.Length > 0)
                throw new AggregateException(exceptions);
              code = doc.Descendants("resultCode").FirstOrDefault();
              description = doc.Descendants("resultMessage").FirstOrDefault();
            }
            else
            {
              code = error.Element("Code");
              description = error.Element("Description");
            }
          }
          if (code != null && description != null && !string.IsNullOrEmpty(code.Value) && code.Value != "0")
            throw new BandwidthIrisException(code.Value, description.Value, response.StatusCode);
          return doc;
        }
      }
      catch (Exception ex)
      {
        if (ex is BandwidthIrisException || ex is AggregateException) throw;
        Debug.WriteLine(ex.Message);
      }
      if (!response.IsSuccessStatusCode)
        throw new BandwidthIrisException("", string.Format("Http code {0}", response.StatusCode), response.StatusCode);
      return new XDocument();
    }

    internal async Task<(XDocument, HttpResponseMessage)> MakeRequestAsync(IrisAuthData authData, HttpMethod method,
      string path, XDocument doc, bool disposeResponse = false, CancellationToken? cancellationToken = null)
    {
      var request = CreateIrisRequest(authData, method, path);
      if (doc != null)
      {
        var xml = doc.ToString();
        request.Content = new StringContent(xml, Encoding.UTF8, "application/xml");
      }
      var response = await Client.Http.SendAsync(request, HttpCompletionOption.ResponseContentRead, cancellationToken);
      try
      {
        return (await CheckResponse(response), response);
      }
      finally
      {
        if (disposeResponse)
          response.Dispose();
      }
    }

    internal async Task<string> CreateApplication(IrisAuthData authData, CreateMessagingApplicationData data,
      CancellationToken? cancellationToken = null)
    {
      var xml = new XDocument(
        new XElement("Application",
          new XElement("AppName", data.Name),
          new XElement("CallbackUrl", data.CallbackUrl),
          new XElement("CallBackCreds",
            data.CallbackAuthData == null
              ? null
              : new[]
              {
                new XElement("UserId", data.CallbackAuthData.UserName),
                new XElement("Password", data.CallbackAuthData.Password)
              })
        )
      );
      var (doc, _) = await MakeRequestAsync(authData, HttpMethod.Post, "/applications", xml, true, cancellationToken);
      return doc.Descendants("ApplicationId").First().Value;
    }

    internal async Task<string> CreateLocation(IrisAuthData authData, CreateMessagingApplicationData data,
      CancellationToken? cancellationToken = null)
    {
      var xml = new XDocument(
        new XElement("SipPeer",
          new XElement("PeerName", data.LocationName),
          new XElement("IsDefaultPeer", data.IsDefaultLocation)
        )
      );
      var (_, response) = await MakeRequestAsync(authData, HttpMethod.Post, $"/sites/{authData.SubaccountId}/sippeers",
        xml, false, cancellationToken);
      using (response)
      {
        return (response.Headers.Location ?? new Uri("http://localhost")).AbsolutePath.Split('/').LastOrDefault();
      }
    }

    internal async Task EnableSms(IrisAuthData authData, SmsOptions options, MessagingApplication application,
      CancellationToken? cancellationToken = null)
    {
      if (options == null || !options.Enabled) return;
      var xml = new XDocument(
        new XElement("SipPeerSmsFeature",
          new XElement("SipPeerSmsFeatureSettings",
            new XElement("TollFree", options.TollFreeEnabled),
            new XElement("ShortCode", options.ShortCodeEnabled),
            new XElement("Protocol", "HTTP"),
            new XElement("Zone1", true),
            new XElement("Zone2", false),
            new XElement("Zone3", false),
            new XElement("Zone4", false),
            new XElement("Zone5", false)
          ),
          new XElement("HttpSettings",
            new XElement("ProxyPeerId", "539692")
          )
        )
      );
      await MakeRequestAsync(authData, HttpMethod.Post,
        $"/sites/{authData.SubaccountId}/sippeers/{application.LocationId}/products/messaging/features/sms", xml, true,
        cancellationToken);
    }

    internal async Task EnableMms(IrisAuthData authData, MmsOptions options, MessagingApplication application,
      CancellationToken? cancellationToken = null)
    {
      if (options == null || !options.Enabled) return;
      var xml = new XDocument(
        new XElement("MmsFeature",
          new XElement("MmsSettings",
            new XElement("protocol", "HTTP")
          ),
          new XElement("Protocols",
            new XElement("HTTP",
              new XElement("HttpSettings",
                new XElement("ProxyPeerId", "539692")
              )
            )
          )
        )
      );
      await MakeRequestAsync(authData, HttpMethod.Post,
        $"/sites/{authData.SubaccountId}/sippeers/{application.LocationId}/products/messaging/features/mms", xml, true,
        cancellationToken);
    }

    internal async Task AssignApplicationToLocation(IrisAuthData authData, MessagingApplication application,
      CancellationToken? cancellationToken = null)
    {
      var xml = new XDocument(
        new XElement("ApplicationsSettings",
          new XElement("HttpMessagingV2AppId", application.ApplicationId)
        )
      );
      await MakeRequestAsync(authData, HttpMethod.Put,
        $"/sites/{authData.SubaccountId}/sippeers/{application.LocationId}/products/messaging/applicationSettings", xml,
        true, cancellationToken);
    }
  }

  /// <summary>
  ///   Parameters to send an message
  /// </summary>
  public class MessageData
  {
    /// <summary>
    ///   The message sender's telephone number (or short code).
    /// </summary>
    public string From { get; set; }

    /// <summary>
    ///   Message recipient telephone number (or short code).
    /// </summary>
    public string[] To { get; set; }

    /// <summary>
    ///   The message contents.
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    ///   Array containing list of media urls to be sent as content for an mms.
    /// </summary>
    public string[] Media { get; set; }

    /// <summary>
    ///   The complete URL where the events related to the outgoing message will be sent.
    /// </summary>
    public string Tag { get; set; }

    /// <summary>
    ///   Messaging application id
    /// </summary>
    public string ApplicationId { get; set; }
  }

  /// <summary>
  ///   Result of batch send of some messages
  /// </summary>
  public class Message
  {
    /// <summary>
    ///   Id of the message
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    ///   The message sender's telephone number (or short code).
    /// </summary>
    public string From { get; set; }

    /// <summary>
    ///   Message recipient telephone number (or short code).
    /// </summary>
    public string[] To { get; set; }

    /// <summary>
    ///   The message contents.
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    ///   Array containing list of media urls to be sent as content for an mms.
    /// </summary>
    public string[] Media { get; set; }

    /// <summary>
    ///   The complete URL where the events related to the outgoing message will be sent.
    /// </summary>
    public string Tag { get; set; }

    /// <summary>
    ///   The message owner
    /// </summary>
    public string Owner { get; set; }

    /// <summary>
    ///   Direction of the message
    /// </summary>
    public MessageDirection Direction { get; set; }

    /// <summary>
    ///   The message creation time
    /// </summary>
    public DateTime Time { get; set; }
    
    /// <summary>
    /// Number of segments the message sent as
    /// </summary>
    public int SegmentCount { get; set; }
  }

  /// <summary>
  ///   Directions of message
  /// </summary>
  public enum MessageDirection
  {
    /// <summary>
    ///   A message that came from the telephone network to one of your numbers (an "inbound" message)
    /// </summary>
    In,

    /// <summary>
    ///   A message that was sent from one of your numbers to the telephone network (an "outbound" message)
    /// </summary>
    Out
  }

  /// <summary>
  ///   MessagingApplication
  /// </summary>
  public class MessagingApplication
  {
    /// <summary>
    ///   Messaging application id
    /// </summary>
    public string ApplicationId { get; set; }

    /// <summary>
    ///   Location (pear) id
    /// </summary>
    public string LocationId { get; set; }
  }

  /// <summary>
  ///   Data to create messagin application
  /// </summary>
  public class CreateMessagingApplicationData
  {
    /// <summary>
    ///   Application name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///   Callback url to receive messages events
    /// </summary>
    public string CallbackUrl { get; set; }

    /// <summary>
    ///   Optional auth data for http requests to CallbackUrl
    /// </summary>
    public CallbackAuthData CallbackAuthData { get; set; }

    /// <summary>
    ///   Location name
    /// </summary>
    /// <returns></returns>
    public string LocationName { get; set; }

    /// <summary>
    ///   Is created location default
    /// </summary>
    public bool IsDefaultLocation { get; set; }

    /// <summary>
    ///   Options for SMS
    /// </summary>
    public SmsOptions SmsOptions { get; set; }

    /// <summary>
    ///   Options for MMS
    /// </summary>
    public MmsOptions MmsOptions { get; set; }
  }

  /// <summary>
  ///   Auth data for Bandwidth Dashboard
  /// </summary>
  public class IrisAuthData
  {
    /// <summary>
    ///   Account id
    /// </summary>
    /// <returns></returns>
    public string AccountId { get; set; }

    /// <summary>
    ///   Subaccount (site) id
    /// </summary>
    public string SubaccountId { get; set; }

    /// <summary>
    ///   User name
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    ///   Password
    /// </summary>
    public string Password { get; set; }
  }

  /// <summary>
  ///   Callback auth data
  /// </summary>
  public class CallbackAuthData
  {
    /// <summary>
    ///   User name
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    ///   Password
    /// </summary>
    public string Password { get; set; }
  }

  /// <summary>
  ///   Options for SMS
  /// </summary>
  public class SmsOptions
  {
    /// <summary>
    ///   Enabled
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    ///   TollFreeEnabled
    /// </summary>
    public bool TollFreeEnabled { get; set; }

    /// <summary>
    ///   ShortCodeEnabled
    /// </summary>
    public bool ShortCodeEnabled { get; set; }
  }

  /// <summary>
  ///   Options for MMS
  /// </summary>
  public class MmsOptions
  {
    /// <summary>
    ///   Enabled
    /// </summary>
    public bool Enabled { get; set; } = true;
  }

  /// <summary>
  ///   Query to search phone numbers
  /// </summary>
  public abstract class SearchAndOrderNumbersQuery
  {
    /// <summary>
    ///   Quantity
    /// </summary>
    /// <returns></returns>
    public int Quantity { get; set; } = 10;

    /// <summary>
    ///   Request timeout
    /// </summary>
    public TimeSpan Timeout { get; set; } = TimeSpan.FromMinutes(1);

    internal abstract XElement ToXElement();
  }

  /// <summary>
  ///   Area code query to search phone numbers
  /// </summary>
  public class AreaCodeSearchAndOrderNumbersQuery : SearchAndOrderNumbersQuery
  {
    /// <summary>
    ///   Area code
    /// </summary>
    public string AreaCode { get; set; }

    internal override XElement ToXElement()
    {
      return new XElement("AreaCodeSearchAndOrderType",
        new XElement("AreaCode", AreaCode),
        new XElement("Quantity", Quantity)
      );
    }
  }

  /// <summary>
  ///   Rate center query to search phone numbers
  /// </summary>
  public class RateCenterSearchAndOrderNumbersQuery: SearchAndOrderNumbersQuery
  {
    /// <summary>
    ///   Rate center
    /// </summary>
    public string RateCenter { get; set; }

    /// <summary>
    ///   State
    /// </summary>
    public string State { get; set; }

    internal override XElement ToXElement()
    {
      return new XElement("RateCenterSearchAndOrderType",
        new XElement("RateCenter", RateCenter),
        new XElement("State", State),
        new XElement("Quantity", Quantity)
      );
    }
  }

  /// <summary>
  ///   Npa Nxx query to search phone numbers
  /// </summary>
  public class NpaNxxSearchAndOrderNumbersQuery : SearchAndOrderNumbersQuery
  {
    /// <summary>
    ///   NpaNxx
    /// </summary>
    public string NpaNxx { get; set; }

    /// <summary>
    ///   EnableTnDetail
    /// </summary>
    public bool EnableTnDetail { get; set; }

    /// <summary>
    ///   EnableLca
    /// </summary>
    public bool EnableLca { get; set; }

    internal override XElement ToXElement()
    {
      return new XElement("NPANXXSearchAndOrderType",
        new XElement("NpaNxx", NpaNxx),
        new XElement("EnableTNDetail", EnableTnDetail),
        new XElement("EnableLCA", EnableLca),
        new XElement("Quantity", Quantity)
      );
    }
  }

  /// <summary>
  ///   Toll free vanity query to search phone numbers
  /// </summary>
  public class TollFreeVanitySearchAndOrderNumbersQuery : SearchAndOrderNumbersQuery
  {
    /// <summary>
    ///   TollFreeVanity
    /// </summary>
    public string TollFreeVanity { get; set; }

    internal override XElement ToXElement()
    {
      return new XElement("TollFreeVanitySearchAndOrderType",
        new XElement("TollFreeVanity", TollFreeVanity),
        new XElement("Quantity", Quantity)
      );
    }
  }

  /// <summary>
  ///   Toll free wild сhar query to search phone numbers
  /// </summary>
  public class TollFreeWildCharSearchAndOrderNumbersQuery : SearchAndOrderNumbersQuery
  {
    /// <summary>
    ///   TollFreeWildCardPattern
    /// </summary>
    public string TollFreeWildCardPattern { get; set; }

    internal override XElement ToXElement()
    {
      return new XElement("TollFreeWildCharSearchAndOrderType",
        new XElement("TollFreeWildCardPattern", TollFreeWildCardPattern),
        new XElement("Quantity", Quantity)
      );
    }
  }

  /// <summary>
  ///   State query to search phone numbers
  /// </summary>
  public class StateSearchAndOrderNumbersQuery : SearchAndOrderNumbersQuery
  {
    /// <summary>
    ///   State
    /// </summary>
    public string State { get; set; }

    internal override XElement ToXElement()
    {
      return new XElement("StateSearchAndOrderType",
        new XElement("State", State),
        new XElement("Quantity", Quantity)
      );
    }
  }

  /// <summary>
  ///   City query to search phone numbers
  /// </summary>
  public class CitySearchAndOrderNumbersQuery : SearchAndOrderNumbersQuery
  {
    /// <summary>
    ///   State
    /// </summary>
    public string State { get; set; }

    /// <summary>
    ///   City
    /// </summary>
    public string City { get; set; }

    internal override XElement ToXElement()
    {
      return new XElement("CitySearchAndOrderType",
        new XElement("State", State),
        new XElement("City", City),
        new XElement("Quantity", Quantity)
      );
    }
  }

  /// <summary>
  ///   Zip query to search phone numbers
  /// </summary>
  public class ZipSearchAndOrderNumbersQuery : SearchAndOrderNumbersQuery
  {
    /// <summary>
    ///   Zip
    /// </summary>
    public string Zip { get; set; }

    internal override XElement ToXElement()
    {
      return new XElement("ZIPSearchAndOrderType",
        new XElement("Zip", Zip),
        new XElement("Quantity", Quantity)
      );
    }
  }

  /// <summary>
  ///   Lata query to search phone numbers
  /// </summary>
  public class LataSearchAndOrderNumbersQuery : SearchAndOrderNumbersQuery
  {
    /// <summary>
    ///   Lata
    /// </summary>
    public string Lata { get; set; }

    internal override XElement ToXElement()
    {
      return new XElement("LATASearchAndOrderType",
        new XElement("Lata", Lata),
        new XElement("Quantity", Quantity)
      );
    }
  }

  /// <summary>
  ///   Combined query to search phone numbers
  /// </summary>
  public class CombinedSearchAndOrderNumbersQuery : SearchAndOrderNumbersQuery
  {
    /// <summary>
    ///   AreaCode
    /// </summary>
    public string AreaCode { get; set; }

    /// <summary>
    ///   RateCenter
    /// </summary>
    public string RateCenter { get; set; }

    /// <summary>
    ///   NpaNxx
    /// </summary>
    public string NpaNxx { get; set; }

    /// <summary>
    ///   EnableTnDetail
    /// </summary>
    public bool? EnableTnDetail { get; set; }

    /// <summary>
    ///   EnableLca
    /// </summary>
    public bool? EnableLca { get; set; }

    /// <summary>
    ///   TollFreeVanity
    /// </summary>
    public string TollFreeVanity { get; set; }

    /// <summary>
    ///   TollFreeWildCardPattern
    /// </summary>
    public string TollFreeWildCardPattern { get; set; }

    /// <summary>
    ///   State
    /// </summary>
    public string State { get; set; }

    /// <summary>
    ///   City
    /// </summary>
    public string City { get; set; }

    /// <summary>
    ///   Zip
    /// </summary>
    public string Zip { get; set; }

    /// <summary>
    ///   Lata
    /// </summary>
    public string Lata { get; set; }

    internal override XElement ToXElement()
    {
      var elements = new List<XElement>(new[] {new XElement("Quantity", Quantity)});
      if (AreaCode != null) elements.Add(new XElement("AreaCode", AreaCode));
      if (RateCenter != null) elements.Add(new XElement("RateCenter", RateCenter));
      if (NpaNxx != null) elements.Add(new XElement("NpaNxx", NpaNxx));
      if (EnableTnDetail != null) elements.Add(new XElement("EnableTNDetail", EnableTnDetail.Value));
      if (EnableLca != null) elements.Add(new XElement("EnableLCA", EnableLca.Value));
      if (TollFreeVanity != null) elements.Add(new XElement("TollFreeVanity", TollFreeVanity));
      if (TollFreeWildCardPattern != null)
        elements.Add(new XElement("TollFreeWildCardPattern", TollFreeWildCardPattern));
      if (State != null) elements.Add(new XElement("State", State));
      if (City != null) elements.Add(new XElement("City", City));
      if (Zip != null) elements.Add(new XElement("Zip", Zip));
      if (Lata != null) elements.Add(new XElement("Lata", Lata));
      return new XElement("CombinedSearchAndOrderType", elements.ToArray());
    }
  }
}
