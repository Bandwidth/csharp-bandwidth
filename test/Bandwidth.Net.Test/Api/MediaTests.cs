using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Bandwidth.Net.Api;
using LightMock;
using Xunit;

namespace Bandwidth.Net.Test.Api
{
  public class MediaTest
  {
    [Fact]
    public void TestList()
    {
      var response = new HttpResponseMessage
      {
        Content =
          new JsonContent($"[{Helpers.GetJsonResourse("MediaFile")}]")
      };
      var context = new MockContext<IHttp>();
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidListRequest(r)), HttpCompletionOption.ResponseContentRead,
            null)).Returns(Task.FromResult(response));
      var api = Helpers.GetClient(context).Media;
      var file = api.List().First();
      Assert.Equal("mediaName1", file.MediaName);
      Assert.Equal(10, file.ContentLength);
    }

    [Fact]
    public async void TestUploadBuffer()
    {
      var context = new MockContext<IHttp>();
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidUploadRequest(r)), HttpCompletionOption.ResponseContentRead,
            null)).Returns(Task.FromResult(new HttpResponseMessage()));
      var api = Helpers.GetClient(context).Media;
      await api.UploadAsync(new UploadMediaFileData
      {
        MediaName = "file",
        ContentType = "text/plain",
        Buffer = Encoding.UTF8.GetBytes("1234")
      });
    }

    [Fact]
    public async void TestUploadStream()
    {
      var context = new MockContext<IHttp>();
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidUploadRequest(r)), HttpCompletionOption.ResponseContentRead,
            null)).Returns(Task.FromResult(new HttpResponseMessage()));
      var api = Helpers.GetClient(context).Media;
      using (var stream = new MemoryStream(Encoding.UTF8.GetBytes("1234")))
      {
        await api.UploadAsync(new UploadMediaFileData
        {
          MediaName = "file",
          ContentType = "text/plain",
          Stream = stream
        });
      }
    }

    [Fact]
    public async void TestUploadString()
    {
      var context = new MockContext<IHttp>();
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidUploadRequest(r)), HttpCompletionOption.ResponseContentRead,
            null)).Returns(Task.FromResult(new HttpResponseMessage()));
      var api = Helpers.GetClient(context).Media;
      await api.UploadAsync(new UploadMediaFileData
      {
        MediaName = "file",
        ContentType = "text/plain",
        String = "1234"
      });
    }

    [Fact]
    public async void TestUploadPath()
    {
      var context = new MockContext<IHttp>();
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidUploadRequest(r)), HttpCompletionOption.ResponseContentRead,
            null)).Returns(Task.FromResult(new HttpResponseMessage()));
      var api = Helpers.GetClient(context).Media;
      var path = Path.GetTempFileName();
      try
      {
        File.WriteAllText(path, "1234");
        await api.UploadAsync(new UploadMediaFileData
        {
          MediaName = "file",
          ContentType = "text/plain",
          Path = path
        });
      }
      finally
      {
        File.Delete(path);
      }
    }

    [Fact]
    public async void TestUploadFailWithNullData()
    {
      var context = new MockContext<IHttp>();
      var api = Helpers.GetClient(context).Media;
      await Assert.ThrowsAsync<ArgumentNullException>(() => api.UploadAsync(null));
    }

    [Fact]
    public async void TestUploadFailWithMissingMediaName()
    {
      var context = new MockContext<IHttp>();
      var api = Helpers.GetClient(context).Media;
      await Assert.ThrowsAsync<ArgumentException>(() => api.UploadAsync(new UploadMediaFileData{String = "1234"}));
    }

    [Fact]
    public async void TestUploadFailWithMissingContent()
    {
      var context = new MockContext<IHttp>();
      var api = Helpers.GetClient(context).Media;
      await Assert.ThrowsAsync<ArgumentException>(() => api.UploadAsync(new UploadMediaFileData{MediaName = "file"}));
    }

    [Fact]
    public async void TestDownload()
    {
      var context = new MockContext<IHttp>();
      var response = new HttpResponseMessage
      {
        Content = new StringContent("1234", Encoding.UTF8, "text/plain")
      };
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidDownloadRequest(r)), HttpCompletionOption.ResponseHeadersRead,
            null)).Returns(Task.FromResult(response));
      var api = Helpers.GetClient(context).Media;
      using (var data = await api.DownloadAsync("file"))
      {
        Assert.Equal("text/plain", data.ContentType);
        Assert.Equal(4, data.ContentLength);
        Assert.Equal("1234", await data.ReadAsStringAsync());
        Assert.Equal("1234", Encoding.UTF8.GetString(await data.ReadAsByteArrayAsync()));
        using (var reader = new StreamReader(await data.ReadAsStreamAsync(), Encoding.UTF8))
        {
          Assert.Equal("1234", await reader.ReadToEndAsync());
        }
      }
    }

    [Fact]
    public async void TestDownloadFailWithMissingMediaName()
    {
      var context = new MockContext<IHttp>();
      var api = Helpers.GetClient(context).Media;
      await Assert.ThrowsAsync<ArgumentNullException>(() => api.DownloadAsync(null));
    }
   
    [Fact]
    public async void TestDelete()
    {
      var context = new MockContext<IHttp>();
      context.Arrange(
        m =>
          m.SendAsync(The<HttpRequestMessage>.Is(r => IsValidDeleteRequest(r)), HttpCompletionOption.ResponseContentRead,
            null)).Returns(Task.FromResult(new HttpResponseMessage()));
      var api = Helpers.GetClient(context).Media;
      await api.DeleteAsync("file");
    }

    public static bool IsValidListRequest(HttpRequestMessage request)
    {
      return request.Method == HttpMethod.Get && request.RequestUri.PathAndQuery == "/v1/users/userId/media";
    }

    public static bool IsValidUploadRequest(HttpRequestMessage request)
    {
      return request.Method == HttpMethod.Put && request.RequestUri.PathAndQuery == "/v1/users/userId/media/file" &&
             request.Content.Headers.ContentType.MediaType == "text/plain" &&
             request.Content.Headers.ContentLength == 4L &&
             request.Content.ReadAsStringAsync().Result == "1234";
    }

    public static bool IsValidDownloadRequest(HttpRequestMessage request)
    {
      return request.Method == HttpMethod.Get && request.RequestUri.PathAndQuery == "/v1/users/userId/media/file";
    }

    public static bool IsValidDeleteRequest(HttpRequestMessage request)
    {
      return request.Method == HttpMethod.Delete && request.RequestUri.PathAndQuery == "/v1/users/userId/media/file";
    }
  }
}
