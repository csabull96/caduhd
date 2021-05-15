namespace Caduhd.Input.Camera
{
    using System;
    using System.Timers;
    using Caduhd.Common;
    using Emgu.CV;
    using Emgu.CV.CvEnum;
    using Emgu.CV.Structure;

    /// <summary>
    /// A wrapper class around the Emgu.CV.VideoCapture for web camera capture.
    /// </summary>
    public class WebCamera : IWebCamera, IDisposable
    {
        private const int DEFAULT_WIDTH = 320;
        private const int DEFAULT_HEIGHT = 180;
        private const int DEFAULT_FPS = 30;

        private readonly Timer timer;
        private ICapture videoCapture;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebCamera"/> class.
        /// </summary>
        /// <param name="width">The width of the frames delivered by the web camera.</param>
        /// <param name="height">The height of the frames delivered by the web camera.</param>
        public WebCamera(int width, int height)
            : this(new VideoCapture(0, VideoCapture.API.DShow), 30, width, height)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebCamera"/> class.
        /// </summary>
        /// <param name="capture">An implementation of the <see cref="ICapture"/> interface that is going to fetch the web camera frames.</param>
        /// <param name="fps">The desired FPS of the web camera capture.</param>
        /// <param name="width">The width of the web camera capture.</param>
        /// <param name="height">The height of the web camera capture.</param>
        public WebCamera(ICapture capture, int fps = DEFAULT_FPS, int width = DEFAULT_WIDTH, int height = DEFAULT_HEIGHT)
        {
            if (fps < 1)
            {
                throw new ArgumentException($"The FPS has to be greater than 0.");
            }

            if (width < 1 || height < 1)
            {
                throw new ArgumentException($"Invalid resolution. Both width and height have to be greater than 0.");
            }

            this.videoCapture = capture;

            this.Fps = fps;
            this.Width = width;
            this.Height = height;
            this.IsOn = false;

            int interval = 1000 / this.Fps;
            this.timer = new Timer(interval);
            this.timer.Elapsed += this.QueryFrame;
        }

        /// <summary>
        /// An event that is fired whenever a new web camera frame is available.
        /// </summary>
        public event NewWebCameraFrameEventHandler NewFrame;

        /// <summary>
        /// Gets the desired FPS that is set for the web camera.
        /// </summary>
        public int Fps { get; private set; }

        /// <summary>
        /// Gets the width of the frames delivered by the web camera.
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// Gets the height of the frames delivered by the web camera.
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the web camera is on or not.
        /// </summary>
        public bool IsOn { get; private set; }

        /// <summary>
        /// Turns on the web camera.
        /// </summary>
        public void On()
        {
            if (!this.IsOn)
            {
                this.timer.Start();
                this.IsOn = true;
            }
        }

        /// <summary>
        /// Turns off the web camera.
        /// </summary>
        public void Off()
        {
            if (this.IsOn)
            {
                this.timer.Stop();
                this.IsOn = false;
            }
        }

        /// <summary>
        /// Disposes this web camera object.
        /// </summary>
        public void Dispose()
        {
            this.timer.Dispose();
            (this.videoCapture as IDisposable)?.Dispose();
        }

        private void QueryFrame(object sender, ElapsedEventArgs e)
        {
            try
            {
                Image<Bgr, byte> frame = this.videoCapture.QueryFrame()
                .ToImage<Bgr, byte>()
                .Resize(this.Width, this.Height, Inter.Area, false)
                .Flip(FlipType.Horizontal);

                this.NewFrame?.Invoke(this, new NewWebCameraFrameEventArgs(new BgrImage(frame)));
            }
            catch (Exception)
            {
            }
        }
    }
}
