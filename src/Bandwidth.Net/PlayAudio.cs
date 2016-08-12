using System.Threading;
using System.Threading.Tasks;

namespace Bandwidth.Net
{
  /// <summary>
  ///   Add ability to play audio
  /// </summary>
  public interface IPlayAudio
  {
    /// <summary>
    ///   Play audio
    /// </summary>
    /// <param name="id">ID of bridge, call, conference, etc</param>
    /// <param name="data">Parameters for play audio</param>
    /// <param name="cancellationToken">Optional token to cancel async operation</param>
    /// <returns>>Task instance for async operation</returns>
    /// <example>
    ///   <code>
    /// await client.Bridge.PlayAudioAsync("bridgeId", new PlayAudioData {FileUrl = "url"}); 
    /// </code>
    /// </example>
    Task PlayAudioAsync(string id, PlayAudioData data, CancellationToken? cancellationToken = null);
  }

  /// <summary>
  ///   Data for play audio operation
  /// </summary>
  public class PlayAudioData
  {
    /// <summary>
    ///   The location of an audio file to play (WAV and MP3 supported).
    /// </summary>
    public string FileUrl { get; set; }

    /// <summary>
    ///   The sentence to speak.
    /// </summary>
    public string Sentence { get; set; }

    /// <summary>
    ///   The gender of the voice used to synthesize the sentence. It will be considered only if sentence is not null. The
    ///   female gender will be used by default.
    /// </summary>
    public string Gender { get; set; }

    /// <summary>
    ///   The locale used to get the accent of the voice used to synthesize the sentence.
    /// </summary>
    public string Locale { get; set; }

    /// <summary>
    ///   The voice to speak the sentence.
    /// </summary>
    public string Voice { get; set; }

    /// <summary>
    ///   When value is true, the audio will keep playing in a loop. Default: false.
    /// </summary>
    public bool? LoopEnabled { get; set; }

    /// <summary>
    ///   A string that will be included in the events delivered when the audio playback starts or finishes
    /// </summary>
    public string Tag { get; set; }
  }

  /// <summary>
  ///   Usefull extension methods for IPlayAudio
  /// </summary>
  public static class PlayAudioExtensions
  {
    /// <summary>
    ///   Speak a sentence
    /// </summary>
    /// <param name="instance">Instance of <see cref="IPlayAudio" /></param>
    /// <param name="id">ID of bridge, call, conference, etc</param>
    /// <param name="sentence">The sentence to speak</param>
    /// <param name="gender">The gender of the voice used to synthesize the sentence.</param>
    /// <param name="voice">The voice to speak the sentence.</param>
    /// <param name="locale">The locale used to get the accent of the voice used to synthesize the sentence.</param>
    /// <param name="tag">A string that will be included in the events delivered when the audio playback starts or finishes</param>
    /// <param name="cancellationToken">
    ///   Optional token to cancel async operation</param>
    /// <returns>Task instance for async operation</returns>
    /// <example>
    ///   <code>
    /// await client.Bridge.SpeakSentenceAsync("bridgeId", "Hello");
    /// </code>
    /// </example>
    public static Task SpeakSentenceAsync(this IPlayAudio instance, string id, string sentence, string gender = "female",
      string voice = "susan", string locale = "en_US", string tag = null,
      CancellationToken? cancellationToken = null)
    {
      return instance.PlayAudioAsync(id, new PlayAudioData
      {
        Sentence = sentence,
        Gender = gender,
        Voice = voice,
        Locale = locale,
        Tag = tag
      }, cancellationToken);
    }

    /// <summary>
    ///   Play audio file by url
    /// </summary>
    /// <param name="instance">>Instance of <see cref="IPlayAudio" /></param>
    /// <param name="id">ID of bridge, call, conference, etc</param>
    /// <param name="fileUrl">Url to file to play</param>
    /// <param name="tag">A string that will be included in the events delivered when the audio playback starts or finishes</param>
    /// <param name="cancellationToken">
    ///   Optional token to cancel async operation</param>
    /// <returns>Task instance for async operation</returns>
    /// <example>
    ///   <code>
    /// await client.Bridge.PlayAudioFileAsync("bridgeId", "http://host/path/to/file.mp3");
    /// </code>
    /// </example>
    public static Task PlayAudioFileAsync(this IPlayAudio instance, string id, string fileUrl, string tag = null,
      CancellationToken? cancellationToken = null)
    {
      return instance.PlayAudioAsync(id, new PlayAudioData
      {
        FileUrl = fileUrl,
        Tag = tag
      }, cancellationToken);
    }
  }
}
