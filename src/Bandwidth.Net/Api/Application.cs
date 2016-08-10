using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Bandwidth.Net.Api
{
  /// <summary>
  ///   Access to Application Api
  /// </summary>
  public interface IApplication
  {
    /// <summary>
    ///   Get a list of applications
    /// </summary>
    /// <param name="query">Optional query parameters</param>
    /// <param name="cancellationToken">>Optional token to cancel async operation</param>
    /// <returns>Collection with <see cref="Application" /> instances</returns>
    IEnumerable<Application> List(ApplicationQuery query = null,
      CancellationToken? cancellationToken = null);

    /// <summary>
    ///   Create an application that can handle calls and messages for one of your phone number.
    /// </summary>
    /// <param name="data">Parameters of new application</param>
    /// <param name="cancellationToken">Optional token to cancel async operation</param>
    /// <returns>Instance of created application</returns>
    Task<ILazyInstance<Application>> CreateAsync(CreateApplicationData data, CancellationToken? cancellationToken = null);


    /// <summary>
    ///   Get information about an application
    /// </summary>
    /// <param name="applicationId">Id of application to get</param>
    /// <param name="cancellationToken">Optional token to cancel async operation</param>
    /// <returns>Task with <see cref="Application" />Application instance</returns>
    Task<Application> GetAsync(string applicationId, CancellationToken? cancellationToken = null);

    /// <summary>
    ///   Update an application
    /// </summary>
    /// <param name="applicationId">Id of application to change</param>
    /// <param name="data">Changed data</param>
    /// <param name="cancellationToken">Optional token to cancel async operation</param>
    /// <returns>Task instance for async operation</returns>
    Task UpdateAsync(string applicationId, UpdateApplicationData data, CancellationToken? cancellationToken = null);

    /// <summary>
    ///   Delete an application
    /// </summary>
    /// <param name="applicationId">Id of application to change</param>
    /// <param name="cancellationToken">Optional token to cancel async operation</param>
    /// <returns>Task instance for async operation</returns>
    Task DeleteAsync(string applicationId, CancellationToken? cancellationToken = null);
  }

  internal class ApplicationApi : ApiBase, IApplication
  {
    public IEnumerable<Application> List(ApplicationQuery query = null, CancellationToken? cancellationToken = null)
    {
      return new LazyEnumerable<Application>(Client,
        () =>
          Client.MakeJsonRequestAsync(HttpMethod.Get, $"/users/{Client.UserId}/applications", cancellationToken, query));
    }

    public async Task<ILazyInstance<Application>> CreateAsync(CreateApplicationData data,
      CancellationToken? cancellationToken = null)
    {
      var id = await Client.MakePostJsonRequestAsync($"/users/{Client.UserId}/applications", cancellationToken, data);
      return new LazyInstance<Application>(id, () => GetAsync(id));
    }

    public Task<Application> GetAsync(string applicationId, CancellationToken? cancellationToken = null)
    {
      return Client.MakeJsonRequestAsync<Application>(HttpMethod.Get,
        $"/users/{Client.UserId}/applications/{applicationId}", cancellationToken);
    }

    public Task UpdateAsync(string applicationId, UpdateApplicationData data,
      CancellationToken? cancellationToken = null)
    {
      return Client.MakeJsonRequestAsync<Application>(HttpMethod.Post,
        $"/users/{Client.UserId}/applications/{applicationId}", cancellationToken, null, data);
    }

    public Task DeleteAsync(string applicationId, CancellationToken? cancellationToken = null)
    {
      return Client.MakeJsonRequestAsync<Application>(HttpMethod.Delete,
        $"/users/{Client.UserId}/applications/{applicationId}", cancellationToken);
    }
  }

  /// <summary>
  ///   Application information
  /// </summary>
  public class Application
  {
    /// <summary>
    ///   The unique identifier for the application.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    ///   A name you choose for this application.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///   A URL where call events will be sent for an inbound call. This is the endpoint where the Application Platform will
    ///   send all call events. Either incomingCallUrl or incomingMessageUrl is required.
    /// </summary>
    public string IncomingCallUrl { get; set; }

    /// <summary>
    ///   Determine how long should the platform wait for incomingCallUrl's response before timing out in milliseconds.
    /// </summary>
    public int IncomingCallUrlCallbackTimeout { get; set; }

    /// <summary>
    ///   The URL used to send the callback event if the request to incomingCallUrl fails.
    /// </summary>
    public string IncomingCallFallbackUrl { get; set; }

    /// <summary>
    ///   A URL where message events will be sent for an inbound message.
    /// </summary>
    public string IncomingMessageUrl { get; set; }

    /// <summary>
    ///   Determine how long should the platform wait for incomingMessageUrl's response before timing out in milliseconds.
    /// </summary>
    public int IncomingMessageUrlCallbackTimeout { get; set; }

    /// <summary>
    ///   The URL used to send the callback event if the request to incomingMessageUrl fails.
    /// </summary>
    public string IncomingMessageFallbackUrl { get; set; }

    /// <summary>
    ///   Determine if the callback event should be sent via HTTP GET or HTTP POST
    /// </summary>
    public CallbackHttpMethod CallbackHttpMethod { get; set; }

    /// <summary>
    ///   Determines whether or not an incoming call should be automatically answered.
    /// </summary>
    public bool AutoAnswer { get; set; }
  }

  /// <summary>
  ///   Query to get applications
  /// </summary>
  public class ApplicationQuery
  {
    /// <summary>
    ///   Used for pagination to indicate the size of each page requested for querying a list of applications. If no value is
    ///   specified the default value is 25 (maximum value 1000).
    /// </summary>
    public int? Size { get; set; }
  }

  /// <summary>
  ///   Parameters to create an application
  /// </summary>
  public class CreateApplicationData
  {
    /// <summary>
    ///   A name you choose for this application.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///   A URL where call events will be sent for an inbound call. This is the endpoint where the Application Platform will
    ///   send all call events. Either incomingCallUrl or incomingMessageUrl is required.
    /// </summary>
    public string IncomingCallUrl { get; set; }

    /// <summary>
    ///   Determine how long should the platform wait for incomingCallUrl's response before timing out in milliseconds.
    /// </summary>
    public int? IncomingCallUrlCallbackTimeout { get; set; }

    /// <summary>
    ///   The URL used to send the callback event if the request to incomingCallUrl fails.
    /// </summary>
    public string IncomingCallFallbackUrl { get; set; }

    /// <summary>
    ///   A URL where message events will be sent for an inbound message.
    /// </summary>
    public string IncomingMessageUrl { get; set; }

    /// <summary>
    ///   Determine how long should the platform wait for incomingMessageUrl's response before timing out in milliseconds.
    /// </summary>
    public int? IncomingMessageUrlCallbackTimeout { get; set; }

    /// <summary>
    ///   The URL used to send the callback event if the request to incomingMessageUrl fails.
    /// </summary>
    public string IncomingMessageFallbackUrl { get; set; }

    /// <summary>
    ///   Determine if the callback event should be sent via HTTP GET or HTTP POST
    /// </summary>
    public CallbackHttpMethod? CallbackHttpMethod { get; set; }

    /// <summary>
    ///   Determines whether or not an incoming call should be automatically answered.
    /// </summary>
    public bool? AutoAnswer { get; set; }
  }

  /// <summary>
  ///   Parameters of an application to change
  /// </summary>
  public class UpdateApplicationData : CreateApplicationData
  {
  }

  /// <summary>
  ///   Available HTTP methods for callbacks
  /// </summary>
  public enum CallbackHttpMethod
  {
    /// <summary>
    ///   HTTP POST
    /// </summary>
    Post,

    /// <summary>
    ///   HTTP GET
    /// </summary>
    Get
  }
}