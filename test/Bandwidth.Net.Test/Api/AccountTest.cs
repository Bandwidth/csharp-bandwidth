using System.Linq;
using System.Net.Http;
using System.Text;
using Bandwidth.Net.Api;
using LightMock;
using Xunit;

namespace Bandwidth.Net.Test.Api
{
  public class AccountTest
  {
    [Fact]
    public async void TestGet()
    {
      var response = new HttpResponseMessage
      {
        Content =
          new StringContent("{\"balance\": \"538.37250\",\"accountType\": \"pre-pay\"}", Encoding.UTF8,
            "application/json")
      };
      var context = new MockContext<IHttp>();
      context.ArrangeSendAsync(The<HttpRequestMessage>.Is(m => IsValidGetRequest(m)), response);
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
        Content =
          new StringContent(
            "[{\"id\": \"transactionId1\", \"time\": \"2013-02-21T13:39:09Z\",\"amount\": \"0.00750\",\"type\": \"charge\",\"units\": \"1\",\"productType\": \"sms-out\",\"number\": \"1234567890\"}]",
            Encoding.UTF8, "application/json")
      };
      var context = new MockContext<IHttp>();
      context.ArrangeSendAsync(The<HttpRequestMessage>.Is(m => IsValidGetTransactionsRequest(m)), response);
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
