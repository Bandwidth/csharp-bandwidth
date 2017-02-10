using System.IO;
using System.Text;

namespace Bandwidth.Net
{
  internal class Utf8StringWriter : StringWriter
  {
    public override Encoding Encoding => Encoding.UTF8;
  }
}
