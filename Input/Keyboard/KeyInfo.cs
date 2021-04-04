using System.Windows.Input;

namespace Ksvydo.Input.Keyboard
{
    public class KeyInfo
    {
        public Key Key { get; private set; }
        public KeyState KeyState { get; private set; }

        public KeyInfo(Key key, KeyState keyState = KeyState.Up)
        {
            Key = key;
            KeyState = keyState;
        }
    }
}
