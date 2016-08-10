using System;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Bandwidth.Net
{
  internal static class JsonHelpers
  {
    public static JsonSerializerSettings GetSerializerSettings()
    {
      var settings = new JsonSerializerSettings
      {
        ContractResolver = new CamelCasePropertyNamesContractResolver()
      };
      settings.Converters.Add(new JsonStringEnumConverter());
      settings.DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate;
      return settings;
    }

    public static void SetJsonContent(this HttpRequestMessage message, object data)
    {
      var json = JsonConvert.SerializeObject(data, Formatting.None, GetSerializerSettings());
      message.Content = new StringContent(json, Encoding.UTF8, "application/json");
    }

    public static async Task CheckResponseAsync(this HttpResponseMessage response)
    {
      if (!response.IsSuccessStatusCode)
      {
        var json = await response.Content.ReadAsStringAsync();
        try
        {
          var msg = JsonConvert.DeserializeAnonymousType(json, new { Message = "", Code = "" }, GetSerializerSettings());
          var message = msg.Message ?? msg.Code;
          if (!string.IsNullOrEmpty(message))
          {
            throw new BandwidthException(message, response.StatusCode);
          }
        }
        catch (Exception ex)
        {
          if (ex is BandwidthException) throw;
          Debug.WriteLine(ex.Message);
        }
        throw new BandwidthException(json, response.StatusCode);
      }
    }

    public static async Task<TResult> ReadAsJsonAsync<TResult>(this HttpResponseMessage response)
    {
      if (response.Content.Headers.ContentType.MediaType == "application/json")
      {
        var json = await response.Content.ReadAsStringAsync();
        return json.Length > 0
            ? JsonConvert.DeserializeObject<TResult>(json, GetSerializerSettings())
            : default(TResult);
      }
      return default(TResult);
    }
  }
}
