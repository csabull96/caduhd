namespace Caduhd.Drone.Event
{
    using System;
    using Caduhd.Common;

    /// <summary>
    /// New drone camera frame event arguments.
    /// </summary>
    public class NewDroneCameraFrameEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NewDroneCameraFrameEventArgs"/> class.
        /// </summary>
        /// <param name="frame">Gets the new frame as <see cref="BgrImage"/>.</param>
        public NewDroneCameraFrameEventArgs(BgrImage frame)
        {
            this.Frame = frame;
        }

        /// <summary>
        /// Gets the new frame as <see cref="BgrImage"/>.
        /// </summary>
        public BgrImage Frame { get; private set; }
    }
}
