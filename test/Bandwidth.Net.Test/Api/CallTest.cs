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
  public class CallTest
  {
    [Fact]
    public void TestList()
    {
      var response = new HttpResponseMessage
      {
        Content =
          new JsonContent($"[{Helpers.GetJsonResourse("Call")}]")
      };
      var context = new MockContext<IHttp>();
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidListRequest(r)), HttpCompletionOption.ResponseContentRead,
            null)).Returns(Task.FromResult(response));
      var api = Helpers.GetClient(context).Call;
      var calls = api.List();
      ValidateCall(calls.First());
    }

    [Fact]
    public async void TestCreate()
    {
      var response = new HttpResponseMessage(HttpStatusCode.Created);
      response.Headers.Location = new Uri("http://localhost/path/id");
      var getResponse = new HttpResponseMessage
      {
        Content = Helpers.GetJsonContent("Call")
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
      var api = Helpers.GetClient(context).Call;
      var call = await api.CreateAsync(new CreateCallData{From = "+1234567890", To = "+1234567981"});
      context.Assert(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidGetRequest(r)), HttpCompletionOption.ResponseContentRead,
            null), Invoked.Never);
      Assert.Equal("id", call.Id);
      ValidateCall(call.Instance);
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
        Content = Helpers.GetJsonContent("Call")
      };
      var context = new MockContext<IHttp>();
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidGetRequest(r)), HttpCompletionOption.ResponseContentRead,
            null)).Returns(Task.FromResult(response));
      var api = Helpers.GetClient(context).Call;
      var call = await api.GetAsync("id");
      ValidateCall(call);
    }

    [Fact]
    public async void TestUpdate()
    {
      var context = new MockContext<IHttp>();
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidUpdateRequest(r)), HttpCompletionOption.ResponseContentRead,
            null)).Returns(Task.FromResult(new HttpResponseMessage()));
      var api = Helpers.GetClient(context).Call;
      await api.UpdateAsync("id", new UpdateCallData {State = CallState.Completed});
    }

    [Fact]
    public async void TestPlayAudio()
    {
      var context = new MockContext<IHttp>();
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidPlayAudioRequest(r)), HttpCompletionOption.ResponseContentRead,
            null)).Returns(Task.FromResult(new HttpResponseMessage()));
      var api = Helpers.GetClient(context).Call;
      await api.PlayAudioAsync("id", new PlayAudioData {FileUrl = "url"});
    }

    [Fact]
    public async void TestSendDtmfAsync()
    {
      var context = new MockContext<IHttp>();
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidSendDtmfRequest(r)), HttpCompletionOption.ResponseContentRead,
            null)).Returns(Task.FromResult(new HttpResponseMessage()));
      var api = Helpers.GetClient(context).Call;
      await api.SendDtmfAsync("id", new SendDtmfData{DtmfOut = "1"});
    }

    [Fact]
    public void TestGetEvents()
    {
      var context = new MockContext<IHttp>();
      var response = new HttpResponseMessage
      {
        Content = new JsonContent($"[{Helpers.GetJsonResourse("CallEvent")}]")
      };
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidGetEventsRequest(r)), HttpCompletionOption.ResponseContentRead,
            null)).Returns(Task.FromResult(response));
      var api = Helpers.GetClient(context).Call;
      var events = api.GetEvents("id");
      Assert.Equal("eventId", events.First().Id);
    }

    [Fact]
    public async void TestGetEvent()
    {
      var context = new MockContext<IHttp>();
      var response = new HttpResponseMessage
      {
        Content = new JsonContent(Helpers.GetJsonResourse("CallEvent"))
      };
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidGetEventRequest(r)), HttpCompletionOption.ResponseContentRead,
            null)).Returns(Task.FromResult(response));
      var api = Helpers.GetClient(context).Call;
      var ev = await api.GetEventAsync("id", "eventId");
      Assert.Equal("eventId", ev.Id);
    }

    [Fact]
    public void TestGetRecordings()
    {
      var context = new MockContext<IHttp>();
      var response = new HttpResponseMessage
      {
        Content = new JsonContent($"[{Helpers.GetJsonResourse("CallRecording")}]")
      };
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidGetRecordingsRequest(r)), HttpCompletionOption.ResponseContentRead,
            null)).Returns(Task.FromResult(response));
      var api = Helpers.GetClient(context).Call;
      var recordings = api.GetRecordings("id");
      Assert.Equal("recordingId", recordings.First().Id);
    }

    [Fact]
    public void TestGetTranscriptions()
    {
      var context = new MockContext<IHttp>();
      var response = new HttpResponseMessage
      {
        Content = new JsonContent($"[{Helpers.GetJsonResourse("CallTranscription")}]")
      };
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidGetTranscriptionsRequest(r)), HttpCompletionOption.ResponseContentRead,
            null)).Returns(Task.FromResult(response));
      var api = Helpers.GetClient(context).Call;
      var transcriptions = api.GetTranscriptions("id");
      Assert.Equal("transcriptionId", transcriptions.First().Id);
    }

    [Fact]
    public async void TestCreateGather()
    {
      var response = new HttpResponseMessage(HttpStatusCode.Created);
      response.Headers.Location = new Uri("http://localhost/path/gatherId");
      var getResponse = new HttpResponseMessage
      {
        Content = Helpers.GetJsonContent("CallGather")
      };
      var context = new MockContext<IHttp>();
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidCreateGatherRequest(r)), HttpCompletionOption.ResponseContentRead,
            null)).Returns(Task.FromResult(response));
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidGetGatherRequest(r)), HttpCompletionOption.ResponseContentRead,
            null)).Returns(Task.FromResult(getResponse));
      var api = Helpers.GetClient(context).Call;
      var gather = await api.CreateGatherAsync("id", new CreateGatherData {MaxDigits = "1"});
      context.Assert(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidGetGatherRequest(r)), HttpCompletionOption.ResponseContentRead,
            null), Invoked.Never);
      Assert.Equal("gatherId", gather.Id);
      ValidateCallGather(gather.Instance);
      context.Assert(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidGetGatherRequest(r)), HttpCompletionOption.ResponseContentRead,
            null), Invoked.Once);
    }

    [Fact]
    public async void TestGetGather()
    {
      var response = new HttpResponseMessage
      {
        Content = Helpers.GetJsonContent("CallGather")
      };
      var context = new MockContext<IHttp>();
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidGetGatherRequest(r)), HttpCompletionOption.ResponseContentRead,
            null)).Returns(Task.FromResult(response));
      var api = Helpers.GetClient(context).Call;
      var gather = await api.GetGatherAsync("id", "gatherId");
      ValidateCallGather(gather);
    }

    [Fact]
    public async void TestUpdateGather()
    {
      var context = new MockContext<IHttp>();
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidUpdateGatherRequest(r)), HttpCompletionOption.ResponseContentRead,
            null)).Returns(Task.FromResult(new HttpResponseMessage()));
      var api = Helpers.GetClient(context).Call;
      await api.UpdateGatherAsync("id", "gatherId", new UpdateGatherData{State = CallGatherState.Completed});
    }

    public static bool IsValidListRequest(HttpRequestMessage request)
    {
      return request.Method == HttpMethod.Get && request.RequestUri.PathAndQuery == "/v1/users/userId/calls";
    }

    public static bool IsValidCreateRequest(HttpRequestMessage request)
    {
      return request.Method == HttpMethod.Post && request.RequestUri.PathAndQuery == "/v1/users/userId/calls" &&
             request.Content.Headers.ContentType.MediaType == "application/json" &&
             request.Content.ReadAsStringAsync().Result == "{\"from\":\"+1234567890\",\"to\":\"+1234567981\"}";
    }

    public static bool IsValidGetRequest(HttpRequestMessage request)
    {
      return request.Method == HttpMethod.Get && request.RequestUri.PathAndQuery == "/v1/users/userId/calls/id";
    }

    public static bool IsValidUpdateRequest(HttpRequestMessage request)
    {
      return request.Method == HttpMethod.Post && request.RequestUri.PathAndQuery == "/v1/users/userId/calls/id" &&
             request.Content.Headers.ContentType.MediaType == "application/json" &&
             request.Content.ReadAsStringAsync().Result == "{\"state\":\"completed\"}";
    }

    public static bool IsValidPlayAudioRequest(HttpRequestMessage request)
    {
      return request.Method == HttpMethod.Post && request.RequestUri.PathAndQuery == "/v1/users/userId/calls/id/audio" &&
             request.Content.Headers.ContentType.MediaType == "application/json" &&
             request.Content.ReadAsStringAsync().Result == "{\"fileUrl\":\"url\"}";
    }

    public static bool IsValidSendDtmfRequest(HttpRequestMessage request)
    {
      return request.Method == HttpMethod.Post && request.RequestUri.PathAndQuery == "/v1/users/userId/calls/id/dtmf" &&
             request.Content.Headers.ContentType.MediaType == "application/json" &&
             request.Content.ReadAsStringAsync().Result == "{\"dtmfOut\":\"1\"}";
    }

    public static bool IsValidGetEventsRequest(HttpRequestMessage request)
    {
      return request.Method == HttpMethod.Get && request.RequestUri.PathAndQuery == "/v1/users/userId/calls/id/events";
    }

    public static bool IsValidGetEventRequest(HttpRequestMessage request)
    {
      return request.Method == HttpMethod.Get && request.RequestUri.PathAndQuery == "/v1/users/userId/calls/id/events/eventId";
    }

    public static bool IsValidGetRecordingsRequest(HttpRequestMessage request)
    {
      return request.Method == HttpMethod.Get && request.RequestUri.PathAndQuery == "/v1/users/userId/calls/id/recordings";
    }

    public static bool IsValidGetTranscriptionsRequest(HttpRequestMessage request)
    {
      return request.Method == HttpMethod.Get && request.RequestUri.PathAndQuery == "/v1/users/userId/calls/id/transcriptions";
    }

    public static bool IsValidCreateGatherRequest(HttpRequestMessage request)
    {
      return request.Method == HttpMethod.Post && request.RequestUri.PathAndQuery == "/v1/users/userId/calls/id/gather" &&
             request.Content.Headers.ContentType.MediaType == "application/json" &&
             request.Content.ReadAsStringAsync().Result == "{\"maxDigits\":\"1\"}";
    }

    public static bool IsValidGetGatherRequest(HttpRequestMessage request)
    {
      return request.Method == HttpMethod.Get && request.RequestUri.PathAndQuery == "/v1/users/userId/calls/id/gather/gatherId";
    }

    public static bool IsValidUpdateGatherRequest(HttpRequestMessage request)
    {
      return request.Method == HttpMethod.Post && request.RequestUri.PathAndQuery == "/v1/users/userId/calls/id/gather/gatherId" &&
             request.Content.Headers.ContentType.MediaType == "application/json" &&
             request.Content.ReadAsStringAsync().Result == "{\"state\":\"completed\"}";
    }

    private static void ValidateCall(Call item)
    {
      Assert.Equal("callId", item.Id);
      Assert.Equal("+1234567890", item.From);
      Assert.Equal("+1234567891", item.To);
      Assert.Equal(CallState.Completed, item.State);
    }

    private static void ValidateCallGather(CallGather item)
    {
      Assert.Equal("gatherId", item.Id);
      Assert.Equal("1", item.Digits);
    }
  }

  public class CallExtensionsTest
  {
    [Fact]
    public static async void TestAnswer()
    {
      var context = new MockContext<ICall>(); 
      var call = new Mocks.Call(context);
      context.Arrange(m => m.UpdateAsync("id", The<UpdateCallData>.Is(d => d.State == CallState.Active), null)).Returns(Task.FromResult(new HttpResponseMessage()));
      await call.AnswerAsync("id");
    }

    [Fact]
    public static async void TestReject()
    {
      var context = new MockContext<ICall>(); 
      var call = new Mocks.Call(context);
      context.Arrange(m => m.UpdateAsync("id", The<UpdateCallData>.Is(d => d.State == CallState.Rejected), null)).Returns(Task.FromResult(new HttpResponseMessage()));
      await call.RejectAsync("id");
    }

    [Fact]
    public static async void TestHangup()
    {
      var context = new MockContext<ICall>(); 
      var call = new Mocks.Call(context);
      context.Arrange(m => m.UpdateAsync("id", The<UpdateCallData>.Is(d => d.State == CallState.Completed), null)).Returns(Task.FromResult(new HttpResponseMessage()));
      await call.HangupAsync("id");
    }

    [Fact]
    public static async void TestTurnCallRecording()
    {
      var context = new MockContext<ICall>(); 
      var call = new Mocks.Call(context);
      context.Arrange(m => m.UpdateAsync("id", The<UpdateCallData>.Is(d => d.RecordingEnabled), null)).Returns(Task.FromResult(new HttpResponseMessage()));
      await call.TurnCallRecordingAsync("id", true);
    }
    
    [Fact]
    public static async void TestTransfer()
    {
      var context = new MockContext<ICall>(); 
      var response = new HttpResponseMessage(HttpStatusCode.Created);
      response.Headers.Location = new Uri("http://localhost/path/transferedId");
      var call = new Mocks.Call(context);
      context.Arrange(m => m.UpdateAsync("id", The<UpdateCallData>.Is(d => d.State == CallState.Transferring && d.TransferTo == "to"), null)).Returns(Task.FromResult(response));
      var callId = await call.TransferAsync("id", "to");
      Assert.Equal("transferedId", callId);
    }
  }
}
