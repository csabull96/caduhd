using System;

namespace Caduhd.Drone
{
    public class DroneState
    {
        public int Pitch { get; set; } = 0;
        public int Roll { get; set; } = 0;
        public int Yaw { get; set; } = 0;
        public int SpeedX { get; set; } = 0;
        public int SpeedY { get; set; } = 0;
        public int SpeedZ { get; set; } = 0;
        // with Tello X and Y speed only works if VPN (visual positioning system) works
        public double Speed => Math.Pow(Math.Pow(Math.Abs(SpeedX), 3) + Math.Pow(Math.Abs(SpeedY), 3) + Math.Pow(Math.Abs(SpeedZ), 3), (double)1 / 3);
        public int LowestTemperature { get; set; } = 0;
        public int HighestTemperature { get; set; } = 0;
        // The Time-of-Flight principle (ToF) is a method for measuring the distance between a sensor and an object,
        // based on the time difference between the emission of a signal and its return to the sensor,
        // after being reflected by an object.
        public int ToF { get; set; } = 0;
        // the actual height above the starting point
        public int Height { get; set; } = 0;
        public int Battery { get; set; } = 0;
        public double Barometer { get; set; } = 0;
        // actual flight time in seconds
        public int Time { get; set; } = -1;

        public double AccelerationX { get; set; } = 0;
        public double AccelerationY { get; set; } = 0;
        public double AccelerationZ { get; set; } = 0;

        // it probably needs the VPN just as the speed
        public double Acceleration => Math.Pow(Math.Pow(Math.Abs(AccelerationX), 3) + Math.Pow(Math.Abs(AccelerationY), 3) + Math.Pow(Math.Abs(AccelerationZ), 3), (double)1 / 3);

        // the wifi SNR (signal to noise ratio)
        public int Wifi { get; set; } = -1;
    }
}
