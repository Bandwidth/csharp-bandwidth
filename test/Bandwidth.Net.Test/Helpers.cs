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
  }
}
