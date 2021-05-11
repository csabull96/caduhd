using Caduhd.Common;
using Caduhd.Input.Camera;
using System.Drawing;
using Xunit;

namespace Caduhd.Input.Tests.Camera
{
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
