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
  public class ApplicationTests
  {
    [Fact]
    public void TestList()
    {
      var response = new HttpResponseMessage
      {
        Content =
          new JsonContent($"[{Helpers.GetJsonResourse("Application")}]")
      };
      var context = new MockContext<IHttp>();
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidListRequest(r)), HttpCompletionOption.ResponseContentRead,
            null)).Returns(Task.FromResult(response));
      var api = Helpers.GetClient(context).Application;
      var applications = api.List();
      ValidateApplication(applications.First());
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
      var api = Helpers.GetClient(context).Application;
      var applicationId = await api.CreateAsync(new CreateApplicationData {Name = "MyFirstApp"});
      context.Assert(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidGetRequest(r)), HttpCompletionOption.ResponseContentRead,
            null), Invoked.Never);
      Assert.Equal("id", applicationId);
    }

    [Fact]
    public async void TestGet()
    {
      var response = new HttpResponseMessage
      {
        Content = Helpers.GetJsonContent("Application")
      };
      var context = new MockContext<IHttp>();
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidGetRequest(r)), HttpCompletionOption.ResponseContentRead,
            null)).Returns(Task.FromResult(response));
      var api = Helpers.GetClient(context).Application;
      var application = await api.GetAsync("id");
      ValidateApplication(application);
    }

    [Fact]
    public async void TestUpdate()
    {
      var context = new MockContext<IHttp>();
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidUpdateRequest(r)), HttpCompletionOption.ResponseContentRead,
            null)).Returns(Task.FromResult(new HttpResponseMessage()));
      var api = Helpers.GetClient(context).Application;
      await api.UpdateAsync("id", new UpdateApplicationData {Name = "NewName"});
    }

    [Fact]
    public async void TestDelete()
    {
      var context = new MockContext<IHttp>();
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidDeleteRequest(r)), HttpCompletionOption.ResponseContentRead,
            null)).Returns(Task.FromResult(new HttpResponseMessage()));
      var api = Helpers.GetClient(context).Application;
      await api.DeleteAsync("id");
    }

    public static bool IsValidListRequest(HttpRequestMessage request)
    {
      return request.Method == HttpMethod.Get && request.RequestUri.PathAndQuery == "/v1/users/userId/applications";
    }

    public static bool IsValidCreateRequest(HttpRequestMessage request)
    {
      return request.Method == HttpMethod.Post && request.RequestUri.PathAndQuery == "/v1/users/userId/applications" &&
             request.Content.Headers.ContentType.MediaType == "application/json" &&
             request.Content.ReadAsStringAsync().Result == "{\"name\":\"MyFirstApp\"}";
    }

    public static bool IsValidGetRequest(HttpRequestMessage request)
    {
      return request.Method == HttpMethod.Get && request.RequestUri.PathAndQuery == "/v1/users/userId/applications/id";
    }

    public static bool IsValidUpdateRequest(HttpRequestMessage request)
    {
      return request.Method == HttpMethod.Post && request.RequestUri.PathAndQuery == "/v1/users/userId/applications/id" &&
             request.Content.Headers.ContentType.MediaType == "application/json" &&
             request.Content.ReadAsStringAsync().Result == "{\"name\":\"NewName\"}";
    }

    public static bool IsValidDeleteRequest(HttpRequestMessage request)
    {
      return request.Method == HttpMethod.Delete &&
             request.RequestUri.PathAndQuery == "/v1/users/userId/applications/id";
    }

    private static void ValidateApplication(ApplicationData item)
    {
      Assert.Equal("applicationId", item.Id);
      Assert.Equal("MyFirstApp", item.Name);
      Assert.Equal("http://example.com/calls.php", item.IncomingCallUrl);
      Assert.Equal("http://example.com/messages.php", item.IncomingMessageUrl);
      Assert.True(item.AutoAnswer);
    }
  }
}
