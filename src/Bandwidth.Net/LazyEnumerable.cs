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
      var getData = _getFirstPageFunc;
      var nextPageUrl = "";
      while (true)
      {
        using (var response = getData().Result)
        {

          var list = response.Content.ReadAsJsonAsync<T[]>().Result;
          foreach (var item in list)
          {
            yield return item;
          }
          IEnumerable<string> linkValues;
          nextPageUrl = "";
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
        }
        if (string.IsNullOrEmpty(nextPageUrl))
        {
          yield break;
        }
        var request = _client.CreateRequest(HttpMethod.Get, nextPageUrl, _client.CatapultAuthData);
        getData = () => _client.MakeJsonRequestAsync(request, _client.CatapultAuthData);
      }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }
}
