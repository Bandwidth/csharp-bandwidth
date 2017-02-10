using Bandwidth.Net.Api;

namespace Bandwidth.Net
{
  public partial class Client
  {

    /// <summary>
    /// Access to Account Api
    /// </summary>
    public IIrisAccount IrisAccount { get; private set; }

    /// <summary>
    /// Access to AvailableNpaNxx Api
    /// </summary>
    public IAvailableNpaNxx AvailableNpaNxx { get; private set; }

    /// <summary>
    /// Access to AvailableNumber Api
    /// </summary>
    public IIrisAvailableNumber IrisAvailableNumber { get; private set; }

    /// <summary>
    /// Access to City Api
    /// </summary>
    public ICity City { get; private set; }

    private void SetupIrisApis()
    {
      IrisAccount = new IrisAccountApi { Client = this };
      AvailableNpaNxx = new AvailableNpaNxxApi {Client = this};
      IrisAvailableNumber = new IrisAvailableNumberApi {Client = this};
      City = new CityApi {Client = this};
    }
  }
}
