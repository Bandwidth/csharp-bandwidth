using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bandwidth.Net.Api
{
  /// <summary>
  ///   Message queue allows to send messages with given rate (all messages are buffered in memory)
  /// </summary>
  public class MessageQueue
  {
    internal readonly IMessage Api;
    internal readonly string From;
    internal readonly TimeSpan Interval;
    internal readonly List<MessageData> MessagesQueue;
    internal readonly int MessagesToSendPerTime;
    internal readonly List<SendMessageResult> Results;
    internal IDelay Delay;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="api">instance of <see cref="IMessage" /> to send messages</param>
    /// <param name="from">Phone number to send messages</param>
    /// <param name="rate">Max rate of sending messages (messages per second)</param>
    /// <param name="cancellationToken">Optional token to cancel sending messages task</param>
    /// <example>
    /// <code>
    /// var queue = new MessageQueue(cleint.Message, "+1234567890", 2); // 2 messages per second
    /// </code>
    /// </example>
    public MessageQueue(IMessage api, string from, double rate = 1, CancellationToken? cancellationToken = null)
    {
      if (api == null) throw new ArgumentNullException(nameof(api));
      if (string.IsNullOrEmpty(@from)) throw new ArgumentNullException(nameof(@from));
      if (rate <= 0) throw new ArgumentOutOfRangeException(nameof(rate), $"{nameof(rate)} should be more than 0");
      Api = api;
      From = @from;
      if (rate >= 1)
      {
        MessagesToSendPerTime = (int) Math.Round(rate);
        Interval = TimeSpan.FromSeconds(1);
        // send N messages per second
      }
      else
      {
        Interval = TimeSpan.FromSeconds(1/rate);
        MessagesToSendPerTime = 1;
        // send 1 messages per given interval (more 1 second)
      }
      MessagesQueue = new List<MessageData>();
      Results = new List<SendMessageResult>();
      Task.Run(() => StartSendMessages(cancellationToken ?? CancellationToken.None));
        // Start sending of messages in background thread
    }

    /// <summary>
    ///   Add one or more messages to queue
    /// </summary>
    /// <param name="data">Messages to send</param>
    /// <example>
    /// <code>
    /// queue.Queue(new MessageData{To = "+1234567891", Text = "text1"});
    /// // or
    /// queue.Queue(new MessageData{To = "+1234567891", Text = "text1"}, new MessageData{To = "+1234567892", Text = "text2"});
    /// </code>
    /// </example>
    public void Queue(params MessageData[] data)
    {
      Queue((IEnumerable<MessageData>) data);
    }

    /// <summary>
    ///   Add one or more messages to queue
    /// </summary>
    /// <param name="data">Messages to send</param>
    /// <example>
    /// <code>
    /// var list = new List&lt;MessageData&gt;(new[] {new MessageData{To = "+1234567891", Text = "text1"}, new MessageData{To = "+1234567892", Text = "text2"}});
    /// queue.Queue(list);
    /// </code>
    /// </example>
    public void Queue(IEnumerable<MessageData> data)
    {
      var list = data.ToList();
      foreach (var messageData in list)
      {
        messageData.From = From;
      }
      lock (MessagesQueue)
      {
        MessagesQueue.AddRange(list);
      }
    }

    /// <summary>
    ///   Get results of sent messages
    /// </summary>
    /// <returns>List with results</returns>
    /// <example>
    /// <code>
    /// var results = queue.GetResults();
    /// </code>
    /// </example>
    public SendMessageResult[] GetResults()
    {
      lock (Results)
      {
        return Results.ToArray();
      }
    }


    internal async Task StartSendMessages(CancellationToken cancellationToken)
    {
      while (!cancellationToken.IsCancellationRequested)
      {
        MessageData[] messages;
        var startTime = DateTime.Now;
        lock (MessagesQueue)
        {
          messages = MessagesQueue.Take(MessagesToSendPerTime).ToArray();
          MessagesQueue.RemoveRange(0, messages.Length);
        }
        var delay = Delay ?? new DelayInternal();
        if (messages.Length > 0)
        {
          var results = await Api.SendAsync(messages, cancellationToken);
          var rateLimitResults =
            results.Where(
              r => r.Error != null && (r.Error.Code == "message-rate-limit" || r.Error.Code == "multi-message-limit"))
              .ToList();
          var rateLimitMessages = rateLimitResults.Select(r => r.Message).ToList();
          if (rateLimitMessages.Count > 0)
          {
            Queue(rateLimitMessages); //repeat sending of rate limited messages;
            await delay.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            // wait a bit to avoid rate limit error for next messages
          }
          var otherResults = results.Where(r => !rateLimitResults.Contains(r));
          lock (Results)
          {
            Results.AddRange(otherResults);
          }
        }
        var executionTime = DateTime.Now - startTime;
        var timeToWait = Interval - executionTime;
        if (timeToWait.TotalMilliseconds > 0)
        {
          await delay.Delay(timeToWait, cancellationToken);
        }
      }
    }
  }

  internal interface IDelay
  {
    Task Delay(TimeSpan interval, CancellationToken cancellationToken);
  }

  internal class DelayInternal : IDelay
  {
    public Task Delay(TimeSpan interval, CancellationToken cancellationToken)
    {
      return Task.Delay(interval, cancellationToken);
    }
  }
}