using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Bandwidth.Net.Api
{
  /// <summary>
  ///   Access to IntelligenceService Api
  /// </summary>
  public interface IIntelligenceService
  {

    /// <summary>
    ///   Get number intelligence data
    /// </summary>
    /// <param name="number">The telephone number in E.164 format.</param>
    /// <param name="query">Optional query parameters</param>
    /// <param name="cancellationToken">Optional token to cancel async operation</param>
    /// <returns>Task with <see cref="NumberIntelligenceData" />IntelligenceService instance</returns>
    /// <example>
    ///   <code>
    /// var data = await client.IntelligenceService.GetNumberIntelligenceDataAsync("+1234567890");
    /// </code>
    /// </example>
    Task<NumberIntelligenceData> GetNumberIntelligenceDataAsync(string number,  GetNumberIntelligenceDataQuery query = null, CancellationToken? cancellationToken = null);

  }

  internal class IntelligenceServiceApi : ApiBase, IIntelligenceService
  {
    public Task<NumberIntelligenceData> GetNumberIntelligenceDataAsync(string number,
      GetNumberIntelligenceDataQuery query = default(GetNumberIntelligenceDataQuery),
      CancellationToken? cancellationToken = null)
    {
      return Client.MakeJsonRequestAsync<NumberIntelligenceData>(HttpMethod.Get,
        $"/users/{Client.UserId}/intelligenceServices/number/{Uri.EscapeDataString(number)}", cancellationToken, query);
    }

  }

  /// <summary>
  /// Special converter of enumerable types for IntelligenceService
  /// </summary>
  public sealed class IntelligenceServiceEnumConverter : StringEnumConverter
  {
    /// <summary>
    /// Convert string to enum value
    /// </summary>
    /// <param name="reader">reader</param>
    /// <param name="objectType">object type</param>
    /// <param name="existingValue">existing value</param>
    /// <param name="serializer">serializer</param>
    /// <returns></returns>
    public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
      JsonSerializer serializer)
    {
      // convert string like "enum_value" to EnumType.EnumValue
      var rawString = (string)reader.Value;
      var result = string.Join("",
        rawString.Split('_').Select(v => $"{char.ToUpperInvariant(v[0])}{v.Substring(1)}"));
      return Enum.Parse(objectType, result);
    }
  }

  /// <summary>
  ///   Intelligence data of number
  /// </summary>
  public class NumberIntelligenceData
  {
    /// <summary>
    /// The telephone number in E.164 format.
    /// </summary>
    public string Number { get; set; }

    /// <summary>
    /// The telephone number in a friendly national format, e.g. (555) 555-5555
    /// </summary>
    public string NationalNumber { get; set; }

    /// <summary>
    /// The company who provides the service for this number. 
    /// </summary>
    public string Carrier { get; set; }

    /// <summary>
    /// The type of service provided on this number
    /// </summary>
    [JsonConverter(typeof(IntelligenceServiceEnumConverter))]
    public NumberIntelligenceDataLineType LineType { get; set; }

    /// <summary>
    /// Measures the risk of the number being associated with spam / fraudulent behavior. Valid values between 1 - 4.
    /// </summary>
    public string ReputationScore { get; set; }

    /// <summary>
    /// Type of risk this number is associated with. 
    /// </summary>
    [JsonConverter(typeof(IntelligenceServiceEnumConverter))]
    public NumberIntelligenceDataRiskType RiskType { get; set; }

    /// <summary>
    /// Category of risk associated with the number. 
    /// </summary>
    [JsonConverter(typeof(IntelligenceServiceEnumConverter))]
    public NumberIntelligenceDataRiskCategory RiskCategory { get; set; }
  }
  
  /// <summary>
  ///   Query to get intelligence data of number
  /// </summary>
  public class GetNumberIntelligenceDataQuery
  {
    /// <summary>
    ///   Includes number reputation information. 
    /// </summary>
    public bool? Reputation { get; set; }
  }

  /// <summary>
  /// Line types
  /// </summary>
  public enum NumberIntelligenceDataLineType
  {
    /// <summary>
    /// Landline
    /// </summary>
    Landline,
    
    /// <summary>
    /// FixedVoip
    /// </summary>
    FixedVoip,

    /// <summary>
    /// NonFixedVoip
    /// </summary>
    NonFixedVoip,

    /// <summary>
    /// Mobile
    /// </summary>
    Mobile,

    /// <summary>
    /// Voicemail
    /// </summary>
    Voicemail,

    /// <summary>
    /// Tollfree
    /// </summary>
    Tollfree,

    /// <summary>
    /// Premium
    /// </summary>
    Premium,

    /// <summary>
    /// Other
    /// </summary>
    Other
  }

  /// <summary>
  /// Risk types
  /// </summary>
  public enum NumberIntelligenceDataRiskType
  {
    /// <summary>
    /// Spam
    /// </summary>
    Spam,

    /// <summary>
    /// Risk
    /// </summary>
    Risk,

    /// <summary>
    /// NotApplicable
    /// </summary>
    NotApplicable
  }

  /// <summary>
  /// Risk categories
  /// </summary>
  public enum NumberIntelligenceDataRiskCategory
  {
    /// <summary>
    /// NotSpam
    /// </summary>
    NotSpam,

    /// <summary>
    /// DebtCollector
    /// </summary>
    DebtCollector,

    /// <summary>
    /// Telemarketer
    /// </summary>
    Telemarketer,

    /// <summary>
    /// PoliticalCall
    /// </summary>
    PoliticalCall,

    /// <summary>
    /// PhoneSurvey
    /// </summary>
    PhoneSurvey,

    /// <summary>
    /// Phishing
    /// </summary>
    Phishing,
    
    /// <summary>
    /// Extortion
    /// </summary>
    Extortion,
    
    /// <summary>
    /// IrsScam
    /// </summary>
    IrsScam,

    /// <summary>
    /// TaxScam
    /// </summary>
    TaxScam,

    /// <summary>
    /// TechSupportScam
    /// </summary>
    TechSupportScam,

    /// <summary>
    /// VacationScam
    /// </summary>
    VacationScam,

    /// <summary>
    /// LuckyWinnerScam
    /// </summary>
    LuckyWinnerScam,

    /// <summary>
    /// Scam
    /// </summary>
    Scam,

    /// <summary>
    /// TollfreePumping
    /// </summary>
    TollfreePumping,

    /// <summary>
    /// OtherSpam
    /// </summary>
    OtherSpam
  }
  
}
