using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Bandwidth.Net.Test.Mocks;
using LightMock;

namespace Bandwidth.Net.Test
{
  public static class Helpers
  {
    public static Client GetClient(MockContext<IHttp> context = null)
    {
      return new Client("userId", "apiToken", "apiSecret", "http://localhost",
        context == null ? null : new Http(context));
    }

    public static void ArrangeSendAsync(this MockContext<IHttp> context, HttpRequestMessage estimatedRequest,
      HttpResponseMessage responseToSend)
    {
      context.Arrange(
        m => m.SendAsync(estimatedRequest, HttpCompletionOption.ResponseContentRead, CancellationToken.None))
        .Returns(Task.FromResult(responseToSend));
    }
  }
}
