namespace Caduhd.Input.Tests.Keyboard
{
    using System.Windows.Input;
    using Caduhd.Input.Keyboard;
    using Xunit;

    public class KeyEventProcessorTests
    {
        private KeyEventProcessor keyEventProcessor;

        public KeyEventProcessorTests()
        {
            this.keyEventProcessor = new KeyEventProcessor();
        }

        [Fact]
        public void ProcessKeyEvent_KeyDownNotRepeated_ReturnsCorrectKeyInfo()
        {
            var keyInfo = this.keyEventProcessor.ProcessKeyEvent(Key.Enter);
            Assert.Equal(Key.Enter, keyInfo.Key);
            Assert.Equal(KeyState.Down, keyInfo.KeyState);
        }

        [Fact]
        public void ProcessKeyEvent_KeyUpNotRepeated_ReturnsCorrectKeyInfo()
        {
            var keyInfo = this.keyEventProcessor.ProcessKeyEvent(Key.Enter, false);
            Assert.Equal(Key.Enter, keyInfo.Key);
            Assert.Equal(KeyState.Up, keyInfo.KeyState);
        }

        [Fact]
        public void ProcessKeyEvent_KeyDownAndRepeated_ReturnsNull()
        {
            var keyInfo = this.keyEventProcessor.ProcessKeyEvent(Key.Enter, isRepeat: true);
            Assert.Null(keyInfo);
        }

        [Fact]
        public void ProcessKeyEvent_KeyUpAndRepeated_ReturnsNull()
        {
            var keyInfo = this.keyEventProcessor.ProcessKeyEvent(Key.Enter, false, true);
            Assert.Null(keyInfo);
        }
    }
}
