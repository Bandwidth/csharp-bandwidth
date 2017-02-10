using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Bandwidth.Net
{
  internal static class XmlHelpers
  {
    public static void SetXmlContent(this HttpRequestMessage message, object data)
    {
      var serializer = new XmlSerializer(data.GetType());
      using (var writer = new Utf8StringWriter())
      {
        serializer.Serialize(writer, data);
        var xml = writer.ToString();
        message.Content = new StringContent(xml, Encoding.UTF8, "application/xml");
      }
    }

    public static async Task CheckXmlResponseAsync(this HttpResponseMessage response)
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
                    (Exception)
                      new BandwidthException((item.Element("Description") ?? new XElement("Description")).Value,
                        new Dictionary<string, string> {{"Code", (item.Element("Code") ?? new XElement("Code")).Value}},
                        response.StatusCode)).ToArray();
              if (exceptions.Length > 0)
              {
                throw new AggregateException(exceptions);
              }
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
          {
            throw new BandwidthException(description.Value, new Dictionary<string, string> {{"Code", code.Value}},
              response.StatusCode);
          }
        }
      }
      catch (Exception ex)
      {
        if (ex is BandwidthException || ex is AggregateException) throw;
        Debug.WriteLine(ex.Message);
      }
      if (!response.IsSuccessStatusCode)
      {
        throw new BandwidthException($"Http code {response.StatusCode}", response.StatusCode);
      }
    }

    public static async Task<TResult> ReadAsXmlAsync<TResult>(this HttpContent content)
    {
      if (content.Headers.ContentType.MediaType == "application/xml" ||
          content.Headers.ContentType.MediaType == "text/xml")
      {
        var serializer = new XmlSerializer(typeof (TResult));
        using (var stream = await content.ReadAsStreamAsync())
        {
          return (TResult) serializer.Deserialize(stream);
        }
      }
      return default(TResult);
    }
  }
}
