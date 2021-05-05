using Caduhd.Common;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Timers;

namespace Caduhd.Input.Camera
{
    public class WebCamera : IWebCamera
    {
        private const int DEFAULT_WIDTH = 640;
        private const int DEFAULT_HEIGHT = 320;
        private const int DEFAULT_FPS = 30;

        private readonly Timer _timer;
        private VideoCapture _videoCapture;

        public int Fps { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public bool IsOn { get; private set; }

        public event NewWebCameraFrameEventHandler NewFrame;

        public WebCamera(int width, int height) : this(30, width, height) { }

        public WebCamera(int fps = DEFAULT_FPS, int width = DEFAULT_WIDTH, int height = DEFAULT_HEIGHT)
        {
            if (fps < 1)
                throw new ArgumentException($"The FPS has to be greater than 0.");

            if (width < 1 || height < 1)
                throw new ArgumentException($"Invalid resolution. Both width and height have to be greater than 0.");

            Fps = fps;
            Width = width;
            Height = height;
            IsOn = false;

            int interval = 1000 / Fps;
            _timer = new Timer(interval);
        }

        private void QueryFrame(object sender, ElapsedEventArgs e)
        {
            Image<Bgr, byte> frame = _videoCapture.QueryFrame()
                .ToImage<Bgr, byte>()
                .Resize(Width, Height, Inter.Area, false)
                .Flip(FlipType.Horizontal);
            
            NewFrame?.Invoke(this, new NewWebCameraFrameEventArgs(new BgrImage(frame)));
        }

        public void TurnOn()
        {
            if (!IsOn)
            {
                _videoCapture = new VideoCapture(0, VideoCapture.API.DShow);
                _timer.Elapsed += QueryFrame;
                _timer.Start();
                IsOn = true;
            }
        }

        public void TurnOff()
        {
            if (IsOn)
            {
                _timer.Stop();
                _timer.Elapsed -= QueryFrame;
                _videoCapture.Dispose();
                IsOn = false;
            }
        }
    }
}
