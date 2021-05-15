namespace Caduhd
{
    using Caduhd.Common;
    using Caduhd.Controller.InputAnalyzer;
    using Caduhd.Drone;
    using Caduhd.Drone.Command;

    /// <summary>
    /// Caduhd user interface connector.
    /// </summary>
    public interface ICaduhdUIConnector
    {
        /// <summary>
        /// Sets the hands analyzer state.
        /// </summary>
        /// <param name="handsAnalyzerState">The state of the hands analyzer.</param>
        void SetHandsAnalyzerState(HandsAnalyzerState handsAnalyzerState);

        /// <summary>
        /// Sets the image coming from the drone's camera.
        /// </summary>
        /// <param name="image">The image from the drone's camera.</param>
        void SetDroneCameraImage(BgrImage image);

        /// <summary>
        /// Sets the image coming from the computer's primary camera.
        /// </summary>
        /// <param name="image">The image from the computer's primary camera.</param>
        void SetComputerCameraImage(BgrImage image);

        /// <summary>
        /// Sets the state of the drone.
        /// </summary>
        /// <param name="droneState">The state of the drone.</param>
        void SetDroneState(DroneState droneState);

        /// <summary>
        /// Sets the evaluated hands input.
        /// </summary>
        /// <param name="handsInputEvaluated">The evaluated hands command as <see cref="MoveCommand"/>.</param>
        void SetEvaluatedHandsInput(MoveCommand handsInputEvaluated);
    }
}
