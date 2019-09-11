using Xunit;

namespace Bandwidth.Net.Test.ApiV2
{
  public class CallbackEventTests
  {
    [Fact]
    public void TestCreateFromJson()
    {
      var callbackEvents = Net.ApiV2.CallbackEvent.CreateFromJson(Helpers.GetJsonResourse("IncomingMessage2"));
      Assert.Equal(1, callbackEvents.Length);
      var callbackEvent = callbackEvents[0];
      Assert.Equal(Net.ApiV2.CallbackEventType.MessageReceived, callbackEvent.Type);
      Assert.Equal("Incoming message received", callbackEvent.Description);
      var message = callbackEvent.Message;
      Assert.Equal("+12345678901", message.From);
      Assert.Equal("+12345678902", message.To[0]);
      Assert.Equal(1, message.ReplyTo.Length);
      Assert.Equal("+12345678901", message.ReplyTo[0]);
    }

	 [Fact]
    public void TestCreateFromJsonMessageFalied()
    {
      var callbackEvents = Net.ApiV2.CallbackEvent.CreateFromJson(Helpers.GetJsonResourse("IncomingMessage3"));
      Assert.Equal(1, callbackEvents.Length);
      var callbackEvent = callbackEvents[0];
      Assert.Equal(Net.ApiV2.CallbackEventType.MessageFailed, callbackEvent.Type);
      Assert.Equal("rejected-forbidden-country", callbackEvent.Description);
      var message = callbackEvent.Message;
      Assert.Equal(4432, callbackEvent.ErrorCode);
      Assert.Equal("+15082837070", message.From);
      Assert.Equal("345757070", message.To[0]);
      Assert.Equal(1, message.ReplyTo.Length);
    }

    [Fact]
    public void TestCreateFromJson2()
    {
      var callbackEvents = Net.ApiV2.CallbackEvent.CreateFromJson(Helpers.GetJsonResourse("IncomingGroupMessage2"));
      Assert.Equal(1, callbackEvents.Length);
      var callbackEvent = callbackEvents[0];
      Assert.Equal(Net.ApiV2.CallbackEventType.MessageReceived, callbackEvent.Type);
      Assert.Equal("Incoming message received", callbackEvent.Description);
      Assert.Equal("+12345678902", callbackEvent.To);
      var message = callbackEvent.Message;
      Assert.Equal("+12345678901", message.From);
      Assert.Equal("+12345678902", message.To[0]);
      Assert.Equal("+12345678903", message.To[1]);
      Assert.Equal(2, message.ReplyTo.Length);
      Assert.Equal("+12345678903", message.ReplyTo[0]);
      Assert.Equal("+12345678901", message.ReplyTo[1]);
    }

  }
}
