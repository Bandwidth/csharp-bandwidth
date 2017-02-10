using Bandwidth.Net.Api;

namespace Bandwidth.Net
{
  public partial class Client
  {

    /// <summary>
    /// Access to Error Api
    /// </summary>
    public IError Error { get; private set; }

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

    /// <summary>
    /// Access to Bridge Api
    /// </summary>
    public IBridge Bridge { get; private set; }

    /// <summary>
    /// Access to Domain Api
    /// </summary>
    public IDomain Domain { get; private set; }

    /// <summary>
    /// Access to Call Api
    /// </summary>
    public ICall Call { get; private set; }


    /// <summary>
    /// Access to Conference Api
    /// </summary>
    public IConference Conference { get; private set; }

    /// <summary>
    /// Access to Message Api
    /// </summary>
    public IMessage Message { get; private set; }

    /// <summary>
    /// Access to NumberInfo Api
    /// </summary>
    public INumberInfo NumberInfo { get; private set; }

    /// <summary>
    /// Access to PhoneNumber Api
    /// </summary>
    public IPhoneNumber PhoneNumber { get; private set; }

    /// <summary>
    /// Access to Recording Api
    /// </summary>
    public IRecording Recording { get; private set; }

    /// <summary>
    /// Access to Transcription Api
    /// </summary>
    public ITranscription Transcription { get; private set; }

    /// <summary>
    /// Access to Media Api
    /// </summary>
    public IMedia Media { get; private set; }

    /// <summary>
    /// Access to Endpoint Api
    /// </summary>
    public IEndpoint Endpoint { get; private set; }


    private void SetupCatapultApis()
    {
      Error = new ErrorApi { Client = this };
      Account = new AccountApi {Client = this};
      Application = new ApplicationApi { Client = this };
      AvailableNumber = new AvailableNumberApi { Client = this };
      Bridge = new BridgeApi{ Client = this };
      Domain = new DomainApi{ Client = this };
      Call = new CallApi { Client = this };
      Conference = new ConferenceApi { Client = this };
      Message = new MessageApi { Client = this };
      NumberInfo = new NumberInfoApi { Client = this };
      PhoneNumber = new PhoneNumberApi { Client = this };
      Recording = new RecordingApi { Client = this };
      Transcription = new TranscriptionApi { Client = this };
      Media = new MediaApi { Client = this };
      Endpoint = new EndpointApi { Client = this };
    }
  }
}
