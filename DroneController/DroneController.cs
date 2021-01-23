using Caduhd.HandDetector.Detector;
using Caduhd.Input.Camera;
using Caduhd.Input.Keyboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caduhd.Controller
{
    public class DroneController
    {
        public IKeyboardState KeyboardState { get; private set; }

        private IWebCamera m_webCamera;
        private IHandDetector m_handDetector;

        public DroneController(IWebCamera webCamera, IKeyboardState keyboardState, IHandDetector handDetector)
        {
            m_webCamera = webCamera;
            KeyboardState = keyboardState;
            m_handDetector = handDetector;

            KeyboardState.KeyboardStateChanged += KeyboardStateChanged;
        }

        private void KeyboardStateChanged(object sender, KeyboardStateChangedEventArgs args)
        {
            throw new NotImplementedException();
        }
    }
}
