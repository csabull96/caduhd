namespace Caduhd.Common.Tests
{
    using System.Text;
    using Xunit;

    public class StringExtensionsTests
    {
        private const int FIRST = 32;
        private const int LAST = 126;

        [Fact]
        public void AsBytes_StringIsNull_ReturnsNull()
        {
            string characters = null;
            Assert.Null(characters.AsBytes());
        }

        [Fact]
        public void AsBytes_StringIsEmpty_ReturnsNull()
        {
            string characters = string.Empty;
            Assert.Null(characters.AsBytes());
        }

        [Fact]
        public void AsBytes_ASCIICharacters_ReturnsCharactersAsByteArray()
        {
            byte[] bytesExpected = this.GetBytes();
            string characters = Encoding.ASCII.GetString(bytesExpected);
            Assert.Equal(bytesExpected, characters.AsBytes());
        }

        public byte[] GetBytes()
        {
            byte[] bytes = new byte[LAST - FIRST + 1];
            for (int i = FIRST; i <= LAST; i++)
            {
                bytes[i - FIRST] = (byte)i;
            }

            return bytes;
        }
    }
}
