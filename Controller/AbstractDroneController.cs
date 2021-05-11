﻿namespace Caduhd.Controller
{
    using Caduhd.Controller.Command;

    /// <summary>
    /// Abstract drone controller.
    /// </summary>
    public abstract class AbstractDroneController
    {
        private readonly IControllableDrone drone;

        private DroneCommand lastCommandSentToDrone;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractDroneController"/> class.
        /// </summary>
        /// <param name="drone">The <see cref="IControllableDrone"/> that we would like to control with this controller.</param>
        public AbstractDroneController(IControllableDrone drone)
        {
            this.drone = drone;
            this.lastCommandSentToDrone = null;
        }

        /// <summary>
        /// Sends a <see cref="ConnectCommand"/> to the drone.
        /// </summary>
        public void Connect() => this.InternalControl(new ConnectCommand());

        /// <summary>
        /// Sends a <see cref="StartStreamingVideoCommand"/> to the drone.
        /// </summary>
        public void StartStreamingVideo() => this.InternalControl(new StartStreamingVideoCommand());

        /// <summary>
        /// Sends a <see cref="StopStreamingVideoCommand"/> to the drone.
        /// </summary>
        public void StopStreamingVideo() => this.InternalControl(new StopStreamingVideoCommand());

        /// <summary>
        /// Evaluate the latest inputs together from all sources.
        /// </summary>
        protected abstract void Control();

        /// <summary>
        /// Sends the requested <see cref="DroneCommand"/> to the drone. If the command equals the last command sent, then it won't be sent again.
        /// </summary>
        /// <param name="droneCommand">The requested <see cref="DroneCommand"/>.</param>
        protected void InternalControl(DroneCommand droneCommand)
        {
            if (this.lastCommandSentToDrone == null ||
                droneCommand != null && !this.lastCommandSentToDrone.Equals(droneCommand))
            {
                this.lastCommandSentToDrone = droneCommand.Copy();
                this.drone.Control(droneCommand);
            }
        }
    }
}
