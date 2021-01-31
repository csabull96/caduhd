using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Caduhd.Input.Keyboard
{
    public class InputKeys
    {
        public IDictionary<Key, KeyState> Keys { get; private set; }

        public InputKeys()
        {
            Keys = new Dictionary<Key, KeyState>();
            
            Initialize(Key.W);
            Initialize(Key.S);
            Initialize(Key.A);
            Initialize(Key.D);
            Initialize(Key.Up);
            Initialize(Key.Down);
            Initialize(Key.Left);
            Initialize(Key.Right);
            Initialize(Key.Space);
            Initialize(Key.Enter);
            Initialize(Key.Back);
        }
       
        public bool TryUpdate(Key key, KeyState keyState)
        {
            if (IsInputKey(key) && Keys[key] != keyState)
            {
                Keys[key] = keyState;
                return true;
            }
            return false;
        }

        private bool IsInputKey(Key key) => Keys.ContainsKey(key);

        private void Initialize(Key key) => Keys.Add(key, KeyState.Up);
    }
}
