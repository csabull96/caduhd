namespace Caduhd.Input.Tests.Keyboard
{
    using System.Windows.Input;
    using Caduhd.Input.Keyboard;
    using Xunit;

    public class KeyInfoTests
    {
        [Theory]
        [InlineData(Key.A, KeyState.Down)]
        [InlineData(Key.B, KeyState.Up)]
        [InlineData(Key.Escape, KeyState.Down)]
        public void KeyGetter_ReturnsTheSameValueThatIsPassedToTheConstructor(Key key, KeyState keyState)
        {
            var keyInfo = new KeyInfo(key, keyState);
            Assert.Equal(key, keyInfo.Key);
        }

        [Theory]
        [InlineData(Key.Back, KeyState.Down)]
        [InlineData(Key.RightAlt, KeyState.Up)]
        [InlineData(Key.Z, KeyState.Up)]
        public void KeyStateGetter_ReturnsTheSameValueThatIsPassedToTheConstructor(Key key, KeyState keyState)
        {
            var keyInfo = new KeyInfo(key, keyState);
            Assert.Equal(keyState, keyInfo.KeyState);
        }
    }
}
