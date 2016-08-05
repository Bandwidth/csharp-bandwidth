﻿using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Bandwidth.Net
{
  /// <summary>
  /// Catapult API client
  /// </summary>
  public class Client
  {
    internal readonly string UserId;
    private readonly IHttp _http;
    private static readonly ProductInfoHeaderValue _userAgent = BuildUserAgent();
    private readonly AuthenticationHeaderValue _authentication;
    private readonly string _baseUrl;
    private const string _version = "v1";

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="userId">Id of user on Catapult API</param>
    /// <param name="apiToken">Authorization token of Catapult API</param>
    /// <param name="apiSecret">Authorization secret of Catapult API</param>
    /// <param name="baseUrl">Base url of Catapult API server</param>
    /// <param name="http">Optional processor of http requests. Use it to owerwrite default http request processing (useful for test, logs, etc)</param>
    public Client(string userId, string apiToken, string apiSecret, string baseUrl = "https://api.catapult.inetwork.com", IHttp http = null)
    {
      if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(apiToken) || string.IsNullOrEmpty(apiSecret))
      {
        throw new MissingCredentialsException();
      }
      if (string.IsNullOrEmpty(baseUrl))
      {
        throw new InvalidBaseUrlException();
      }
      UserId = userId;
      _baseUrl = baseUrl;
      _http = http ?? new Http<HttpClientHandler>();
      _authentication =
          new AuthenticationHeaderValue("Basic",
              Convert.ToBase64String(Encoding.UTF8.GetBytes($"{apiToken}:{apiSecret}")));
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
      var type = query.GetType().GetTypeInfo();
      return string.Join("&", from p in type.GetProperties()
                              select $"{TransformQueryParameterName(p.Name)}={Uri.EscapeDataString(TransformQueryParameterValue(p.GetValue(query)))}");
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

    internal HttpRequestMessage CreateRequest(HttpMethod method, string path, object query = null)
    {
      var url = new UriBuilder(_baseUrl);
      url.Path = $"/{_version}{path}";
      url.Query = BuildQueryString(query);
      var message = new HttpRequestMessage(method, url.Uri);
      message.Headers.UserAgent.Add(_userAgent);
      message.Headers.Authorization = _authentication;
      return message;
    }

    internal async Task<HttpResponseMessage> MakeRequest(HttpRequestMessage request, HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead, CancellationToken? cancellationToken = null)
    {
      var response = await _http.SendAsync(request, completionOption, cancellationToken ?? CancellationToken.None);
      await response.CheckResponse();
      return response;
    }

    internal async Task<T> MakeJsonRequest<T>(HttpMethod method, string path, CancellationToken? cancellationToken = null, object query = null, object body = null)
    {
      using (var response = await MakeJsonRequest(method, path, cancellationToken, query, body))
      {
        return await response.ReadAsJsonAsync<T>();
      }
    }

    internal async Task<HttpResponseMessage> MakeJsonRequest(HttpMethod method, string path, CancellationToken? cancellationToken = null, object query = null, object body = null)
    {
      var request = CreateRequest(method, path, query);
      request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
      if (body != null)
      {
        request.SetJsonContent(body);
      }
      return await MakeRequest(request, HttpCompletionOption.ResponseContentRead, cancellationToken);
    }
  }
}
