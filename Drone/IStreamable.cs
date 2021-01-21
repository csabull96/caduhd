using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caduhd.Drone
{
    interface IStreamable
    {
        void StartVideoStream();
        void StopVideoStream();
        string GetVideoStreamAddress();
    }
}
