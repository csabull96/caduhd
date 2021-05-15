namespace Caduhd.Drone.Dji
{
    using System;
    using Caduhd.Common;

    /// <summary>
    /// The state parser for Tello.
    /// </summary>
    public class TelloStateParser
    {
        /// <summary>
        /// Parses the state of the Tello received as a sequence of bytes.
        /// After converting the bytes into ASCII characters it has to match the following pattern:
        /// “pitch:%d;roll:%d;yaw:%d;vgx:%d;vgy%d;vgz:%d;templ:%d;temph:%d;tof:%d;h:%d;bat:%d;baro:%.2f; time:%d;agx:%.2f;agy:%.2f;agz:%.2f;\r\n”
        /// </summary>
        /// <param name="stateData">The Tello's state as a sequence of bytes.</param>
        /// <returns>Tello's parsed state information as <see cref="DroneState"/>.</returns>
        public DroneState Parse(byte[] stateData)
        {
            string[] properties = stateData.AsString().Trim().Split(';');
            DroneState droneState = new DroneState();
            try
            {
                droneState.Pitch = int.Parse(properties[0].Substring(6));
                droneState.Roll = int.Parse(properties[1].Substring(5));
                droneState.Yaw = int.Parse(properties[2].Substring(4));

                // with Tello X and Y speed is only provided if VPN (visual positioning system) works
                droneState.SpeedX = int.Parse(properties[3].Substring(4));
                droneState.SpeedY = int.Parse(properties[4].Substring(4));
                droneState.SpeedZ = int.Parse(properties[5].Substring(4));
                droneState.LowestTemperature = int.Parse(properties[6].Substring(6));
                droneState.HighestTemperature = int.Parse(properties[7].Substring(6));
                droneState.ToF = int.Parse(properties[8].Substring(4));
                droneState.Height = int.Parse(properties[9].Substring(2));
                droneState.BatteryPercentage = int.Parse(properties[10].Substring(4));
                droneState.Barometer = double.Parse(properties[11].Substring(5));
                droneState.Time = int.Parse(properties[12].Substring(5));

                // with Tello X and Y acceleration is only provided if VPN (visual positioning system) works
                droneState.AccelerationX = double.Parse(properties[13].Substring(4));
                droneState.AccelerationY = double.Parse(properties[14].Substring(4));
                droneState.AccelerationZ = double.Parse(properties[15].Substring(4));
            }
            catch (Exception)
            {
            }

            return droneState;
        }
    }
}
