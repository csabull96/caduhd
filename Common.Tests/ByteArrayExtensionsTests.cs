namespace Caduhd.Common.Tests
{
    using System;
    using System.Text;
    using Xunit;

    public class ByteArrayExtensionsTests
    {
        [Fact]
        public void AsString_ByteArrayIsNull_ReturnsEmptyString()
        {
            byte[] byteArray = null;
            Assert.Equal(string.Empty, byteArray.AsString());
        }

        [Fact]
        public void AsString_ValidByteArray_ReturnsBytesAsASCIICharacters()
        {
            string charactersExpected = this.GetASCIICharacters();
            byte[] byteArray = Encoding.ASCII.GetBytes(charactersExpected);
            Assert.Equal(charactersExpected, byteArray.AsString());
        }

        public string GetASCIICharacters()
        {
            StringBuilder characters = new StringBuilder();
            for (int i = 32; i < 127; i++)
            {
                characters.Append(Convert.ToChar(i));
            }

            return characters.ToString();
        }
    }
}
