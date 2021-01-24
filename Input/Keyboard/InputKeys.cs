using Input.Keyboard;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Caduhd.Input.Keyboard
{
    public class InputKeys : IKeyboardState, IEnumerable<KeyValuePair<Key, KeyStatus>>
    {
        public IDictionary<Key, KeyStatus> Keys { get; private set; }

        public event KeyStatusChangedEventHandler KeyStatusChanged;

        public InputKeys()
        {
            Keys = new Dictionary<Key, KeyStatus>();

            Keys = new Dictionary<Key, KeyStatus>();
            Keys.Add(Key.W, KeyStatus.Up);
            Keys.Add(Key.S, KeyStatus.Up);
            Keys.Add(Key.A, KeyStatus.Up);
            Keys.Add(Key.D, KeyStatus.Up);

            Keys.Add(Key.Up, KeyStatus.Up);
            Keys.Add(Key.Down, KeyStatus.Up);
            Keys.Add(Key.Left, KeyStatus.Up);
            Keys.Add(Key.Right, KeyStatus.Up);

            Keys.Add(Key.Back, KeyStatus.Up);
            Keys.Add(Key.Enter, KeyStatus.Up);
        }

        public bool IsInputKey(Key key) => Keys.ContainsKey(key);
       
        private bool TryUpdate(Key key, KeyStatus keyStatus)
        {
            if (IsInputKey(key) && Keys[key] != keyStatus)
            {
                Keys[key] = keyStatus;
                return true;
            }
            return false;
        }

        public void UpdateKeyState(Key key, KeyStatus keyStatus)
        {
            if (TryUpdate(key, keyStatus))
            {
                KeyStatusChanged?.Invoke(this, new KeyStatusChangedEventArgs(key, keyStatus));
            }
        }

        public IEnumerator<KeyValuePair<Key, KeyStatus>> GetEnumerator() => Keys.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
