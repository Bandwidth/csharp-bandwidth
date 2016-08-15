using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bandwidth.Net.Api
{
  /// <summary>
  ///   Access to Conference Api
  /// </summary>
  public interface IConference
  {

    /// <summary>
    ///   Create a conference
    /// </summary>
    /// <param name="data">Parameters of new conference</param>
    /// <param name="cancellationToken">Optional token to cancel async operation</param>
    /// <returns>Instance of created conference</returns>
    /// <example>
    ///   <code>
    /// var conference = await client.Conference.CreateAsync(new CreateConferenceData{ From = "+1234567890"});
    /// </code>
    /// </example>
    Task<ILazyInstance<Conference>> CreateAsync(CreateConferenceData data, CancellationToken? cancellationToken = null);


    /// <summary>
    ///   Get information about a conference
    /// </summary>
    /// <param name="conferenceId">Id of conference to get</param>
    /// <param name="cancellationToken">Optional token to cancel async operation</param>
    /// <returns>Task with <see cref="Conference" />Conference instance</returns>
    /// <example>
    ///   <code>
    /// var conference = await client.Conference.GetAsync("conferenceId");
    /// </code>
    /// </example>
    Task<Conference> GetAsync(string conferenceId, CancellationToken? cancellationToken = null);

    /// <summary>
    ///   Change the conference properties and/or status
    /// </summary>
    /// <param name="conferenceId">Id of conference to change</param>
    /// <param name="data">Changed data</param>
    /// <param name="cancellationToken">Optional token to cancel async operation</param>
    /// <returns>Task instance for async operation</returns>
    /// <example>
    ///   <code>
    /// await client.Conference.UpdateAsync("conferenceId", new UpdateConferenceData {Mute = true});
    /// </code>
    /// </example>
    Task UpdateAsync(string conferenceId, UpdateConferenceData data, CancellationToken? cancellationToken = null);

    /// <summary>
    /// List all members from a conference
    /// </summary>
    /// <param name="conferenceId">Id of conference to get members</param>
    /// <param name="cancellationToken">Optional token to cancel async operation</param>
    /// <returns>Collection with <see cref="ConferenceMember" /> instances</returns>
    /// <example>
    /// <code>
    /// var members = client.Conference.GetMembers("conferenceId");
    /// </code>
    /// </example>
    IEnumerable<ConferenceMember> GetMembers(string conferenceId, CancellationToken? cancellationToken = null);

    /// <summary>
    /// Add a member to a conference.
    /// </summary>
    /// <param name="conferenceId">Id of conference to add member</param>
    /// <param name="data">Data for creation of new member</param>
    /// <param name="cancellationToken">Optional token to cancel async operation</param>
    /// <returns>Created member</returns>
    /// <example>
    /// <code>
    /// var member = await client.Conference.CreateMemberAsync("conferenceId", new CreateConferenceMemberData{From = "+1234567980"});
    /// </code>
    /// </example>
    Task<ILazyInstance<ConferenceMember>> CreateMemberAsync(string conferenceId, CreateConferenceMemberData data, CancellationToken? cancellationToken = null);

    /// <summary>
    /// Retrieve properties for a single conference member
    /// </summary>
    /// <param name="conferenceId">Id of the conference</param>
    /// <param name="memberId">Id of the member to get data</param>
    /// <param name="cancellationToken">Optional token to cancel async operation</param>
    /// <returns>Conference member data</returns>
    /// <example>
    /// <code>
    /// var member = await client.Conference.GetMemberAsync("conferenceId", "memberId");
    /// </code>
    /// </example>
    Task<ConferenceMember> GetMemberAsync(string conferenceId, string memberId, CancellationToken? cancellationToken = null);

    /// <summary>
    /// Update a conference member (remove, mute, hold)
    /// </summary>
    /// <param name="conferenceId">Id of the conference</param>
    /// <param name="memberId">Id of the member to update</param>
    /// <param name="data">Changed data</param>
    /// <param name="cancellationToken">Optional token to cancel async operation</param>
    /// <returns>Task instance for async operation</returns>
    /// <example>
    /// <code>
    /// await client.Conference.UpdateMemberAsync("conferenceId", "memberId", UpdateConferenceMemberData{Mute = true});
    /// </code>
    /// </example>
    Task UpdateMemberAsync(string conferenceId, string memberId, UpdateConferenceMemberData data, CancellationToken? cancellationToken = null);

  }
  
  internal class ConferenceApi : ApiBase, IConference
  {
    public Task<ILazyInstance<Conference>> CreateAsync(CreateConferenceData data, CancellationToken? cancellationToken = null)
    {
      throw new NotImplementedException();
    }

    public Task<Conference> GetAsync(string conferenceId, CancellationToken? cancellationToken = null)
    {
      throw new NotImplementedException();
    }

    public Task UpdateAsync(string conferenceId, UpdateConferenceData data, CancellationToken? cancellationToken = null)
    {
      throw new NotImplementedException();
    }

    public IEnumerable<ConferenceMember> GetMembers(string conferenceId, CancellationToken? cancellationToken = null)
    {
      throw new NotImplementedException();
    }

    public Task<ILazyInstance<ConferenceMember>> CreateMemberAsync(string conferenceId, CreateConferenceMemberData data, CancellationToken? cancellationToken = null)
    {
      throw new NotImplementedException();
    }

    public Task<ConferenceMember> GetMemberAsync(string conferenceId, string memberId, CancellationToken? cancellationToken = null)
    {
      throw new NotImplementedException();
    }

    public Task UpdateMemberAsync(string conferenceId, string memberId, UpdateConferenceMemberData data,
      CancellationToken? cancellationToken = null)
    {
      throw new NotImplementedException();
    }
  }


  /// <summary>
  ///   Conference information
  /// </summary>
  public class Conference
  {
    /// <summary>
    ///   The unique identifier for the conference.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// The phone number that will host the conference.
    /// </summary>
    public string From { get; set; }

    /// <summary>
    /// Conference state
    /// </summary>
    public ConferenceState State { get; set; }

    /// <summary>
    /// The time that the Conference was created 
    /// </summary>
    public DateTime CreatedTime { get; set; }

    /// <summary>
    /// The time that the Conference was completed 
    /// </summary>
    public DateTime CompletedTime { get; set; }

    /// <summary>
    /// The number of active conference members.
    /// </summary>
    public int ActiveMembers { get; set; }

    /// <summary>
    /// If "true", all member can't hear or speak in the conference. If "false", all members can hear and speak in the conference (unless set at the member level).
    /// </summary>
    public bool Hold { get; set; }

    /// <summary>
    /// If "true", all member can't speak in the conference. If "false", all members can speak in the conference (unless set at the member level).
    /// </summary>
    public bool Mute { get; set; }

    /// <summary>
    /// The complete URL where the events related to the Conference will be sent to.
    /// </summary>
    public string CallbackUrl { get; set; }

    /// <summary>
    /// Determine if the callback event should be sent via HTTP GET or HTTP POST.
    /// </summary>
    public CallbackHttpMethod CallbackHttpMethod { get; set; }

    /// <summary>
    /// Determine how long should the platform wait for callbackUrl's response before timing out in milliseconds.
    /// </summary>
    public int CallbackTimeout { get; set; }

    /// <summary>
    /// The URL used to send the callback event if the request to callbackUrl fails.
    /// </summary>
    public string FallbackUrl { get; set; }

    /// <summary>
    /// A string that will be included in the callback events of the conference.
    /// </summary>
    public string Tag { get; set; }

  }

  /// <summary>
  /// Possible conference states
  /// </summary>
  public enum ConferenceState
  {
    /// <summary>
    /// Conference was created and has no members.
    /// </summary>
    Created,

    /// <summary>
    /// Conference was created and has one or more ACTIVE members. As soon as the first member is added to a conference, the state is changed to active.
    /// </summary>
    Active,

    /// <summary>
    /// The conference was completed. Once the conference is completed, It can not be used anymore.
    /// </summary>
    Completed

  }

  /// <summary>
  /// Data to create a conference
  /// </summary>
  public class CreateConferenceData
  {
    /// <summary>
    /// The phone number that will host the conference.
    /// </summary>
    public string From { get; set; }
    
    /// <summary>
    /// The complete URL where the events related to the Conference will be sent to.
    /// </summary>
    public string CallbackUrl { get; set; }

    /// <summary>
    /// Determine if the callback event should be sent via HTTP GET or HTTP POST.
    /// </summary>
    public CallbackHttpMethod? CallbackHttpMethod { get; set; }

    /// <summary>
    /// Determine how long should the platform wait for callbackUrl's response before timing out in milliseconds.
    /// </summary>
    public int? CallbackTimeout { get; set; }

    /// <summary>
    /// The URL used to send the callback event if the request to callbackUrl fails.
    /// </summary>
    public string FallbackUrl { get; set; }

    /// <summary>
    /// A string that will be included in the callback events of the conference.
    /// </summary>
    public string Tag { get; set; }

  }

  /// <summary>
  /// Conference member
  /// </summary>
  public class ConferenceMember
  {
    /// <summary>
    ///   The unique identifier for the member.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Conference state
    /// </summary>
    public ConferenceMemberState State { get; set; }

    /// <summary>
    /// If "true", member can't hear the conference. If "false", the member can hear the conference.
    /// </summary>
    public bool Hold { get; set; }

    /// <summary>
    /// If "true", the member can't speak in the conference, but can hear audio. If "false", the member can speak in the conference.
    /// </summary>
    public bool Mute { get; set; }

    /// <summary>
    /// If "true", will play a tone when the member joins the conference. If "false", no tone is played when the member joins the conference.
    /// </summary>
    public bool JoinTone { get; set; }

    /// <summary>
    /// If "true", will play a tone when the member leaves the conference. If "false", no tone is played when the member leaves the conference.
    /// </summary>
    public bool LeavingTone { get; set; }

    /// <summary>
    /// Date and time when the member was added to the conference
    /// </summary>
    public DateTime AddedTime { get; set; }

    /// <summary>
    /// Date and time when the member was removed to the conference 
    /// </summary>
    public DateTime RemovedTime { get; set; }

    /// <summary>
    /// The URL of the call resource for this member.
    /// </summary>
    public string Call { get; set; }

    /// <summary>
    /// The id of the call for this member.
    /// </summary>
    public string CallId => Call.Split('/').Last();
  }


  /// <summary>
  /// Possible conference member states
  /// </summary>
  public enum ConferenceMemberState
  {
    /// <summary>
    /// Active
    /// </summary>
    Active,

    /// <summary>
    /// Completed
    /// </summary>
    Completed
  }

  /// <summary>
  /// Data to add member to the conference
  /// </summary>
  public class CreateConferenceMemberData
  {
    /// <summary>
    /// If "true", member can't hear the conference. If "false", the member can hear the conference.
    /// </summary>
    public bool Hold { get; set; }

    /// <summary>
    /// If "true", the member can't speak in the conference, but can hear audio. If "false", the member can speak in the conference.
    /// </summary>
    public bool Mute { get; set; }

    /// <summary>
    /// If "true", will play a tone when the member joins the conference. If "false", no tone is played when the member joins the conference.
    /// </summary>
    public bool JoinTone { get; set; }

    /// <summary>
    /// If "true", will play a tone when the member leaves the conference. If "false", no tone is played when the member leaves the conference.
    /// </summary>
    public bool LeavingTone { get; set; }

    /// <summary>
    /// The callId must refer to an active call that was created using this conferenceId.
    /// </summary>
    public string CallId { get; set; }
  }

  /// <summary>
  /// Changable conference data 
  /// </summary>
  public class UpdateConferenceData
  {

    /// <summary>
    /// Conference state
    /// </summary>
    public ConferenceState State { get; set; }
    
    /// <summary>
    /// If "true", all member can't hear or speak in the conference. If "false", all members can hear and speak in the conference (unless set at the member level).
    /// </summary>
    public bool Hold { get; set; }

    /// <summary>
    /// If "true", all member can't speak in the conference. If "false", all members can speak in the conference (unless set at the member level).
    /// </summary>
    public bool Mute { get; set; }

    /// <summary>
    /// The complete URL where the events related to the Conference will be sent to.
    /// </summary>
    public string CallbackUrl { get; set; }

    /// <summary>
    /// Determine if the callback event should be sent via HTTP GET or HTTP POST.
    /// </summary>
    public CallbackHttpMethod CallbackHttpMethod { get; set; }

    /// <summary>
    /// Determine how long should the platform wait for callbackUrl's response before timing out in milliseconds.
    /// </summary>
    public int CallbackTimeout { get; set; }

    /// <summary>
    /// The URL used to send the callback event if the request to callbackUrl fails.
    /// </summary>
    public string FallbackUrl { get; set; }

    /// <summary>
    /// A string that will be included in the callback events of the conference.
    /// </summary>
    public string Tag { get; set; }
  }

  /// <summary>
  /// Changable conference member data
  /// </summary>
  public class UpdateConferenceMemberData
  {
    /// <summary>
    /// If "true", member can't hear the conference. If "false", the member can hear the conference.
    /// </summary>
    public bool Hold { get; set; }

    /// <summary>
    /// If "true", the member can't speak in the conference, but can hear audio. If "false", the member can speak in the conference.
    /// </summary>
    public bool Mute { get; set; }

    /// <summary>
    /// If "true", will play a tone when the member joins the conference. If "false", no tone is played when the member joins the conference.
    /// </summary>
    public bool JoinTone { get; set; }

    /// <summary>
    /// If "true", will play a tone when the member leaves the conference. If "false", no tone is played when the member leaves the conference.
    /// </summary>
    public bool LeavingTone { get; set; }
  }


}
