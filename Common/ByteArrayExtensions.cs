using System.Text;

namespace Caduhd.Common
{
    public static class ByteArrayExtensions
    {
        public static string AsString(this byte[] bytes) =>
           bytes == null ? string.Empty : Encoding.ASCII.GetString(bytes);
    }
}
