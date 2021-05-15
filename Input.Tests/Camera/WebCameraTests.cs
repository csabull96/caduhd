namespace Caduhd.Input.Tests.Camera
{
    using System;
    using System.Drawing;
    using System.Threading.Tasks;
    using Caduhd.Input.Camera;
    using Emgu.CV;
    using Emgu.CV.Structure;
    using Moq;
    using Xunit;

    public class WebCameraTests
    {
        private readonly int width;
        private readonly int height;
        private readonly Mat fakeFrame;
        private readonly Color customColor;
        private readonly Mock<ICapture> captureMock;

        public WebCameraTests()
        {
            this.width = 1920;
            this.height = 1080;

            this.customColor = Color.FromArgb(97, 1, 227);
            this.fakeFrame = new Image<Bgr, byte>(this.width, this.height, new Bgr(this.customColor)).Mat;

            this.captureMock = new Mock<ICapture>();
            this.captureMock.Setup(c => c.QueryFrame()).Returns(this.fakeFrame);
        }

        [Theory]
        [InlineData(-1, 200)]
        [InlineData(-23, 197)]
        [InlineData(-178, 1)]
        public void Constructor_InvalidWidth_CannotConstructWebCamera(int width, int height)
        {
            Assert.Throws<ArgumentException>(() => new WebCamera(width, height));
        }

        [Theory]
        [InlineData(200, -1)]
        [InlineData(47, -83)]
        [InlineData(200, 0)]
        public void Constructor_InvalidHeight_CannotConstructWebCamera(int width, int height)
        {
            Assert.Throws<ArgumentException>(() => new WebCamera(width, height));
        }

        [Theory]
        [InlineData(-97, 0)]
        [InlineData(0, -1)]
        [InlineData(-451, -13)]
        public void Constructor_InvalidWidthAndHeight_CannotConstructWebCamera(int width, int height)
        {
            Assert.Throws<ArgumentException>(() => new WebCamera(width, height));
        }

        [Theory]
        [InlineData(-1, 97, 12)]
        [InlineData(0, 130, 243)]
        [InlineData(-23, 1280, 720)]
        public void Constructor_InvalidFps_CannotConstructWebCamera(int fps, int width, int height)
        {
            Assert.Throws<ArgumentException>(() => new WebCamera(this.captureMock.Object, fps, width, height));
        }

        [Fact]
        public void Constructor_CameraIsOffByDefault()
        {
            IWebCamera webCamera = new WebCamera(this.width, this.height);
            Assert.False(webCamera.IsOn);
        }

        [Fact]
        public void Constructor_CaptureIsPassedToConstructor_CameraIsOffByDefault()
        {
            IWebCamera webCamera = new WebCamera(this.captureMock.Object);
            Assert.False(webCamera.IsOn);
        }

        [Fact]
        public async Task Constructor_SuccessfulConstruction_NoFrameQueried()
        {
            IWebCamera webCamera = new WebCamera(this.captureMock.Object);
            await Task.Delay(100);
            this.captureMock.Verify(c => c.QueryFrame(), Times.Never);
        }

        [Fact]
        public async Task Constructor_CameraTurnedOn_FrameQueried()
        {
            IWebCamera webCamera = new WebCamera(this.captureMock.Object);
            webCamera.On();
            await Task.Delay(100);
            this.captureMock.Verify(c => c.QueryFrame(), Times.AtLeastOnce);
        }

        [Fact]
        public async Task Constructor_SuccessfulConstruction_NewFrameNotFired()
        {
            bool fired = false;
            IWebCamera webCamera = new WebCamera(this.captureMock.Object);
            webCamera.NewFrame += (s, e) => { fired = true; };
            await Task.Delay(100);
            Assert.False(fired);
        }

        [Fact]
        public async Task Constructor_CameraTurnedOn_NewFrameFired()
        {
            bool fired = false;
            IWebCamera webCamera = new WebCamera(this.captureMock.Object);
            webCamera.NewFrame += (s, e) => { fired = true; };
            webCamera.On();
            await Task.Delay(100);
            Assert.True(fired);
        }

        [Fact]
        public async Task Constructor_CameraTurnedOnThenTurnedOff_NewFrameNotFiredAfterOff()
        {
            bool fired = false;
            IWebCamera webCamera = new WebCamera(this.captureMock.Object);
            webCamera.NewFrame += (s, e) => { fired = true; };
            webCamera.On();
            fired = false;
            webCamera.Off();
            await Task.Delay(100);
            Assert.False(fired);
        }

        [Fact]
        public async Task Constructor_CameraTurnedOnOffAndOnAgain_NewFrameFiredAgainAfterSecondOn()
        {
            bool fired = false;
            IWebCamera webCamera = new WebCamera(this.captureMock.Object);
            webCamera.NewFrame += (s, e) => { fired = true; };
            webCamera.On();
            webCamera.Off();
            webCamera.On();
            fired = false;
            await Task.Delay(100);
            Assert.True(fired);
        }
    }
}
