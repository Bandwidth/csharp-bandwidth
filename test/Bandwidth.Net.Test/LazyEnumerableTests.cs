using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LightMock;
using Xunit;

namespace Bandwidth.Net.Test
{
  public class LazyEnumerableTests
  {
    [Fact]
    public void TestConstructorWithNullParameters()
    {
      Assert.Throws<ArgumentNullException>(
        () => new LazyEnumerable<string>(null, () => Task.FromResult(new HttpResponseMessage())));
      Assert.Throws<ArgumentNullException>(() => new LazyEnumerable<string>(Helpers.GetClient(), null));
    }

    [Fact]
    public void TestGetEnumerator()
    {
      var response = new HttpResponseMessage
      {
        Content = new StringContent("[\"1\", \"2\"]", Encoding.UTF8, "application/json")
      };
      var list = new LazyEnumerable<string>(Helpers.GetClient(), () => Task.FromResult(response));
      Assert.Equal(new[] {"1", "2"}, list);
    }

    [Fact]
    public void TestGetEnumeratorWithLinkHeader()
    {
      var response = new HttpResponseMessage
      {
        Content = new StringContent("[\"1\", \"2\"]", Encoding.UTF8, "application/json")
      };
      response.Headers.Add("Link",
        "<https://api.catapult.inetwork.com/v1/users/userId/account/transactions?page=0&size=25>; rel=\"first\"");
      var list = new LazyEnumerable<string>(Helpers.GetClient(), () => Task.FromResult(response));
      Assert.Equal(new[] {"1", "2"}, list);
    }

    [Fact]
    public void TestGetEnumeratorWithNextPage()
    {
      var response = new HttpResponseMessage
      {
        Content = new StringContent("[\"1\"]", Encoding.UTF8, "application/json")
      };
      response.Headers.Add("Link",
        "<https://api.catapult.inetwork.com/v1/users/userId/account/transactions?page=0&size=25>; rel=\"first\", <https://api.catapult.inetwork.com/v1/users/userId/account/transactions?page=1&size=25>; rel=\"next\"");
      var nextPageResponse = new HttpResponseMessage
      {
        Content = new StringContent("[\"2\", \"3\"]", Encoding.UTF8, "application/json")
      };
      var context = new MockContext<IHttp>();
      context.Arrange(m => m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidRequest(r)), HttpCompletionOption.ResponseContentRead, null)).Returns(Task.FromResult(nextPageResponse));
      var list = new LazyEnumerable<string>(Helpers.GetClient(context), () => Task.FromResult(response));
      Assert.Equal(new[] {"1", "2", "3"}, list);
    }

    public static bool IsValidRequest(HttpRequestMessage request)
    {
      return request.RequestUri.ToString() ==
             "https://api.catapult.inetwork.com/v1/users/userId/account/transactions?page=1&size=25"
             && request.Method == HttpMethod.Get;
    }

    [Fact]
    public void TestGetEnumeratorWithWrongLinkHeader()
    {
      var response = new HttpResponseMessage
      {
        Content = new StringContent("[\"1\", \"2\"]", Encoding.UTF8, "application/json")
      };
      response.Headers.Add("Link",
        "<https://api.catapult.inetwork.com/v1/users/userId/account/transactions?page=0&size=25> rel=\"next\"");
      var list = new LazyEnumerable<string>(Helpers.GetClient(), () => Task.FromResult(response));
      Assert.Equal(new[] {"1", "2"}, list);
    }
  }
}
