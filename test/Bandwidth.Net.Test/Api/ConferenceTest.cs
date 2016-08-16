using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Bandwidth.Net.Api;
using LightMock;
using Xunit;

namespace Bandwidth.Net.Test.Api
{
  public class ConferenceTest
  {

    [Fact]
    public async void TestCreate()
    {
      var response = new HttpResponseMessage(HttpStatusCode.Created);
      response.Headers.Location = new Uri("http://localhost/path/id");
      var getResponse = new HttpResponseMessage
      {
        Content = Helpers.GetJsonContent("Conference")
      };
      var context = new MockContext<IHttp>();
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidCreateRequest(r)), HttpCompletionOption.ResponseContentRead,
            null)).Returns(Task.FromResult(response));
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidGetRequest(r)), HttpCompletionOption.ResponseContentRead,
            null)).Returns(Task.FromResult(getResponse));
      var api = Helpers.GetClient(context).Conference;
      var conference = await api.CreateAsync(new CreateConferenceData{From = "+1234567980"});
      context.Assert(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidGetRequest(r)), HttpCompletionOption.ResponseContentRead,
            null), Invoked.Never);
      Assert.Equal("id", conference.Id);
      ValidateConference(conference.Instance);
      context.Assert(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidGetRequest(r)), HttpCompletionOption.ResponseContentRead,
            null), Invoked.Once);
    }

    [Fact]
    public async void TestGet()
    {
      var response = new HttpResponseMessage
      {
        Content = Helpers.GetJsonContent("Conference")
      };
      var context = new MockContext<IHttp>();
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidGetRequest(r)), HttpCompletionOption.ResponseContentRead,
            null)).Returns(Task.FromResult(response));
      var api = Helpers.GetClient(context).Conference;
      var conference = await api.GetAsync("id");
      ValidateConference(conference);
    }

    [Fact]
    public async void TestUpdate()
    {
      var context = new MockContext<IHttp>();
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidUpdateRequest(r)), HttpCompletionOption.ResponseContentRead,
            null)).Returns(Task.FromResult(new HttpResponseMessage()));
      var api = Helpers.GetClient(context).Conference;
      await api.UpdateAsync("id", new UpdateConferenceData {State = ConferenceState.Completed});
    }

    [Fact]
    public async void TestPlayAudio()
    {
      var context = new MockContext<IHttp>();
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidPlayAudioRequest(r)), HttpCompletionOption.ResponseContentRead,
            null)).Returns(Task.FromResult(new HttpResponseMessage()));
      var api = Helpers.GetClient(context).Conference;
      await api.PlayAudioAsync("id", new PlayAudioData {FileUrl = "url"});
    }

    [Fact]
    public void TestGetMembers()
    {
      var context = new MockContext<IHttp>();
      var response = new HttpResponseMessage
      {
        Content = new JsonContent($"[{Helpers.GetJsonResourse("ConferenceMember")}]")
      };
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidGetMembersRequest(r)), HttpCompletionOption.ResponseContentRead,
            null)).Returns(Task.FromResult(response));
      var api = Helpers.GetClient(context).Conference;
      var members = api.GetMembers("id");
      Assert.Equal("memberId", members.First().Id);
    }

    [Fact]
    public async void TestGetMember()
    {
      var context = new MockContext<IHttp>();
      var response = new HttpResponseMessage
      {
        Content = new JsonContent(Helpers.GetJsonResourse("ConferenceMember"))
      };
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidGetMemberRequest(r)), HttpCompletionOption.ResponseContentRead,
            null)).Returns(Task.FromResult(response));
      var api = Helpers.GetClient(context).Conference;
      var member = await api.GetMemberAsync("id", "memberId");
      Assert.Equal("memberId", member.Id);
    }

    [Fact]
    public async void TestCreateMember()
    {
      var response = new HttpResponseMessage(HttpStatusCode.Created);
      response.Headers.Location = new Uri("http://localhost/path/memberId");
      var getResponse = new HttpResponseMessage
      {
        Content = Helpers.GetJsonContent("ConferenceMember")
      };
      var context = new MockContext<IHttp>();
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidCreateMemberRequest(r)), HttpCompletionOption.ResponseContentRead,
            null)).Returns(Task.FromResult(response));
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidGetMemberRequest(r)), HttpCompletionOption.ResponseContentRead,
            null)).Returns(Task.FromResult(getResponse));
      var api = Helpers.GetClient(context).Conference;
      var member = await api.CreateMemberAsync("id", new CreateConferenceMemberData {CallId = "callId"});
      context.Assert(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidGetMemberRequest(r)), HttpCompletionOption.ResponseContentRead,
            null), Invoked.Never);
      Assert.Equal("memberId", member.Id);
      ValidateConferenceMember(member.Instance);
      context.Assert(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidGetMemberRequest(r)), HttpCompletionOption.ResponseContentRead,
            null), Invoked.Once);
    }

    [Fact]
    public async void TestUpdateMember()
    {
      var context = new MockContext<IHttp>();
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidUpdateMemberRequest(r)), HttpCompletionOption.ResponseContentRead,
            null)).Returns(Task.FromResult(new HttpResponseMessage()));
      var api = Helpers.GetClient(context).Conference;
      await api.UpdateMemberAsync("id", "memberId", new UpdateConferenceMemberData{State = ConferenceMemberState.Completed});
    }

    public static bool IsValidCreateRequest(HttpRequestMessage request)
    {
      return request.Method == HttpMethod.Post && request.RequestUri.PathAndQuery == "/v1/users/userId/conferences" &&
             request.Content.Headers.ContentType.MediaType == "application/json" &&
             request.Content.ReadAsStringAsync().Result == "{\"from\":\"+1234567980\"}";
    }

    public static bool IsValidGetRequest(HttpRequestMessage request)
    {
      return request.Method == HttpMethod.Get && request.RequestUri.PathAndQuery == "/v1/users/userId/conferences/id";
    }

    public static bool IsValidUpdateRequest(HttpRequestMessage request)
    {
      return request.Method == HttpMethod.Post && request.RequestUri.PathAndQuery == "/v1/users/userId/conferences/id" &&
             request.Content.Headers.ContentType.MediaType == "application/json" &&
             request.Content.ReadAsStringAsync().Result == "{\"state\":\"completed\"}";
    }

    public static bool IsValidPlayAudioRequest(HttpRequestMessage request)
    {
      return request.Method == HttpMethod.Post && request.RequestUri.PathAndQuery == "/v1/users/userId/conferences/id/audio" &&
             request.Content.Headers.ContentType.MediaType == "application/json" &&
             request.Content.ReadAsStringAsync().Result == "{\"fileUrl\":\"url\"}";
    }

    public static bool IsValidGetMembersRequest(HttpRequestMessage request)
    {
      return request.Method == HttpMethod.Get && request.RequestUri.PathAndQuery == "/v1/users/userId/conferences/id/members";
    }

    public static bool IsValidGetMemberRequest(HttpRequestMessage request)
    {
      return request.Method == HttpMethod.Get && request.RequestUri.PathAndQuery == "/v1/users/userId/conferences/id/members/memberId";
    }

    public static bool IsValidCreateMemberRequest(HttpRequestMessage request)
    {
      return request.Method == HttpMethod.Post && request.RequestUri.PathAndQuery == "/v1/users/userId/conferences/id/members" &&
             request.Content.Headers.ContentType.MediaType == "application/json" &&
             request.Content.ReadAsStringAsync().Result == "{\"callId\":\"callId\"}";
    }

    public static bool IsValidUpdateMemberRequest(HttpRequestMessage request)
    {
      return request.Method == HttpMethod.Post && request.RequestUri.PathAndQuery == "/v1/users/userId/conferences/id/members/memberId" &&
             request.Content.Headers.ContentType.MediaType == "application/json" &&
             request.Content.ReadAsStringAsync().Result == "{\"state\":\"completed\"}";
    }

    private static void ValidateConference(Conference item)
    {
      Assert.Equal("conferenceId", item.Id);
      Assert.Equal("+19703255647", item.From);
      Assert.Equal(ConferenceState.Created, item.State);
    }

    private static void ValidateConferenceMember(ConferenceMember item)
    {
      Assert.Equal("memberId", item.Id);
      Assert.Equal("callId1", item.CallId);
      Assert.Equal(ConferenceMemberState.Active, item.State);
    }
  }

  public class ConferenceExtensionsTest
  {
    [Fact]
    public static async void TestSpeakSentenceToMember()
    {
      var context = new MockContext<IConference>(); 
      var conference = new Mocks.Conference(context);
      context.Arrange(m => m.PlayAudioToMemberAsync("conferenceId", "memberId", The<PlayAudioData>.Is(r => r.Sentence == "Hello"), null)).Returns(Task.FromResult(new HttpResponseMessage()));
      await conference.SpeakSentenceToMemberAsync("conferenceId", "memberId", "Hello");
    }

    [Fact]
    public static async void TestPlayAudioFileToMember()
    {
      var context = new MockContext<IConference>(); 
      var conference = new Mocks.Conference(context);
      context.Arrange(m => m.PlayAudioToMemberAsync("conferenceId", "memberId", The<PlayAudioData>.Is(r => r.FileUrl == "url"), null)).Returns(Task.FromResult(new HttpResponseMessage()));
      await conference.PlayAudioFileToMemberAsync("conferenceId", "memberId", "url");
    }

    [Fact]
    public static async void TestDeleteMember()
    {
      var context = new MockContext<IConference>(); 
      var conference = new Mocks.Conference(context);
      context.Arrange(m => m.UpdateMemberAsync("conferenceId", "memberId", The<UpdateConferenceMemberData>.Is(r => r.State == ConferenceMemberState.Completed), null)).Returns(Task.FromResult(new HttpResponseMessage()));
      await conference.DeleteMemberAsync("conferenceId", "memberId");
    }

    [Fact]
    public static async void TestHoldMember()
    {
      var context = new MockContext<IConference>(); 
      var conference = new Mocks.Conference(context);
      context.Arrange(m => m.UpdateMemberAsync("conferenceId", "memberId", The<UpdateConferenceMemberData>.Is(r => r.Hold), null)).Returns(Task.FromResult(new HttpResponseMessage()));
      await conference.HoldMemberAsync("conferenceId", "memberId", true);
    }

    [Fact]
    public static async void TestMuteMember()
    {
      var context = new MockContext<IConference>(); 
      var conference = new Mocks.Conference(context);
      context.Arrange(m => m.UpdateMemberAsync("conferenceId", "memberId", The<UpdateConferenceMemberData>.Is(r => r.Mute), null)).Returns(Task.FromResult(new HttpResponseMessage()));
      await conference.MuteMemberAsync("conferenceId", "memberId", true);
    }

    [Fact]
    public static async void TestTerminate()
    {
      var context = new MockContext<IConference>(); 
      var conference = new Mocks.Conference(context);
      context.Arrange(m => m.UpdateAsync("conferenceId", The<UpdateConferenceData>.Is(r => r.State == ConferenceState.Completed), null)).Returns(Task.FromResult(new HttpResponseMessage()));
      await conference.TerminateAsync("conferenceId");
    }

    [Fact]
    public static async void TestHold()
    {
      var context = new MockContext<IConference>(); 
      var conference = new Mocks.Conference(context);
      context.Arrange(m => m.UpdateAsync("conferenceId", The<UpdateConferenceData>.Is(r => r.Hold), null)).Returns(Task.FromResult(new HttpResponseMessage()));
      await conference.HoldAsync("conferenceId", true);
    }

    [Fact]
    public static async void TestMute()
    {
      var context = new MockContext<IConference>(); 
      var conference = new Mocks.Conference(context);
      context.Arrange(m => m.UpdateAsync("conferenceId", The<UpdateConferenceData>.Is(r => r.Mute), null)).Returns(Task.FromResult(new HttpResponseMessage()));
      await conference.MuteAsync("conferenceId", true);
    }
  }
}
