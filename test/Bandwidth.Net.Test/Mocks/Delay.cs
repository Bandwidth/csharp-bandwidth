using System;
using System.Threading;
using System.Threading.Tasks;
using Bandwidth.Net.Api;
using LightMock;

namespace Bandwidth.Net.Test.Mocks
{
    internal class Delay: IDelay
    {
      private readonly IInvocationContext<IDelay> _context;

      public Delay(IInvocationContext<IDelay> context)
      {
        _context = context;
      }

      Task IDelay.Delay(TimeSpan interval, CancellationToken cancellationToken)
      {
        return _context.Invoke(d => d.Delay(interval, cancellationToken));
      }
    }
}
