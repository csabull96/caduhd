using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Caduhd.Input.Keyboard;

namespace Caduhd.Input.Keyboard
{
    public delegate void KeyboardStateEventHandler(object sender, KeyboardStateChangedEventArgs args);

    public interface IKeyboardState
    {
        event KeyboardStateEventHandler KeyboardStateChanged;
        void SetKeyState(Key key, KeyState keyState);
    }
}
