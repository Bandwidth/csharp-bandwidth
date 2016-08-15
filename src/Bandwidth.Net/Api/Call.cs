using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Bandwidth.Net.Api
{
  /// <summary>
  ///   Access to Call Api
  /// </summary>
  public interface ICall : IPlayAudio
  {
    /// <summary>
    ///   Get a list of previous calls that were made or received
    /// </summary>
    /// <param name="query">Optional query parameters</param>
    /// <param name="cancellationToken">>Optional token to cancel async operation</param>
    /// <returns>Collection with <see cref="Call" /> instances</returns>
    /// <example>
    ///   <code>
    /// var calls = client.Call.List(); 
    /// </code>
    /// </example>
    IEnumerable<Call> List(CallQuery query = null,
      CancellationToken? cancellationToken = null);

    /// <summary>
    ///   Create an outbound phone call
    /// </summary>
    /// <param name="data">Parameters of new call</param>
    /// <param name="cancellationToken">Optional token to cancel async operation</param>
    /// <returns>Instance of created call</returns>
    /// <example>
    ///   <code>
    /// var call = await client.Call.CreateAsync(new CreateCallData{ CallIds = new[]{"callId"}});
    /// </code>
    /// </example>
    Task<ILazyInstance<Call>> CreateAsync(CreateCallData data, CancellationToken? cancellationToken = null);


    /// <summary>
    ///   Get information about a call that was made or received
    /// </summary>
    /// <param name="callId">Id of call to get</param>
    /// <param name="cancellationToken">Optional token to cancel async operation</param>
    /// <returns>Task with <see cref="Call" />Call instance</returns>
    /// <example>
    ///   <code>
    /// var call = await client.Call.GetAsync("callId");
    /// </code>
    /// </example>
    Task<Call> GetAsync(string callId, CancellationToken? cancellationToken = null);

    /// <summary>
    ///   Manage an active phone call. E.g. Answer an incoming call, reject an incoming call, turn on / off recording, transfer, hang up.
    /// </summary>
    /// <param name="callId">Id of call to change</param>
    /// <param name="data">Changed data</param>
    /// <param name="cancellationToken">Optional token to cancel async operation</param>
    /// <returns>Http response message</returns>
    /// <example>
    ///   <code>
    /// await client.Call.UpdateAsync("callId", new UpdateCallData {CallAudio = true});
    /// </code>
    /// </example>
    Task<HttpResponseMessage> UpdateAsync(string callId, UpdateCallData data, CancellationToken? cancellationToken = null);

    /// <summary>
    ///   Send DTMF (phone keypad digit presses)
    /// </summary>
    /// <param name="callId">Id of call to change</param>
    /// <param name="data">DTMF data</param>
    /// <param name="cancellationToken">Optional token to cancel async operation</param>
    /// <returns>Task instance for async operation</returns>
    /// <example>
    ///   <code>
    /// await client.Call.SendDtmfAsync("callId", new SendDtmfData {DtmfOut = "1234"});
    /// </code>
    /// </example>
    Task SendDtmfAsync(string callId, SendDtmfData data, CancellationToken? cancellationToken = null);

    /// <summary>
    ///   Get a list of events of call
    /// </summary>
    /// <param name="callId">Id of call to get events</param>
    /// <param name="cancellationToken">>Optional token to cancel async operation</param>
    /// <returns>Collection with <see cref="CallEvent" /> instances</returns>
    /// <example>
    ///   <code>
    /// var events = client.Call.GetEvents("callId"); 
    /// </code>
    /// </example>
    IEnumerable<CallEvent> GetEvents(string callId, CancellationToken? cancellationToken = null);

    /// <summary>
    ///   Get an event of the call
    /// </summary>
    /// <param name="callId">Id of call to get event</param>
    /// <param name="eventId">Id of event</param>
    /// <param name="cancellationToken">>Optional token to cancel async operation</param>
    /// <returns>Collection with <see cref="CallEvent" /> instances</returns>
    /// <example>
    ///   <code>
    /// var ev = client.Call.GetEventAsync("callId", "eventId"); 
    /// </code>
    /// </example>
    Task<CallEvent> GetEventAsync(string callId, string eventId, CancellationToken? cancellationToken = null);

    /// <summary>
    ///   Get a list of recordings of call
    /// </summary>
    /// <param name="callId">Id of call to get recordings</param>
    /// <param name="cancellationToken">>Optional token to cancel async operation</param>
    /// <returns>Collection with <see cref="Recording" /> instances</returns>
    /// <example>
    ///   <code>
    /// var recordings = client.Call.GetRecordings("callId"); 
    /// </code>
    /// </example>
    IEnumerable<Recording> GetRecordings(string callId, CancellationToken? cancellationToken = null);

    /// <summary>
    ///   Get a list of transcriptions of call
    /// </summary>
    /// <param name="callId">Id of call to get transcriptions</param>
    /// <param name="cancellationToken">>Optional token to cancel async operation</param>
    /// <returns>Collection with <see cref="Transcription" /> instances</returns>
    /// <example>
    ///   <code>
    /// var transcriptions = client.Call.GetTranscriptions("callId"); 
    /// </code>
    /// </example>
    IEnumerable<Transcription> GetTranscriptions(string callId, CancellationToken? cancellationToken = null);

    /// <summary>
    ///   Gather the DTMF digits pressed
    /// </summary>
    /// <param name="callId">Id of the calls</param>
    /// <param name="data">Parameters of new call</param>
    /// <param name="cancellationToken">Optional token to cancel async operation</param>
    /// <returns>Instance of created gather</returns>
    /// <example>
    ///   <code>
    /// var gather = await client.Call.CreateGatherAsync("callId", new CreateGatherData{ MaxDigits = 1});
    /// </code>
    /// </example>
    Task<ILazyInstance<CallGather>> CreateGatherAsync(string callId, CreateGatherData data, CancellationToken? cancellationToken = null);

    /// <summary>
    ///  Get the gather DTMF parameters and results
    /// </summary>
    /// <param name="callId">Id of the call</param>
    /// <param name="gatherId">Id of the gather</param>
    /// <param name="cancellationToken">Optional token to cancel async operation</param>
    /// <returns>Instance of found gather</returns>
    /// <example>
    ///   <code>
    /// var gather = await client.Call.GetGatherAsync("callId", "gatherId");
    /// </code>
    /// </example>
    Task<CallGather> GetGatherAsync(string callId, string gatherId, CancellationToken? cancellationToken = null);

    /// <summary>
    ///   Update the gather 
    /// </summary>
    /// <param name="callId">Id of the call</param>
    /// <param name="gatherId">Id of the gather</param>
    /// <param name="data">Changed data</param>
    /// <param name="cancellationToken">Optional token to cancel async operation</param>
    /// <returns>Task instance for async operation</returns>
    /// <example>
    ///   <code>
    /// await client.Call.UpdateGatherAsync("callId", "gatherId", new UpdateGatherData {State = CallGatherState.Completed});
    /// </code>
    /// </example>
    Task UpdateGatherAsync(string callId, string gatherId, UpdateGatherData data, CancellationToken? cancellationToken = null);

  }
  
  internal class CallApi : ApiBase, ICall
  {
    public IEnumerable<Call> List(CallQuery query = null, CancellationToken? cancellationToken = null)
    {
      return new LazyEnumerable<Call>(Client,
        () =>
          Client.MakeJsonRequestAsync(HttpMethod.Get, $"/users/{Client.UserId}/calls", cancellationToken, query));
    }

    public async Task<ILazyInstance<Call>> CreateAsync(CreateCallData data,
      CancellationToken? cancellationToken = null)
    {
      var id = await Client.MakePostJsonRequestAsync($"/users/{Client.UserId}/calls", cancellationToken, data);
      return new LazyInstance<Call>(id, () => GetAsync(id));
    }

    public Task<Call> GetAsync(string callId, CancellationToken? cancellationToken = null)
    {
      return Client.MakeJsonRequestAsync<Call>(HttpMethod.Get,
        $"/users/{Client.UserId}/calls/{callId}", cancellationToken);
    }

    public Task<HttpResponseMessage> UpdateAsync(string callId, UpdateCallData data,
      CancellationToken? cancellationToken = null)
    {
      return Client.MakeJsonRequestAsync(HttpMethod.Post,
        $"/users/{Client.UserId}/calls/{callId}", cancellationToken, null, data);
    }

    public Task SendDtmfAsync(string callId, SendDtmfData data, CancellationToken? cancellationToken = null)
    {
      return Client.MakeJsonRequestAsync(HttpMethod.Post,
        $"/users/{Client.UserId}/calls/{callId}/dtmf", cancellationToken, null, data);
    }

    public IEnumerable<CallEvent> GetEvents(string callId, CancellationToken? cancellationToken = null)
    {
      return new LazyEnumerable<CallEvent>(Client,
        () =>
          Client.MakeJsonRequestAsync(HttpMethod.Get, $"/users/{Client.UserId}/calls/{callId}/events", cancellationToken));
    }

    public Task<CallEvent> GetEventAsync(string callId, string eventId, CancellationToken? cancellationToken = null)
    {
      return Client.MakeJsonRequestAsync<CallEvent>(HttpMethod.Get, $"/users/{Client.UserId}/calls/{callId}/events/{eventId}",
        cancellationToken);
    }

    public IEnumerable<Recording> GetRecordings(string callId, CancellationToken? cancellationToken = null)
    {
      return new LazyEnumerable<Recording>(Client,
        () =>
          Client.MakeJsonRequestAsync(HttpMethod.Get, $"/users/{Client.UserId}/calls/{callId}/recordings", cancellationToken));
    }

    public IEnumerable<Transcription> GetTranscriptions(string callId, CancellationToken? cancellationToken = null)
    {
      return new LazyEnumerable<Transcription>(Client,
        () =>
          Client.MakeJsonRequestAsync(HttpMethod.Get, $"/users/{Client.UserId}/calls/{callId}/transcriptions", cancellationToken));
    }

    public async Task<ILazyInstance<CallGather>> CreateGatherAsync(string callId, CreateGatherData data, CancellationToken? cancellationToken = null)
    {
      var id = await Client.MakePostJsonRequestAsync($"/users/{Client.UserId}/calls/{callId}/gather", cancellationToken, data);
      return new LazyInstance<CallGather>(id, () => GetGatherAsync(callId, id));
    }

    public Task<CallGather> GetGatherAsync(string callId, string gatherId, CancellationToken? cancellationToken = null)
    {
      return Client.MakeJsonRequestAsync<CallGather>(HttpMethod.Get,
        $"/users/{Client.UserId}/calls/{callId}/gather/{gatherId}", cancellationToken);
    }

    public Task UpdateGatherAsync(string callId, string gatherId, UpdateGatherData data,
      CancellationToken? cancellationToken = null)
    {
      return Client.MakeJsonRequestAsync(HttpMethod.Post,
        $"/users/{Client.UserId}/calls/{callId}/gather/{gatherId}", cancellationToken, null, data);
    }

    public Task PlayAudioAsync(string callId, PlayAudioData data, CancellationToken? cancellationToken = null)
    {
      return
        Client.MakeJsonRequestAsync(HttpMethod.Post,
          $"/users/{Client.UserId}/calls/{callId}/audio", cancellationToken, null, data);
    }
  }

  /// <summary>
  /// Additional methods for ICall
  /// </summary>
  public static class CallExtensions
  {
    /// <summary>
    /// Answer incoming call
    /// </summary>
    /// <param name="call">Instance of <see cref="ICall"/></param>
    /// <param name="callId">Id of call</param>
    /// <param name="cancellationToken">Optional token to cancel async operation</param>
    /// <returns>Task instance for async operation</returns>
    /// <example>
    /// <code>
    /// await call.AnswerAsync("callId");
    /// </code>
    /// </example>
    public static Task AnswerAsync(this ICall call, string callId, CancellationToken? cancellationToken = null)
    {
      return call.UpdateAsync(callId, new UpdateCallData
      {
        State = CallState.Active
      }, cancellationToken);
    }

    /// <summary>
    /// Reject incoming call
    /// </summary>
    /// <param name="call">Instance of <see cref="ICall"/></param>
    /// <param name="callId">Id of call</param>
    /// <param name="cancellationToken">Optional token to cancel async operation</param>
    /// <returns>Task instance for async operation</returns>
    /// <example>
    /// <code>
    /// await call.RejectAsync("callId");
    /// </code>
    /// </example>
    public static Task RejectAsync(this ICall call, string callId, CancellationToken? cancellationToken = null)
    {
      return call.UpdateAsync(callId, new UpdateCallData
      {
        State = CallState.Rejected
      }, cancellationToken);
    }

    /// <summary>
    /// Complete active call
    /// </summary>
    /// <param name="call">Instance of <see cref="ICall"/></param>
    /// <param name="callId">Id of call</param>
    /// <param name="cancellationToken">Optional token to cancel async operation</param>
    /// <returns>Task instance for async operation</returns>
    /// <example>
    /// <code>
    /// await call.HangupAsync("callId");
    /// </code>
    /// </example>
    public static Task HangupAsync(this ICall call, string callId, CancellationToken? cancellationToken = null)
    {
      return call.UpdateAsync(callId, new UpdateCallData
      {
        State = CallState.Completed
      }, cancellationToken);
    }

    /// <summary>
    /// Tune on (or tune off) call recording
    /// </summary>
    /// <param name="call">Instance of <see cref="ICall"/></param>
    /// <param name="callId">Id of call</param>
    /// <param name="enabled">Enable or disable call recording</param>
    /// <param name="cancellationToken">Optional token to cancel async operation</param>
    /// <returns>Task instance for async operation</returns>
    /// <example>
    /// <code>
    /// await call.TurnCallRecordingAsync("callId", true); // tune on call recording
    /// </code>
    /// </example>
    public static Task TurnCallRecordingAsync(this ICall call, string callId, bool enabled, CancellationToken? cancellationToken = null)
    {
      return call.UpdateAsync(callId, new UpdateCallData
      {
        RecordingEnabled = enabled 
      }, cancellationToken);
    }

    /// <summary>
    /// Transfer current call
    /// </summary>
    /// <param name="call">Instance of <see cref="ICall"/></param>
    /// <param name="callId">Id of call</param>
    /// <param name="to">Phone number or SIP address that the call is going to be transferred to.</param>
    /// <param name="callerId">This is the caller id that will be used when the call is transferred.</param>
    /// <param name="whisperAudio">Audio to be played to the caller that the call will be transferred to.</param>
    /// <param name="callbackUrl">The full server URL where the call events related to the Call will be sent to.</param>
    /// <param name="cancellationToken">Optional token to cancel async operation</param>
    /// <returns>Id of transfered call</returns>
    /// <example>
    /// <code>
    /// var transferedCallId = await call.TransferAsync("callId", "number"); 
    /// </code>
    /// </example>
    public static async Task<string> TransferAsync(this ICall call, string callId, string to, string callerId = null, WhisperAudio whisperAudio = null, string callbackUrl = null, CancellationToken ? cancellationToken = null)
    {
      var response = await call.UpdateAsync(callId, new UpdateCallData
      {
        State = CallState.Transferring,
        TransferTo = to,
        TransferCallerId = callerId,
        CallbackUrl = callbackUrl,
        WhisperAudio = whisperAudio
      }, cancellationToken);
      return response.Headers.Location.AbsolutePath.Split('/').Last();
    }
  }


  /// <summary>
  ///   Call information
  /// </summary>
  public class Call
  {
    /// <summary>
    ///   The unique identifier for the call.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Call direction
    /// </summary>
    public CallDirection Direction { get; set; }

    /// <summary>
    /// The phone number or SIP address that made the call. Phone numbers are in E.164 format (e.g. +15555555555) -or- SIP addresses (e.g. identify@domain.com).
    /// </summary>
    public string From { get; set; }

    /// <summary>
    /// The phone number or SIP address that received the call. Phone numbers are in E.164 format (e.g. +15555555555) -or- SIP addresses (e.g. identify@domain.com).
    /// </summary>
    public string To { get; set; }

    /// <summary>
    /// Call state
    /// </summary>
    public CallState State { get; set; }

    /// <summary>
    /// Date when the call was created. 
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// Date when the call was answered.
    /// </summary>
    public DateTime ActiveTime { get; set; }

    /// <summary>
    /// Date when the call ended.
    /// </summary>
    public DateTime EndTime { get; set; }

    /// <summary>
    /// Determine how long should the platform wait for call answer before timing out in seconds (milliseconds).
    /// </summary>
    public int CallTimeout { get; set; }

    /// <summary>
    /// The server URL where the call events related to the call will be sent.
    /// </summary>
    public string CallbackUrl { get; set; }

    /// <summary>
    /// Determine if the callback event should be sent via HTTP GET or HTTP POST.
    /// </summary>
    public CallbackHttpMethod CallbackHttpMethod { get; set; }

    /// <summary>
    /// Determine how long should the platform wait for callbackUrl's response before timing out (milliseconds).
    /// </summary>
    public int CallbackTimeout { get; set; }

    /// <summary>
    /// The server URL used to send the call events if the request to callbackUrl fails.
    /// </summary>
    public string FallbackUrl { get; set; }

    /// <summary>
    /// The seconds between ActiveTime and EndTime
    /// </summary>
    public int ChargeableDuration { get; set; }

    /// <summary>
    /// Id of the bridge where the call will be added
    /// </summary>
    public string BridgeId { get; set; }

    /// <summary>
    /// Id of the conference where the call will be added
    /// </summary>
    public string ConferenceId { get; set; }

    /// <summary>
    /// Audio to be played to the number that the call will be transfered to
    /// </summary>
    public WhisperAudio WhisperAudio { get; set; }

    /// <summary>
    /// This is the caller id that will be used when the call is transferred
    /// </summary>
    public string TransferCallerId { get; set; }

    /// <summary>
    /// Number that the call is going to be transferred to
    /// </summary>
    public string TransferTo { get; set; }

    /// <summary>
    /// Indicates if the call should be recorded after being created
    /// </summary>
    public bool RecordingEnabled { get; set; }

    /// <summary>
    /// The file format of the recorded call
    /// </summary>
    public string RecordingFileFormat { get; set; }

    /// <summary>
    /// Indicates the maximum duration of call recording in seconds
    /// </summary>
    public int RecordingMaxDuration { get; set; }

    /// <summary>
    /// Whether all the recordings for this call should be be automatically transcribed.
    /// </summary>
    public bool TranscriptionEnabled { get; set; }

    /// <summary>
    /// A string that will be included in the callback events of the call
    /// </summary>
    public string Tag { get; set; }

    /// <summary>
    /// Map of Sip headers prefixed by "X-". Up to 5 headers can be sent per call.
    /// </summary>
    public Dictionary<string, string> SipHeaders { get; set; }
  }

  /// <summary>
  /// Audio to be played to the number that the call will be transfered to
  /// </summary>
  public class WhisperAudio
  {
    /// <summary>
    /// The gender of the voice
    /// </summary>
    public Gender Gender { get; set; }

    /// <summary>
    /// The voice to speak the sentence
    /// </summary>
    public string Voice { get; set; }

    /// <summary>
    /// The sentence
    /// </summary>
    public string Sentence { get; set; }

    /// <summary>
    /// The locale used to get the accent of the voice used to synthesize the sentence
    /// </summary>
    public string Locale { get; set; }
  }

  /// <summary>
  /// Possible call directions
  /// </summary>
  public enum CallDirection
  {
    /// <summary>
    /// Incoming call
    /// </summary>
    In,

    /// <summary>
    /// Outgoing call
    /// </summary>
    Out
  }

  /// <summary>
  /// Possible call state
  /// </summary>
  public enum CallState
  {
    /// <summary>
    /// Call is created but not answered.
    /// </summary>
    Started,

    /// <summary>
    /// Incoming call was rejected.
    /// </summary>
    Rejected,

    /// <summary>
    /// Call is answered and isn't completed.
    /// </summary>
    Active,

    /// <summary>
    /// Call is finished.
    /// </summary>
    Completed,

    /// <summary>
    /// Transferring connects audio to a new outbound call.
    /// </summary>
    Transferring
  }

  /// <summary>
  ///   Query to get calls
  /// </summary>
  public class CallQuery
  {
    /// <summary>
    /// The id of the bridge for querying a list of calls history
    /// </summary>
    public string BridgeId { get; set; }

    /// <summary>
    /// The id of the conference for querying a list of calls history 
    /// </summary>
    public string ConferenceId { get; set; }

    /// <summary>
    /// The number to filter calls that came from (must be either an E.164 formated number, like +19195551212, or a valid SIP URI, like sip:someone@somewhere.com).
    /// </summary>
    public string From { get; set; }

    /// <summary>
    /// The number to filter calls that was called to (must be either an E.164 formated number, like +19195551212, or a valid SIP URI, like sip:someone@somewhere.com).
    /// </summary>
    public string To { get; set; }

    /// <summary>
    ///   Used for pagination to indicate the size of each page requested for querying a list of calls. If no value is
    ///   specified the default value is 25 (maximum value 1000).
    /// </summary>
    public int? Size { get; set; }

    /// <summary>
    /// How to sort the calls. If no value is specified the default value is Desc
    /// </summary>
    public SortOrder? SortOrder { get; set; }
  }

  /// <summary>
  /// Sort order types
  /// </summary>
  public enum SortOrder
  {
    /// <summary>
    /// Descending
    /// </summary>
    Desc,

    /// <summary>
    /// Ascending 
    /// </summary>
    Asc
  }

  /// <summary>
  ///   Parameters to create an call
  /// </summary>
  public class CreateCallData
  {
    /// <summary>
    /// The phone number or SIP address that made the call. Phone numbers are in E.164 format (e.g. +15555555555) -or- SIP addresses (e.g. identify@domain.com).
    /// </summary>
    public string From { get; set; }

    /// <summary>
    /// The phone number or SIP address that received the call. Phone numbers are in E.164 format (e.g. +15555555555) -or- SIP addresses (e.g. identify@domain.com).
    /// </summary>
    public string To { get; set; }

    /// <summary>
    /// Determine how long should the platform wait for call answer before timing out in seconds (milliseconds).
    /// </summary>
    public int? CallTimeout { get; set; }

    /// <summary>
    /// The server URL where the call events related to the call will be sent.
    /// </summary>
    public string CallbackUrl { get; set; }

    /// <summary>
    /// Determine if the callback event should be sent via HTTP GET or HTTP POST.
    /// </summary>
    public CallbackHttpMethod? CallbackHttpMethod { get; set; }

    /// <summary>
    /// Determine how long should the platform wait for callbackUrl's response before timing out (milliseconds).
    /// </summary>
    public int? CallbackTimeout { get; set; }

    /// <summary>
    /// The server URL used to send the call events if the request to callbackUrl fails.
    /// </summary>
    public string FallbackUrl { get; set; }


    /// <summary>
    /// Id of the bridge where the call will be added
    /// </summary>
    public string BridgeId { get; set; }

    /// <summary>
    /// Id of the conference where the call will be added
    /// </summary>
    public string ConferenceId { get; set; }


    /// <summary>
    /// Indicates if the call should be recorded after being created
    /// </summary>
    public bool? RecordingEnabled { get; set; }

    /// <summary>
    /// Indicates the maximum duration of call recording in seconds
    /// </summary>
    public int? RecordingMaxDuration { get; set; }

    /// <summary>
    /// Whether all the recordings for this call should be be automatically transcribed.
    /// </summary>
    public bool? TranscriptionEnabled { get; set; }

    /// <summary>
    /// A string that will be included in the callback events of the call
    /// </summary>
    public string Tag { get; set; }

    /// <summary>
    /// Map of Sip headers prefixed by "X-". Up to 5 headers can be sent per call.
    /// </summary>
    public Dictionary<string, string> SipHeaders { get; set; }
  }

  /// <summary>
  ///   Parameters of a call to change
  /// </summary>
  public class UpdateCallData
  {
    /// <summary>
    /// Call state
    /// </summary>
    public CallState State { get; set; }

    /// <summary>
    /// This is the caller id that will be used when the call is transferred
    /// </summary>
    public string TransferCallerId { get; set; }

    /// <summary>
    /// Number that the call is going to be transferred to
    /// </summary>
    public string TransferTo { get; set; }

    /// <summary>
    /// Indicates if the call should be recorded after being created
    /// </summary>
    public bool RecordingEnabled { get; set; }


    /// <summary>
    /// Audio to be played to the number that the call will be transfered to
    /// </summary>
    public WhisperAudio WhisperAudio { get; set; }

    /// <summary>
    /// The server URL where the call events related to the call will be sent.
    /// </summary>
    public string CallbackUrl { get; set; }
  }


  /// <summary>
  /// Parameters to create a gather
  /// </summary>
  public class CreateGatherData
  {
    /// <summary>
    /// The maximum number of digits to collect, not including terminating digits (maximum 30).
    /// </summary>
    public int? MaxDigits { get; set; }

    /// <summary>
    /// Stop gathering if a DTMF digit is not detected in this many seconds (default 5.0; maximum 30.0).
    /// </summary>
    public double? InterDigitTimeout { get; set; }

    /// <summary>
    /// Terminating digits
    /// </summary>
    public string TerminatingDigits { get; set; }

    /// <summary>
    ///  String that will be included with the response and events for this gather operation.
    /// </summary>
    public string Tag { get; set; }

    /// <summary>
    /// Gather prompt
    /// </summary>
    public GatherPrompt Prompt { get; set; }
  }

  /// <summary>
  /// Gather prompt
  /// </summary>
  public class GatherPrompt : PlayAudioData
  {
    /// <summary>
    /// Make the prompt (audio or sentence) bargeable (will be interrupted at first digit gathered). Default: true
    /// </summary>
    public bool? Bargeable { get; set; }
  }

  /// <summary>
  /// Possible gather states
  /// </summary>
  public enum CallGatherState
  {
    /// <summary>
    /// Completed
    /// </summary>
    Completed,

    /// <summary>
    /// Created
    /// </summary>
    Created
  }

  /// <summary>
  /// Parameters to change gather
  /// </summary>
  public class UpdateGatherData
  {
    /// <summary>
    /// Gather state
    /// </summary>
    public CallGatherState State { get; set; }
  }

  /// <summary>
  /// Gather of a call
  /// </summary>
  public class CallGather
  {
    /// <summary>
    /// The gather event unique id.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Gather state
    /// </summary>
    public CallGatherState State { get; set; }

    /// <summary>
    /// Gather state
    /// </summary>
    public CallGatherReason Reason { get; set; }

    /// <summary>
    /// Time of creation
    /// </summary>
    public DateTime CreatedTime { get; set; }

    /// <summary>
    /// Completed time
    /// </summary>
    public DateTime CompletedTime { get; set; }

    /// <summary>
    /// Url to linked call
    /// </summary>
    public string Call { get; set; }

    /// <summary>
    /// Id of linked call
    /// </summary>
    public string CallId => Call.Split('/').Last();

    /// <summary>
    /// Digits
    /// </summary>
    public string Digits { get; set; }
  }

  /// <summary>
  /// Possible gather reasons
  /// </summary>
  public enum CallGatherReason
  {
    /// <summary>
    /// The maximum number of digits specified for the gather command.
    /// </summary>
    MaxDigits,

    /// <summary>
    /// The digit specified in the gather command was entered.
    /// </summary>
    TerminatingDigit,

    /// <summary>
    /// A timeout occurred indicating the maximum amount of time to wait between digits, or before the first digit was pressed.
    /// </summary>
    InterDigitTimeout,

    /// <summary>
    /// Call was hung up thus terminating the gather
    /// </summary>
    HungUp
  }

  /// <summary>
  /// The event that occurred during the call
  /// </summary>
  public class CallEvent
  {
    /// <summary>
    /// The call event id
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// The name of the event.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The time the event occurred.
    /// </summary>
    public DateTime Time { get; set; }

    /// <summary>
    /// Optional event data
    /// </summary>
    public Dictionary<string, object> Data { get; set; }
  }

  /// <summary>
  /// Send DTMF data
  /// </summary>
  public class SendDtmfData
  {
    /// <summary>
    /// String containing the DTMF characters to be sent in a call.
    /// </summary>
    public string DtmfOut { get; set; }
  }

}
