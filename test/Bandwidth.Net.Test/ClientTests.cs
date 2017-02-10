using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LightMock;
using Xunit;

namespace Bandwidth.Net.Test
{
  public class ClientTests
  {
    [Fact]
    public void TestConstructor()
    {
      var api = new Client(new CatapultAuthData(), new IrisAuthData());
      Assert.NotNull(api.CatapultAuthData);
      Assert.NotNull(api.IrisAuthData);
    }

    [Fact]
    public void TestConstructorEmpty()
    {
      var api = new Client();
      Assert.NotNull(api.CatapultAuthData);
      Assert.NotNull(api.IrisAuthData);
    }


    [Fact]
    public void TestCreateRequest()
    {
      var api = Helpers.GetClient();
      var request = api.CreateRequest(HttpMethod.Get, "/test", api.CatapultAuthData);
      Assert.Equal(HttpMethod.Get, request.Method);
      Assert.Equal("http://localhost/v1/test", request.RequestUri.ToString());
      var hash = Convert.ToBase64String(Encoding.UTF8.GetBytes("apiToken:apiSecret"));
      Assert.Equal($"Basic {hash}", request.Headers.Authorization.ToString());
    }

    [Fact]
    public void TestCreateRequestWithAbsoluteUrl()
    {
      var api = Helpers.GetClient();
      var request = api.CreateRequest(HttpMethod.Get, "http://my-host/test", api.CatapultAuthData);
      Assert.Equal(HttpMethod.Get, request.Method);
      Assert.Equal("http://my-host/test", request.RequestUri.ToString());
    }

    [Fact]
    public void TestCreateRequestWithQuery()
    {
      var api = Helpers.GetClient();
      var request = api.CreateRequest(HttpMethod.Get, "/test",  api.CatapultAuthData,
        new
        {
          Field1 = 1,
          Field2 = "text value",
          Field3 = new DateTime(2016, 8, 1, 0, 0, 0, DateTimeKind.Utc),
          EmptyField = (string) null,
          EmptyField2 = ""
        });
      Assert.Equal(HttpMethod.Get, request.Method);
      Assert.Equal(
        "http://localhost/v1/test?field1=1&field2=text value&field3=2016-08-01T00:00:00.0000000Z",
        request.RequestUri.ToString());
      var hash = Convert.ToBase64String(Encoding.UTF8.GetBytes("apiToken:apiSecret"));
      Assert.Equal($"Basic {hash}", request.Headers.Authorization.ToString());
    }

