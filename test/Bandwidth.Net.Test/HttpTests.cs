using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Bandwidth.Net.Test
{
  public class HttpTests
  {
    [Fact]
    public async void TestSendAsync()
    {
      var http = new Http<TestMessageHandler>();
      var response =
        await
          http.SendAsync(new HttpRequestMessage(HttpMethod.Get, "http://localhost/"),
            HttpCompletionOption.ResponseContentRead, CancellationToken.None);
      Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    private class TestMessageHandler : HttpMessageHandler
    {
      protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
      {
        Assert.Equal(HttpMethod.Get, request.Method);
        Assert.Equal("http://localhost/", request.RequestUri.ToString());
        return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK) {Content = new StringContent("Test")});
      }
    }
  }
}
