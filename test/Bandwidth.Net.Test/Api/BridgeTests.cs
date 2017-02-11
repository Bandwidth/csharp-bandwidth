using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Bandwidth.Net.Api;
using LightMock;
using Xunit;

namespace Bandwidth.Net.Test.Api
{
  public class BridgeTests
  {
    [Fact]
    public void TestList()
    {
      var response = new HttpResponseMessage
      {
        Content =
          new JsonContent($"[{Helpers.GetJsonResourse("Bridge")}]")
      };
      var context = new MockContext<IHttp>();
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidListRequest(r)), HttpCompletionOption.ResponseContentRead,
            null)).Returns(Task.FromResult(response));
      var api = Helpers.GetClient(context).Bridge;
      var bridges = api.List();
      ValidateBridge(bridges.First());
    }

    [Fact]
    public async void TestCreate()
    {
      var response = new HttpResponseMessage(HttpStatusCode.Created);
      response.Headers.Location = new Uri("http://localhost/path/id");
      var context = new MockContext<IHttp>();
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidCreateRequest(r)), HttpCompletionOption.ResponseContentRead,
            null)).Returns(Task.FromResult(response));
      var api = Helpers.GetClient(context).Bridge;
      var bridgeId = await api.CreateAsync(new CreateBridgeData {CallIds = new []{"callId"}});
      context.Assert(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidGetRequest(r)), HttpCompletionOption.ResponseContentRead,
            null), Invoked.Never);
      Assert.Equal("id", bridgeId);
    }

    [Fact]
    public async void TestGet()
    {
      var response = new HttpResponseMessage
      {
        Content = Helpers.GetJsonContent("Bridge")
      };
      var context = new MockContext<IHttp>();
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidGetRequest(r)), HttpCompletionOption.ResponseContentRead,
            null)).Returns(Task.FromResult(response));
      var api = Helpers.GetClient(context).Bridge;
      var bridge = await api.GetAsync("id");
      ValidateBridge(bridge);
    }

    [Fact]
    public async void TestUpdate()
    {
      var context = new MockContext<IHttp>();
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidUpdateRequest(r)), HttpCompletionOption.ResponseContentRead,
            null)).Returns(Task.FromResult(new HttpResponseMessage()));
      var api = Helpers.GetClient(context).Bridge;
      await api.UpdateAsync("id", new UpdateBridgeData {CallIds = new []{"callId"}});
    }

    [Fact]
    public async void TestPlayAudio()
    {
      var context = new MockContext<IHttp>();
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidPlayAudioRequest(r)), HttpCompletionOption.ResponseContentRead,
            null)).Returns(Task.FromResult(new HttpResponseMessage()));
      var api = Helpers.GetClient(context).Bridge;
      await api.PlayAudioAsync("id", new PlayAudioData {FileUrl = "url"});
    }

    [Fact]
    public void TestGetCalls()
    {
      var context = new MockContext<IHttp>();
      var response = new HttpResponseMessage
      {
        Content = new JsonContent($"[{Helpers.GetJsonResourse("Call")}]")
      };
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidGetCallsRequest(r)), HttpCompletionOption.ResponseContentRead,
            null)).Returns(Task.FromResult(response));
      var api = Helpers.GetClient(context).Bridge;
      var calls = api.GetCalls("id");
      Assert.Equal("callId", calls.First().Id);
    }


    public static bool IsValidListRequest(HttpRequestMessage request)
    {
      return request.Method == HttpMethod.Get && request.RequestUri.PathAndQuery == "/v1/users/userId/bridges";
    }

    public static bool IsValidCreateRequest(HttpRequestMessage request)
    {
      return request.Method == HttpMethod.Post && request.RequestUri.PathAndQuery == "/v1/users/userId/bridges" &&
             request.Content.Headers.ContentType.MediaType == "application/json" &&
             request.Content.ReadAsStringAsync().Result == "{\"callIds\":[\"callId\"]}";
    }

    public static bool IsValidGetRequest(HttpRequestMessage request)
    {
      return request.Method == HttpMethod.Get && request.RequestUri.PathAndQuery == "/v1/users/userId/bridges/id";
    }

    public static bool IsValidUpdateRequest(HttpRequestMessage request)
    {
      return request.Method == HttpMethod.Post && request.RequestUri.PathAndQuery == "/v1/users/userId/bridges/id" &&
             request.Content.Headers.ContentType.MediaType == "application/json" &&
             request.Content.ReadAsStringAsync().Result == "{\"callIds\":[\"callId\"]}";
    }

    public static bool IsValidPlayAudioRequest(HttpRequestMessage request)
    {
      return request.Method == HttpMethod.Post && request.RequestUri.PathAndQuery == "/v1/users/userId/bridges/id/audio" &&
             request.Content.Headers.ContentType.MediaType == "application/json" &&
             request.Content.ReadAsStringAsync().Result == "{\"fileUrl\":\"url\"}";
    }

    public static bool IsValidGetCallsRequest(HttpRequestMessage request)
    {
      return request.Method == HttpMethod.Get && request.RequestUri.PathAndQuery == "/v1/users/userId/bridges/id/calls";
    }

    private static void ValidateBridge(BridgeData item)
    {
      Assert.Equal("bridgeId", item.Id);
      Assert.True(item.BridgeAudio);
      Assert.Equal(BridgeState.Completed, item.State);
    }
  }
}
