using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bandwidth.Net.Api;
using Xunit;

namespace Bandwidth.Net.Test.Mocks
{
  public class Message : IMessage
  {
    private readonly MessageData[] _estimatedData;
    private readonly SendMessageResult[] _results;

    public int SendAsyncCallCount {get; private set;}

    public Message(MessageData[] estimatedData, SendMessageResult[] results)
    {
      _estimatedData = estimatedData;
      _results = results;
    }

    public IEnumerable<Net.Api.Message> List(MessageQuery query = null, CancellationToken? cancellationToken = null)
    {
      throw new NotImplementedException();
    }

    public Task<ILazyInstance<Net.Api.Message>> SendAsync(MessageData data, CancellationToken? cancellationToken = null)
    {
      throw new NotImplementedException();
    }

    public Task<SendMessageResult[]> SendAsync(MessageData[] data, CancellationToken? cancellationToken = null)
    {
      for (var i = 0; i < data.Length; i++)
      {
        var message = data[i];
        var estimatedMessage = _estimatedData[i];
        Assert.Equal(estimatedMessage.To, message.To);
        Assert.Equal(estimatedMessage.Text, message.Text);
      }
      SendAsyncCallCount++;
      return Task.FromResult(_results);
    }

    public Task<Net.Api.Message> GetAsync(string messageId, CancellationToken? cancellationToken = null)
    {
      throw new NotImplementedException();
    }
  }
}
