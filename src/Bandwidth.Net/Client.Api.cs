using Bandwidth.Net.Api;

namespace Bandwidth.Net
{
  public partial class Client
  {
    /// <summary>
    /// Access to Account Api
    /// </summary>
    public IAccount Account { get; private set; }

    /// <summary>
    /// Access to Application Api
    /// </summary>
    public IApplication Application { get; private set; }

    /// <summary>
    /// Access to AvailableNumber Api
    /// </summary>
    public IAvailableNumber AvailableNumber { get; private set; }


    private void SetupApis()
    {
      Account = new AccountApi {Client = this};
      Application = new ApplicationApi { Client = this };
      AvailableNumber = new AvailableNumberApi { Client = this };
    }
  }
}
