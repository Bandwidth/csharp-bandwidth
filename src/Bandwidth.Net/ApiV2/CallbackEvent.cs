using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Bandwidth.Net.Api;
using Newtonsoft.Json;

namespace Bandwidth.Net.ApiV2
{
  /// <summary>
  ///   Catapult Api callback event
  /// </summary>
  /// <example>
  /// <code>
  /// var callbackEvent = CallbackEvent.CreateFromJson("{\"type\": \"message-received\"}");
  /// switch(callbackEvent.Type)
  /// {
  ///   case CallbackEventType.MessageReceived:
  ///     Console.WriteLine($"Sms {callbackEvent.Message.From} -> {string.Join(", ", callbackEvent.Message.To)}: {callbackEvent.Message.Text}");
  ///     break;
  /// }
  /// </code>
  /// </example>
  public class CallbackEvent
  {
    /// <summary>
    ///   Event type
    /// </summary>
    public CallbackEventType Type { get; set; }

    /// <summary>
    ///   Event time
    /// </summary>
    public DateTime Time { get; set; }

    /// <summary>
    ///   Event description
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    ///   Event target phone number
    /// </summary>
    public string To { get; set; }

    /// <summary>
    ///   Error Code
    /// </summary>
    public int ErrorCode { get; set; }

    /// <summary>
    ///   Message data
    /// </summary>
    public IncomingMessage Message { get; set; }


    /// <summary>
    ///   Phone numbers for answer
    /// </summary>
    public string ReplyTo { get; set; }
    
    
    /// <summary>
    /// Number of segments the message sent as
    /// </summary>
    public int SegmentCount { get; set; }


    /// <summary>
    ///   Parse callback eevent data from JSON
    /// </summary>
    public static CallbackEvent[] CreateFromJson(string json)
      => JsonConvert.DeserializeObject<CallbackEvent[]>(json, JsonHelpers.GetSerializerSettings()).Select(SetReplyTo).ToArray();

   private static CallbackEvent SetReplyTo(CallbackEvent callbackEvent)
   {
      if (callbackEvent.Message != null) 
      {
        if (string.IsNullOrEmpty(callbackEvent.To))
        {
          callbackEvent.Message.ReplyTo = new[] { callbackEvent.Message.From };
        }
        else
        {
          callbackEvent.Message.ReplyTo = callbackEvent.Message.To
            .Where(n => n != callbackEvent.To)
            .Union(new[] { callbackEvent.Message.From }).ToArray();
        }
      }
      return callbackEvent;
   }   

  }

  /// <summary>
  ///   Incoming message
  /// </summary>
  public class IncomingMessage: Message {
    /// <summary>
    ///   Phone numbers for answer
    /// </summary>
    public string[] ReplyTo { get; internal set; }  
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
    ///   Message received
    /// </summary>
    MessageReceived,

	/// <summary>
    ///   Message received
    /// </summary>
    MessageFailed,

    /// <summary>
    ///   Message sent
    /// </summary>
    MessageSent,

    /// <summary>
    ///   Message delivered
    /// </summary>
    MessageDelivered,

    /// <summary>
    ///   Message rejected forbidden country
    /// </summary>
    MessageRejectedForbiddenCountry,

    /// <summary>
    ///   Message sending
    /// </summary>
    MessageSending
  }
}
