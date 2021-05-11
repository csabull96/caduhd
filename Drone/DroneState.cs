namespace Caduhd.Drone
{
    using System;

    /// <summary>
    /// Drone state.
    /// </summary>
    public class DroneState
    {
        /// <summary>
        /// Gets or sets the pitch value.
        /// </summary>
        public int Pitch { get; set; } = 0;

        /// <summary>
        /// Gets or sets the roll value.
        /// </summary>
        public int Roll { get; set; } = 0;

        /// <summary>
        /// Gets or sets the yaw value.
        /// </summary>
        public int Yaw { get; set; } = 0;

        /// <summary>
        /// Gets or sets the value for the x component of the speed.
        /// </summary>
        public int SpeedX { get; set; } = 0;

        /// <summary>
        /// Gets or sets the value for the y component of the speed.
        /// </summary>
        public int SpeedY { get; set; } = 0;

        /// <summary>
        /// Gets or sets the value for the z component of the speed.
        /// </summary>
        public int SpeedZ { get; set; } = 0;

        /// <summary>
        /// Gets the absolute speed.
        /// </summary>
        public double Speed => Math.Pow(Math.Pow(Math.Abs(this.SpeedX), 3) + Math.Pow(Math.Abs(this.SpeedY), 3) + Math.Pow(Math.Abs(this.SpeedZ), 3), 1D / 3);

        /// <summary>
        /// Gets or sets the lowest temperature value.
        /// </summary>
        public int LowestTemperature { get; set; } = 0;

        /// <summary>
        /// Gets or sets the highest temperature value.
        /// </summary>
        public int HighestTemperature { get; set; } = 0;

        /// <summary>
        /// Gets or sets the Time-of-Flight value.
        /// The Time-of-Flight principle (ToF) is a method for measuring the distance between a sensor and an object,
        /// based on the time difference between the emission of a signal and its return to the sensor,
        /// after being reflected by an object.
        /// </summary>
        public int ToF { get; set; } = 0;

        /// <summary>
        /// Gets or sets the height value.
        /// The actual height above the starting point.
        /// </summary>
        public int Height { get; set; } = 0;

        /// <summary>
        /// Gets or sets the battery value.
        /// </summary>
        public int Battery { get; set; } = 0;

        /// <summary>
        /// Gets or sets the barometer value.
        /// </summary>
        public double Barometer { get; set; } = 0;

        /// <summary>
        /// Gets or sets the time value.
        /// Tctual flight time in seconds.
        /// </summary>
        public int Time { get; set; } = -1;

        /// <summary>
        /// Gets or sets the value for the x component of the acceleration.
        /// </summary>
        public double AccelerationX { get; set; } = 0;

        /// <summary>
        /// Gets or sets the value for the y component of the acceleration.
        /// </summary>
        public double AccelerationY { get; set; } = 0;

        /// <summary>
        /// Gets or sets the value for the z component of the acceleration.
        /// </summary>
        public double AccelerationZ { get; set; } = 0;

        /// <summary>
        /// Gets the absolute speed.
        /// </summary>
        public double Acceleration => Math.Pow(Math.Pow(Math.Abs(this.AccelerationX), 3) + Math.Pow(Math.Abs(this.AccelerationY), 3) + Math.Pow(Math.Abs(this.AccelerationZ), 3), 1D / 3);

        /// <summary>
        /// Gets or sets the Wi-Fi value.
        /// The Wi-Fi SNR (signal to noise ratio).
        /// </summary>
        public int Wifi { get; set; } = -1;
    }
}
