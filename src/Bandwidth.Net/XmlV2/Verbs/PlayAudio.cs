using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Bandwidth.Net.Xml;

namespace Bandwidth.Net.XmlV2.Verbs
{
  /// <summary>
  ///   The PlayAudio verb is used to play an audio file in the call.
  /// </summary>
  /// <seealso href="http://ap.bandwidth.com/docs/xml/playaudio/" />
  public class PlayAudio : IXmlSerializable, IVerb
  {
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
      writer.WriteString(Url);
    }
  }
}
