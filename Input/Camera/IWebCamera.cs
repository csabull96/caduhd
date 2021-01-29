using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Caduhd.Input.Camera
{
    public delegate void NewWebCameraFrameEventHandler(object sender, NewWebCameraFrameEventArgs args);

    public interface IWebCamera
    {
        bool IsOn { get; }
        event NewWebCameraFrameEventHandler NewFrame;
        void TurnOn();
        void TurnOff();
    }
}
