namespace Caduhd.Input.Camera
{
    /// <summary>
    /// New web camera frame event handler.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="args">The event arguments.</param>
    public delegate void NewWebCameraFrameEventHandler(object sender, NewWebCameraFrameEventArgs args);

    public interface IWebCamera
    {
        /// <summary>
        /// An event that is fired whenever a new web camera frame is available.
        /// </summary>
        event NewWebCameraFrameEventHandler NewFrame;

        /// <summary>
        /// Gets a value indicating whether the web camera is on or not.
        /// </summary>
        bool IsOn { get; }

        /// <summary>
        /// Turns on the web camera.
        /// </summary>
        void On();

        /// <summary>
        /// Turns on the web camera.
        /// </summary>
        void Off();
    }
}
