using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caduhd.Controller
{
    public interface IWebCameraInputHandler
    {
        void HandleWebCameraInput(Bitmap frame);
    }
}
