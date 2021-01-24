using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Caduhd.Input.Keyboard;

namespace Caduhd.Input.Keyboard
{
    public delegate void KeyStatusChangedEventHandler(object sender, KeyStatusChangedEventArgs args);

    public interface IKeyboardState
    {
        event KeyStatusChangedEventHandler KeyStatusChanged;
        void UpdateKeyState(Key key, KeyStatus keyState);
    }
}
