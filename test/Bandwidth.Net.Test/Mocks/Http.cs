using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using LightMock;

namespace Bandwidth.Net.Test.Mocks
{
  public class Http : IHttp
  {
    private readonly IInvocationContext<IHttp> _context;

    public Http(IInvocationContext<IHttp> context)
    {
      _context = context;
    }

    public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead,
      CancellationToken? cancellationToken = null)
    {
      return _context.Invoke(f => f.SendAsync(request, completionOption, cancellationToken));
    }
  }
}
