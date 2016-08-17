using System.Net.Http;
using System.Threading.Tasks;
using Bandwidth.Net.Api;
using LightMock;
using Xunit;

namespace Bandwidth.Net.Test.Api
{
  public class IntelligenceServiceTests
  {
    [Fact]
    public async void TestGetNumberIntelligenceData()
    {
      var response = new HttpResponseMessage
      {
        Content = Helpers.GetJsonContent("NumberIntelligenceData")
      };
      var context = new MockContext<IHttp>();
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidGetNumberIntelligenceDataRequest(r)), HttpCompletionOption.ResponseContentRead,
            null)).Returns(Task.FromResult(response));
      var api = Helpers.GetClient(context).IntelligenceService;
      var data = await api.GetNumberIntelligenceDataAsync("1234567890");
      Assert.Equal("+1234567890", data.Number);
      Assert.Equal(NumberIntelligenceDataLineType.FixedVoip, data.LineType);
      Assert.Equal(NumberIntelligenceDataRiskType.NotApplicable, data.RiskType);
      Assert.Equal(NumberIntelligenceDataRiskCategory.OtherSpam, data.RiskCategory);
    }

    public static bool IsValidGetNumberIntelligenceDataRequest(HttpRequestMessage request)
    {
      return request.Method == HttpMethod.Get && request.RequestUri.PathAndQuery == "/v1/users/userId/intelligenceServices/number/1234567890";
    }
  }
}
