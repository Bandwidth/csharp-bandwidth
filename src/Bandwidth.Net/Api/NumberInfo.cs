using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Bandwidth.Net.Api
{
  /// <summary>
  ///   Access to NumberInfo Api
  /// </summary>
  public interface INumberInfo
  {
    /// <summary>
    ///   Get the CNAM info of a number
    /// </summary>
    /// <param name="number">Phone number to get CNAM information</param>
    /// <param name="cancellationToken">Optional token to cancel async operation</param>
    /// <returns>Task with <see cref="NumberInfo" />NumberInfo instance</returns>
    /// <example>
    ///   <code>
    /// var numberInfo = await client.NumberInfo.GetAsync("1234567890");
    /// </code>
    /// </example>
    Task<NumberInfo> GetAsync(string number, CancellationToken? cancellationToken = null);
  }

  internal class NumberInfoApi : ApiBase, INumberInfo
  {
    public Task<NumberInfo> GetAsync(string number, CancellationToken? cancellationToken = null)
    {
      return Client.MakeJsonRequestAsync<NumberInfo>(HttpMethod.Get,
        $"/phoneNumbers/numberInfo/{Uri.EscapeDataString(number)}", cancellationToken);
    }
  }


  /// <summary>
  ///   CNAM information
  /// </summary>
  public class NumberInfo
  {
    /// <summary>
    ///   The Caller ID name information.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///   The full phone number, specified in E.164 format.
    /// </summary>
    public string Number { get; set; }

    /// <summary>
    ///   The time this Caller ID information was first queried
    /// </summary>
    public DateTime Created { get; set; }

    /// <summary>
    ///   The time this Caller ID information was last updated
    /// </summary>
    public DateTime Updated { get; set; }
  }
}
