namespace Caduhd.Controller.InputEvaluator
{
    using Caduhd.Controller.Command;
    using Caduhd.HandsDetector;

    /// <summary>
    /// <see cref="IDroneHandsInputEvaluator"/> interface.
    /// </summary>
    public interface IDroneHandsInputEvaluator
    {
        /// <summary>
        /// Evaluates <see cref="NormalizedHands"/> input.
        /// </summary>
        /// <param name="hands"><see cref="NormalizedHands"/> to evaluate.</param>
        /// <returns>The evaluated <paramref name="hands"/> as <see cref="MoveCommand"/>.</returns>
        MoveCommand EvaluateHands(NormalizedHands hands);
    }
}
