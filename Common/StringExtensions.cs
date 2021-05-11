namespace Caduhd.Common
{
    using System.Text;

    /// <summary>
    /// String extensions.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Gets the byte array that corresponds to the provided ASCII characters.
        /// </summary>
        /// <param name="characters">The ASCII characters to be converted to byte array.</param>
        /// <returns>The characters converted to a byte array.</returns>
        public static byte[] AsBytes(this string characters) =>
            string.IsNullOrEmpty(characters) ? null : Encoding.ASCII.GetBytes(characters);
    }
}
