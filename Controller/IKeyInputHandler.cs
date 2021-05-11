namespace Caduhd.Controller
{
    using Caduhd.Controller.InputEvaluator;
    using Caduhd.Input.Keyboard;

    /// <summary>
    /// <see cref="IKeyInputHandler"/> interface.
    /// </summary>
    public interface IKeyInputHandler
    {
        /// <summary>
        /// Processes a <see cref="KeyInfo"/> input.
        /// </summary>
        /// <param name="keyInfo">The <see cref="KeyInfo"/> to process.</param>
        /// <returns>The result of the process.</returns>
        InputProcessResult ProcessKeyInput(KeyInfo keyInfo);
    }
}