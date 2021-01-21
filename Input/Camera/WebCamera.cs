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

namespace Input.Camera
{
    public class WebCamera
    {
        private VideoCapture m_videoCapture;

        public delegate void WebCameraEventHandler(object sender, WebCameraEventArgs args);
        public event WebCameraEventHandler Feed;

        public int FPS { get; set; }

        private Timer m_timer;

        public WebCamera(int fps)
        {
            m_videoCapture = new VideoCapture(0, VideoCapture.API.DShow);
            FPS = fps;
            int interval = 1000 / FPS;
            m_timer = new Timer(interval);
            m_timer.Elapsed += OnElapsed;
        }

        private void OnElapsed(object sender, ElapsedEventArgs e)
        {
            Bitmap frame = m_videoCapture.QueryFrame().Bitmap;        
            Feed?.Invoke(this, new WebCameraEventArgs(frame));
        }

        public void TurnOn()
        {
            m_timer.Start();
        }

        public void TurnOff()
        {
            m_timer.Stop();
            // if we turn on again we have to reconstruct the VideoCapture?!
            m_videoCapture.Dispose();
        }
    }
}
