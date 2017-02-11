using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Bandwidth.Net.Api
{

  /// <summary>
  /// Access to Recording Api
  /// </summary>
  public interface IRecording
  {
    /// <summary>
    ///   Get a list of recordings
    /// </summary>
    /// <param name="query">Optional query parameters</param>
    /// <param name="cancellationToken">>Optional token to cancel async operation</param>
    /// <returns>Collection with <see cref="RecordingData" /> instances</returns>
    /// <example>
    ///   <code>
    /// var recordings = client.Recording.List(); 
    /// </code>
    /// </example>
    IEnumerable<RecordingData> List(RecordingQuery query = null,
      CancellationToken? cancellationToken = null);

    /// <summary>
    ///   Get information about a recording
    /// </summary>
    /// <param name="recordingId">Id of recording to get</param>
    /// <param name="cancellationToken">Optional token to cancel async operation</param>
    /// <returns>Task with <see cref="RecordingData" />Recording instance</returns>
    /// <example>
    ///   <code>
    /// var recording = await client.Recording.GetAsync("recordingId");
    /// </code>
    /// </example>
    Task<RecordingData> GetAsync(string recordingId, CancellationToken? cancellationToken = null);

  }

  internal class RecordingApi : ApiBase, IRecording
  {
    public IEnumerable<RecordingData> List(RecordingQuery query = null, CancellationToken? cancellationToken = null)
    {
      return new LazyEnumerable<RecordingData>(Client,
        () =>
          Client.MakeJsonRequestAsync(HttpMethod.Get, $"/users/{Client.UserId}/recordings", cancellationToken, query));
    }

    public Task<RecordingData> GetAsync(string recordingId, CancellationToken? cancellationToken = null)
    {
      return Client.MakeJsonRequestAsync<RecordingData>(HttpMethod.Get,
        $"/users/{Client.UserId}/recordings/{recordingId}", cancellationToken);
    }
  }
  /// <summary>
  /// Recording data
  /// </summary>
  public class RecordingData
  {
    /// <summary>
    /// The unique id of the recordings resource.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Date/time when the recording started.
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// Date/time when the recording ended. 
    /// </summary>
    public DateTime EndTime { get; set; }

    /// <summary>
    /// The complete URL to the call resource this recording is associated with.
    /// </summary>
    public string Call { get; set; }

    /// <summary>
    /// Id of associated call
    /// </summary>
    public string CallId => Call.Split('/').Last();

    /// <summary>
    /// The complete URL to the media resource this recording is associated with.
    /// </summary>
    public string Media { get; set; }

    /// <summary>
    /// Name of associated media resource
    /// </summary>
    public string MediaName => Media.Split('/').Last();

    /// <summary>
    /// The state of the recording,
    /// </summary>
    public RecordingState State { get; set; }
  }

  /// <summary>
  ///   Query to get bridges
  /// </summary>
  public class RecordingQuery
  {
    /// <summary>
    ///   Used for pagination to indicate the size of each page requested for querying a list of recordings. If no value is
    ///   specified the default value is 25 (maximum value 1000).
    /// </summary>
    public int? Size { get; set; }
  }

  /// <summary>
  /// States of recording
  /// </summary>
  public enum RecordingState
  {
    /// <summary>
    /// Recording is currently active.
    /// </summary>
    Recording,

    /// <summary>
    /// Recording complete and available for downloading or playing.
    /// </summary>
    Complete,

    /// <summary>
    /// Recording is complete but it is not available to download yet.
    /// </summary>
    Saving,

    /// <summary>
    /// Recording could not be uploaded
    /// </summary>
    Error
  }
}
