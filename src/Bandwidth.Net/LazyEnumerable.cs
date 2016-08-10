using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Bandwidth.Net
{
  internal class LazyEnumerable<T> : IEnumerable<T>
  {
    private readonly Client _client;
    private readonly Func<Task<HttpResponseMessage>> _getFirstPageFunc;

    public LazyEnumerable(Client client, Func<Task<HttpResponseMessage>> getFirstPageFunc)
    {
      if (client == null) throw new ArgumentNullException(nameof(client));
      if (getFirstPageFunc == null) throw new ArgumentNullException(nameof(getFirstPageFunc));
      _client = client;
      _getFirstPageFunc = getFirstPageFunc;
    }

    public IEnumerator<T> GetEnumerator()
    {
      var response = _getFirstPageFunc().Result;
      while (true)
      {
        var list = response.ReadAsJsonAsync<T[]>().Result;
        foreach (var item in list)
        {
          yield return item;
        }
        var nextPageUrl = "";
        IEnumerable<string> linkValues;
        if (response.Headers.TryGetValues("Link", out linkValues))
        {
          var links = linkValues.First().Split(',');
          foreach (var link in links)
          {
            var values = link.Split(';');
            if (values.Length == 2 && values[1].Trim() == "rel=\"next\"")
            {
              nextPageUrl = values[0].Replace('<', ' ').Replace('>', ' ').Trim();
              break;
            }
          }
        }
        if (string.IsNullOrEmpty(nextPageUrl))
        {
          yield break;
        }
        var request = _client.CreateGetRequest(nextPageUrl);
        response = _client.MakeRequestAsync(request).Result;
      }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }
}
