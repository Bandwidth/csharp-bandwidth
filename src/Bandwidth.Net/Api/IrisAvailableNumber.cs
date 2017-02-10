using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bandwidth.Net.Api
{
  public interface IIrisAvailableNumber
  {
    Task<IrisAvailableNumbersResult> ListAsync(IrisAvailableNumberQuery query = null,
      CancellationToken? cancellationToken = null);
  }

  internal class IrisAvailableNumberApi : ApiBase, IIrisAvailableNumber
  {
    public async Task<IrisAvailableNumbersResult> ListAsync(IrisAvailableNumberQuery query = null,
      CancellationToken? cancellationToken = null)
    {
      return await Client.MakeXmlRequestAsync<IrisAvailableNumbersResult>(HttpMethod.Get,
        $"/accounts/{Client.IrisAuthData.AccountId}/availableNumbers", Client.IrisAuthData, cancellationToken, query);
    }
  }

  public class IrisAvailableNumberQuery
  {
    public string AreaCode { get; set; }
    public int? Quantity { get; set; }
    public bool? EnableTNDetail { get; set; }
  }

  [XmlType("SearchResult")]
  public class IrisAvailableNumbersResult
  {
    public int ResultCount { get; set; }

    [XmlArrayItem("TelephoneNumber")]
    public string[] TelephoneNumberList { get; set; }

    public TelephoneNumberDetail[] TelephoneNumberDetailList { get; set; }
  }

  public class TelephoneNumberDetail
  {
    public string City { get; set; }

    [XmlElement("LATA")]
    public string Lata { get; set; }

    public string RateCenter { get; set; }
    public string State { get; set; }
    public string TelephoneNumber { get; set; }
    public string Tier { get; set; }
    public string VendorId { get; set; }
    public string VendorName { get; set; }
  }
}
