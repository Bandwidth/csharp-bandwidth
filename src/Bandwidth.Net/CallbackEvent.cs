using System;
using System.Net.Http;
using System.Threading.Tasks;
using Bandwidth.Net.Api;
using Newtonsoft.Json;

namespace Bandwidth.Net
{
  /// <summary>
  ///   Catapult Api callback event
  /// </summary>
  /// <example>
  /// <code>
  /// var callbackEvent = CallbackEvent.CreateFromJson("{\"eventType\": \"sms\"}");
  /// switch(callbackEvent.EventType)
  /// {
  ///   case CallbackEventType.Sms:
  ///     Console.WriteLine($"Sms {callbackEvent.From} -> {callbackEvent.To}: {callbackEvent.Text}");
  ///     break;
  /// }
  /// </code>
  /// </example>
  public class CallbackEvent
  {
    /// <summary>
    ///   Event type
    /// </summary>
    public CallbackEventType EventType { get; set; }

    /// <summary>
    ///   Message direction
    /// </summary>
    public MessageDirection Direction { get; set; }

    /// <summary>
    ///   From
    /// </summary>
    public string From { get; set; }

    /// <summary>
    ///   To
    /// </summary>
    public string To { get; set; }

    /// <summary>
    ///   Id of message
    /// </summary>
    public string MessageId { get; set; }

    /// <summary>
    ///   Url to message
    /// </summary>
    public string MessageUri { get; set; }

    /// <summary>
    ///   Text of message or transcription
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    ///   Application Id
    /// </summary>
    public string ApplicationId { get; set; }

    /// <summary>
    ///   Time
    /// </summary>
    public DateTime Time { get; set; }

    /// <summary>
    ///   State
    /// </summary>
    public CallbackEventState State { get; set; }

    /// <summary>
    ///   Delivery state of message
    /// </summary>
    public MessageDeliveryState DeliveryState { get; set; }

    /// <summary>
    ///   Delivery code of message
    /// </summary>
    public int DeliveryCode { get; set; }

    /// <summary>
    ///   Delivery description of message
    /// </summary>
    public string DeliveryDescription { get; set; }

    /// <summary>
    ///   Urls to attached media files to message
    /// </summary>
    public string[] Media { get; set; }

    /// <summary>
    ///   Call state
    /// </summary>
    public CallState CallState { get; set; }

    /// <summary>
    ///   Id of call
    /// </summary>
    public string CallId { get; set; }

    /// <summary>
    ///   Url to the call
    /// </summary>
    public string CallUri { get; set; }

    /// <summary>
    ///   Tag
    /// </summary>
    public string Tag { get; set; }

    /// <summary>
    ///   Status
    /// </summary>
    public CallbackEventStatus Status { get; set; }

    /// <summary>
    ///   If of conference
    /// </summary>
    public string ConferenceId { get; set; }

    /// <summary>
    ///   Url to the conference
    /// </summary>
    public string ConferenceUri { get; set; }

    /// <summary>
    ///   Created time of the conference
    /// </summary>
    public DateTime CreatedTime { get; set; }

    /// <summary>
    ///   Completed time of the conference
    /// </summary>
    public DateTime CompletedTime { get; set; }

    /// <summary>
    ///   Active members count of the conference
    /// </summary>
    public int ActiveMembers { get; set; }

    /// <summary>
    ///   Id of conference member
    /// </summary>
    public string MemberId { get; set; }

    /// <summary>
    ///   Url to the conference member
    /// </summary>
    public string MemberUri { get; set; }

    /// <summary>
    ///   Members is on hold in conference and can not hear or speak.
    /// </summary>
    public bool Hold { get; set; }

    /// <summary>
    ///   Members audio is muted in conference.
    /// </summary>
    public bool Mute { get; set; }

    /// <summary>
    ///   The digits pressed
    /// </summary>
    public string Digits { get; set; }

    /// <summary>
    ///   The digit pressed (for DTMF event)
    /// </summary>
    public string DtmfDigit { get; set; }


    /// <summary>
    ///   Id of gather
    /// </summary>
    public string GatherId { get; set; }

    /// <summary>
    ///   Reason
    /// </summary>
    public CallbackEventReason Reason { get; set; }

    /// <summary>
    ///   Cause of call termination
    /// </summary>
    public string Cause { get; set; }

    /// <summary>
    ///   Id of recording
    /// </summary>
    public string RecordingId { get; set; }

    /// <summary>
    ///   Url to the recording
    /// </summary>
    public string RecordingUri { get; set; }

    /// <summary>
    ///   Created time of the recording
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    ///   Completed time of the recording
    /// </summary>
    public DateTime EndTime { get; set; }


    /// <summary>
    ///   Id of transcription
    /// </summary>
    public string TranscriptionId { get; set; }

    /// <summary>
    ///   Url to the transcription
    /// </summary>
    public string TranscriptionUri { get; set; }

    /// <summary>
    ///   Total character count of text.
    /// </summary>
    public int TextSize { get; set; }

    /// <summary>
    ///   The full URL of the entire text content of the transcription.
    /// </summary>
    public string TextUrl { get; set; }

