using System;
using System.Net.Http.Headers;
using System.Text;

namespace Bandwidth.Net
{
  /// <summary>
  ///   Auth data
  /// </summary>
  public interface IAuthData
  {
    /// <summary>
    ///   Base url of service
    /// </summary>
    string BaseUrl { get; set; }

    /// <summary>
    ///   Validate if parameters are correct
    /// </summary>
    void Validate();

    /// <summary>
    ///   Authentication Header
    /// </summary>
    AuthenticationHeaderValue AuthenticationHeader { get; }
  }

  /// <summary>
  ///   Auth data for Catapult
  /// </summary>
  public class CatapultAuthData : IAuthData
  {
    /// <summary>
    ///   User ID (not your user name)
    /// </summary>
    public string UserId { get; set; }

    /// <summary>
    ///   Api Token
    /// </summary>
    public string ApiToken { get; set; }

    /// <summary>
    ///   Api Secret
    /// </summary>
    public string ApiSecret { get; set; }

    /// <summary>
    ///   Authentication Header
    /// </summary>
    public AuthenticationHeaderValue AuthenticationHeader => new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{ApiToken}:{ApiSecret}")));

    /// <summary>
    ///   Optional base url of catapult services
    /// </summary>
    public string BaseUrl { get; set; } = "https://api.catapult.inetwork.com/v1";

    /// <summary>
    ///   Validate if parameters are correct
    /// </summary>
    public void Validate()
    {
      if (string.IsNullOrEmpty(UserId) || string.IsNullOrEmpty(ApiToken) || string.IsNullOrEmpty(ApiSecret))
      {
        throw new MissingCredentialsException("Catapult");
      }
      if (string.IsNullOrEmpty(BaseUrl))
      {
        throw new InvalidBaseUrlException();
      }
    }

  }

  /// <summary>
  ///   Auth data for Iris
  /// </summary>
  public class IrisAuthData : IAuthData
  {
    /// <summary>
    ///   Account ID
    /// </summary>
    public string AccountId { get; set; }

    /// <summary>
    ///   User Name
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    ///   Password
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    ///   Validate if parameters are correct
    /// </summary>
    public void Validate()
    {
      if (string.IsNullOrEmpty(AccountId) || string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password))
      {
        throw new MissingCredentialsException("Iris");
      }
      if (string.IsNullOrEmpty(BaseUrl))
      {
        throw new InvalidBaseUrlException();
      }
    }

    /// <summary>
    ///   Optional base url of iris services
    /// </summary>
    public string BaseUrl { get; set; } = "https://api.inetwork.com/v1.0";

    /// <summary>
    ///   Authentication Header
    /// </summary>
    public AuthenticationHeaderValue AuthenticationHeader => new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{UserName}:{Password}")));
  }
}
