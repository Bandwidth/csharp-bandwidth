using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Bandwidth.Net.Api
{
  /// <summary>
  ///   Access to Transcription Api
  /// </summary>
  public interface ITranscription
  {
    /// <summary>
    ///   Get a list of transcriptions
    /// </summary>
    /// <param name="recordingId">Id of the recording</param>
    /// <param name="query">Optional query parameters</param>
    /// <param name="cancellationToken">>Optional token to cancel async operation</param>
    /// <returns>Collection with <see cref="Transcription" /> instances</returns>
    /// <example>
    ///   <code>
    /// var transcriptions = client.Transcription.List("recordingId"); 
    /// </code>
    /// </example>
    IEnumerable<Transcription> List(string recordingId, TranscriptionQuery query = null,
      CancellationToken? cancellationToken = null);

    /// <summary>
    ///   Create a transcription.
    /// </summary>
    /// <param name="recordingId">Id of the recording</param>
    /// <param name="cancellationToken">Optional token to cancel async operation</param>
    /// <returns>Created transcription Id</returns>
    /// <example>
    ///   <code>
    /// var transcriptionId = await client.Transcription.CreateAsync("recordingId");
    /// </code>
    /// </example>
    Task<string> CreateAsync(string recordingId, CancellationToken? cancellationToken = null);


    /// <summary>
    ///   Get information about a transcription
    /// </summary>
    /// <param name="recordingId">Id of the recording</param>
    /// <param name="transcriptionId">Id of transcription to get</param>
    /// <param name="cancellationToken">Optional token to cancel async operation</param>
    /// <returns>Task with <see cref="Transcription" />Transcription instance</returns>
    /// <example>
    ///   <code>
    /// var transcription = await client.Transcription.GetAsync("recordingId", "transcriptionId");
    /// </code>
    /// </example>
    Task<Transcription> GetAsync(string recordingId, string transcriptionId, CancellationToken? cancellationToken = null);
  }

  internal class TranscriptionApi : ApiBase, ITranscription
  {
    public IEnumerable<Transcription> List(string recordingId, TranscriptionQuery query = null,
      CancellationToken? cancellationToken = null)
    {
      return new LazyEnumerable<Transcription>(Client,
        () =>
          Client.MakeJsonRequestAsync(HttpMethod.Get, $"/users/{Client.CatapultAuthData.UserId}/recordings/{recordingId}/transcriptions", Client.CatapultAuthData,
            cancellationToken, query));
    }

    public Task<string> CreateAsync(string recordingId,
      CancellationToken? cancellationToken = null)
    {
      return Client.MakePostJsonRequestAsync($"/users/{Client.CatapultAuthData.UserId}/recordings/{recordingId}/transcriptions", Client.CatapultAuthData,
            cancellationToken, new object());
    }

    public Task<Transcription> GetAsync(string recordingId, string transcriptionId,
      CancellationToken? cancellationToken = null)
    {
      return Client.MakeJsonRequestAsync<Transcription>(HttpMethod.Get,
        $"/users/{Client.CatapultAuthData.UserId}/bridges/recordings/{recordingId}/transcriptions/{transcriptionId}", Client.CatapultAuthData, cancellationToken);
    }
  }

  /// <summary>
  ///   Transcription data
  /// </summary>
  public class Transcription
  {
    /// <summary>
    ///   The unique id of the transcriptions resource.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// The state of the transcription
    /// </summary>
    public TranscriptionState State { get; set; }

    /// <summary>
    /// The transcribed text.
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    /// The date/time the transcription resource was created
    /// </summary>
    public DateTime Time { get; set; }

    /// <summary>
    /// The seconds between activeTime and endTime for the recording; this is the time that is going to be used to charge the resource.
    /// </summary>
    public int ChargeableDuration { get; set; }

    /// <summary>
    /// The size of the transcribed text. If the text is longer than 1000 characters it will be cropped; the full text can be retrieved from the url available at textUrl property.
    /// </summary>
    public int TextSize { get; set; }

    /// <summary>
    /// An url to the full text; this property is available regardless the textSize.
    /// </summary>
    public string TextUrl { get; set; }

    /// <summary>
    /// Media name of full text file
    /// </summary>
    public string TextMediaName => TextUrl.Split('/').Last();
  }

  /// <summary>
  ///   Query to get transcriptions
  /// </summary>
  public class TranscriptionQuery
  {
    /// <summary>
    ///   Used for pagination to indicate the size of each page requested for querying a list of transcriptions. If no value is
    ///   specified the default value is 25 (maximum value 1000).
    /// </summary>
    public int? Size { get; set; }
  }

  /// <summary>
  /// States of transcription
  /// </summary>
  public enum TranscriptionState
  {
    /// <summary>
    /// Transcribing
    /// </summary>
    Transcribing,

    /// <summary>
    /// Completed
    /// </summary>
    Completed,

    /// <summary>
    /// Error
    /// </summary>
    Error
  }
}