    /// <summary>
    ///   Create instance from JSON string
    /// </summary>
    /// <param name="json">JSON string with callback event data</param>
    /// <returns>New instance of CallbackEvent</returns>
    public static CallbackEvent CreateFromJson(string json)
      => JsonConvert.DeserializeObject<CallbackEvent>(json, JsonHelpers.GetSerializerSettings());
  }

  /// <summary>
  ///   Possible event types
  /// </summary>
  public enum CallbackEventType
  {
    /// <summary>
    ///   Unknown type
    /// </summary>
    Unknown,

    /// <summary>
    ///   Sms
    /// </summary>
    Sms,

    /// <summary>
    ///   Mms
    /// </summary>
    Mms,

    /// <summary>
    ///   Answer call
    /// </summary>
    Answer,

    /// <summary>
    ///   Playback uadio
    /// </summary>
    Playback,

    /// <summary>
    ///   Call timeout
    /// </summary>
    Timeout,

    /// <summary>
    ///   Conference
    /// </summary>
    Conference,

    /// <summary>
    ///   Play audio to the conference
    /// </summary>
    ConferencePlayback,

    /// <summary>
    ///   Conference member
    /// </summary>
    ConferenceMember,

    /// <summary>
    ///   Speak text to the conference
    /// </summary>
    ConferenceSpeak,

    /// <summary>
    ///   DTMF
    /// </summary>
    Dtmf,

    /// <summary>
    ///   Gather
    /// </summary>
    Gather,

    /// <summary>
    ///   Incoming call
    /// </summary>
    Incomingcall,

    /// <summary>
    ///   Call completed
    /// </summary>
    Hangup,

    /// <summary>
    ///   Recording
    /// </summary>
    Recording,

    /// <summary>
    ///   Speak text
    /// </summary>
    Speak,

    /// <summary>
    ///   Transcription
    /// </summary>
    Transcription, 

    /// <summary>
    ///   Transfer complete
    /// </summary>
    Transfercomplete,

    /// <summary>
    ///   Redirect
    /// </summary>
    Redirect
  }

  /// <summary>
  ///   Possible statuses of calback events
  /// </summary>
  public enum CallbackEventStatus
  {
    /// <summary>
    ///   Started
    /// </summary>
    Started,

    /// <summary>
    ///   Done
    /// </summary>
    Done,

    /// <summary>
    ///   Created
    /// </summary>
    Created,

    /// <summary>
    ///   Completed
    /// </summary>
    Completed,

    /// <summary>
    ///   Complete
    /// </summary>
    Complete,

    /// <summary>
    ///   Error
    /// </summary>
    Error
  }

  /// <summary>
  ///   Callback event states
  /// </summary>
  public enum CallbackEventState
  {
    /// <summary>
    ///   Active
    /// </summary>
    Active,

    /// <summary>
    ///   Completed
    /// </summary>
    Completed,

    /// <summary>
    ///   Received
    /// </summary>
    Received,

    /// <summary>
    ///   Queued
    /// </summary>
    Queued,

    /// <summary>
    ///   Sending
    /// </summary>
    Sending,

    /// <summary>
    ///   Sent
    /// </summary>
    Sent,

    /// <summary>
    ///   Complete
    /// </summary>
    Complete,

    /// <summary>
    ///   Error
    /// </summary>
    Error,

    /// <summary>
    ///   Start of playback
    /// </summary>
    PlaybackStart,

    /// <summary>
    ///   End of playback
    /// </summary>
    PlaybackStop
  }

  /// <summary>
  ///   Possible reasons of calback events
  /// </summary>
  public enum CallbackEventReason
  {
    /// <summary>
    ///   Max digits reached
    /// </summary>
    MaxDigits,

    /// <summary>
    ///   Terminating digit
    /// </summary>
    TerminatingDigit,

    /// <summary>
    ///   Interdigit timeout
    /// </summary>
    InterDigitTimeout,

    /// <summary>
    ///   Hung up
    /// </summary>
    HungUp,

    /// <summary>
    ///  Cancelled
    /// </summary>
    Cancelled
  }

  /// <summary>
  ///   Helper for HttpContent to parse CallbackEvent
  /// </summary>
  public static class CallbackEventHelpers
  {
    /// <summary>
    ///   Read CallbackEvent instance from http content
    /// </summary>
    /// <param name="content">Content</param>
    /// <returns>Callback event data or null if response content is not json</returns>
    /// <example>
    /// <code>
    /// var callbackEvent = await request.Content.ReadAsCallbackEventAsync(); // response is instance of HttpRequestMessage
    /// switch(callbackEvent.EventType)
    /// {
    ///   case CallbackEventType.Sms:
    ///     Console.WriteLine($"Sms {callbackEvent.From} -> {callbackEvent.To}: {callbackEvent.Text}");
    ///     break;
    /// }
    /// </code>
    /// </example>
    public static Task<CallbackEvent> ReadAsCallbackEventAsync(this HttpContent content)
      => content.ReadAsJsonAsync<CallbackEvent>();
  }
}
