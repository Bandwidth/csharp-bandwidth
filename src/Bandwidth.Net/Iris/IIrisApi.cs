﻿namespace Bandwidth.Net.Iris
{
  /// <summary>
  ///   Iris Api interface
  /// </summary>
  public interface IIrisApi
  {
    /// <summary>
    /// Access to Account Api
    /// </summary>
    IAccount Account { get;}

    /// <summary>
    /// Access to AvailableNpaNxx Api
    /// </summary>
    IAvailableNpaNxx AvailableNpaNxx { get; }

    /// <summary>
    /// Access to AvailableNumber Api
    /// </summary>
    IAvailableNumber AvailableNumber { get;}

    /// <summary>
    /// Access to City Api
    /// </summary>
    ICity City { get; }

    /// <summary>
    /// Access to CoveredRateCenter Api
    /// </summary>
    ICoveredRateCenter CoveredRateCenter { get; }

    /// <summary>
    /// Access to DiscNumber Api
    /// </summary>
    IDiscNumber DiscNumber { get; }

    /// <summary>
    /// Access to Disconnect Api
    /// </summary>
    IDisconnect Disconnect { get; }

    /// <summary>
    /// Access to Dlda Api
    /// </summary>
    IDlda Dlda { get; }

    /// <summary>
    /// Access to Host Api
    /// </summary>
    IHost Host { get; }

    /// <summary>
    /// Access to ImportToAccount Api
    /// </summary>
    IImportToAccount ImportToAccount { get; }

    /// <summary>
    /// Access to InserviceNumber Api
    /// </summary>
    IInserviceNumber InserviceNumber { get; }

    /// <summary>
    /// Access to Lidb Api
    /// </summary>
    ILidb Lidb { get; }

    /// <summary>
    /// Access to LineOptionOrder Api
    /// </summary>
    ILineOptionOrder LineOptionOrder { get; }

    /// <summary>
    /// Access to LnpChecker Api
    /// </summary>
    ILnpChecker LnpChecker { get; }

    /// <summary>
    /// Access to LsrOrder Api
    /// </summary>
    ILsrOrder LsrOrder { get; }

    /// <summary>
    /// Access to Order Api
    /// </summary>
    IOrder Order { get; }

    /// <summary>
    /// Access to Portin Api
    /// </summary>
    IPortin Portin { get; }

    /// <summary>
    /// Access to Portout Api
    /// </summary>
    IPortout Portout { get; }

    /// <summary>
    /// Access to RateCenter Api
    /// </summary>
    IRateCenter RateCenter { get; }

    /// <summary>
    /// Access to SipPeer Api
    /// </summary>
    ISipPeer SipPeer { get; }

    /// <summary>
    /// Access to Site Api
    /// </summary>
    ISite Site { get; }

  }

  /// <summary>
  ///   Auth data for Iris Api
  /// </summary>
  public class IrisAuthData
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
    ///   Optional base url of iris services
    /// </summary>
    public string BaseUrl { get; set; } = "https://api.inetwork.com/v1.0";
  }
}
