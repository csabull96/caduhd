using Caduhd.Input.Keyboard;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Input.Keyboard
{
    public class ReadonlyInputKeys
    {
        private IDictionary<Key, KeyStatus> m_keys;

        public ReadonlyInputKeys()
        {
            m_keys = new Dictionary<Key, KeyStatus>();
            m_keys.Add(Key.W, KeyStatus.Up);
            m_keys.Add(Key.S, KeyStatus.Up);
            m_keys.Add(Key.A, KeyStatus.Up);
            m_keys.Add(Key.D, KeyStatus.Up);

            m_keys.Add(Key.Up, KeyStatus.Up);
            m_keys.Add(Key.Down, KeyStatus.Up);
            m_keys.Add(Key.Left, KeyStatus.Up);
            m_keys.Add(Key.Right, KeyStatus.Up);

            m_keys.Add(Key.Back, KeyStatus.Up);
            m_keys.Add(Key.Enter, KeyStatus.Up);
        }

        public bool DoesKeyStateNeedToBeUpdated(Key key, KeyStatus keyState) => m_keys.ContainsKey(key) && m_keys[key] != keyState;

        public void Update(Key key, KeyStatus keyState)
        {
            m_keys[key] = keyState;
        }

        public bool UpdateKeyStateIfOutdated(Key key, KeyStatus keyState)
        {
            if (m_keys.ContainsKey(key) && m_keys[key] != keyState)
            {
                m_keys[key] = keyState;
                return true;
            }
            return false;
        }
    }
}
