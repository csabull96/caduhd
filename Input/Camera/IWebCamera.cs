using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Caduhd.Input.Camera
{
    public delegate void WebCameraEventHandler(object sender, WebCameraEventArgs args);

    public interface IWebCamera
    {
        event WebCameraEventHandler Feed;
        void TurnOn();
        void TurnOff();
    }
}
