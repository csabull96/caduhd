namespace Caduhd.Input.Tests.Camera
{
    using System.Drawing;
    using Caduhd.Common;
    using Caduhd.Input.Camera;
    using Xunit;

    public class NewWebCameraFrameEventArgsTests
    {
        [Fact]
        public void FrameGetter_ReturnedFrameIsTheSameAsTheOnePassedToTheConstructor()
        {
            BgrImage image = BgrImage.GetBlank(100, 100, Color.White);
            var eventArgs = new NewWebCameraFrameEventArgs(image);
            Assert.Same(image, eventArgs.Frame);
        }
    }
}
