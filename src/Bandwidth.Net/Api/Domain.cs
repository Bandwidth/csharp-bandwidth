using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Bandwidth.Net.Api
{
  /// <summary>
  ///   Access to Domain Api
  /// </summary>
  public interface IDomain
  {
    /// <summary>
    ///   Get a list of domains
    /// </summary>
    /// <param name="query">Optional query parameters</param>
    /// <param name="cancellationToken">>Optional token to cancel async operation</param>
    /// <returns>Collection with <see cref="DomainData" /> instances</returns>
    /// <example>
    ///   <code>
    /// var domains = client.Domain.List(); 
    /// </code>
    /// </example>
    IEnumerable<DomainData> List(DomainQuery query = null,
      CancellationToken? cancellationToken = null);

    /// <summary>
    ///   Create a domain.
    /// </summary>
    /// <param name="data">Parameters of new domain</param>
    /// <param name="cancellationToken">Optional token to cancel async operation</param>
    /// <returns>Created domain id</returns>
    /// <example>
    ///   <code>
    /// var domainId = await client.Domain.CreateAsync(new CreateDomainData{ Name = "new-domain"});
    /// </code>
    /// </example>
    Task<string> CreateAsync(CreateDomainData data, CancellationToken? cancellationToken = null);


    /// <summary>
    ///   Remove the domain
    /// </summary>
    /// <param name="domainId">Id of domain to remove</param>
    /// <param name="cancellationToken">Optional token to cancel async operation</param>
    /// <returns>Task instance for async operation</returns>
    /// <example>
    ///   <code>
    /// await client.Domain.DeleteAsync("domainId");
    /// </code>
    /// </example>
    Task DeleteAsync(string domainId, CancellationToken? cancellationToken = null);
  }

  internal class DomainApi : ApiBase, IDomain
  {
    public IEnumerable<DomainData> List(DomainQuery query = null, CancellationToken? cancellationToken = null)
    {
      return new LazyEnumerable<DomainData>(Client,
        () =>
          Client.MakeJsonRequestAsync(HttpMethod.Get, $"/users/{Client.UserId}/domains", cancellationToken, query));
    }

    public Task<string> CreateAsync(CreateDomainData data,
      CancellationToken? cancellationToken = null)
    {
      return Client.MakePostJsonRequestAsync($"/users/{Client.UserId}/domains", cancellationToken, data);
    }

    public Task DeleteAsync(string domainId, CancellationToken? cancellationToken = null)
    {
      return Client.MakeJsonRequestWithoutResponseAsync(HttpMethod.Delete,
        $"/users/{Client.UserId}/domains/{domainId}", cancellationToken);
    }
  }


  /// <summary>
  ///   Domain information
  /// </summary>
  public class DomainData
  {
    /// <summary>
    ///   The unique identifier for the domain.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    ///The name is a unique URI to be used in DNS lookups
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Description of the domain
    /// </summary>
    public string Description { get; set; }
    
  }

  /// <summary>
  /// Data to create domain
  /// </summary>
  public class CreateDomainData
  {
    /// <summary>
    ///The name is a unique URI to be used in DNS lookups
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Description of the domain
    /// </summary>
    public string Description { get; set; }
  }

  /// <summary>
  /// Query parameters to domain search
  /// </summary>
  public class DomainQuery
  {
    /// <summary>
    /// Used for pagination to indicate the size of each page requested for querying a list of domain. If no value is specified the default value is 25 (maximum value 100).
    /// </summary>
    public int Size { get; set; }
  }

}
