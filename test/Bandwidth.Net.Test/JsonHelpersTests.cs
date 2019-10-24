using System;
using System.Net;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Xunit;

namespace Bandwidth.Net.Test
{
  public class JsonHelpersTests
  {
    [Fact]
    public void TestGetSerializerSettings()
    {
      var settings = JsonHelpers.GetSerializerSettings();
      Assert.Equal(DefaultValueHandling.IgnoreAndPopulate, settings.DefaultValueHandling);
      var converter = settings.Converters[0] as JsonStringEnumConverter;
      Assert.NotNull(converter);
    }

    [Fact]
    public async void TestSetJsonContent()
    {
      using (var request = new HttpRequestMessage())
      {
        request.SetJsonContent(new {Field1 = "test"});
        Assert.Equal("application/json", request.Content.Headers.ContentType.MediaType);
        var json = await request.Content.ReadAsStringAsync();
        Assert.Equal("{\"field1\":\"test\"}", json);
      }
    }

    [Fact]
    public async void TestReadAsJsonAsync()
    {
      var content = new StringContent("{\"field1\": 100}", Encoding.UTF8, "application/json");
      var item = await content.ReadAsJsonAsync<TestItem>();
      Assert.NotNull(item);
      Assert.Equal(100, item.Field1);
    }

    [Fact]
    public async void TestReadAsJsonAsyncForNonJsonContent()
    {
      var content = new StringContent("text", Encoding.UTF8, "plain/text");
      var item = await content.ReadAsJsonAsync<TestItem>();
      Assert.Null(item);
    }

    [Fact]
    public async void TestCheckResponseForSuccess()
    {
      using (var response = new HttpResponseMessage(HttpStatusCode.OK))
      {
        response.Content = new StringContent("{\"field1\": 100}", Encoding.UTF8, "application/json");
        await response.CheckResponseAsync();
      }
    }

    [Fact]
    public async void TestCheckResponseForErrorWithJsonPayload()
    {
      var ex = await Assert.ThrowsAsync<BandwidthException>(() =>
      {
        using (var response = new HttpResponseMessage(HttpStatusCode.BadRequest))
        {
          response.Content = new StringContent("{\"code\": \"100\", \"message\": \"Error message\"}", Encoding.UTF8,
            "application/json");
          return response.CheckResponseAsync();
        }
      });
      Assert.Equal(HttpStatusCode.BadRequest, ex.Code);
      Assert.Equal("Error message", ex.Message);
    }

    [Fact]
    public async void TestCheckResponseForErrorWithCodeOnly()
    {
      var ex = await Assert.ThrowsAsync<BandwidthException>(() =>
      {
        using (var response = new HttpResponseMessage(HttpStatusCode.BadRequest))
        {
          response.Content = new StringContent("{\"code\": \"100\"}", Encoding.UTF8, "application/json");
          return response.CheckResponseAsync();
        }
      });
      Assert.Equal(HttpStatusCode.BadRequest, ex.Code);
      Assert.Equal("100", ex.Message);
    }

    [Fact]
    public async void TestCheckResponseWithInvalidJson()
    {
      var ex = await Assert.ThrowsAsync<BandwidthException>(() =>
      {
        using (var response = new HttpResponseMessage(HttpStatusCode.BadRequest))
        {
          response.Content = new StringContent("{\"code\" \"100", Encoding.UTF8, "application/json");
          return response.CheckResponseAsync();
        }
      });
      Assert.Equal(HttpStatusCode.BadRequest, ex.Code);
    }

    [Fact]
    public async void TestCheckResponseWithRateLimitError()
    {
      var ex = await Assert.ThrowsAsync<RateLimitException>(() =>
      {
        using (var response = new HttpResponseMessage((HttpStatusCode)429))
        {
          response.Headers.Add("X-RateLimit-Reset", "1479308598680");
          return response.CheckResponseAsync();
        }
      });
      Assert.Equal(429, (int)ex.Code);
      var time = (ex.ResetTime.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
    }

    [Fact]
    public async void TestCheckResponseWithRateLimitErrorNoHeader()
    {
      var ex = await Assert.ThrowsAsync<RateLimitException>(() =>
      {
        using (var response = new HttpResponseMessage((HttpStatusCode)429))
        {
          return response.CheckResponseAsync();
        }
      });
      Assert.Equal(429, (int)ex.Code);
      var time = (ex.ResetTime.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
    }

    [Fact]
    public async void TestCheckResponseWithNonJson()
    {
      var ex = await Assert.ThrowsAsync<BandwidthException>(() =>
      {
        using (var response = new HttpResponseMessage(HttpStatusCode.BadRequest))
        {
          response.Content = new StringContent("Error message", Encoding.UTF8, "text/plain");
          return response.CheckResponseAsync();
        }
      });
      Assert.Equal(HttpStatusCode.BadRequest, ex.Code);
      Assert.Equal("Error message", ex.Message);
    }

    public class TestItem
    {
      public int Field1 { get; set; }
    }
  }
}
