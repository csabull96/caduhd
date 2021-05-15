namespace Caduhd.Tests
{
    using System;
    using System.Drawing;
    using System.Net;
    using System.Net.Sockets;
    using System.Windows.Input;
    using Caduhd.Application;
    using Caduhd.Common;
    using Caduhd.Controller.InputEvaluator;
    using Caduhd.Drone.Dji;
    using Caduhd.HandsDetector;
    using Caduhd.Input.Keyboard;
    using Moq;
    using Xunit;

    /// <summary>
    /// Unit tests for the <see cref="CaduhdApp"/> class.
    /// </summary>
    public class CaduhdAppTests : IDisposable
    {
        private readonly Tello tello;
        private readonly UdpClient udpClient;
        private CaduhdApp caduhdApp;
        private IPEndPoint anyEndPoint;

        public CaduhdAppTests()
        {
            string ip = "127.0.0.1";
            int port = 8889;

            this.tello = new Tello(ip, port);

            this.udpClient = new UdpClient(new IPEndPoint(IPAddress.Parse(ip), port));
            this.udpClient.Client.ReceiveTimeout = 1000;

            this.anyEndPoint = new IPEndPoint(IPAddress.Any, 0);
        }

        public void Dispose()
        {
            this.udpClient.Dispose();
        }

        [Fact]
        public void Tello_TelloKeyInputEvaluator_Compatibility()
        {
            var keyInputEvaluator = new TelloKeyInputEvaluator();
            var handsInputEvaluator = new DroneControllerHandsInputEvaluator();

            this.caduhdApp = new CaduhdApp(null, null, this.tello, keyInputEvaluator, handsInputEvaluator);
            string response = string.Empty;

            this.caduhdApp.Input(new KeyInfo(Key.D0, KeyState.Down));

            try
            {
                response = this.udpClient.Receive(ref this.anyEndPoint).AsString();
            }
            catch (SocketException)
            {
            }

            // it needs to be disposed of here
            this.tello.Dispose();

            Assert.Equal("command", response);
        }

        [Fact]
        public void Tello_GeneralDroneKeyInputEvaluator_Compatibility()
        {
            var keyInputEvaluator = new GeneralDroneKeyInputEvaluator();
            var handsInputEvaluator = new DroneControllerHandsInputEvaluator();

            this.caduhdApp = new CaduhdApp(null, null, this.tello, keyInputEvaluator, handsInputEvaluator);
            string response = string.Empty;

            this.caduhdApp.Input(new KeyInfo(Key.D0, KeyState.Down));

            try
            {
                response = this.udpClient.Receive(ref this.anyEndPoint).AsString();
            }
            catch (SocketException)
            {
            }

            // it needs to be disposed of here
            this.tello.Dispose();

            Assert.Equal("command", response);
        }

        [Fact]
        public void HandsInputAnalyzerTuned_HandsInput_Neutral_HandsInputEvaluated_CorrectMoveCommandSent()
        {
            var skinColorHandsDetectorMock = new Mock<ISkinColorHandsDetector>();
            skinColorHandsDetectorMock.Setup(schd => schd.Tuned).Returns(true);

            var left = new NormalizedHand(0.15, 0.5, 0.18);
            var right = new NormalizedHand(0.85, 0.5, 0.18);
            var hands = new NormalizedHands(left, right);
            var handsDetectorResult = new HandsDetectorResult(hands, null);

            skinColorHandsDetectorMock.Setup(schd => schd.DetectHands(It.IsAny<BgrImage>()))
                .Returns(handsDetectorResult);

            var keyInputEvaluator = new GeneralDroneKeyInputEvaluator();
            var handsInputEvaluator = new DroneControllerHandsInputEvaluator();
            handsInputEvaluator.Tune(hands);

            this.caduhdApp = new CaduhdApp(null, skinColorHandsDetectorMock.Object, this.tello, keyInputEvaluator, handsInputEvaluator);

            string response = string.Empty;
            this.caduhdApp.Input(BgrImage.GetBlank(1, 1, Color.Black));

            try
            {
                response = this.udpClient.Receive(ref this.anyEndPoint).AsString();
            }
            catch (SocketException)
            {
            }

            // it needs to be disposed of here
            this.tello.Dispose();

            Assert.Equal("rc 0 0 0 0", response);
        }

        [Fact]
        public void HandsInputAnalyzerTuned_HandsInput_MoveDownwards_HandsInputEvaluated_CorrectMoveCommandSent()
        {
            var skinColorHandsDetectorMock = new Mock<ISkinColorHandsDetector>();
            skinColorHandsDetectorMock.Setup(schd => schd.Tuned).Returns(true);

            var neutralLeft = new NormalizedHand(0.15, 0.5, 0.18);
            var neutralRight = new NormalizedHand(0.85, 0.5, 0.18);
            var neutralHands = new NormalizedHands(neutralLeft, neutralRight);

            var left = new NormalizedHand(0.15, 0.8, 0.18);
            var right = new NormalizedHand(0.85, 0.8, 0.18);
            var hands = new NormalizedHands(left, right);
            var handsDetectorResult = new HandsDetectorResult(hands, null);

            skinColorHandsDetectorMock.Setup(schd => schd.DetectHands(It.IsAny<BgrImage>()))
                .Returns(handsDetectorResult);

            var keyInputEvaluator = new GeneralDroneKeyInputEvaluator();
            var handsInputEvaluator = new DroneControllerHandsInputEvaluator();
            handsInputEvaluator.Tune(neutralHands);

            this.caduhdApp = new CaduhdApp(null, skinColorHandsDetectorMock.Object, this.tello, keyInputEvaluator, handsInputEvaluator);

            string response = string.Empty;
            this.caduhdApp.Input(BgrImage.GetBlank(1, 1, Color.Black));

            try
            {
                response = this.udpClient.Receive(ref this.anyEndPoint).AsString();
            }
            catch (SocketException)
            {
            }

            // it needs to be disposed of here
            this.tello.Dispose();

            Assert.Equal($"rc 0 0 -{this.tello.Speed} 0", response);
        }

        [Fact]
        public void HandsInputIgnored_WhenMoveKeyInputSentBeforeThat()
        {
            var skinColorHandsDetectorMock = new Mock<ISkinColorHandsDetector>();
            skinColorHandsDetectorMock.Setup(schd => schd.Tuned).Returns(true);

            var neutralLeft = new NormalizedHand(0.15, 0.5, 0.18);
            var neutralRight = new NormalizedHand(0.85, 0.5, 0.18);
            var neutralHands = new NormalizedHands(neutralLeft, neutralRight);

            var left = new NormalizedHand(0.15, 0.8, 0.18);
            var right = new NormalizedHand(0.85, 0.8, 0.18);
            var hands = new NormalizedHands(left, right);
            var handsDetectorResult = new HandsDetectorResult(hands, null);

            skinColorHandsDetectorMock.Setup(schd => schd.DetectHands(It.IsAny<BgrImage>()))
                .Returns(handsDetectorResult);

            var keyInputEvaluator = new GeneralDroneKeyInputEvaluator();
            var handsInputEvaluator = new DroneControllerHandsInputEvaluator();
            handsInputEvaluator.Tune(neutralHands);

            this.caduhdApp = new CaduhdApp(null, skinColorHandsDetectorMock.Object, this.tello, keyInputEvaluator, handsInputEvaluator);

            this.caduhdApp.Input(new KeyInfo(Key.Left, KeyState.Down));
            this.udpClient.Client.ReceiveTimeout = 1000;

            string response = string.Empty;
            bool handsInputIgnored = false;

            try
            {
                response = this.udpClient.Receive(ref this.anyEndPoint).AsString();
            }
            catch (SocketException)
            {
            }

            this.caduhdApp.Input(BgrImage.GetBlank(1, 1, Color.Black));

            try
            {
                response = this.udpClient.Receive(ref this.anyEndPoint).AsString();
            }
            catch (SocketException e)
            {
                if (e.SocketErrorCode == SocketError.TimedOut)
                {
                    handsInputIgnored = true;
                }
            }

            this.tello.Dispose();
            Assert.True(handsInputIgnored);
        }

        [Fact]
        public void HandsInputNotIgnored_WhenLandKeyInputSentBeforeThat()
        {
            var skinColorHandsDetectorMock = new Mock<ISkinColorHandsDetector>();
            skinColorHandsDetectorMock.Setup(schd => schd.Tuned).Returns(true);

            var neutralLeft = new NormalizedHand(0.15, 0.5, 0.18);
            var neutralRight = new NormalizedHand(0.85, 0.5, 0.18);
            var neutralHands = new NormalizedHands(neutralLeft, neutralRight);

            var left = new NormalizedHand(0.15, 0.2, 0.18);
            var right = new NormalizedHand(0.85, 0.2, 0.18);
            var hands = new NormalizedHands(left, right);
            var handsDetectorResult = new HandsDetectorResult(hands, null);

            skinColorHandsDetectorMock.Setup(schd => schd.DetectHands(It.IsAny<BgrImage>()))
                .Returns(handsDetectorResult);

            var keyInputEvaluator = new GeneralDroneKeyInputEvaluator();
            var handsInputEvaluator = new DroneControllerHandsInputEvaluator();
            handsInputEvaluator.Tune(neutralHands);

            this.caduhdApp = new CaduhdApp(null, skinColorHandsDetectorMock.Object, this.tello, keyInputEvaluator, handsInputEvaluator);

            this.caduhdApp.Input(new KeyInfo(Key.Space, KeyState.Down));
            this.udpClient.Client.ReceiveTimeout = 1000;

            string response = string.Empty;
            bool handsInputIgnored = false;

            try
            {
                response = this.udpClient.Receive(ref this.anyEndPoint).AsString();
            }
            catch (SocketException)
            {
            }

            this.caduhdApp.Input(BgrImage.GetBlank(1, 1, Color.Black));

            try
            {
                response = this.udpClient.Receive(ref this.anyEndPoint).AsString();
            }
            catch (SocketException e)
            {
                if (e.SocketErrorCode == SocketError.TimedOut)
                {
                    handsInputIgnored = true;
                }
            }

            this.tello.Dispose();
            Assert.False(handsInputIgnored);
            Assert.Equal($"rc 0 0 {this.tello.Speed} 0", response);
        }

        [Fact]
        public void KeyInputIsSentAfterHandsInput_KeyInputIsNotIgnored()
        {
            var skinColorHandsDetectorMock = new Mock<ISkinColorHandsDetector>();
            skinColorHandsDetectorMock.Setup(schd => schd.Tuned).Returns(true);

            var left = new NormalizedHand(0.15, 0.2, 0.18);
            var right = new NormalizedHand(0.85, 0.2, 0.18);
            var hands = new NormalizedHands(left, right);
            var handsDetectorResult = new HandsDetectorResult(hands, null);

            skinColorHandsDetectorMock.Setup(schd => schd.DetectHands(It.IsAny<BgrImage>()))
                .Returns(handsDetectorResult);

            var keyInputEvaluator = new GeneralDroneKeyInputEvaluator();
            var handsInputEvaluator = new DroneControllerHandsInputEvaluator();
            var neutralLeft = new NormalizedHand(0.15, 0.5, 0.18);
            var neutralRight = new NormalizedHand(0.85, 0.5, 0.18);
            var neutralHands = new NormalizedHands(neutralLeft, neutralRight);
            handsInputEvaluator.Tune(neutralHands);

            this.caduhdApp = new CaduhdApp(null, skinColorHandsDetectorMock.Object, this.tello, keyInputEvaluator, handsInputEvaluator);

            this.caduhdApp.Input(BgrImage.GetBlank(1, 1, Color.Black));
            this.udpClient.Client.ReceiveTimeout = 1000;

            string response = string.Empty;
            bool handsInputIgnored = false;

            try
            {
                response = this.udpClient.Receive(ref this.anyEndPoint).AsString();
                Assert.Equal($"rc 0 0 {this.tello.Speed} 0", response);
            }
            catch (SocketException)
            {
            }

            this.caduhdApp.Input(new KeyInfo(Key.D, KeyState.Down));

            try
            {
                response = this.udpClient.Receive(ref this.anyEndPoint).AsString();
            }
            catch (SocketException e)
            {
                if (e.SocketErrorCode == SocketError.TimedOut)
                {
                    handsInputIgnored = true;
                }
            }

            this.tello.Dispose();
            Assert.False(handsInputIgnored);
            Assert.Equal($"rc 0 0 0 {this.tello.Speed}", response);
        }

        [Fact]
        public void SameKeyInputTwwice_OnlySentOnce()
        {
            var skinColorHandsDetector = new SkinColorHandsDetector();
            var keyInputEvaluator = new GeneralDroneKeyInputEvaluator();
            var handsInputEvaluator = new DroneControllerHandsInputEvaluator();

            this.caduhdApp = new CaduhdApp(null, skinColorHandsDetector, this.tello, keyInputEvaluator, handsInputEvaluator);

            this.udpClient.Client.ReceiveTimeout = 1000;

            var keyInfo = new KeyInfo(Key.Right, KeyState.Down);

            this.caduhdApp.Input(keyInfo);
            this.caduhdApp.Input(keyInfo);

            int counter = 0;

            try
            {
                string response = this.udpClient.Receive(ref this.anyEndPoint).AsString();
                Assert.Equal($"rc {this.tello.Speed} 0 0 0", response);
                counter++;
                response = this.udpClient.Receive(ref this.anyEndPoint).AsString();
                counter++;
            }
            catch (SocketException)
            {
            }

            this.tello.Dispose();
            Assert.Equal(1, counter);
        }

        [Fact]
        public void DifferentKeyInputsAfterEachOther_BothSent()
        {
            var skinColorHandsDetector = new SkinColorHandsDetector();
            var keyInputEvaluator = new GeneralDroneKeyInputEvaluator();
            var handsInputEvaluator = new DroneControllerHandsInputEvaluator();

            this.caduhdApp = new CaduhdApp(null, skinColorHandsDetector, this.tello, keyInputEvaluator, handsInputEvaluator);

            this.udpClient.Client.ReceiveTimeout = 1000;

            this.caduhdApp.Input(new KeyInfo(Key.Right, KeyState.Down));
            this.caduhdApp.Input(new KeyInfo(Key.Up, KeyState.Down));

            int counter = 0;

            try
            {
                string response = this.udpClient.Receive(ref this.anyEndPoint).AsString();
                Assert.Equal($"rc {this.tello.Speed} 0 0 0", response);
                counter++;
                response = this.udpClient.Receive(ref this.anyEndPoint).AsString();
                Assert.Equal($"rc {this.tello.Speed} {this.tello.Speed} 0 0", response);
                counter++;
            }
            catch (SocketException)
            {
            }

            this.tello.Dispose();
            Assert.Equal(2, counter);
        }

        [Fact]
        public void SameHandsInputTwice_OnlySentOnce()
        {
            var skinColorHandsDetectorMock = new Mock<ISkinColorHandsDetector>();
            skinColorHandsDetectorMock.Setup(schd => schd.Tuned).Returns(true);

            var neutralLeft = new NormalizedHand(0.15, 0.5, 0.18);
            var neutralRight = new NormalizedHand(0.85, 0.5, 0.18);
            var neutralHands = new NormalizedHands(neutralLeft, neutralRight);

            var left = new NormalizedHand(0.15, 0.8, 0.18);
            var right = new NormalizedHand(0.85, 0.2, 0.18);
            var hands = new NormalizedHands(left, right);
            var handsDetectorResult = new HandsDetectorResult(hands, null);

            skinColorHandsDetectorMock.Setup(schd => schd.DetectHands(It.IsAny<BgrImage>()))
                .Returns(handsDetectorResult);

            var keyInputEvaluator = new GeneralDroneKeyInputEvaluator();
            var handsInputEvaluator = new DroneControllerHandsInputEvaluator();
            handsInputEvaluator.Tune(neutralHands);

            this.caduhdApp = new CaduhdApp(null, skinColorHandsDetectorMock.Object, this.tello, keyInputEvaluator, handsInputEvaluator);

            this.caduhdApp.Input(BgrImage.GetBlank(1, 1, Color.Black));
            this.caduhdApp.Input(BgrImage.GetBlank(1, 1, Color.Black));

            this.udpClient.Client.ReceiveTimeout = 1000;
            int counter = 0;

            try
            {
                string response = this.udpClient.Receive(ref this.anyEndPoint).AsString();
                Assert.Equal($"rc -{this.tello.Speed} 0 0 0", response);
                counter++;
                response = this.udpClient.Receive(ref this.anyEndPoint).AsString();
                counter++;
            }
            catch (SocketException)
            {
            }

            this.tello.Dispose();
            Assert.Equal(1, counter);
        }

        [Fact]
        public void DifferentHandsInputsAfterEachOther_BothSent()
        {
            var skinColorHandsDetectorMock = new Mock<ISkinColorHandsDetector>();
            skinColorHandsDetectorMock.Setup(schd => schd.Tuned).Returns(true);

            var firstLeft = new NormalizedHand(0.15, 0.2, 0.18);
            var firstRight = new NormalizedHand(0.85, 0.2, 0.18);
            var firstHands = new NormalizedHands(firstLeft, firstRight);
            var firstHandsDetectorResult = new HandsDetectorResult(firstHands, null);

            var secondLeft = new NormalizedHand(0.15, 0.8, 0.18);
            var secondRight = new NormalizedHand(0.85, 0.7, 0.18);
            var secondHands = new NormalizedHands(secondLeft, secondRight);
            var secondandsDetectorResult = new HandsDetectorResult(secondHands, null);

            skinColorHandsDetectorMock.SetupSequence(schd => schd.DetectHands(It.IsAny<BgrImage>()))
                .Returns(firstHandsDetectorResult)
                .Returns(secondandsDetectorResult);

            var keyInputEvaluator = new GeneralDroneKeyInputEvaluator();
            var handsInputEvaluator = new DroneControllerHandsInputEvaluator();
            var neutralLeft = new NormalizedHand(0.15, 0.5, 0.18);
            var neutralRight = new NormalizedHand(0.85, 0.5, 0.18);
            var neutralHands = new NormalizedHands(neutralLeft, neutralRight);
            handsInputEvaluator.Tune(neutralHands);

            this.caduhdApp = new CaduhdApp(null, skinColorHandsDetectorMock.Object, this.tello, keyInputEvaluator, handsInputEvaluator);

            this.caduhdApp.Input(BgrImage.GetBlank(1, 1, Color.Black));
            this.caduhdApp.Input(BgrImage.GetBlank(1, 1, Color.Black));

            this.udpClient.Client.ReceiveTimeout = 1000;
            int counter = 0;

            try
            {
                string response = this.udpClient.Receive(ref this.anyEndPoint).AsString();
                Assert.Equal($"rc 0 0 {this.tello.Speed} 0", response);
                counter++;
                response = this.udpClient.Receive(ref this.anyEndPoint).AsString();
                Assert.Equal($"rc 0 0 -{this.tello.Speed} 0", response);
                counter++;
            }
            catch (SocketException)
            {
            }

            this.tello.Dispose();
            Assert.Equal(2, counter);
        }
    }
}
