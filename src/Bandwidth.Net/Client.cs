using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Bandwidth.Net
{
  /// <summary>
  /// Catapult API client
  /// </summary>
  public partial class Client
  {
    internal readonly IrisAuthData IrisAuthData;
    internal readonly CatapultAuthData CatapultAuthData;
    private readonly IHttp _http;
    private static readonly ProductInfoHeaderValue UserAgent = BuildUserAgent();

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="catapultAuthData">Auth data for Catapult</param>
    /// <param name="irisAuthData">Auth data for Iris</param>
    /// <param name="http">Optional processor of http requests. Use it to owerwrite default http request processing (useful for test, logs, etc)</param>
    /// <example>
    /// Regular usage
    /// <code>
    /// var client = new Client(new CatapultAuthData("userId", "apiToken", "apiSecret"), new IrisAuthData("accountId", "userName", "password"));
    /// </code>
    /// 
    /// Using Catapult API only
    /// <code>
    /// var client = new Client(new CatapultAuthData("userId", "apiToken", "apiSecret"));
    /// </code>
    /// 
    /// Using Iris API only
    /// <code>
    /// var client = new Client(null, new IrisAuthData("accountId", "userName", "password"));
    /// </code>
    /// 
    /// Using with own implementaion of HTTP processing (usefull for tests)
    /// <code>
    /// var client = new Client(new CatapultAuthData("userId", "apiToken", "apiSecret"), new IrisAuthData("accountId", "userName", "password"), new YourMockHttp());
    /// </code>
    /// </example>
    public Client(CatapultAuthData catapultAuthData = null, IrisAuthData irisAuthData = null, IHttp http = null)
    {
      CatapultAuthData = catapultAuthData ?? new CatapultAuthData();
      IrisAuthData = irisAuthData ?? new IrisAuthData();
      _http = http ?? new Http<HttpClientHandler>();
      SetupCatapultApis();
      SetupIrisApis();
    }

    private static ProductInfoHeaderValue BuildUserAgent()
    {
      var assembly = typeof(Client).GetTypeInfo().Assembly;
      var assemblyName = new AssemblyName(assembly.FullName);
      return new ProductInfoHeaderValue("csharp-bandwidth", $"v{assemblyName.Version.Major}.{assemblyName.Version.Minor}");
    }

    private static string BuildQueryString(object query)
    {
      if (query == null)
      {
        return "";
      }
      var type = query.GetType();
      return string.Join("&", from p in type.GetRuntimeProperties()
                              let v = p.GetValue(query)
                              where v != null
                              let tv = TransformQueryParameterValue(v)
                              where !string.IsNullOrEmpty(tv)
                              select $"{TransformQueryParameterName(p.Name)}={Uri.EscapeDataString(tv)}");
    }

    private static string TransformQueryParameterName(string name)
    {
      return $"{char.ToLowerInvariant(name[0])}{name.Substring(1)}";
    }

    private static string TransformQueryParameterValue(object value)
    {
      if (value is DateTime)
      {
        return ((DateTime)value).ToUniversalTime().ToString("o");
      }
      return Convert.ToString(value);
    }

    internal HttpRequestMessage CreateRequest(HttpMethod method, string path, IAuthData authData, object query = null)
    {
      authData.Validate();
      var url = path.Contains("://") ? path : $"{authData.BaseUrl}{path}";
      var urlWithQuery = new UriBuilder(url);
      if (query != null)
      {
        urlWithQuery.Query = BuildQueryString(query);
      }
      var message = new HttpRequestMessage(method, urlWithQuery.Uri);
      message.Headers.UserAgent.Add(UserAgent);
      message.Headers.Authorization = authData.AuthenticationHeader;
      return message;
    }

    internal async Task<HttpResponseMessage> MakeJsonRequestAsync(HttpRequestMessage request, IAuthData authData, CancellationToken? cancellationToken = null, HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead)
    {
      var response = await _http.SendAsync(request, completionOption, cancellationToken);
      await response.CheckJsonResponseAsync();
      return response;
    }

    internal async Task<HttpResponseMessage> MakeXmlRequestAsync(HttpRequestMessage request, IAuthData authData, CancellationToken? cancellationToken = null, HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead)
    {
      var response = await _http.SendAsync(request, completionOption, cancellationToken);
      await response.CheckXmlResponseAsync();
      return response;
    }


    internal async Task<T> MakeJsonRequestAsync<T>(HttpMethod method, string path, IAuthData authData, CancellationToken? cancellationToken = null, object query = null, object body = null)
    {
      using (var response = await MakeJsonRequestAsync(method, path, authData,  cancellationToken, query, body))
      {
        return await response.Content.ReadAsJsonAsync<T>();
      }
    }

    internal async Task<T> MakeXmlRequestAsync<T>(HttpMethod method, string path, IAuthData authData, CancellationToken? cancellationToken = null, object query = null, object body = null)
    {
      using (var response = await MakeXmlRequestAsync(method, path, authData, cancellationToken, query, body))
      {
        return await response.Content.ReadAsXmlAsync<T>();
      }
    }

    internal async Task<HttpResponseMessage> MakeJsonRequestAsync(HttpMethod method, string path, IAuthData authData, CancellationToken? cancellationToken = null, object query = null, object body = null)
    {
      var request = CreateRequest(method, path, authData, query);
      request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
      if (body != null)
      {
        request.SetJsonContent(body);
      }
      return await MakeJsonRequestAsync(request, authData, cancellationToken);
    }

    internal async Task<HttpResponseMessage> MakeXmlRequestAsync(HttpMethod method, string path, IAuthData authData, CancellationToken? cancellationToken = null, object query = null, object body = null)
    {
      var request = CreateRequest(method, path, authData, query);
      request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
      if (body != null)
      {
        request.SetXmlContent(body);
      }
      return await MakeXmlRequestAsync(request, authData, cancellationToken);
    }

    internal async Task MakeJsonRequestWithoutResponseAsync(HttpMethod method, string path, IAuthData authData,
      CancellationToken? cancellationToken = null, object query = null, object body = null)
    {
      using (await MakeJsonRequestAsync(method, path, authData, cancellationToken, query, body))
      {
      }
    }

    internal async Task MakeXmlRequestWithoutResponseAsync(HttpMethod method, string path, IAuthData authData,
      CancellationToken? cancellationToken = null, object query = null, object body = null)
    {
      using (await MakeXmlRequestAsync(method, path, authData, cancellationToken, query, body))
      {
      }
    }

    internal async Task<string> MakePostJsonRequestAsync(string path, IAuthData authData, CancellationToken? cancellationToken = null, object body = null)
    {
      using (var response = await MakeJsonRequestAsync(HttpMethod.Post, path, authData, cancellationToken, null, body))
      {
        return (response.Headers.Location ?? new Uri("http://localhost")).AbsolutePath.Split('/').LastOrDefault();
      }
    }

    internal async Task<string> MakePostXmlRequestAsync(string path, IAuthData authData, CancellationToken? cancellationToken = null, object body = null)
    {
      using (var response = await MakeXmlRequestAsync(HttpMethod.Post, path, authData, cancellationToken, null, body))
      {
        return (response.Headers.Location ?? new Uri("http://localhost")).AbsolutePath.Split('/').LastOrDefault();
      }
    }
  }
}
