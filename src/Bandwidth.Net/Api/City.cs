using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bandwidth.Net.Api
{
    public interface ICity
    {
        Task<City[]> ListAsync(CityQuery query = null, CancellationToken? cancellationToken = null);
    }

    internal class CityApi : ApiBase, ICity
    {
        public async Task<City[]> ListAsync(CityQuery query = null,
          CancellationToken? cancellationToken = null)
        {
            return (await Client.MakeXmlRequestAsync<CityResponse>(HttpMethod.Get,
              "/cities", Client.IrisAuthData, cancellationToken, query)).Cities;
        }
    }

    public class CityQuery
    {
        public string State { get; set; }
        public bool? Supported { get; set; }
        public bool? Available { get; set; }
    }

    public class City
    {
        public string RcAbbreviation { get; set; }
        public string Name { get; set; }
    }

    public class CityResponse
    {
        public City[] Cities { get; set; }
    }
}
