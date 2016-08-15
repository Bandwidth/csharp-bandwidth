using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Bandwidth.Net.Api
{
  /// <summary>
  ///   Access to Bridge Api
  /// </summary>
  public interface IBridge : IPlayAudio
  {
    /// <summary>
    ///   Get a list of bridges
    /// </summary>
    /// <param name="query">Optional query parameters</param>
    /// <param name="cancellationToken">>Optional token to cancel async operation</param>
    /// <returns>Collection with <see cref="Bridge" /> instances</returns>
    /// <example>
    ///   <code>
    /// var bridges = client.Bridge.List(); 
    /// </code>
    /// </example>
    IEnumerable<Bridge> List(BridgeQuery query = null,
      CancellationToken? cancellationToken = null);

    /// <summary>
    ///   Create a bridge.
    /// </summary>
    /// <param name="data">Parameters of new bridge</param>
    /// <param name="cancellationToken">Optional token to cancel async operation</param>
    /// <returns>Instance of created bridge</returns>
    /// <example>
    ///   <code>
    /// var bridge = await client.Bridge.CreateAsync(new CreateBridgeData{ CallIds = new[]{"callId"}});
    /// </code>
    /// </example>
    Task<ILazyInstance<Bridge>> CreateAsync(CreateBridgeData data, CancellationToken? cancellationToken = null);


    /// <summary>
    ///   Get information about a bridge
    /// </summary>
    /// <param name="bridgeId">Id of bridge to get</param>
    /// <param name="cancellationToken">Optional token to cancel async operation</param>
    /// <returns>Task with <see cref="Bridge" />Bridge instance</returns>
    /// <example>
    ///   <code>
    /// var bridge = await client.Bridge.GetAsync("bridgeId");
    /// </code>
    /// </example>
    Task<Bridge> GetAsync(string bridgeId, CancellationToken? cancellationToken = null);

    /// <summary>
    ///   Update a bridge
    /// </summary>
    /// <param name="bridgeId">Id of bridge to change</param>
    /// <param name="data">Changed data</param>
    /// <param name="cancellationToken">Optional token to cancel async operation</param>
    /// <returns>Task instance for async operation</returns>
    /// <example>
    ///   <code>
    /// await client.Bridge.UpdateAsync("bridgeId", new UpdateBridgeData {BridgeAudio = true});
    /// </code>
    /// </example>
    Task UpdateAsync(string bridgeId, UpdateBridgeData data, CancellationToken? cancellationToken = null);

    /// <summary>
    ///   Get a list of calls of bridge
    /// </summary>
    /// <param name="bridgeId">Id of bridge to get calls</param>
    /// <param name="cancellationToken">>Optional token to cancel async operation</param>
    /// <returns>Collection with <see cref="Call" /> instances</returns>
    /// <example>
    ///   <code>
    /// var calls = client.Bridge.GetCalls(); 
    /// </code>
    /// </example>
    IEnumerable<Call> GetCalls(string bridgeId, CancellationToken? cancellationToken = null);
  }

  internal class BridgeApi : ApiBase, IBridge
  {
    public IEnumerable<Bridge> List(BridgeQuery query = null, CancellationToken? cancellationToken = null)
    {
      return new LazyEnumerable<Bridge>(Client,
        () =>
          Client.MakeJsonRequestAsync(HttpMethod.Get, $"/users/{Client.UserId}/bridges", cancellationToken, query));
    }

    public async Task<ILazyInstance<Bridge>> CreateAsync(CreateBridgeData data,
      CancellationToken? cancellationToken = null)
    {
      var id = await Client.MakePostJsonRequestAsync($"/users/{Client.UserId}/bridges", cancellationToken, data);
      return new LazyInstance<Bridge>(id, () => GetAsync(id));
    }

    public Task<Bridge> GetAsync(string bridgeId, CancellationToken? cancellationToken = null)
    {
      return Client.MakeJsonRequestAsync<Bridge>(HttpMethod.Get,
        $"/users/{Client.UserId}/bridges/{bridgeId}", cancellationToken);
    }

    public Task UpdateAsync(string bridgeId, UpdateBridgeData data,
      CancellationToken? cancellationToken = null)
    {
      return Client.MakeJsonRequestAsync(HttpMethod.Post,
        $"/users/{Client.UserId}/bridges/{bridgeId}", cancellationToken, null, data);
    }

    public IEnumerable<Call> GetCalls(string bridgeId, CancellationToken? cancellationToken = null)
    {
      return new LazyEnumerable<Call>(Client,
        () =>
          Client.MakeJsonRequestAsync(HttpMethod.Get, $"/users/{Client.UserId}/bridges/{bridgeId}/calls",
            cancellationToken));
    }

    public Task PlayAudioAsync(string bridgeId, PlayAudioData data, CancellationToken? cancellationToken = null)
    {
      return
        Client.MakeJsonRequestAsync(HttpMethod.Post,
          $"/users/{Client.UserId}/bridges/{bridgeId}/audio", cancellationToken, null, data);
    }
  }


  /// <summary>
  ///   Bridge information
  /// </summary>
  public class Bridge
  {
    /// <summary>
    ///   The unique identifier for the bridge.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Bridge state
    /// </summary>
    public BridgeState State { get; set; }

    /// <summary>
    /// List of call Ids that will be in the bridge.
    /// </summary>
    public string[] CallIds { get; set; }

    /// <summary>
    /// Enable/Disable two way audio path.
    /// </summary>
    public bool BridgeAudio { get; set; }

    /// <summary>
    /// The time when the bridge was completed.
    /// </summary>
    public DateTime CompletedTime { get; set; }

    /// <summary>
    /// The time that bridge was created.
    /// </summary>
    public DateTime CreatedTime { get; set; }

    /// <summary>
    /// The time that the bridge got into active state.
    /// </summary>
    public DateTime ActivatedTime { get; set; }

  }

  /// <summary>
  /// Possible bridge state
  /// </summary>
  public enum BridgeState
  {
    /// <summary>
    /// The bridge was created but the audio was never bridged.
    /// </summary>
    Created,

    /// <summary>
    /// The bridge has two active calls and the audio was already bridged before.
    /// </summary>
    Active,

    /// <summary>
    /// The bridge calls are on hold (bridgeAudio was set to false).
    /// </summary>
    Hold,

    /// <summary>
    /// The bridge was completed. The bridge is completed when all calls hangup or when all calls are removed from bridge.
    /// </summary>
    Completed,

    /// <summary>
    /// Some error was detected in bridge.
    /// </summary>
    Error
  }

  /// <summary>
  ///   Query to get bridges
  /// </summary>
  public class BridgeQuery
  {
    /// <summary>
    ///   Used for pagination to indicate the size of each page requested for querying a list of bridges. If no value is
    ///   specified the default value is 25 (maximum value 1000).
    /// </summary>
    public int? Size { get; set; }
  }
   
  /// <summary>
  ///   Parameters to create an bridge
  /// </summary>
  public class CreateBridgeData
  {
    /// <summary>
    /// List of call Ids that will be in the bridge.
    /// </summary>
    public string[] CallIds { get; set; }

    /// <summary>
    /// Enable/Disable two way audio path.
    /// </summary>
    public bool? BridgeAudio { get; set; }
  }

  /// <summary>
  ///   Parameters of a bridge to change
  /// </summary>
  public class UpdateBridgeData : CreateBridgeData
  {
  }
}
