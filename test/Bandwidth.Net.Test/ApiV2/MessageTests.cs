using System;
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
      return request.Method == HttpMethod.Post && request.RequestUri.PathAndQuery == "/v2/users/userId/messages" &&
             request.Content.Headers.ContentType.MediaType == "application/json" &&
             request.Content.ReadAsStringAsync().Result ==
             "{\"from\":\"+12345678901\",\"to\":[\"+12345678902\"],\"text\":\"Hey, check this out!\",\"applicationId\":\"id\"}";
    }

    public static bool IsValidCreateApplicationRequest(HttpRequestMessage request)
    {
      return request.RequestUri.AbsoluteUri == "https://dashboard.bandwidth.com/api/accounts/AccountId/applications"
             && request.Method == HttpMethod.Post
             && request.Headers.Authorization.Parameter == "VXNlck5hbWU6UGFzc3dvcmQ="
             && request.Content.ReadAsStringAsync().Result ==
             "<Application>\r\n  <AppName>App1</AppName>\r\n  <CallbackUrl>url</CallbackUrl>\r\n  <CallBackCreds />\r\n</Application>";
    }

    public static bool IsValidCreateLocationRequest(HttpRequestMessage request)
    {
      return request.RequestUri.AbsoluteUri == "https://dashboard.bandwidth.com/api/accounts/AccountId/sites/SubaccountId/sippeers"
             && request.Method == HttpMethod.Post
             && request.Headers.Authorization.Parameter == "VXNlck5hbWU6UGFzc3dvcmQ="
             && request.Content.ReadAsStringAsync().Result ==
             "<SipPeer>\r\n  <PeerName>Location1</PeerName>\r\n  <IsDefaultPeer>false</IsDefaultPeer>\r\n</SipPeer>";
    }

    public static bool IsEnableSms(HttpRequestMessage request)
    {
      return request.RequestUri.AbsoluteUri == "https://dashboard.bandwidth.com/api/accounts/AccountId/sites/SubaccountId/sippeers/LocationId/products/messaging/features/sms"
             && request.Method == HttpMethod.Post
             && request.Headers.Authorization.Parameter == "VXNlck5hbWU6UGFzc3dvcmQ="
             && request.Content.ReadAsStringAsync().Result ==
             "<SipPeerSmsFeature>\r\n  <SipPeerSmsFeatureSettings>\r\n    <TollFree>true</TollFree>\r\n    <ShortCode>false</ShortCode>\r\n    <Protocol>HTTP</Protocol>\r\n    <Zone1>true</Zone1>\r\n    <Zone2>false</Zone2>\r\n    <Zone3>false</Zone3>\r\n    <Zone4>false</Zone4>\r\n    <Zone5>false</Zone5>\r\n  </SipPeerSmsFeatureSettings>\r\n  <HttpSettings>\r\n    <ProxyPeerId>539692</ProxyPeerId>\r\n  </HttpSettings>\r\n</SipPeerSmsFeature>";
    }

    public static bool IsEnableMms(HttpRequestMessage request)
    {
      return request.RequestUri.AbsoluteUri == "https://dashboard.bandwidth.com/api/accounts/AccountId/sites/SubaccountId/sippeers/LocationId/products/messaging/features/mms"
             && request.Method == HttpMethod.Post
             && request.Headers.Authorization.Parameter == "VXNlck5hbWU6UGFzc3dvcmQ="
             && request.Content.ReadAsStringAsync().Result ==
             "<MmsFeature>\r\n  <MmsSettings>\r\n    <protocol>HTTP</protocol>\r\n  </MmsSettings>\r\n  <Protocols>\r\n    <HTTP>\r\n      <HttpSettings>\r\n        <ProxyPeerId>539692</ProxyPeerId>\r\n      </HttpSettings>\r\n    </HTTP>\r\n  </Protocols>\r\n</MmsFeature>";
    }

    public static bool IsAssignApplicationToLocationRequest(HttpRequestMessage request)
    {
      return request.RequestUri.AbsoluteUri == "https://dashboard.bandwidth.com/api/accounts/AccountId/sites/SubaccountId/sippeers/LocationId/products/messaging/applicationSettings"
             && request.Method == HttpMethod.Put
             && request.Headers.Authorization.Parameter == "VXNlck5hbWU6UGFzc3dvcmQ="
             && request.Content.ReadAsStringAsync().Result ==
             "<ApplicationsSettings>\r\n  <HttpMessagingV2AppId>ApplicationId</HttpMessagingV2AppId>\r\n</ApplicationsSettings>";

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
  }
}
