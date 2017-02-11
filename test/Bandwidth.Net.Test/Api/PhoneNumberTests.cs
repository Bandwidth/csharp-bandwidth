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
  public class PhoneNumberTest
  {
    [Fact]
    public void TestList()
    {
      var response = new HttpResponseMessage
      {
        Content =
          new JsonContent($"[{Helpers.GetJsonResourse("PhoneNumber")}]")
      };
      var context = new MockContext<IHttp>();
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidListRequest(r)), HttpCompletionOption.ResponseContentRead,
            null)).Returns(Task.FromResult(response));
      var api = Helpers.GetClient(context).PhoneNumber;
      var phoneNumbers = api.List();
      ValidatePhoneNumber(phoneNumbers.First());
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
      var api = Helpers.GetClient(context).PhoneNumber;
      var phoneNumberId = await api.CreateAsync(new CreatePhoneNumberData {Number = "+1234567890"});
      context.Assert(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidGetRequest(r)), HttpCompletionOption.ResponseContentRead,
            null), Invoked.Never);
      Assert.Equal("id", phoneNumberId);
    }

    [Fact]
    public async void TestGet()
    {
      var response = new HttpResponseMessage
      {
        Content = Helpers.GetJsonContent("PhoneNumber")
      };
      var context = new MockContext<IHttp>();
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidGetRequest(r)), HttpCompletionOption.ResponseContentRead,
            null)).Returns(Task.FromResult(response));
      var api = Helpers.GetClient(context).PhoneNumber;
      var phoneNumber = await api.GetAsync("id");
      ValidatePhoneNumber(phoneNumber);
    }

    [Fact]
    public async void TestUpdate()
    {
      var context = new MockContext<IHttp>();
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidUpdateRequest(r)), HttpCompletionOption.ResponseContentRead,
            null)).Returns(Task.FromResult(new HttpResponseMessage()));
      var api = Helpers.GetClient(context).PhoneNumber;
      await api.UpdateAsync("id", new UpdatePhoneNumberData {ApplicationId = "applicationId"});
    }

    [Fact]
    public async void TestDelete()
    {
      var context = new MockContext<IHttp>();
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidDeleteRequest(r)), HttpCompletionOption.ResponseContentRead,
            null)).Returns(Task.FromResult(new HttpResponseMessage()));
      var api = Helpers.GetClient(context).PhoneNumber;
      await api.DeleteAsync("id");
    }

    public static bool IsValidListRequest(HttpRequestMessage request)
    {
      return request.Method == HttpMethod.Get && request.RequestUri.PathAndQuery == "/v1/users/userId/phoneNumbers";
    }

    public static bool IsValidCreateRequest(HttpRequestMessage request)
    {
      return request.Method == HttpMethod.Post && request.RequestUri.PathAndQuery == "/v1/users/userId/phoneNumbers" &&
             request.Content.Headers.ContentType.MediaType == "application/json" &&
             request.Content.ReadAsStringAsync().Result == "{\"number\":\"+1234567890\"}";
    }

    public static bool IsValidGetRequest(HttpRequestMessage request)
    {
      return request.Method == HttpMethod.Get && request.RequestUri.PathAndQuery == "/v1/users/userId/phoneNumbers/id";
    }

    public static bool IsValidUpdateRequest(HttpRequestMessage request)
    {
      return request.Method == HttpMethod.Post && request.RequestUri.PathAndQuery == "/v1/users/userId/phoneNumbers/id" &&
             request.Content.Headers.ContentType.MediaType == "application/json" &&
             request.Content.ReadAsStringAsync().Result == "{\"applicationId\":\"applicationId\"}";
    }

    public static bool IsValidDeleteRequest(HttpRequestMessage request)
    {
      return request.Method == HttpMethod.Delete &&
             request.RequestUri.PathAndQuery == "/v1/users/userId/phoneNumbers/id";
    }

    private static void ValidatePhoneNumber(PhoneNumberData item)
    {
      Assert.Equal("numberId", item.Id);
      Assert.Equal(PhoneNumberState.Enabled, item.NumberState);
    }
  }
}
