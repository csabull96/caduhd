using Caduhd.Common;
using System;

namespace Caduhd.Drone.Dji
{
    public class TelloStateParser
    {
        public DroneState Parse(byte[] stateData)
        {
            string[] properties = stateData.AsString().Split(';');
            DroneState droneState = new DroneState();
            try
            {
                droneState.Pitch = int.Parse(properties[0].Substring(6));
                droneState.Roll = int.Parse(properties[1].Substring(5));
                droneState.Yaw = int.Parse(properties[2].Substring(4));
                droneState.SpeedX = int.Parse(properties[3].Substring(4));
                droneState.SpeedY = int.Parse(properties[4].Substring(4));
                droneState.SpeedZ = int.Parse(properties[5].Substring(4));
                droneState.LowestTemperature = int.Parse(properties[6].Substring(6));
                droneState.HighestTemperature = int.Parse(properties[7].Substring(6));
                droneState.ToF = int.Parse(properties[8].Substring(4));
                droneState.Height = int.Parse(properties[9].Substring(2));
                droneState.Battery = int.Parse(properties[10].Substring(4));
                droneState.Barometer = double.Parse(properties[11].Substring(5));
                droneState.Time = int.Parse(properties[12].Substring(5));
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
