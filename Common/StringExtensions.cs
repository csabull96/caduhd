using System.Text;

namespace Caduhd.Common
{
    public static class StringExtensions
    {
        public static byte[] AsBytes(this string characters) =>
            string.IsNullOrEmpty(characters) ? null : Encoding.ASCII.GetBytes(characters);
    }
}
