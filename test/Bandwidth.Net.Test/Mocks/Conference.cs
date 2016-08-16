using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bandwidth.Net.Api;
using LightMock;

namespace Bandwidth.Net.Test.Mocks
{
  public class Conference : IConference
  {
    private readonly IInvocationContext<IConference> _context;

    public Conference(IInvocationContext<IConference> context)
    {
      _context = context;
    }


    public Task PlayAudioAsync(string id, PlayAudioData data, CancellationToken? cancellationToken = null)
    {
      throw new NotImplementedException();
    }

    public Task<ILazyInstance<Net.Api.Conference>> CreateAsync(CreateConferenceData data,
      CancellationToken? cancellationToken = null)
    {
      throw new NotImplementedException();
    }

    public Task<Net.Api.Conference> GetAsync(string conferenceId, CancellationToken? cancellationToken = null)
    {
      throw new NotImplementedException();
    }

    public Task UpdateAsync(string conferenceId, UpdateConferenceData data, CancellationToken? cancellationToken = null)
    {
      return _context.Invoke(m => m.UpdateAsync(conferenceId, data, cancellationToken));
    }

    public IEnumerable<ConferenceMember> GetMembers(string conferenceId, CancellationToken? cancellationToken = null)
    {
      throw new NotImplementedException();
    }

    public Task<ILazyInstance<ConferenceMember>> CreateMemberAsync(string conferenceId, CreateConferenceMemberData data,
      CancellationToken? cancellationToken = null)
    {
      throw new NotImplementedException();
    }

    public Task<ConferenceMember> GetMemberAsync(string conferenceId, string memberId,
      CancellationToken? cancellationToken = null)
    {
      throw new NotImplementedException();
    }

    public Task UpdateMemberAsync(string conferenceId, string memberId, UpdateConferenceMemberData data,
      CancellationToken? cancellationToken = null)
    {
      return _context.Invoke(m => m.UpdateMemberAsync(conferenceId, memberId, data, cancellationToken));
    }

    public Task PlayAudioToMemberAsync(string conferenceId, string memberId, PlayAudioData data,
      CancellationToken? cancellationToken = null)
    {
      return _context.Invoke(m => m.PlayAudioToMemberAsync(conferenceId, memberId, data, cancellationToken));
    }
  }
}
