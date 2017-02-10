using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bandwidth.Net.Api
{
  /// <summary>
  ///   Access to Media Api
  /// </summary>
  public interface IMedia
  {
    /// <summary>
    ///   Get a list of media files
    /// </summary>
    /// <param name="cancellationToken">>Optional token to cancel async operation</param>
    /// <returns>Collection with <see cref="MediaFile" /> instances</returns>
    /// <example>
    ///   <code>
    /// var files = client.Media.List(); 
    /// </code>
    /// </example>
    IEnumerable<MediaFile> List(CancellationToken? cancellationToken = null);

    /// <summary>
    ///   Upload a media file.
    /// </summary>
    /// <param name="data">Parameters of new media file</param>
    /// <param name="cancellationToken">Optional token to cancel async operation</param>
    /// <returns>Task instance for async operation</returns>
    /// <example>
    ///   <code>
    /// await client.Media.UploadAsync(new UploadMediaData{ MediaName = "file.txt", String = "file content"});
    /// </code>
    /// </example>
    Task UploadAsync(UploadMediaFileData data, CancellationToken? cancellationToken = null);


    /// <summary>
    ///   Download a media file
    /// </summary>
    /// <param name="mediaName">Name of media file to download</param>
    /// <param name="cancellationToken">Optional token to cancel async operation</param>
    /// <returns>Task with <see cref="DownloadMediaFileData" /> instance</returns>
    /// <example>
    ///   <code>
    /// using(var data = await client.Media.DownloadAsync("file.txt"))
    /// {
    ///   var fileContent = await data.ReadAsStringAsync();
    /// }
    /// </code>
    /// </example>
    Task<DownloadMediaFileData> DownloadAsync(string mediaName, CancellationToken? cancellationToken = null);

    /// <summary>
    ///   Remove a media file
    /// </summary>
    /// <param name="mediaName">Name of media file to remove</param>
    /// <param name="cancellationToken">Optional token to cancel async operation</param>
    /// <returns>Task instance for async operation</returns>
    /// <example>
    ///   <code>
    /// await client.Media.DeleteAsync("file.txt");
    /// </code>
    /// </example>
    Task DeleteAsync(string mediaName, CancellationToken? cancellationToken = null);

  }

  internal class MediaApi : ApiBase, IMedia
  {
    public IEnumerable<MediaFile> List(CancellationToken? cancellationToken = null)
    {
      return new LazyEnumerable<MediaFile>(Client,
        () =>
          Client.MakeJsonRequestAsync(HttpMethod.Get, $"/users/{Client.CatapultAuthData.UserId}/media", Client.CatapultAuthData, cancellationToken));
    }

    public async Task UploadAsync(UploadMediaFileData data, CancellationToken? cancellationToken = null)
    {
      if (data == null) throw new ArgumentNullException(nameof(data));
      if (string.IsNullOrEmpty(data.MediaName)) throw new ArgumentException("data.MediaName is required");
      var request = Client.CreateRequest(HttpMethod.Put,
        $"/users/{Client.CatapultAuthData.UserId}/media/{Uri.EscapeDataString(data.MediaName)}", Client.CatapultAuthData);
#if !WithoutFileIO
      IDisposable resourceToClean = null;
      if (data.Path != null)
      {
        var stream = File.OpenRead(data.Path);
        resourceToClean = stream;
        request.Content = new StreamContent(stream);
      }
#endif
      if (data.Stream != null)
      {
        request.Content = new StreamContent(data.Stream);
      }
      if (data.Buffer != null)
      {
        request.Content = new ByteArrayContent(data.Buffer);
      }
      if (data.String != null)
      {
        request.Content = new StringContent(data.String, Encoding.UTF8);
      }
      if (request.Content == null) throw new ArgumentException("Path, Stream, Buffer or String is required. Please fill one of them.");
      request.Content.Headers.ContentType = new MediaTypeHeaderValue(data.ContentType ?? "application/octet-stream");
      using (await Client.MakeJsonRequestAsync(request, Client.CatapultAuthData, cancellationToken))
      {
#if !WithoutFileIO
        resourceToClean?.Dispose();
#endif
      }
    }

    public async Task<DownloadMediaFileData> DownloadAsync(string mediaName, CancellationToken? cancellationToken = null)
    {
      if (string.IsNullOrEmpty(mediaName)) throw new ArgumentNullException(nameof(mediaName));
      var request = Client.CreateRequest(HttpMethod.Get,
        $"/users/{Client.CatapultAuthData.UserId}/media/{Uri.EscapeDataString(mediaName)}", Client.CatapultAuthData);
      var response = await Client.MakeJsonRequestAsync(request, Client.CatapultAuthData, cancellationToken, HttpCompletionOption.ResponseHeadersRead);
      response.EnsureSuccessStatusCode();
      return new DownloadMediaFileData(response);
    }

    public Task DeleteAsync(string mediaName, CancellationToken? cancellationToken = null)
    {
      return Client.MakeJsonRequestWithoutResponseAsync(HttpMethod.Delete,
        $"/users/{Client.CatapultAuthData.UserId}/media/{Uri.EscapeDataString(mediaName)}", Client.CatapultAuthData, cancellationToken);
    }
  }


  /// <summary>
  ///   Media file information
  /// </summary>
  public class MediaFile
  {
    /// <summary>
    ///   Name of media file
    /// </summary>
    public string MediaName { get; set; }

    /// <summary>
    /// Length of media file
    /// </summary>
    public int ContentLength { get; set; }

  }

  /// <summary>
  /// Data to upload media file
  /// </summary>
  public class UploadMediaFileData
  {
    /// <summary>
    ///   Name of media file
    /// </summary>
    public string MediaName { get; set; }

    /// <summary>
    /// Content type of media file
    /// </summary>
    public string ContentType { get; set; }

#if !WithoutFileIO

    /// <summary>
    ///   Path to file to upload
    /// </summary>
    public string Path { get; set; }
#endif
    /// <summary>
    ///   Byte array to upload
    /// </summary>
    public byte[] Buffer { get; set; }

    /// <summary>
    ///   Stream to upload
    /// </summary>
    public Stream Stream { get; set; }

    /// <summary>
    ///   String to upload
    /// </summary>
    public string String { get; set; }
  }

  /// <summary>
  /// Downloaded media file data
  /// </summary>
  public sealed class DownloadMediaFileData : IDisposable
  {
    private readonly HttpResponseMessage _response;

    internal DownloadMediaFileData(HttpResponseMessage response)
    {
      _response = response;
    }

    /// <summary>
    /// Length of media file
    /// </summary>
    public long? ContentLength => _response.Content.Headers.ContentLength;

    /// <summary>
    /// Content type of media file
    /// </summary>
    public string ContentType => _response.Content.Headers.ContentType.MediaType;

    /// <summary>
    /// Read content of downloaded file as byte array
    /// </summary>
    /// <returns>Byte array</returns>
    public Task<byte[]> ReadAsByteArrayAsync()
    {
      return _response.Content.ReadAsByteArrayAsync();
    }

    /// <summary>
    /// Read content of downloaded file as stream
    /// </summary>
    /// <returns>Stream</returns>
    public Task<Stream> ReadAsStreamAsync()
    {
      return _response.Content.ReadAsStreamAsync();
    }

    /// <summary>
    /// Read content of downloaded file as string
    /// </summary>
    /// <returns>String content of file</returns>
    public Task<string> ReadAsStringAsync()
    {
      return _response.Content.ReadAsStringAsync();
    }

    /// <summary>
    /// Free allocated resources
    /// </summary>
    public void Dispose()
    {
      _response.Dispose();
    }
  }

}
