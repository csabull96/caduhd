namespace Caduhd.Controller.InputEvaluator
{
    using Caduhd.Drone.Command;
    using Caduhd.HandsDetector;

    /// <summary>
    /// <see cref="IDroneControllerHandsInputEvaluator"/> interface.
    /// </summary>
    public interface IDroneControllerHandsInputEvaluator
    {
        /// <summary>
        /// Evaluates <see cref="NormalizedHands"/> input.
        /// </summary>
        /// <param name="hands"><see cref="NormalizedHands"/> to evaluate.</param>
        /// <returns>The evaluated <paramref name="hands"/> as <see cref="MoveCommand"/>.</returns>
        MoveCommand EvaluateHands(NormalizedHands hands);
    }
}
