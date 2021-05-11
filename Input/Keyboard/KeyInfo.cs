namespace Caduhd.Input.Keyboard
{
    using System.Windows.Input;

    /// <summary>
    /// A class that represents the actual state of a key.
    /// </summary>
    public class KeyInfo
    {
        /// <summary>
        /// Gets the key.
        /// </summary>
        public Key Key { get; private set; }

        /// <summary>
        /// Gets or sets the state of the key, whether it's down or up.
        /// </summary>
        public KeyState KeyState { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyInfo"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="keyState">The state of the key. By default it's <see cref="KeyState.Up"/>.</param>
        public KeyInfo(Key key, KeyState keyState = KeyState.Up)
        {
            this.Key = key;
            this.KeyState = keyState;
        }
    }
}
