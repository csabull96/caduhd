namespace Caduhd.Common
{
    using System.Text;

    /// <summary>
    /// Byte array extensions.
    /// </summary>
    public static class ByteArrayExtensions
    {
        /// <summary>
        /// Gets the byte array as ASCII characters.
        /// </summary>
        /// <param name="bytes">The bytes to be convert to string.</param>
        /// <returns>The bytes converted to ASCII string.</returns>
        public static string AsString(this byte[] bytes) =>
           bytes == null ? string.Empty : Encoding.ASCII.GetString(bytes);
    }
}
