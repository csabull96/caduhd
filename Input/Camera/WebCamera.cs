using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Drawing;
using System.Timers;

namespace Ksvydo.Input.Camera
{
    public class WebCamera : IWebCamera
    {
        private const int DEFAULT_WIDTH = 1280;
        private const int DEFAULT_HEIGHT = 720;
        private const int DEFAULT_FPS = 30;

        private Timer m_timer;
        private VideoCapture m_videoCapture;

        public int FPS { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public event NewWebCameraFrameEventHandler NewFrame;
        public bool IsOn { get; private set; }

        public WebCamera(int fps = DEFAULT_FPS, int width = DEFAULT_WIDTH, int height = DEFAULT_HEIGHT)
        {
            FPS = fps;
            Width = width;
            Height = height;

            IsOn = false;
            int interval = 1000 / FPS;
            m_timer = new Timer(interval);
            m_timer.Elapsed += OnElapsed;
        }

        private void OnElapsed(object sender, ElapsedEventArgs e)
        {
            Bitmap frame = m_videoCapture.QueryFrame()
                .ToImage<Bgr, byte>()
                .Resize(Width, Height, Inter.Area, false)
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
