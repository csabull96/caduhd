using System.Windows.Input;

namespace Caduhd.Input.Keyboard
{
    public class KeyEventProcessor
    {
        public KeyInfo ProcessKeyEvent(KeyEventArgs keyEventArgs)
        {
            if (keyEventArgs.IsRepeat)
                return null;

            KeyState keyState = keyEventArgs.IsDown ? KeyState.Down : KeyState.Up;
            return new KeyInfo(keyEventArgs.Key, keyState);
        }
    }
}
