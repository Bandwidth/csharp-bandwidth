using System.Threading.Tasks;
using Bandwidth.Net.Test.Mocks;
using LightMock;
using Xunit;

namespace Bandwidth.Net.Test
{
  public class PlayAudioTests
  {
    [Fact]
    public async void TestSpeakSentence()
    {
      var context = new MockContext<IPlayAudio>();
      context.Arrange(m => m.PlayAudioAsync("id", The<PlayAudioData>.Is(d => IsValidSpeakSentenceData(d)), null))
        .Returns(Task.FromResult(0));
      var instance = new PlayAudio(context);
      await instance.SpeakSentenceAsync("id", "Hello");
    }

    [Fact]
    public async void TestPlayAudioFile()
    {
      var context = new MockContext<IPlayAudio>();
      context.Arrange(m => m.PlayAudioAsync("id", The<PlayAudioData>.Is(d => IsValidPlayAudioFileData(d)), null))
        .Returns(Task.FromResult(0));
      var instance = new PlayAudio(context);
      await instance.PlayAudioFileAsync("id", "url");
    }

    public static bool IsValidSpeakSentenceData(PlayAudioData data)
    {
      return data.Sentence == "Hello" && data.Gender == Gender.Female && data.Voice == "susan";
    }

    public static bool IsValidPlayAudioFileData(PlayAudioData data)
    {
      return data.FileUrl == "url";
    }
  }
}
