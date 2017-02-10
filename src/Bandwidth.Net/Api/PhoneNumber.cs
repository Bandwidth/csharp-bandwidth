using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Bandwidth.Net.Api
{
  /// <summary>
  ///   Access to PhoneNumber Api
  /// </summary>
  public interface IPhoneNumber
  {
    /// <summary>
    ///   Get a list of users phone numbers
    /// </summary>
    /// <param name="query">Optional query parameters</param>
    /// <param name="cancellationToken">>Optional token to cancel async operation</param>
    /// <returns>Collection with <see cref="PhoneNumber" /> instances</returns>
    /// <example>
    ///   <code>
    /// var phoneNumbers = client.PhoneNumber.List(); 
    /// </code>
    /// </example>
    IEnumerable<PhoneNumber> List(PhoneNumberQuery query = null,
      CancellationToken? cancellationToken = null);

    /// <summary>
    ///   Allocate a number so you can use it.
    /// </summary>
    /// <param name="data">Parameters of new phone number</param>
    /// <param name="cancellationToken">Optional token to cancel async operation</param>
    /// <returns>Created phone number id</returns>
    /// <example>
    ///   <code>
    /// var phoneNumberId = await client.PhoneNumber.CreateAsync(new CreatePhoneNumberData{ Number = "+1234567890"});
    /// </code>
    /// </example>
    Task<string> CreateAsync(CreatePhoneNumberData data, CancellationToken? cancellationToken = null);


    /// <summary>
    ///   Get information about a phone number
    /// </summary>
    /// <param name="phoneNumberId">Id of phone number to get</param>
    /// <param name="cancellationToken">Optional token to cancel async operation</param>
    /// <returns>Task with <see cref="PhoneNumber" />PhoneNumber instance</returns>
    /// <example>
    ///   <code>
    /// var phoneNumber = await client.PhoneNumber.GetAsync("phoneNumberId");
    /// </code>
    /// </example>
    Task<PhoneNumber> GetAsync(string phoneNumberId, CancellationToken? cancellationToken = null);

    /// <summary>
    ///   Make changes to a number
    /// </summary>
    /// <param name="phoneNumberId">Id of phoneNumber to change</param>
    /// <param name="data">Changed data</param>
    /// <param name="cancellationToken">Optional token to cancel async operation</param>
    /// <returns>Task instance for async operation</returns>
    /// <example>
    ///   <code>
    /// await client.PhoneNumber.UpdateAsync("phoneNumberId", new UpdatePhoneNumberData {ApplicationId = "appId"});
    /// </code>
    /// </example>
    Task UpdateAsync(string phoneNumberId, UpdatePhoneNumberData data, CancellationToken? cancellationToken = null);


    /// <summary>
    ///   Remove a phone number
    /// </summary>
    /// <param name="phoneNumberId">Id of phoneNumber to remove</param>
    /// <param name="cancellationToken">Optional token to cancel async operation</param>
    /// <returns>Task instance for async operation</returns>
    /// <example>
    ///   <code>
    /// await client.PhoneNumber.DeleteAsync("phoneNumberId");
    /// </code>
    /// </example>
    Task DeleteAsync(string phoneNumberId, CancellationToken? cancellationToken = null);
  }

  internal class PhoneNumberApi : ApiBase, IPhoneNumber
  {
    public IEnumerable<PhoneNumber> List(PhoneNumberQuery query = null, CancellationToken? cancellationToken = null)
    {
      return new LazyEnumerable<PhoneNumber>(Client,
        () =>
          Client.MakeJsonRequestAsync(HttpMethod.Get, $"/users/{Client.CatapultAuthData.UserId}/phoneNumbers", Client.CatapultAuthData, cancellationToken, query));
    }

    public Task<string> CreateAsync(CreatePhoneNumberData data,
      CancellationToken? cancellationToken = null)
    {
      return Client.MakePostJsonRequestAsync($"/users/{Client.CatapultAuthData.UserId}/phoneNumbers", Client.CatapultAuthData, cancellationToken, data);
    }

    public Task<PhoneNumber> GetAsync(string phoneNumberId, CancellationToken? cancellationToken = null)
    {
      return Client.MakeJsonRequestAsync<PhoneNumber>(HttpMethod.Get,
        $"/users/{Client.CatapultAuthData.UserId}/phoneNumbers/{phoneNumberId}", Client.CatapultAuthData, cancellationToken);
    }

    public Task UpdateAsync(string phoneNumberId, UpdatePhoneNumberData data,
      CancellationToken? cancellationToken = null)
    {
      return Client.MakeJsonRequestWithoutResponseAsync(HttpMethod.Post,
        $"/users/{Client.CatapultAuthData.UserId}/phoneNumbers/{phoneNumberId}", Client.CatapultAuthData, cancellationToken, null, data);
    }

    public Task DeleteAsync(string phoneNumberId, CancellationToken? cancellationToken = null)
    {
      return Client.MakeJsonRequestWithoutResponseAsync(HttpMethod.Delete,
        $"/users/{Client.CatapultAuthData.UserId}/phoneNumbers/{phoneNumberId}", Client.CatapultAuthData, cancellationToken);
    }
  }


  /// <summary>
  ///   Phone number information
  /// </summary>
  public class PhoneNumber
  {
    /// <summary>
    ///   The unique identifier for the phone number.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    ///   A name you choose for this number.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///   The telephone number in E.164 format.
    /// </summary>
    public string Number { get; set; }

    /// <summary>
    ///   The telephone number in a friendly national format, e.g. (555) 5555-5555
    /// </summary>
    public string NationalNumber { get; set; }

    /// <summary>
    ///   The city of the phone number.
    /// </summary>
    public string City { get; set; }

    /// <summary>
    ///   The state of the phone number.
    /// </summary>
    public string State { get; set; }

    /// <summary>
    ///   The unique id of an Application you want to associate with this number.
    /// </summary>
    public string ApplicationId { get; set; }

    /// <summary>
    ///   Number to transfer an incoming call when the callback/fallback events can't be delivered.
    /// </summary>
    public string FallbackNumber { get; set; }

    /// <summary>
    ///   The monthly price for this number.
    /// </summary>
    public string Price { get; set; }

    /// <summary>
    ///   Date when the number was created.
    /// </summary>
    public DateTime CreatedTime { get; set; }

    /// <summary>
    ///   The phone number state
    /// </summary>
    public PhoneNumberState NumberState { get; set; }
  }


  /// <summary>
  ///   Parameters to create an phone number
  /// </summary>
  public class CreatePhoneNumberData
  {
    /// <summary>
    ///   A name you choose for this number.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///   The telephone number in E.164 format.
    /// </summary>
    public string Number { get; set; }

    /// <summary>
    ///   The unique id of an Application you want to associate with this number.
    /// </summary>
    public string ApplicationId { get; set; }

    /// <summary>
    ///   Number to transfer an incoming call when the callback/fallback events can't be delivered.
    /// </summary>
    public string FallbackNumber { get; set; }
  }

  /// <summary>
  ///   Parameters of a phone number to change
  /// </summary>
  public class UpdatePhoneNumberData
  {
    /// <summary>
    ///   A name you choose for this number.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///   The unique id of an Application you want to associate with this number.
    /// </summary>
    public string ApplicationId { get; set; }

    /// <summary>
    ///   Number to transfer an incoming call when the callback/fallback events can't be delivered.
    /// </summary>
    public string FallbackNumber { get; set; }
  }

  /// <summary>
  ///   Phone number states
  /// </summary>
  public enum PhoneNumberState
  {
    /// <summary>
    ///   Active number
    /// </summary>
    Enabled,

    /// <summary>
    ///   Released number
    /// </summary>
    Released
  }

  /// <summary>
  ///   Query to get phone numbers
  /// </summary>
  public class PhoneNumberQuery
  {
    /// <summary>
    ///   Used for pagination to indicate the size of each page requested for querying a list of numbers. If no value is
    ///   specified the default value is 25 (maximum value 1000).
    /// </summary>
    public int? Size { get; set; }

    /// <summary>
    ///   Used to filter the retrieved list of numbers by an associated application ID.
    /// </summary>
    public string ApplicationId { get; set; }

    /// <summary>
    ///   Used to filter the retrieved list of numbers allocated for the authenticated user by a US state.
    /// </summary>
    public string State { get; set; }

    /// <summary>
    ///   Used to filter the retrieved list of numbers allocated for the authenticated user by it's name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///   Used to filter the retrieved list of numbers allocated for the authenticated user by it's city.
    /// </summary>
    public string City { get; set; }

    /// <summary>
    ///   Used to filter the retrieved list of numbers allocated for the authenticated user by the number state.
    /// </summary>
    public PhoneNumberState? NumberState { get; set; }
  }
}
