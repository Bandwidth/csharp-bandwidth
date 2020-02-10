using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Bandwidth.Net.ApiV2;
using LightMock;
using Xunit;

namespace Bandwidth.Net.Test.ApiV2
{
  public class MessageTests
  {
    public static bool IsValidSendRequest(HttpRequestMessage request)
    {
      return request.Method == HttpMethod.Post && request.RequestUri.PathAndQuery == "/api/v2/users/userId/messages" &&
             request.Content.Headers.ContentType.MediaType == "application/json" &&
             request.Content.ReadAsStringAsync().Result ==
             "{\"from\":\"+12345678901\",\"to\":[\"+12345678902\"],\"text\":\"Hey, check this out!\",\"applicationId\":\"id\"}";
    }

    public static bool IsValidCreateApplicationRequest(HttpRequestMessage request)
    {
      return request.RequestUri.AbsoluteUri == "https://dashboard.bandwidth.com/v1.0/api/accounts/AccountId/applications"
             && request.Method == HttpMethod.Post
             && request.Headers.Authorization.Parameter == "VXNlck5hbWU6UGFzc3dvcmQ="
             && request.Content.ReadAsStringAsync().Result.NormilizeLineEnds() ==
             "<Application>\r\n  <AppName>App1</AppName>\r\n  <CallbackUrl>url</CallbackUrl>\r\n  <CallBackCreds />\r\n</Application>".NormilizeLineEnds();
    }

    public static bool IsValidCreateLocationRequest(HttpRequestMessage request)
    {
      return request.RequestUri.AbsoluteUri == "https://dashboard.bandwidth.com/v1.0/api/accounts/AccountId/sites/SubaccountId/sippeers"
             && request.Method == HttpMethod.Post
             && request.Headers.Authorization.Parameter == "VXNlck5hbWU6UGFzc3dvcmQ="
             && request.Content.ReadAsStringAsync().Result.NormilizeLineEnds() ==
             "<SipPeer>\r\n  <PeerName>Location1</PeerName>\r\n  <IsDefaultPeer>false</IsDefaultPeer>\r\n</SipPeer>".NormilizeLineEnds();
    }

    public static bool IsEnableSms(HttpRequestMessage request)
    {
      return request.RequestUri.AbsoluteUri == "https://dashboard.bandwidth.com/v1.0/api/accounts/AccountId/sites/SubaccountId/sippeers/LocationId/products/messaging/features/sms"
             && request.Method == HttpMethod.Post
             && request.Headers.Authorization.Parameter == "VXNlck5hbWU6UGFzc3dvcmQ="
             && request.Content.ReadAsStringAsync().Result.NormilizeLineEnds() ==
             "<SipPeerSmsFeature>\r\n  <SipPeerSmsFeatureSettings>\r\n    <TollFree>true</TollFree>\r\n    <ShortCode>false</ShortCode>\r\n    <Protocol>HTTP</Protocol>\r\n    <Zone1>true</Zone1>\r\n    <Zone2>false</Zone2>\r\n    <Zone3>false</Zone3>\r\n    <Zone4>false</Zone4>\r\n    <Zone5>false</Zone5>\r\n  </SipPeerSmsFeatureSettings>\r\n  <HttpSettings>\r\n    <ProxyPeerId>539692</ProxyPeerId>\r\n  </HttpSettings>\r\n</SipPeerSmsFeature>".NormilizeLineEnds();
    }

    public static bool IsEnableMms(HttpRequestMessage request)
    {
      return request.RequestUri.AbsoluteUri == "https://dashboard.bandwidth.com/v1.0/api/accounts/AccountId/sites/SubaccountId/sippeers/LocationId/products/messaging/features/mms"
             && request.Method == HttpMethod.Post
             && request.Headers.Authorization.Parameter == "VXNlck5hbWU6UGFzc3dvcmQ="
             && request.Content.ReadAsStringAsync().Result.NormilizeLineEnds() ==
             "<MmsFeature>\r\n  <MmsSettings>\r\n    <protocol>HTTP</protocol>\r\n  </MmsSettings>\r\n  <Protocols>\r\n    <HTTP>\r\n      <HttpSettings>\r\n        <ProxyPeerId>539692</ProxyPeerId>\r\n      </HttpSettings>\r\n    </HTTP>\r\n  </Protocols>\r\n</MmsFeature>".NormilizeLineEnds();
    }

