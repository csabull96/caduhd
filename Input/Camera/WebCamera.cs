using Caduhd.Common;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Timers;

namespace Caduhd.Input.Camera
{
    public class WebCamera : IWebCamera, IDisposable
    {
        private const int DEFAULT_WIDTH = 320;
        private const int DEFAULT_HEIGHT = 180;
        private const int DEFAULT_FPS = 30;

        private readonly Timer _timer;
        private ICapture _videoCapture;

        public int Fps { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public bool IsOn { get; private set; }

        public event NewWebCameraFrameEventHandler NewFrame;

        public WebCamera(int width, int height) : this(new VideoCapture(0, VideoCapture.API.DShow), 30, width, height) { }

        public WebCamera(ICapture capture, int fps = DEFAULT_FPS, int width = DEFAULT_WIDTH, int height = DEFAULT_HEIGHT)
        {
            if (fps < 1)
                throw new ArgumentException($"The FPS has to be greater than 0.");

            if (width < 1 || height < 1)
                throw new ArgumentException($"Invalid resolution. Both width and height have to be greater than 0.");

            _videoCapture = capture;

            Fps = fps;
            Width = width;
            Height = height;
            IsOn = false;

            int interval = 1000 / Fps;
            _timer = new Timer(interval);
            _timer.Elapsed += QueryFrame;
        }

        private void QueryFrame(object sender, ElapsedEventArgs e)
        {
            try
            {
                Image<Bgr, byte> frame = _videoCapture.QueryFrame()
                .ToImage<Bgr, byte>()
                .Resize(Width, Height, Inter.Area, false)
                .Flip(FlipType.Horizontal);

                NewFrame?.Invoke(this, new NewWebCameraFrameEventArgs(new BgrImage(frame)));
            }
            catch (Exception)
            {

            }
        }

        public void On()
        {
            if (!IsOn)
            {
                _timer.Start();
                IsOn = true;
            }
        }

        public void Off()
        {
            if (IsOn)
            {
                _timer.Stop();
                IsOn = false;
            }
        }

        public void Dispose()
        {
            _timer.Dispose();
            (_videoCapture as IDisposable)?.Dispose();
        }
    }
}
