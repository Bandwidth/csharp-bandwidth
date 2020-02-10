using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Bandwidth.Net.Xml.Verbs
{
  /// <summary>
  ///   The PlayAudio verb is used to play an audio file in the call.
  /// </summary>
  /// <seealso href="http://ap.bandwidth.com/docs/xml/playaudio/" />
  [Obsolete("Use verb from namespace Bandwidth.Net.XmlV2.Verbs")]
  public class PlayAudio : IXmlSerializable, IVerb
  {
    /// <summary>
    ///   Allows you to play DTMF digits in the call
    /// </summary>
    public string Digits { get; set; }

    /// <summary>
    ///   Url of media resource to play
    /// </summary>
    public string Url { get; set; }

    XmlSchema IXmlSerializable.GetSchema()
    {
      return null;
    }

    void IXmlSerializable.ReadXml(XmlReader reader)
    {
      throw new NotImplementedException();
    }

    void IXmlSerializable.WriteXml(XmlWriter writer)
    {
      if (!string.IsNullOrEmpty(Digits))
      {
        writer.WriteAttributeString("digits", Digits);
      }
      writer.WriteString(Url);
    }
  }
}
