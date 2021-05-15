using Caduhd.Common;
using Caduhd.Drone;
using Caduhd.Drone.Command;

namespace Caduhd
{
    public interface ICaduhdUIConnector
    {
        void SetDroneCameraImage(BgrImage image);

        void SetComputerCameraImage(BgrImage image);

        void SetDroneState(DroneState droneState);

        void SetEvaluatedHandsInput(MoveCommand handsInputEvaluated);
    }
}
