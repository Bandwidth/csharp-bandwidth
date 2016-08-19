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
  public class MessageTests
  {
    [Fact]
    public void TestList()
    {
      var response = new HttpResponseMessage
      {
        Content =
          new JsonContent($"[{Helpers.GetJsonResourse("Message")}]")
      };
      var context = new MockContext<IHttp>();
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidListRequest(r)), HttpCompletionOption.ResponseContentRead,
            null)).Returns(Task.FromResult(response));
      var api = Helpers.GetClient(context).Message;
      var messages = api.List();
      ValidateMessage(messages.First());
    }

    [Fact]
    public async void TestSend()
    {
      var response = new HttpResponseMessage(HttpStatusCode.Created);
      response.Headers.Location = new Uri("http://localhost/path/id");
      var getResponse = new HttpResponseMessage
      {
        Content = Helpers.GetJsonContent("Message")
      };
      var context = new MockContext<IHttp>();
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidSendRequest(r)), HttpCompletionOption.ResponseContentRead,
            null)).Returns(Task.FromResult(response));
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidGetRequest(r)), HttpCompletionOption.ResponseContentRead,
            null)).Returns(Task.FromResult(getResponse));
      var api = Helpers.GetClient(context).Message;
      var message = await api.SendAsync(new MessageData {From = "+1234567890", To = "+1234567891", Text = "Hello"});
      context.Assert(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidGetRequest(r)), HttpCompletionOption.ResponseContentRead,
            null), Invoked.Never);
      Assert.Equal("id", message.Id);
      ValidateMessage(message.Instance);
      context.Assert(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidGetRequest(r)), HttpCompletionOption.ResponseContentRead,
            null), Invoked.Once);
    }

    [Fact]
    public async void TestBatchSend()
    {
      var response = new HttpResponseMessage(HttpStatusCode.Accepted)
      {
        Content = new JsonContent("[{\"result\": \"accepted\", \"location\": \"http://host/path/id\"}]")
      };
      var context = new MockContext<IHttp>();
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidBatchSendRequest(r)), HttpCompletionOption.ResponseContentRead,
            null)).Returns(Task.FromResult(response));
      var api = Helpers.GetClient(context).Message;
      var result = await api.SendAsync(new []{new MessageData {From = "+1234567890", To = "+1234567891", Text = "Hello"}});
      Assert.Equal("id", result[0].Id);
      Assert.Equal(SendMessageResults.Accepted, result[0].Result);
    }

    [Fact]
    public async void TestGet()
    {
      var response = new HttpResponseMessage
      {
        Content = Helpers.GetJsonContent("Message")
      };
      var context = new MockContext<IHttp>();
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidGetRequest(r)), HttpCompletionOption.ResponseContentRead,
            null)).Returns(Task.FromResult(response));
      var api = Helpers.GetClient(context).Message;
      var message = await api.GetAsync("id");
      ValidateMessage(message);
    }


    public static bool IsValidListRequest(HttpRequestMessage request)
    {
      return request.Method == HttpMethod.Get && request.RequestUri.PathAndQuery == "/v1/users/userId/messages";
    }

    public static bool IsValidSendRequest(HttpRequestMessage request)
    {
      return request.Method == HttpMethod.Post && request.RequestUri.PathAndQuery == "/v1/users/userId/messages" &&
             request.Content.Headers.ContentType.MediaType == "application/json" &&
             request.Content.ReadAsStringAsync().Result == "{\"from\":\"+1234567890\",\"to\":\"+1234567891\",\"text\":\"Hello\"}";
    }

    public static bool IsValidBatchSendRequest(HttpRequestMessage request)
    {
      return request.Method == HttpMethod.Post && request.RequestUri.PathAndQuery == "/v1/users/userId/messages" &&
             request.Content.Headers.ContentType.MediaType == "application/json" &&
             request.Content.ReadAsStringAsync().Result == "[{\"from\":\"+1234567890\",\"to\":\"+1234567891\",\"text\":\"Hello\"}]";
    }

    public static bool IsValidGetRequest(HttpRequestMessage request)
    {
      return request.Method == HttpMethod.Get && request.RequestUri.PathAndQuery == "/v1/users/userId/messages/id";
    }


    private static void ValidateMessage(Message item)
    {
      Assert.Equal("messageId", item.Id);
      Assert.Equal(MessageDirection.Out, item.Direction);
      Assert.Equal("Hello", item.Text);
    }
  }

  public class MessageQueryDateTimeTests
  {
    [Fact]
    public void TestFromDateTime()
    {
      MessageQueryDateTime time = new DateTime(2016, 08, 19, 11, 10, 20);
      Assert.Equal("2016-08-19 11:10:20", time.ToString());
    }

    [Fact]
    public void TestToDateTime()
    {
      MessageQueryDateTime time = new DateTime(2016, 08, 19, 11, 00, 00);
      DateTime anotherTime = time;
      Assert.Equal(new DateTime(2016, 08, 19, 11, 00, 00), anotherTime);
    }
  }
}
