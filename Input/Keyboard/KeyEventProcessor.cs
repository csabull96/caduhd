using System.Windows.Input;

namespace Caduhd.Input.Keyboard
{
    public class KeyEventProcessor
    {
        public KeyInfo ProcessKeyEvent(Key key, bool isDown = true, bool isRepeat = false)
        {
            if (isRepeat)
                return null;

            KeyState keyState = isDown ? KeyState.Down : KeyState.Up;
            return new KeyInfo(key, keyState);
        }
    }
}