    public static bool IsAssignApplicationToLocationRequest(HttpRequestMessage request)
    {
      return request.RequestUri.AbsoluteUri == "https://dashboard.bandwidth.com/v1.0/api/accounts/AccountId/sites/SubaccountId/sippeers/LocationId/products/messaging/applicationSettings"
             && request.Method == HttpMethod.Put
             && request.Headers.Authorization.Parameter == "VXNlck5hbWU6UGFzc3dvcmQ="
             && request.Content.ReadAsStringAsync().Result.NormilizeLineEnds() ==
             "<ApplicationsSettings>\r\n  <HttpMessagingV2AppId>ApplicationId</HttpMessagingV2AppId>\r\n</ApplicationsSettings>".NormilizeLineEnds();

    }

    public static bool IsValidSearchAndOrderNumbersRequest(HttpRequestMessage request)
    {
      return request.RequestUri.AbsoluteUri == "https://dashboard.bandwidth.com/v1.0/api/accounts/AccountId/orders"
             && request.Method == HttpMethod.Post
             && request.Headers.Authorization.Parameter == "VXNlck5hbWU6UGFzc3dvcmQ="
             && request.Content.ReadAsStringAsync().Result.NormilizeLineEnds() ==
             "<Order>\r\n  <AreaCodeSearchAndOrderType>\r\n    <AreaCode>910</AreaCode>\r\n    <Quantity>2</Quantity>\r\n  </AreaCodeSearchAndOrderType>\r\n  <SiteId>SubaccountId</SiteId>\r\n  <PeerId>LocationId</PeerId>\r\n</Order>".NormilizeLineEnds();

    }

    public static bool IsValidGetOrderRequest(HttpRequestMessage request)
    {
      return request.RequestUri.AbsoluteUri == "https://dashboard.bandwidth.com/v1.0/api/accounts/AccountId/orders/OrderId"
             && request.Method == HttpMethod.Get
             && request.Headers.Authorization.Parameter == "VXNlck5hbWU6UGFzc3dvcmQ=";

    }

