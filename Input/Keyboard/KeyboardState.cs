using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Caduhd.Input.Keyboard
{
    public class KeyboardState : IKeyboardState
    {
        private IDictionary<Key, KeyState> m_keyStates;

        public event KeyboardStateEventHandler KeyboardStateChanged;

        public KeyboardState()
        {
            m_keyStates = new Dictionary<Key, KeyState>();

            m_keyStates.Add(Key.W, KeyState.Down);
            m_keyStates.Add(Key.S, KeyState.Down);
            m_keyStates.Add(Key.A, KeyState.Down);
            m_keyStates.Add(Key.D, KeyState.Down);

            m_keyStates.Add(Key.Up, KeyState.Down);
            m_keyStates.Add(Key.Down, KeyState.Down);
            m_keyStates.Add(Key.Left, KeyState.Down);
            m_keyStates.Add(Key.Right, KeyState.Down);

            m_keyStates.Add(Key.Back, KeyState.Down);
            m_keyStates.Add(Key.Enter, KeyState.Down);
        }

        public void SetKeyState(Key key, KeyState keyState)
        {
            if (m_keyStates.ContainsKey(key))
            {
                m_keyStates[key] = keyState;
                KeyboardStateChanged?.Invoke(this, new KeyboardStateChangedEventArgs(key));
            }
        }
    }
}
