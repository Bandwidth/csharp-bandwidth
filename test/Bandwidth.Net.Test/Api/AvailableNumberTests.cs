using System.Net.Http;
using System.Threading.Tasks;
using Bandwidth.Net.Api;
using LightMock;
using Xunit;

namespace Bandwidth.Net.Test.Api
{
  public class AvailableNumberTest
  {
    [Fact]
    public async void TestSearchLocal()
    {
      var response = new HttpResponseMessage
      {
        Content = Helpers.GetJsonContent("AvailableNumbers")
      };
      var context = new MockContext<IHttp>();
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidSearchLocalRequest(r)),
            HttpCompletionOption.ResponseContentRead,
            null)).Returns(Task.FromResult(response));
      var api = Helpers.GetClient(context).AvailableNumber;
      var numbers = await api.SearchLocalAsync(new LocalNumberQuery {AreaCode = "910"});
      ValidateAvailableNumbers(numbers);
    }

    [Fact]
    public async void TestSearchTollFree()
    {
      var response = new HttpResponseMessage
      {
        Content = Helpers.GetJsonContent("AvailableNumbers")
      };
      var context = new MockContext<IHttp>();
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidSearchTollFreeRequest(r)),
            HttpCompletionOption.ResponseContentRead,
            null)).Returns(Task.FromResult(response));
      var api = Helpers.GetClient(context).AvailableNumber;
      var numbers = await api.SearchTollFreeAsync(new TollFreeNumberQuery {Quantity = 1});
      ValidateAvailableNumbers(numbers);
    }

    [Fact]
    public async void TestSearchAndOrderLocal()
    {
      var response = new HttpResponseMessage
      {
        Content = Helpers.GetJsonContent("OrderedNumbers")
      };
      var context = new MockContext<IHttp>();
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidSearchAndOrderLocalRequest(r)),
            HttpCompletionOption.ResponseContentRead,
            null)).Returns(Task.FromResult(response));
      var api = Helpers.GetClient(context).AvailableNumber;
      var numbers = await api.SearchAndOrderLocalAsync(new LocalNumberQueryForOrder {AreaCode = "910"});
      ValidateOrderedNumbers(numbers);
    }

    [Fact]
    public async void TestSearchAndOrderTollFree()
    {
      var response = new HttpResponseMessage
      {
        Content = Helpers.GetJsonContent("OrderedNumbers")
      };
      var context = new MockContext<IHttp>();
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidSearchAndOrderTollFreeRequest(r)),
            HttpCompletionOption.ResponseContentRead,
            null)).Returns(Task.FromResult(response));
      var api = Helpers.GetClient(context).AvailableNumber;
      var numbers = await api.SearchAndOrderTollFreeAsync(new TollFreeNumberQueryForOrder {Quantity = 1});
      ValidateOrderedNumbers(numbers);
    }

    public static bool IsValidSearchLocalRequest(HttpRequestMessage request)
    {
      return request.Method == HttpMethod.Get &&
             request.RequestUri.PathAndQuery == "/v1/availableNumbers/local?areaCode=910";
    }

    public static bool IsValidSearchTollFreeRequest(HttpRequestMessage request)
    {
      return request.Method == HttpMethod.Get &&
             request.RequestUri.PathAndQuery == "/v1/availableNumbers/tollFree?quantity=1";
    }

    public static bool IsValidSearchAndOrderLocalRequest(HttpRequestMessage request)
    {
      return request.Method == HttpMethod.Post &&
             request.RequestUri.PathAndQuery == "/v1/availableNumbers/local?areaCode=910";
    }

    public static bool IsValidSearchAndOrderTollFreeRequest(HttpRequestMessage request)
    {
      return request.Method == HttpMethod.Post &&
             request.RequestUri.PathAndQuery == "/v1/availableNumbers/tollFree?quantity=1";
    }

    private static void ValidateAvailableNumbers(AvailableNumber[] items)
    {
      Assert.Equal(1, items.Length);
      Assert.Equal("{number1}", items[0].Number);
      Assert.Equal("CARY", items[0].City);
    }

    private static void ValidateOrderedNumbers(OrderedNumber[] items)
    {
      Assert.Equal(1, items.Length);
      Assert.Equal("{number1}", items[0].Number);
      Assert.Equal("2.00", items[0].Price);
      Assert.Equal("numberId", items[0].Id);
    }
  }
}