    [Fact]
    public async void TestCreateMessagingApplicationAsync()
    {
      var authData = new IrisAuthData
      {
        AccountId = "AccountId",
        UserName = "UserName",
        Password = "Password",
        SubaccountId = "SubaccountId"
      };
      var context = new MockContext<IHttp>();
      var api = Helpers.GetClient(context).V2.Message;

      context.Arrange(m => m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidCreateApplicationRequest(r)),
        HttpCompletionOption.ResponseContentRead,
        null)).Returns(Task.FromResult(new HttpResponseMessage
      {
        Content = Helpers.GetXmlContent("CreateMessagingApplicationResponse")
      }));
      var response = new HttpResponseMessage();
      response.Headers.Location = new Uri("http://localhost/LocationId");
      context.Arrange(m => m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidCreateLocationRequest(r)),
        HttpCompletionOption.ResponseContentRead,
        null)).Returns(Task.FromResult(response));
      context.Arrange(m => m.SendAsync(The<HttpRequestMessage>.Is(r => IsEnableSms(r)),
        HttpCompletionOption.ResponseContentRead,
        null)).Returns(Task.FromResult(new HttpResponseMessage()));
      context.Arrange(m => m.SendAsync(The<HttpRequestMessage>.Is(r => IsEnableMms(r)),
        HttpCompletionOption.ResponseContentRead,
        null)).Returns(Task.FromResult(new HttpResponseMessage()));
      context.Arrange(m => m.SendAsync(The<HttpRequestMessage>.Is(r => IsAssignApplicationToLocationRequest(r)),
        HttpCompletionOption.ResponseContentRead,
        null)).Returns(Task.FromResult(new HttpResponseMessage()));

      var application = await api.CreateMessagingApplicationAsync(authData, new CreateMessagingApplicationData
      {
        Name = "App1",
        CallbackUrl = "url",
        LocationName = "Location1",
        SmsOptions = new SmsOptions
        {
          TollFreeEnabled = true
        },
        MmsOptions = new MmsOptions
        {
          Enabled = true
        }
      });
      Assert.Equal("ApplicationId", application.ApplicationId);
      Assert.Equal("LocationId", application.LocationId);
    }

    [Fact]
    public async void TestCreateMessagingApplicationWithoutSmsAndMmsAsync()
    {
      var authData = new IrisAuthData
      {
        AccountId = "AccountId",
        UserName = "UserName",
        Password = "Password",
        SubaccountId = "SubaccountId"
      };
      var context = new MockContext<IHttp>();
      var api = Helpers.GetClient(context).V2.Message;

      context.Arrange(m => m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidCreateApplicationRequest(r)),
        HttpCompletionOption.ResponseContentRead,
        null)).Returns(Task.FromResult(new HttpResponseMessage
        {
          Content = Helpers.GetXmlContent("CreateMessagingApplicationResponse")
        }));
      var response = new HttpResponseMessage();
      response.Headers.Location = new Uri("http://localhost/LocationId");
      context.Arrange(m => m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidCreateLocationRequest(r)),
        HttpCompletionOption.ResponseContentRead,
        null)).Returns(Task.FromResult(response));
      context.Arrange(m => m.SendAsync(The<HttpRequestMessage>.Is(r => IsAssignApplicationToLocationRequest(r)),
        HttpCompletionOption.ResponseContentRead,
        null)).Returns(Task.FromResult(new HttpResponseMessage()));

      var application = await api.CreateMessagingApplicationAsync(authData, new CreateMessagingApplicationData
      {
        Name = "App1",
        CallbackUrl = "url",
        LocationName = "Location1"
      });
      Assert.Equal("ApplicationId", application.ApplicationId);
      Assert.Equal("LocationId", application.LocationId);
    }

    [Fact]
    public async void TestCheckResponse()
    {
      var authData = new IrisAuthData
      {
        AccountId = "AccountId",
        UserName = "UserName",
        Password = "Password",
        SubaccountId = "SubaccountId"
      };
      var context = new MockContext<IHttp>();
      var api = Helpers.GetClient(context).V2.Message;

      context.Arrange(m => m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidCreateApplicationRequest(r)),
        HttpCompletionOption.ResponseContentRead,
        null)).Returns(Task.FromResult(new HttpResponseMessage
        {
          Content = new StringContent("<Response><ErrorCode>Code</ErrorCode><Description>Description</Description></Response>")
        }));

      var err = await Assert.ThrowsAsync<BandwidthIrisException>(() => api.CreateMessagingApplicationAsync(authData, new CreateMessagingApplicationData
      {
        Name = "App1",
        CallbackUrl = "url",
        LocationName = "Location1"
      }));
      Assert.Equal("Code", err.Code);
      Assert.Equal("Description", err.Message);
    }

    [Fact]
    public async void TestCheckResponse2()
    {
      var authData = new IrisAuthData
      {
        AccountId = "AccountId",
        UserName = "UserName",
        Password = "Password",
        SubaccountId = "SubaccountId"
      };
      var context = new MockContext<IHttp>();
      var api = Helpers.GetClient(context).V2.Message;

      context.Arrange(m => m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidCreateApplicationRequest(r)),
        HttpCompletionOption.ResponseContentRead,
        null)).Returns(Task.FromResult(new HttpResponseMessage
      {
        Content = new StringContent("<Response><Error><Code>Code</Code><Description>Description</Description></Error></Response>")
      }));

      var err = await Assert.ThrowsAsync<BandwidthIrisException>(() => api.CreateMessagingApplicationAsync(authData, new CreateMessagingApplicationData
      {
        Name = "App1",
        CallbackUrl = "url",
        LocationName = "Location1"
      }));
      Assert.Equal("Code", err.Code);
      Assert.Equal("Description", err.Message);
    }

    [Fact]
    public async void TestCheckResponse3()
    {
      var authData = new IrisAuthData
      {
        AccountId = "AccountId",
        UserName = "UserName",
        Password = "Password",
        SubaccountId = "SubaccountId"
      };
      var context = new MockContext<IHttp>();
      var api = Helpers.GetClient(context).V2.Message;

      context.Arrange(m => m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidCreateApplicationRequest(r)),
        HttpCompletionOption.ResponseContentRead,
        null)).Returns(Task.FromResult(new HttpResponseMessage
      {
        Content = new StringContent("<Response><Errors><Code>Code</Code><Description>Description</Description></Errors></Response>")
      }));

      var err = (BandwidthIrisException)(await Assert.ThrowsAsync<AggregateException>(() => api.CreateMessagingApplicationAsync(authData, new CreateMessagingApplicationData
      {
        Name = "App1",
        CallbackUrl = "url",
        LocationName = "Location1"
      }))).InnerExceptions.First();
      Assert.Equal("Code", err.Code);
      Assert.Equal("Description", err.Message);
    }

    [Fact]
    public async void TestCheckResponse4()
    {
      var authData = new IrisAuthData
      {
        AccountId = "AccountId",
        UserName = "UserName",
        Password = "Password",
        SubaccountId = "SubaccountId"
      };
      var context = new MockContext<IHttp>();
      var api = Helpers.GetClient(context).V2.Message;

      context.Arrange(m => m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidCreateApplicationRequest(r)),
        HttpCompletionOption.ResponseContentRead,
        null)).Returns(Task.FromResult(new HttpResponseMessage
      {
        Content = new StringContent("<Response><resultCode>Code</resultCode><resultMessage>Description</resultMessage></Response>")
      }));

      var err = await Assert.ThrowsAsync<BandwidthIrisException>(() => api.CreateMessagingApplicationAsync(authData, new CreateMessagingApplicationData
      {
        Name = "App1",
        CallbackUrl = "url",
        LocationName = "Location1"
      }));
      Assert.Equal("Code", err.Code);
      Assert.Equal("Description", err.Message);
    }

    [Fact]
    public async void TestCheckResponse5()
    {
      var authData = new IrisAuthData
      {
        AccountId = "AccountId",
        UserName = "UserName",
        Password = "Password",
        SubaccountId = "SubaccountId"
      };
      var context = new MockContext<IHttp>();
      var api = Helpers.GetClient(context).V2.Message;

      context.Arrange(m => m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidCreateApplicationRequest(r)),
        HttpCompletionOption.ResponseContentRead,
        null)).Returns(Task.FromResult(new HttpResponseMessage
      {
        StatusCode = HttpStatusCode.NotFound
      }));

      await Assert.ThrowsAsync<BandwidthIrisException>(() => api.CreateMessagingApplicationAsync(authData, new CreateMessagingApplicationData
      {
        Name = "App1",
        CallbackUrl = "url",
        LocationName = "Location1"
      }));
    }


    [Fact]
    public async void TestSearchAndOrderNumbersAsync()
    {
      var authData = new IrisAuthData
      {
        AccountId = "AccountId",
        UserName = "UserName",
        Password = "Password",
        SubaccountId = "SubaccountId"
      };
      var context = new MockContext<IHttp>();
      var api = Helpers.GetClient(context).V2.Message;

      context.Arrange(m => m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidSearchAndOrderNumbersRequest(r)),
        HttpCompletionOption.ResponseContentRead,
        null)).Returns(Task.FromResult(new HttpResponseMessage
      {
        Content = Helpers.GetXmlContent("OrderNumbersResponse")
      }));
      context.Arrange(m => m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidGetOrderRequest(r)),
        HttpCompletionOption.ResponseContentRead,
        null)).Returns(Task.FromResult(new HttpResponseMessage
      {
        Content = Helpers.GetXmlContent("OrderResponse")
      }));

      var numbers = await api.SearchAndOrderNumbersAsync(authData, new MessagingApplication
      {
        ApplicationId = "ApplicationId",
        LocationId = "LocationId"
      }, new AreaCodeSearchAndOrderNumbersQuery
      {
        AreaCode = "910",
        Quantity = 2
      });
      Assert.Equal(2, numbers.Length);
    }

    [Fact]
    public async void TestSearchAndOrderNumbersAsyncFail()
    {
      var authData = new IrisAuthData
      {
        AccountId = "AccountId",
        UserName = "UserName",
        Password = "Password",
        SubaccountId = "SubaccountId"
      };
      var context = new MockContext<IHttp>();
      var api = Helpers.GetClient(context).V2.Message;

      context.Arrange(m => m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidSearchAndOrderNumbersRequest(r)),
        HttpCompletionOption.ResponseContentRead,
        null)).Returns(Task.FromResult(new HttpResponseMessage
      {
        Content = Helpers.GetXmlContent("OrderNumbersResponse")
      }));
      context.Arrange(m => m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidGetOrderRequest(r)),
        HttpCompletionOption.ResponseContentRead,
        null)).Returns(Task.FromResult(new HttpResponseMessage
      {
        Content = Helpers.GetXmlContent("FailedOrderResponse")
      }));

      await Assert.ThrowsAsync<BandwidthException>(() => api.SearchAndOrderNumbersAsync(authData, new MessagingApplication
      {
        ApplicationId = "ApplicationId",
        LocationId = "LocationId"
      }, new AreaCodeSearchAndOrderNumbersQuery
      {
        AreaCode = "910",
        Quantity = 2
      }));
    }

    [Fact]
    public async void TestSearchAndOrderNumbersAsyncTimeout()
    {
      var authData = new IrisAuthData
      {
        AccountId = "AccountId",
        UserName = "UserName",
        Password = "Password",
        SubaccountId = "SubaccountId"
      };
      var context = new MockContext<IHttp>();
      var api = Helpers.GetClient(context).V2.Message;

      context.Arrange(m => m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidSearchAndOrderNumbersRequest(r)),
        HttpCompletionOption.ResponseContentRead,
        null)).Returns(Task.FromResult(new HttpResponseMessage
      {
        Content = Helpers.GetXmlContent("OrderNumbersResponse")
      }));
      context.Arrange(m => m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidGetOrderRequest(r)),
        HttpCompletionOption.ResponseContentRead,
        null)).Returns(Task.FromResult(new HttpResponseMessage
      {
        Content = Helpers.GetXmlContent("OrderNumbersResponse")
      }));

      await Assert.ThrowsAsync<TimeoutException>(() => api.SearchAndOrderNumbersAsync(authData, new MessagingApplication
      {
        ApplicationId = "ApplicationId",
        LocationId = "LocationId"
      }, new AreaCodeSearchAndOrderNumbersQuery
      {
        AreaCode = "910",
        Quantity = 2,
        Timeout = TimeSpan.FromMilliseconds(1)
      }));
    }


    [Fact]
    public async void TestSend()
    {
      var response = new HttpResponseMessage(HttpStatusCode.Accepted);
      response.Content = new JsonContent(Helpers.GetJsonResourse("SendMessageResponse2"));
      var context = new MockContext<IHttp>();
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidSendRequest(r)), HttpCompletionOption.ResponseContentRead,
            null)).Returns(Task.FromResult(response));
      var api = Helpers.GetClient(context).V2.Message;
      var message = await api.SendAsync(new MessageData
      {
        From = "+12345678901",
        To = new[] {"+12345678902"},
        Text = "Hey, check this out!",
        ApplicationId = "id"
      });
      Assert.Equal("14762070468292kw2fuqty55yp2b2", message.Id);
      Assert.Equal(MessageDirection.Out, message.Direction);
    }

    [Fact]
    public void TestAreaCodeSearchAndOrderNumbersQuery()
    {
      var query = new AreaCodeSearchAndOrderNumbersQuery {AreaCode = "910", Quantity = 1};
      Assert.Equal("<AreaCodeSearchAndOrderType>\r\n  <AreaCode>910</AreaCode>\r\n  <Quantity>1</Quantity>\r\n</AreaCodeSearchAndOrderType>".NormilizeLineEnds(), query.ToXElement().ToString().NormilizeLineEnds());
    }

    [Fact]
    public void TestRateCenterSearchAndOrderNumbersQuery()
    {
      var query = new RateCenterSearchAndOrderNumbersQuery{ RateCenter = "NC", Quantity = 1 };
      Assert.Equal("<RateCenterSearchAndOrderType>\r\n  <RateCenter>NC</RateCenter>\r\n  <State />\r\n  <Quantity>1</Quantity>\r\n</RateCenterSearchAndOrderType>".NormilizeLineEnds(), query.ToXElement().ToString().NormilizeLineEnds());
    }

    [Fact]
    public void TestNpaNxxSearchAndOrderNumbersQuery()
    {
      var query = new NpaNxxSearchAndOrderNumbersQuery { NpaNxx = "911", Quantity = 1 };
      Assert.Equal("<NPANXXSearchAndOrderType>\r\n  <NpaNxx>911</NpaNxx>\r\n  <EnableTNDetail>false</EnableTNDetail>\r\n  <EnableLCA>false</EnableLCA>\r\n  <Quantity>1</Quantity>\r\n</NPANXXSearchAndOrderType>".NormilizeLineEnds(), query.ToXElement().ToString().NormilizeLineEnds());
    }

    [Fact]
    public void TestTollFreeVanitySearchAndOrderNumbersQuery()
    {
      var query = new TollFreeVanitySearchAndOrderNumbersQuery { TollFreeVanity = "0", Quantity = 1 };
      Assert.Equal("<TollFreeVanitySearchAndOrderType>\r\n  <TollFreeVanity>0</TollFreeVanity>\r\n  <Quantity>1</Quantity>\r\n</TollFreeVanitySearchAndOrderType>".NormilizeLineEnds(), query.ToXElement().ToString().NormilizeLineEnds());
    }

    [Fact]
    public void TestTollFreeWildCharSearchAndOrderNumbersQuery()
    {
      var query = new TollFreeWildCharSearchAndOrderNumbersQuery { TollFreeWildCardPattern = "*", Quantity = 1 };
      Assert.Equal("<TollFreeWildCharSearchAndOrderType>\r\n  <TollFreeWildCardPattern>*</TollFreeWildCardPattern>\r\n  <Quantity>1</Quantity>\r\n</TollFreeWildCharSearchAndOrderType>".NormilizeLineEnds(), query.ToXElement().ToString().NormilizeLineEnds());
    }

    [Fact]
    public void TestStateSearchAndOrderNumbersQuery()
    {
      var query = new StateSearchAndOrderNumbersQuery { State = "NC", Quantity = 1 };
      Assert.Equal("<StateSearchAndOrderType>\r\n  <State>NC</State>\r\n  <Quantity>1</Quantity>\r\n</StateSearchAndOrderType>".NormilizeLineEnds(), query.ToXElement().ToString().NormilizeLineEnds());
    }

    [Fact]
    public void TestCitySearchAndOrderNumbersQuery()
    {
      var query = new CitySearchAndOrderNumbersQuery { State = "NC", City = "Cary", Quantity = 1 };
      Assert.Equal("<CitySearchAndOrderType>\r\n  <State>NC</State>\r\n  <City>Cary</City>\r\n  <Quantity>1</Quantity>\r\n</CitySearchAndOrderType>".NormilizeLineEnds(), query.ToXElement().ToString().NormilizeLineEnds());
    }

    [Fact]
    public void TestZipSearchAndOrderNumbersQuery()
    {
      var query = new ZipSearchAndOrderNumbersQuery { Zip = "000", Quantity = 1 };
      Assert.Equal("<ZIPSearchAndOrderType>\r\n  <Zip>000</Zip>\r\n  <Quantity>1</Quantity>\r\n</ZIPSearchAndOrderType>".NormilizeLineEnds(), query.ToXElement().ToString().NormilizeLineEnds());
    }

    [Fact]
    public void TestLataSearchAndOrderNumbersQuery()
    {
      var query = new LataSearchAndOrderNumbersQuery { Lata = "000"};
      Assert.Equal("<LATASearchAndOrderType>\r\n  <Lata>000</Lata>\r\n  <Quantity>10</Quantity>\r\n</LATASearchAndOrderType>".NormilizeLineEnds(), query.ToXElement().ToString().NormilizeLineEnds());
    }

    [Fact]
    public void TestCombinedSearchAndOrderNumbersQuery()
    {
      var query = new CombinedSearchAndOrderNumbersQuery { AreaCode = "900" };
      Assert.Equal("<CombinedSearchAndOrderType>\r\n  <Quantity>10</Quantity>\r\n  <AreaCode>900</AreaCode>\r\n</CombinedSearchAndOrderType>".NormilizeLineEnds(), query.ToXElement().ToString().NormilizeLineEnds());
    }
  }

  internal static class StringExtensions
  {
    public static string NormilizeLineEnds(this string text)
    {
      return text.Replace("\r\n", "\n");
    }
  }
}
