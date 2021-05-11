namespace Caduhd.Input.Camera
{
    using System;
    using Caduhd.Common;

    /// <summary>
    /// New web camera frame event args.
    /// </summary>
    public class NewWebCameraFrameEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the new web camera frame as <see cref="BgrImage"/>.
        /// </summary>
        public BgrImage Frame { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NewWebCameraFrameEventArgs"/> class.
        /// </summary>
        /// <param name="frame">The new web camera frame as <see cref="BgrImage"/>.</param>
        public NewWebCameraFrameEventArgs(BgrImage frame)
        {
            this.Frame = frame;
        }
    }
}
