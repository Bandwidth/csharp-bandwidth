using System.Net.Http;
using System.Threading.Tasks;
using LightMock;
using Xunit;

namespace Bandwidth.Net.Test.Api
{
  public class IrisAccountTests
  {
    [Fact]
    public async void TestGet()
    {
      var response = new HttpResponseMessage
      {
        Content = Helpers.GetXmlContent("Account")
      };
      var context = new MockContext<IHttp>();
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidGetRequest(r)), HttpCompletionOption.ResponseContentRead,
            null)).Returns(Task.FromResult(response));
      var api = Helpers.GetClient(context).IrisAccount;
      var account = await api.GetAsync();
      Assert.Equal("14", account.AccountId);
      Assert.Equal("Business", account.AccountType);
    }

    public static bool IsValidGetRequest(HttpRequestMessage request)
    {
      return request.Method == HttpMethod.Get && request.RequestUri.PathAndQuery == "/v1.0/accounts/accountId";
    }
  }
}
