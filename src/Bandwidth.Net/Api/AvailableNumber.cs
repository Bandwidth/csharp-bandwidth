using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Bandwidth.Net.Api
{
  /// <summary>
  ///   Access to AvailableNumber Api
  /// </summary>
  public interface IAvailableNumber
  {
    /// <summary>
    ///   Search for available local numbers
    /// </summary>
    /// <param name="query">Search criterias</param>
    /// <param name="cancellationToken">Optional token to cancel async operation</param>
    /// <returns>Array with <see cref="AvailableNumber" /> instances</returns>
    /// <example>
    ///   <code>
    /// var numbers = await client.AvailableNumber.SearchLocalAsync(new LocalNumberQuery {AreaCode = 910}); 
    /// </code>
    /// </example>
    Task<AvailableNumber[]> SearchLocalAsync(LocalNumberQuery query, CancellationToken? cancellationToken = null);

    /// <summary>
    ///   Search for available toll free numbers
    /// </summary>
    /// <param name="query">Search criterias</param>
    /// <param name="cancellationToken">Optional token to cancel async operation</param>
    /// <returns>Array with <see cref="AvailableNumber" /> instances</returns>
    /// <example>
    ///   <code>
    /// var numbers = await client.AvailableNumber.SearchTollFreeAsync(new TollFreeNumberQuery {Quantity = 5}); 
    /// </code>
    /// </example>
    Task<AvailableNumber[]> SearchTollFreeAsync(TollFreeNumberQuery query, CancellationToken? cancellationToken = null);

    /// <summary>
    ///   Search and order available local numbers
    /// </summary>
    /// <param name="query">Search criterias</param>
    /// <param name="cancellationToken">Optional token to cancel async operation</param>
    /// <returns>Array with <see cref="OrderedNumber" /> instances</returns>
    /// <example>
    ///   <code>
    /// var orderedNumbers = await client.AvailableNumber.SearchAndOrderLocalAsync(new LocalNumberQueryForOrder {AreaCode = 910, Quantity = 1}); 
    /// </code>
    /// </example>
    Task<OrderedNumber[]> SearchAndOrderLocalAsync(LocalNumberQueryForOrder query,
      CancellationToken? cancellationToken = null);

    /// <summary>
    ///   Searches and order available toll free numbers.
    /// </summary>
    /// <param name="query">Search criterias</param>
    /// <param name="cancellationToken">Optional token to cancel async operation</param>
    /// <returns>Array with <see cref="OrderedNumber" /> instances</returns>
    /// <example>
    ///   <code>
    /// var orderedNumbers = await client.AvailableNumber.SearchAndOrderTollFreeAsync(new TollFreeNumberQueryForOrder {Quantity = 1}); 
    /// </code>
    /// </example>
    Task<OrderedNumber[]> SearchAndOrderTollFreeAsync(TollFreeNumberQueryForOrder query,
      CancellationToken? cancellationToken = null);
  }

  internal class AvailableNumberApi : ApiBase, IAvailableNumber
  {
    public Task<AvailableNumber[]> SearchLocalAsync(LocalNumberQuery query, CancellationToken? cancellationToken = null)
    {
      return Client.MakeJsonRequestAsync<AvailableNumber[]>(HttpMethod.Get, "/availableNumbers/local", cancellationToken,
        query);
    }

    public Task<AvailableNumber[]> SearchTollFreeAsync(TollFreeNumberQuery query,
      CancellationToken? cancellationToken = null)
    {
      return Client.MakeJsonRequestAsync<AvailableNumber[]>(HttpMethod.Get, "/availableNumbers/tollFree",
        cancellationToken, query);
    }

    public Task<OrderedNumber[]> SearchAndOrderLocalAsync(LocalNumberQueryForOrder query,
      CancellationToken? cancellationToken = null)
    {
      return Client.MakeJsonRequestAsync<OrderedNumber[]>(HttpMethod.Post, "/availableNumbers/local", cancellationToken,
        query);
    }

    public Task<OrderedNumber[]> SearchAndOrderTollFreeAsync(TollFreeNumberQueryForOrder query,
      CancellationToken? cancellationToken = null)
    {
      return Client.MakeJsonRequestAsync<OrderedNumber[]>(HttpMethod.Post, "/availableNumbers/tollFree",
        cancellationToken, query);
    }
  }

  /// <summary>
  ///   Search criterias for toll free numbers
  /// </summary>
  public class TollFreeNumberQueryForOrder
  {
    /// <summary>
    ///   The maximum number of numbers to return (default 10, maximum 5000).
    /// </summary>
    public int? Quantity { get; set; }
  }

  /// <summary>
  ///   Search criterias for toll free numbers
  /// </summary>
  public class TollFreeNumberQuery : TollFreeNumberQueryForOrder
  {
    /// <summary>
    ///   A number pattern that may include letters, digits, and the following wildcard characters
    /// </summary>
    public string Pattern { get; set; }
  }

  /// <summary>
  ///   Search criterias for local numbers
  /// </summary>
  public class LocalNumberQuery
  {
    /// <summary>
    ///   A city name.
    /// </summary>
    public string City { get; set; }


    /// <summary>
    ///   A two-letter US state abbreviation ("CA" for California).
    /// </summary>
    public string State { get; set; }

    /// <summary>
    ///   A 5-digit US ZIP code.
    /// </summary>
    public string Zip { get; set; }

    /// <summary>
    ///   A 3-digit telephone area code.
    /// </summary>
    public string AreaCode { get; set; }

    /// <summary>
    ///   It is defined as the first digits of a telephone number inside an area code for filtering the results. It must have
    ///   at least 3 digits and the areaCode field must be filled.
    /// </summary>
    public string LocalNumber { get; set; }

    /// <summary>
    ///   Boolean value to indicate that the search for available numbers must consider overlayed areas. Only applied for
    ///   localNumber searching.
    /// </summary>
    public bool? InLocalCallingArea { get; set; }

    /// <summary>
    ///   The maximum number of numbers to return (default 10, maximum 5000).
    /// </summary>
    public int? Quantity { get; set; }
  }

  /// <summary>
  ///   Search criterias for local numbers
  /// </summary>
  public class LocalNumberQueryForOrder : LocalNumberQuery
  {
    /// <summary>
    ///   A number pattern that may include letters, digits, and the following wildcard characters
    /// </summary>
    public string Pattern { get; set; }
  }


  /// <summary>
  ///   Available number result
  /// </summary>
  public class AvailableNumber
  {
    /// <summary>
    ///   The telephone number in E.164 format.
    /// </summary>
    public string Number { get; set; }

    /// <summary>
    ///   The telephone number in a friendly national format.
    /// </summary>
    public string NationalNumber { get; set; }

    /// <summary>
    ///   The telephone number in a friendly national format with some numbers replaced by letters if a pattern was used to
    ///   search the number.
    /// </summary>
    public string PatternMatch { get; set; }

    /// <summary>
    ///   The city of the phone number.
    /// </summary>
    public string City { get; set; }

    /// <summary>
    ///   Local access and transport area (LATA), represents an area within which a regional operating company is permitted to
    ///   offer exchange telecommunications and exchange access services.
    /// </summary>
    public string Lata { get; set; }

    /// <summary>
    ///   The rate center is a term used to identify a telephone local exchange service area.
    /// </summary>
    public string RateCenter { get; set; }

    /// <summary>
    ///   The state of the phone number.
    /// </summary>
    public string State { get; set; }

    /// <summary>
    ///   The monthly price for the phone number.
    /// </summary>
    public string Price { get; set; }
  }

  /// <summary>
  ///   Ordered number result
  /// </summary>
  public class OrderedNumber
  {
    /// <summary>
    ///   Id of ordered number
    /// </summary>
    public string Id => Location.Split('/').LastOrDefault();

    /// <summary>
    ///   The telephone number in E.164 format.
    /// </summary>
    public string Number { get; set; }

    /// <summary>
    ///   The telephone number in a friendly national format.
    /// </summary>
    public string NationalNumber { get; set; }

    /// <summary>
    ///   The monthly price for the phone number.
    /// </summary>
    public string Price { get; set; }

    /// <summary>
    ///   Url of number
    /// </summary>
    public string Location { get; set; }
  }
}
