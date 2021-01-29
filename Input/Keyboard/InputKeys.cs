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
            
            InitializeKey(Key.W);
            InitializeKey(Key.S);
            InitializeKey(Key.A);
            InitializeKey(Key.D);
            InitializeKey(Key.Up);
            InitializeKey(Key.Down);
            InitializeKey(Key.Left);
            InitializeKey(Key.Right);
            InitializeKey(Key.Back);
            InitializeKey(Key.Enter);
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

        private void InitializeKey(Key key) => Keys.Add(key, KeyState.Up);
    }
}
