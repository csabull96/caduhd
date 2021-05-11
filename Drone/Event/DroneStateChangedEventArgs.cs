﻿namespace Caduhd.Drone.Event
{
    using System;

    /// <summary>
    /// Drone state changed event arguments.
    /// </summary>
    public class DroneStateChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the changed <see cref="DroneState"/> object.
        /// </summary>
        public DroneState DroneState { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DroneStateChangedEventArgs"/> class.
        /// </summary>
        /// <param name="droneState">The changed <see cref="DroneState"/> object.</param>
        public DroneStateChangedEventArgs(DroneState droneState)
        {
            this.DroneState = droneState;
        }
    }
}
