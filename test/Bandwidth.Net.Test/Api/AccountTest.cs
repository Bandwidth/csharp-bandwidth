using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Bandwidth.Net.Api;
using LightMock;
using Xunit;
using Xunit.Sdk;

namespace Bandwidth.Net.Test.Api
{
  public class AccountTest
  {
    [Fact]
    public async void TestGet()
    {
      var response = new HttpResponseMessage
      {
        Content = Helpers.GetJsonContent("Account")
      };
      var context = new MockContext<IHttp>();
      context.Arrange(m =>m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidGetRequest(r)), HttpCompletionOption.ResponseContentRead, null)).Returns(Task.FromResult(response));
      var api = Helpers.GetClient(context).Account;
      var account = await api.GetAsync();
      Assert.Equal("538.37250", account.Balance);
      Assert.Equal(AccountType.PrePay, account.AccountType);
    }

    [Fact]
    public void TestGetTransactions()
    {
      var response = new HttpResponseMessage
      {
        Content = Helpers.GetJsonContent("AccountTransactions")
      };
      var context = new MockContext<IHttp>();
      context.Arrange(m => m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidGetTransactionsRequest(r)), HttpCompletionOption.ResponseContentRead, null)).Returns(Task.FromResult(response));
      var api = Helpers.GetClient(context).Account;
      var transactions = api.GetTransactions().ToArray();
      Assert.Equal(1, transactions.Length);
      Assert.Equal("transactionId1", transactions[0].Id);
      Assert.Equal(ProductType.SmsOut, transactions[0].ProductType);
      Assert.Equal(AccountTransactionType.Charge, transactions[0].Type);
    }

    public static bool IsValidGetRequest(HttpRequestMessage request)
    {
      return request.Method == HttpMethod.Get && request.RequestUri.PathAndQuery == "/v1/users/userId/account";
    }

    public static bool IsValidGetTransactionsRequest(HttpRequestMessage request)
    {
      return request.Method == HttpMethod.Get &&
             request.RequestUri.PathAndQuery == "/v1/users/userId/account/transactions";
    }
  }
}
