using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Bandwidth.Net.Api
{
  /// <summary>
  ///   Access to Endpoint Api
  /// </summary>
  public interface IEndpoint
  {
    /// <summary>
    ///   Get a list of endpoints
    /// </summary>
    /// <param name="domainId">Id of endpoint's domain</param>
    /// <param name="query">Optional query parameters</param>
    /// <param name="cancellationToken">>Optional token to cancel async operation</param>
    /// <returns>Collection with <see cref="Endpoint" /> instances</returns>
    /// <example>
    ///   <code>
    /// var endpoints = client.Endpoint.List("domainId"); 
    /// </code>
    /// </example>
    IEnumerable<Endpoint> List(string domainId, EndpointQuery query = null,
      CancellationToken? cancellationToken = null);

    /// <summary>
    ///   Create a endpoint.
    /// </summary>
    /// <param name="data">Parameters of new endpoint</param>
    /// <param name="cancellationToken">Optional token to cancel async operation</param>
    /// <returns>Created endpoint Id</returns>
    /// <example>
    ///   <code>
    /// var endpointId = await client.Endpoint.CreateAsync(new CreateEndpointData{ Name = "endpoint", DomainId="domainId", ApplicationId="applicationId", Credentials = new CreateEndpointCredentials{Password = "123456"}});
    /// </code>
    /// </example>
    Task<string> CreateAsync(CreateEndpointData data, CancellationToken? cancellationToken = null);


    /// <summary>
    ///   Get information about a endpoint
    /// </summary>
    /// <param name="domainId">Id of endpoint's domain</param>
    /// <param name="endpointId">Id of endpoint to get</param>
    /// <param name="cancellationToken">Optional token to cancel async operation</param>
    /// <returns>Task with <see cref="Endpoint" />Endpoint instance</returns>
    /// <example>
    ///   <code>
    /// var endpoint = await client.Endpoint.GetAsync("domainId", "endpointId");
    /// </code>
    /// </example>
    Task<Endpoint> GetAsync(string domainId, string endpointId, CancellationToken? cancellationToken = null);

    /// <summary>
    ///   Update the endpoint
    /// </summary>
    /// <param name="domainId">Id of endpoint's domain</param>
    /// <param name="endpointId">Id of endpoint to change</param>
    /// <param name="data">Changed data</param>
    /// <param name="cancellationToken">Optional token to cancel async operation</param>
    /// <returns>Task instance for async operation</returns>
    /// <example>
    ///   <code>
    /// await client.Endpoint.UpdateAsync("domainId", "endpointId", new UpdateEndpointData {Description = "My SIP account"});
    /// </code>
    /// </example>
    Task UpdateAsync(string domainId, string endpointId, UpdateEndpointData data,
      CancellationToken? cancellationToken = null);

    /// <summary>
    ///   Remove the endpoint
    /// </summary>
    /// <param name="domainId">Id of endpoint's domain</param>
    /// <param name="endpointId">Id of endpoint to remove</param>
    /// <param name="cancellationToken">Optional token to cancel async operation</param>
    /// <returns>Task instance for async operation</returns>
    /// <example>
    ///   <code>
    /// await client.Endpoint.DeleteAsync("domainId", "endpointId");
    /// </code>
    /// </example>
    Task DeleteAsync(string domainId, string endpointId, CancellationToken? cancellationToken = null);

    /// <summary>
    ///   Create auth token for the endpoint (usefull for client applications instead of using SIP account password directly)
    /// </summary>
    /// <param name="domainId">Id of endpoint's domain</param>
    /// <param name="endpointId">Id of endpoint</param>
    /// <param name="data">Optional parameters of new token</param>
    /// <param name="cancellationToken">Optional token to cancel async operation</param>
    /// <returns>Created auth token</returns>
    /// <example>
    ///   <code>
    /// var token = await client.Endpoint.CreateAuthTokenAsync("domainId", "endpointId");
    /// </code>
    /// </example>
    Task<EndpointAuthToken> CreateAuthTokenAsync(string domainId, string endpointId, CreateAuthTokenData data = null,
      CancellationToken? cancellationToken = null);
  }

  internal class EndpointApi : ApiBase, IEndpoint
  {
    public IEnumerable<Endpoint> List(string domainId, EndpointQuery query = null,
      CancellationToken? cancellationToken = null)
    {
      return new LazyEnumerable<Endpoint>(Client,
        () =>
          Client.MakeJsonRequestAsync(HttpMethod.Get, $"/users/{Client.CatapultAuthData.UserId}/domains/{domainId}/endpoints", Client.CatapultAuthData,
            cancellationToken, query));
    }

    public Task<string> CreateAsync(CreateEndpointData data,
      CancellationToken? cancellationToken = null)
    {
      return Client.MakePostJsonRequestAsync($"/users/{Client.CatapultAuthData.UserId}/domains/{data.DomainId}/endpoints", Client.CatapultAuthData, cancellationToken, 
            data);
    }

    public Task<Endpoint> GetAsync(string domainId, string endpointId, CancellationToken? cancellationToken = null)
    {
      return Client.MakeJsonRequestAsync<Endpoint>(HttpMethod.Get,
        $"/users/{Client.CatapultAuthData.UserId}/domains/{domainId}/endpoints/{endpointId}", Client.CatapultAuthData, cancellationToken);
    }

    public Task UpdateAsync(string domainId, string endpointId, UpdateEndpointData data,
      CancellationToken? cancellationToken = null)
    {
      return Client.MakeJsonRequestWithoutResponseAsync(HttpMethod.Post,
        $"/users/{Client.CatapultAuthData.UserId}/domains/{domainId}/endpoints/{endpointId}", Client.CatapultAuthData, cancellationToken, null, data);
    }

    public Task DeleteAsync(string domainId, string endpointId, CancellationToken? cancellationToken = null)
    {
      return Client.MakeJsonRequestWithoutResponseAsync(HttpMethod.Delete,
        $"/users/{Client.CatapultAuthData.UserId}/domains/{domainId}/endpoints/{endpointId}", Client.CatapultAuthData, cancellationToken);
    }

    public Task<EndpointAuthToken> CreateAuthTokenAsync(string domainId, string endpointId,
      CreateAuthTokenData data = null,
      CancellationToken? cancellationToken = null)
    {
      return Client.MakeJsonRequestAsync<EndpointAuthToken>(HttpMethod.Post,
        $"/users/{Client.CatapultAuthData.UserId}/domains/{domainId}/endpoints/{endpointId}/tokens", Client.CatapultAuthData, cancellationToken, null, data);
    }
  }


  /// <summary>
  ///   Endpoint information
  /// </summary>
  public class Endpoint
  {
    /// <summary>
    ///   The unique identifier for the endpoint.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    ///   Name of the endpoint
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///   Description of the endpoint
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    ///   Domain Id of the endpoint
    /// </summary>
    public string DomainId { get; set; }

    /// <summary>
    ///   Application Id of the endpoint
    /// </summary>
    public string ApplicationId { get; set; }

    /// <summary>
    ///   If endpoint enabled/disabled
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    ///   SIP URI of the endpoint
    /// </summary>
    public string SipUri { get; set; }

    /// <summary>
    ///   Credentials data of the endpoint
    /// </summary>
    public EndpointCredentials Credentials { get; set; }
  }

  /// <summary>
  ///   Credentials data of the endpoint
  /// </summary>
  public class EndpointCredentials
  {
    /// <summary>
    ///   Realm string
    /// </summary>
    public string Realm { get; set; }

    /// <summary>
    ///   User name
    /// </summary>
    [JsonProperty("username")]
    public string UserName { get; set; }
  }

  /// <summary>
  ///   Query to get endpoints
  /// </summary>
  public class EndpointQuery
  {
    /// <summary>
    ///   Used for pagination to indicate the size of each page requested for querying a list of endpoints. If no value is
    ///   specified the default value is 25 (maximum value 1000).
    /// </summary>
    public int? Size { get; set; }
  }

  /// <summary>
  ///   Parameters to create an endpoint
  /// </summary>
  public class CreateEndpointData
  {
    /// <summary>
    ///   Name of the endpoint
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///   Description of the endpoint
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    ///   Domain Id of the endpoint
    /// </summary>
    public string DomainId { get; set; }

    /// <summary>
    ///   Application Id of the endpoint
    /// </summary>
    public string ApplicationId { get; set; }

    /// <summary>
    ///   Credentials data
    /// </summary>
    public CreateEndpointCredentials Credentials { get; set; }
  }

  /// <summary>
  ///   Parameters of a endpoint to change
  /// </summary>
  public class UpdateEndpointData
  {
    /// <summary>
    ///   Description of the endpoint
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    ///   Application Id of the endpoint
    /// </summary>
    public string ApplicationId { get; set; }

    /// <summary>
    ///   If endpoint enabled/disabled
    /// </summary>
    public bool? Enabled { get; set; }

    /// <summary>
    ///   Credentials data
    /// </summary>
    public UpdateEndpointCredentials Credentials { get; set; }
  }

  /// <summary>
  ///   Endpoint auth token
  /// </summary>
  public class EndpointAuthToken
  {
    /// <summary>
    ///   Token value
    /// </summary>
    public string Token { get; set; }

    /// <summary>
    ///   Expiration time of token in seconds
    /// </summary>
    public int Expires { get; set; }
  }

  /// <summary>
  ///   Parameters to create new auth token for endpoint
  /// </summary>
  public class CreateAuthTokenData
  {
    /// <summary>
    ///   Expiration time of token in seconds
    /// </summary>
    public int Expires { get; set; }
  }

  /// <summary>
  ///   Credentials data on creating endpoint
  /// </summary>
  public class CreateEndpointCredentials
  {
    /// <summary>
    ///   Password for SIP account
    /// </summary>
    public string Password { get; set; }
  }

  /// <summary>
  ///   Credentials data on changing endpoint
  /// </summary>
  public class UpdateEndpointCredentials : CreateEndpointCredentials
  {
  }
}
