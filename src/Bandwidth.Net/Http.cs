using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Bandwidth.Net
{
  /// <summary>
  ///   Http request processor interface.
  /// </summary>
  /// <remarks>
  ///   Implement own class with this interface to overwrite default http request processing (usefull for tests, logs, etc.)
  /// </remarks>
  public interface IHttp
  {
    /// <summary>
    ///   Send http request and return response message
    /// </summary>
    /// <param name="request">Request message to send</param>
    /// <param name="completionOption">
    ///   Indicates if current http operation should be considered completed either as soon as a
    ///   response is available, or after reading the entire response message including the content.
    /// </param>
    /// <param name="cancellationToken">Cancelation token for current async operation</param>
    /// <returns>Task with response message</returns>
    Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
      HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead,
      CancellationToken? cancellationToken = null);
  }

  internal class Http<THandler> : IHttp where THandler : HttpMessageHandler, new()
  {
    public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
      HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead,
      CancellationToken? cancellationToken = null)
    {
      using (var client = new HttpClient(new THandler(), true))
      {
        return await client.SendAsync(request, completionOption, cancellationToken ?? CancellationToken.None);
      }
    }
  }
}