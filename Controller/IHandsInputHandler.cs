namespace Caduhd.Controller
{
    using Caduhd.Controller.InputEvaluator;
    using Caduhd.HandsDetector;

    /// <summary>
    /// <see cref="IHandsInputHandler"/> interface.
    /// </summary>
    public interface IHandsInputHandler
    {
        /// <summary>
        /// Processes a <see cref="NormalizedHands"/> input.
        /// </summary>
        /// <param name="hands">The <see cref="NormalizedHands"/> to process.</param>
        /// <returns>The result of the process.</returns>
        InputProcessResult ProcessHandsInput(NormalizedHands hands);
    }
}