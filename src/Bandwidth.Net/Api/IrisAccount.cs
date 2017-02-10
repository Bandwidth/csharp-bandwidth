using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bandwidth.Net.Api
{
  /// <summary>
  ///   Access to Account Api (IRIS)
  /// </summary>
  public interface IIrisAccount
  {
    /// <summary>
    ///   Get information about account (IRIS)
    /// </summary>
    /// <param name="cancellationToken">Optional token to cancel async operation</param>
    /// <returns>Task with <see cref="IrisAccount" /> Account instance</returns>
    /// <example>
    ///   <code>
    /// var account = await client.IrisAccount.GetAsync();
    /// </code>
    /// </example>
    Task<IrisAccount> GetAsync(CancellationToken? cancellationToken = null);
  }

  internal class IrisAccountApi : ApiBase, IIrisAccount
  {
    public async Task<IrisAccount> GetAsync(CancellationToken? cancellationToken = null)
    {
      return (await Client.MakeXmlRequestAsync<IrisAccountResponse>(HttpMethod.Get,
        $"/accounts/{Client.IrisAuthData.AccountId}", Client.IrisAuthData, cancellationToken)).Account;
    }
  }

  public class IrisAccount
  {
    public string Id
    {
      get { return AccountId; }
      set { AccountId = value; }
    }

    public string AccountId { get; set; }
    public string CompanyName { get; set; }
    public string AccountType { get; set; }
    public string NenaId { get; set; }

    [XmlArrayItem("Tier")]
    public int[] Tiers { get; set; }

    public bool ReservationAllowed { get; set; }
    public bool LnpEnabled { get; set; }
    public string AltSpid { get; set; }

    [XmlElement("SPID")]
    public string Spid { get; set; }

    public string PortCarrierType { get; set; }

    public Contact Contact { get; set; }
    public Address Address { get; set; }
  }

  [XmlType("AccountResponse")]
  public class IrisAccountResponse
  {
    public IrisAccount Account { get; set; }
  }

  public class Contact
  {
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
  }

  public class Address
  {
    public string HouseNumber { get; set; }
    public string HouseSuffix { get; set; }
    public string StreetName { get; set; }
    public string StreetSuffix { get; set; }
    public string AddressLine2 { get; set; }
    public string City { get; set; }
    public string StateCode { get; set; }
    public string Zip { get; set; }
    public string Country { get; set; }
    public string AddressType { get; set; }
  }
}
