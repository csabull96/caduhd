namespace Caduhd.Input.Keyboard
{
    using System.Windows.Input;

    /// <summary>
    /// A class to process key event.
    /// </summary>
    public class KeyEventProcessor
    {
        /// <summary>
        /// Converts key event into <see cref="KeyInfo"/>.
        /// </summary>
        /// <param name="key">The key to which the actual event belongs.</param>
        /// <param name="isDown">A value indicating whether the key is currently down or not.</param>
        /// <param name="isRepeat">A value indicating whether the key is repeat or not.</param>
        /// <returns>The key event as a <see cref="KeyInfo"/> object.</returns>
        public KeyInfo ProcessKeyEvent(Key key, bool isDown = true, bool isRepeat = false)
        {
            if (isRepeat)
            {
                return null;
            }

            KeyState keyState = isDown ? KeyState.Down : KeyState.Up;
            return new KeyInfo(key, keyState);
        }
    }
}