    [Fact]
    public async void TestMakeJsonRequestReq()
    {
      var context = new MockContext<IHttp>();
      var api = Helpers.GetClient(context);
      var request = new HttpRequestMessage(HttpMethod.Get, "/test");
      context.Arrange(m => m.SendAsync(request, HttpCompletionOption.ResponseContentRead, null))
        .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)));
      using (var response =
        await api.MakeJsonRequestAsync(request, api.CatapultAuthData))
      {
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
      }
    }

    [Fact]
    public async void TestMakeJsonTRequest()
    {
      var context = new MockContext<IHttp>();
      var api = Helpers.GetClient(context);
      context.Arrange(
        m => m.SendAsync(The<HttpRequestMessage>.IsAnyValue, HttpCompletionOption.ResponseContentRead, null))
        .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
        {
          Content = new StringContent("{\"test\": \"value\"}", Encoding.UTF8, "application/json")
        }));
      var result = await api.MakeJsonRequestAsync<MakeJsonRequestDemo>(HttpMethod.Get, "/test", api.CatapultAuthData);
      Assert.Equal("value", result.Test);
    }

    [Fact]
    public async void TestMakeJsonRequest()
    {
      var context = new MockContext<IHttp>();
      var api = Helpers.GetClient(context);
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidRequestWithoutBody(r)),
            HttpCompletionOption.ResponseContentRead, null))
        .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)));
      using (var response = await api.MakeJsonRequestAsync(HttpMethod.Get, "/test", api.CatapultAuthData))
      {
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
      }
    }

    [Fact]
    public async void TestMakeJsonWithoutRequestRequest()
    {
      var context = new MockContext<IHttp>();
      var api = Helpers.GetClient(context);
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidRequestWithoutBody(r)),
            HttpCompletionOption.ResponseContentRead, null))
        .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)));
      await api.MakeJsonRequestWithoutResponseAsync(HttpMethod.Get, "/test", api.CatapultAuthData);

    }

    [Fact]
    public async void TestMakeJsonRequestWithBody()
    {
      var context = new MockContext<IHttp>();
      var api = Helpers.GetClient(context);
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidRequestWithBody(r)),
            HttpCompletionOption.ResponseContentRead, null))
        .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)));
      var response = await api.MakeJsonRequestAsync(HttpMethod.Post, "/test", api.CatapultAuthData, null, null, new {Field = "value"});
      Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async void TestMakePostJsonRequest()
    {
      var context = new MockContext<IHttp>();
      var response = new HttpResponseMessage(HttpStatusCode.OK);
      response.Headers.Location = new Uri("http://host/path/id");
      var api = Helpers.GetClient(context);
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidRequestWithBody(r)),
            HttpCompletionOption.ResponseContentRead, null))
        .Returns(Task.FromResult(response));
      var id = await api.MakePostJsonRequestAsync("/test", api.CatapultAuthData, null, new { Field = "value" });
      Assert.Equal("id", id);
    }

    [Fact]
    public async void TestMakePostJsonRequestWithoutLocationHeader()
    {
      var context = new MockContext<IHttp>();
      var response = new HttpResponseMessage(HttpStatusCode.OK);
      var api = Helpers.GetClient(context);
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidRequestWithBody(r)),
            HttpCompletionOption.ResponseContentRead, null))
        .Returns(Task.FromResult(response));
      var id = await api.MakePostJsonRequestAsync("/test", api.CatapultAuthData, null, new { Field = "value" });
      Assert.True(string.IsNullOrEmpty(id));
    }

    [Fact]
    public async void TestMakeXmlRequestReq()
    {
      var context = new MockContext<IHttp>();
      var api = Helpers.GetClient(context);
      var request = new HttpRequestMessage(HttpMethod.Get, "/test");
      context.Arrange(m => m.SendAsync(request, HttpCompletionOption.ResponseContentRead, null))
        .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)));
      using (var response =
        await api.MakeXmlRequestAsync(request, api.CatapultAuthData))
      {
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
      }
    }

    [Fact]
    public async void TestMakeXmlTRequest()
    {
      var context = new MockContext<IHttp>();
      var api = Helpers.GetClient(context);
      context.Arrange(
        m => m.SendAsync(The<HttpRequestMessage>.IsAnyValue, HttpCompletionOption.ResponseContentRead, null))
        .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
        {
          Content = new StringContent("<MakeJsonRequestDemo><Test>value</Test></MakeJsonRequestDemo>", Encoding.UTF8, "application/xml")
        }));
      var result = await api.MakeXmlRequestAsync<MakeJsonRequestDemo>(HttpMethod.Get, "/test", api.CatapultAuthData);
      Assert.Equal("value", result.Test);
    }

    [Fact]
    public async void TestMakeXmlRequest()
    {
      var context = new MockContext<IHttp>();
      var api = Helpers.GetClient(context);
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidRequestWithoutBody(r)),
            HttpCompletionOption.ResponseContentRead, null))
        .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)));
      using (var response = await api.MakeXmlRequestAsync(HttpMethod.Get, "/test", api.CatapultAuthData))
      {
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
      }
    }

    [Fact]
    public async void TestMakeXmlWithoutRequestRequest()
    {
      var context = new MockContext<IHttp>();
      var api = Helpers.GetClient(context);
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidRequestWithoutBody(r)),
            HttpCompletionOption.ResponseContentRead, null))
        .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)));
      await api.MakeXmlRequestWithoutResponseAsync(HttpMethod.Get, "/test", api.CatapultAuthData);

    }

    [Fact]
    public async void TestMakeXmlRequestWithBody()
    {
      var context = new MockContext<IHttp>();
      var api = Helpers.GetClient(context);
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidXmlRequestWithBody(r)),
            HttpCompletionOption.ResponseContentRead, null))
        .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)));
      var response = await api.MakeXmlRequestAsync(HttpMethod.Post, "/test", api.CatapultAuthData, null, null, new MakeJsonRequestDemo { Test = "value" });
      Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async void TestMakePostXmlRequest()
    {
      var context = new MockContext<IHttp>();
      var response = new HttpResponseMessage(HttpStatusCode.OK);
      response.Headers.Location = new Uri("http://host/path/id");
      var api = Helpers.GetClient(context);
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidXmlRequestWithBody(r)),
            HttpCompletionOption.ResponseContentRead, null))
        .Returns(Task.FromResult(response));
      var id = await api.MakePostXmlRequestAsync("/test", api.CatapultAuthData, null, new MakeJsonRequestDemo { Test = "value" });
      Assert.Equal("id", id);
    }

    [Fact]
    public async void TestMakePostXmlRequestWithoutLocationHeader()
    {
      var context = new MockContext<IHttp>();
      var response = new HttpResponseMessage(HttpStatusCode.OK);
      var api = Helpers.GetClient(context);
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidXmlRequestWithBody(r)),
            HttpCompletionOption.ResponseContentRead, null))
        .Returns(Task.FromResult(response));
      var id = await api.MakePostXmlRequestAsync("/test", api.CatapultAuthData, null, new MakeJsonRequestDemo { Test = "value" });
      Assert.True(string.IsNullOrEmpty(id));
    }


    public static bool IsValidRequestWithBody(HttpRequestMessage request)
    {
      return request.Content.Headers.ContentType.MediaType == "application/json"
             && request.Content.ReadAsStringAsync().Result == "{\"field\":\"value\"}";
    }

    public static bool IsValidXmlRequestWithBody(HttpRequestMessage request)
    {
      return request.Content.Headers.ContentType.MediaType == "application/xml"
             && request.Content.ReadAsStringAsync().Result.Contains("<Test>value</Test>");
    }

    public static bool IsValidRequestWithoutBody(HttpRequestMessage request)
    {
      return request.Content == null;
    }

    public class MakeJsonRequestDemo
    {
      public string Test { get; set; }
    }
  }
}
