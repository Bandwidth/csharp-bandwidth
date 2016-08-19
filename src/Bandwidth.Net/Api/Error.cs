using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Bandwidth.Net.Api
{
  /// <summary>
  ///   Access to Error Api
  /// </summary>
  public interface IError
  {
    /// <summary>
    ///   Get a list of errors
    /// </summary>
    /// <param name="query">Optional query parameters</param>
    /// <param name="cancellationToken">>Optional token to cancel async operation</param>
    /// <returns>Collection with <see cref="Error" /> instances</returns>
    /// <example>
    ///   <code>
    /// var errors = client.Error.List(); 
    /// </code>
    /// </example>
    IEnumerable<Error> List(ErrorQuery query = null,
      CancellationToken? cancellationToken = null);

    /// <summary>
    ///   Get information about an error
    /// </summary>
    /// <param name="errorId">Id of error to get</param>
    /// <param name="cancellationToken">Optional token to cancel async operation</param>
    /// <returns>Task with <see cref="Error" />Error instance</returns>
    /// <example>
    ///   <code>
    /// var error = await client.Error.GetAsync("errorId");
    /// </code>
    /// </example>
    Task<Error> GetAsync(string errorId, CancellationToken? cancellationToken = null);
  }

  internal class ErrorApi : ApiBase, IError
  {
    public IEnumerable<Error> List(ErrorQuery query = null, CancellationToken? cancellationToken = null)
    {
      return new LazyEnumerable<Error>(Client,
        () =>
          Client.MakeJsonRequestAsync(HttpMethod.Get, $"/users/{Client.UserId}/errors", cancellationToken, query));
    }

    public Task<Error> GetAsync(string errorId, CancellationToken? cancellationToken = null)
    {
      return Client.MakeJsonRequestAsync<Error>(HttpMethod.Get,
        $"/users/{Client.UserId}/errors/{errorId}", cancellationToken);
    }
  }


  /// <summary>
  ///   Error information
  /// </summary>
  public class Error
  {
    /// <summary>
    ///   The unique identifier for the error.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    ///   The time the error occurred
    /// </summary>
    public DateTime Time { get; set; }

    /// <summary>
    ///   The error category
    /// </summary>
    public ErrorCategory Category { get; set; }

    /// <summary>
    ///   A specific error code string that identifies the type of error.
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    ///   A message that describes the error condition in detail.
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    ///   A list of name/value pairs of additional details that may help you debug the error.
    /// </summary>
    public ErrorDetail[] Details { get; set; }
  }

  /// <summary>
  ///   Possible error categories
  /// </summary>
  public enum ErrorCategory
  {
    /// <summary>
    ///   Authentication
    /// </summary>
    Authentication,

    /// <summary>
    ///   Authorization
    /// </summary>
    Authorization,

    /// <summary>
    ///   NotFound
    /// </summary>
    NotFound,

    /// <summary>
    ///   BadRequest
    /// </summary>
    BadRequest,

    /// <summary>
    ///   Conflict
    /// </summary>
    Conflict,

    /// <summary>
    ///   Unavailable
    /// </summary>
    Unavailable,

    /// <summary>
    ///   Credit
    /// </summary>
    Credit,

    /// <summary>
    ///   Limit
    /// </summary>
    Limit,

    /// <summary>
    ///   Payment
    /// </summary>
    Payment
  }

  /// <summary>
  ///   Query to get errors
  /// </summary>
  public class ErrorQuery
  {
    /// <summary>
    ///   Used for pagination to indicate the size of each page requested for querying a list of errors. If no value is
    ///   specified the default value is 25 (maximum value 1000).
    /// </summary>
    public int? Size { get; set; }
  }

  /// <summary>
  ///   Error detail (name/value pair)
  /// </summary>
  public class ErrorDetail
  {
    /// <summary>
    ///   Name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///   Value
    /// </summary>
    public string Value { get; set; }
  }
}
