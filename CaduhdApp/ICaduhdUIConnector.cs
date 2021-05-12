using Caduhd.Common;
using Caduhd.Drone.Command;

namespace Caduhd
{
    public interface ICaduhdUIConnector
    {
        void SetDroneImage(BgrImage image);

        void SetComputerCameraImage(BgrImage image);

        void SetHandsInputEvaluated(MoveCommand handsInputEvaluated);
    }
}
