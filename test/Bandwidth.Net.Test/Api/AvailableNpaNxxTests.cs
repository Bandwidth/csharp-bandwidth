using System.Net.Http;
using System.Threading.Tasks;
using LightMock;
using Xunit;

namespace Bandwidth.Net.Test.Api
{
  public class AvailableNpaNxxTests
  {
    [Fact]
    public async void TestGet()
    {
      var response = new HttpResponseMessage
      {
        Content = Helpers.GetXmlContent("AvailableNpaNxx")
      };
      var context = new MockContext<IHttp>();
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidGetRequest(r)), HttpCompletionOption.ResponseContentRead,
            null)).Returns(Task.FromResult(response));
      var api = Helpers.GetClient(context).AvailableNpaNxx;
      var list = await api.ListAsync();
      Assert.Equal(2, list.Length);
    }

    public static bool IsValidGetRequest(HttpRequestMessage request)
    {
      return request.Method == HttpMethod.Get && request.RequestUri.PathAndQuery == "/v1.0/accounts/accountId/availableNpaNxx";
    }
  }
}
