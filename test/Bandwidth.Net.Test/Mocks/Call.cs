using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Bandwidth.Net.Api;
using LightMock;

namespace Bandwidth.Net.Test.Mocks
{
    public class Call: ICall
    {
      private readonly IInvocationContext<ICall> _context;

      public Call(IInvocationContext<ICall> context)
      {
        _context = context;
      }

      public Task PlayAudioAsync(string id, PlayAudioData data, CancellationToken? cancellationToken = null)
      {
        throw new System.NotImplementedException();
      }

      public IEnumerable<Net.Api.Call> List(CallQuery query = null, CancellationToken? cancellationToken = null)
      {
        throw new System.NotImplementedException();
      }

      public Task<ILazyInstance<Net.Api.Call>> CreateAsync(CreateCallData data, CancellationToken? cancellationToken = null)
      {
        throw new System.NotImplementedException();
      }

      public Task<Net.Api.Call> GetAsync(string callId, CancellationToken? cancellationToken = null)
      {
        throw new System.NotImplementedException();
      }

      public Task<HttpResponseMessage> UpdateAsync(string callId, UpdateCallData data, CancellationToken? cancellationToken = null)
      {
        return _context.Invoke(m => m.UpdateAsync(callId, data, cancellationToken));
      }

      public Task SendDtmfAsync(string callId, SendDtmfData data, CancellationToken? cancellationToken = null)
      {
        throw new System.NotImplementedException();
      }

      public IEnumerable<CallEvent> GetEvents(string callId, CancellationToken? cancellationToken = null)
      {
        throw new System.NotImplementedException();
      }

      public Task<CallEvent> GetEventAsync(string callId, string eventId, CancellationToken? cancellationToken = null)
      {
        throw new System.NotImplementedException();
      }

      public IEnumerable<Recording> GetRecordings(string callId, CancellationToken? cancellationToken = null)
      {
        throw new System.NotImplementedException();
      }

      public IEnumerable<Transcription> GetTranscriptions(string callId, CancellationToken? cancellationToken = null)
      {
        throw new System.NotImplementedException();
      }

      public Task<ILazyInstance<CallGather>> CreateGatherAsync(string callId, CreateGatherData data, CancellationToken? cancellationToken = null)
      {
        throw new System.NotImplementedException();
      }

      public Task<CallGather> GetGatherAsync(string callId, string gatherId, CancellationToken? cancellationToken = null)
      {
        throw new System.NotImplementedException();
      }

      public Task UpdateGatherAsync(string callId, string gatherId, UpdateGatherData data,
        CancellationToken? cancellationToken = null)
      {
        throw new System.NotImplementedException();
      }
    }
}
