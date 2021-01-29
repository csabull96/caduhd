using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Caduhd.Input.Camera
{
    public class WebCamera : IWebCamera
    {
        private Timer m_timer;
        private VideoCapture m_videoCapture;
        public event NewWebCameraFrameEventHandler NewFrame;
        public bool IsOn { get; private set; }

        public WebCamera() : this(30) { }

        public WebCamera(int fps)
        {
            IsOn = false;
            int interval = 1000 / fps;
            m_timer = new Timer(interval);
            m_timer.Elapsed += OnElapsed;
        }

        private void OnElapsed(object sender, ElapsedEventArgs e)
        {
            Bitmap frame = m_videoCapture.QueryFrame()
                .ToImage<Bgr, byte>()
                .Flip(FlipType.Horizontal)
                .Bitmap;
            
            NewFrame?.Invoke(this, new NewWebCameraFrameEventArgs(frame));
        }

        public void TurnOn()
        {
            if (!IsOn)
            {
                m_videoCapture = new VideoCapture(0, VideoCapture.API.DShow);
                m_timer.Start();
                IsOn = true;
            }
        }

        public void TurnOff()
        {
            if (IsOn)
            {
                m_timer.Stop();
                m_videoCapture.Dispose();
                IsOn = false;
            }
        }
    }
}
