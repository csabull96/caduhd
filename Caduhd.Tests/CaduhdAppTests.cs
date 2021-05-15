using Caduhd.Common;
using Caduhd.Controller.InputEvaluator;
using Caduhd.Drone.Dji;
using Caduhd.HandsDetector;
using Caduhd.Input.Keyboard;
using Moq;
using System;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Input;
using Xunit;

namespace Caduhd.Tests
{
    public class CaduhdAppTests : IDisposable
    {
        private CaduhdApp _caduhdApp;
        private Tello _tello;
        private UdpClient _udpClient;
        private IPEndPoint _anyEndPoint;

        public CaduhdAppTests()
        {
            string ip = "127.0.0.1";
            int port = 8889;

            _tello = new Tello(ip, port);

            _udpClient = new UdpClient(new IPEndPoint(IPAddress.Parse(ip), port));
            _udpClient.Client.ReceiveTimeout = 1000;

            _anyEndPoint = new IPEndPoint(IPAddress.Any, 0);
        }

        public void Dispose()
        {
            _udpClient.Dispose();
        }

        [Fact]
        public void Tello_TelloKeyInputEvaluator_Compatibility()
        {
            var keyInputEvaluator = new TelloKeyInputEvaluator();
            var handsInputEvaluator = new DroneControllerHandsInputEvaluator();

            _caduhdApp = new CaduhdApp(null, null, _tello, keyInputEvaluator, handsInputEvaluator);
            string response = string.Empty;

            _caduhdApp.Input(new KeyInfo(Key.D0, KeyState.Down));

            try
            {
                response = _udpClient.Receive(ref _anyEndPoint).AsString();
            }
            catch (SocketException)
            {
            }

            // it needs to be disposed of here
            _tello.Dispose();

            Assert.Equal("command", response);
        }

        [Fact]
        public void Tello_GeneralDroneKeyInputEvaluator_Compatibility()
        {
            var keyInputEvaluator = new GeneralDroneKeyInputEvaluator();
            var handsInputEvaluator = new DroneControllerHandsInputEvaluator();

            _caduhdApp = new CaduhdApp(null, null, _tello, keyInputEvaluator, handsInputEvaluator);
            string response = string.Empty;

            _caduhdApp.Input(new KeyInfo(Key.D0, KeyState.Down));

            try
            {
                response = _udpClient.Receive(ref _anyEndPoint).AsString();
            }
            catch (SocketException)
            {
            }

            // it needs to be disposed of here
            _tello.Dispose();

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

            _caduhdApp = new CaduhdApp(null, skinColorHandsDetectorMock.Object, _tello, keyInputEvaluator, handsInputEvaluator);

            string response = string.Empty;
            _caduhdApp.Input(BgrImage.GetBlank(1, 1, Color.Black));

            try
            {
                response = _udpClient.Receive(ref _anyEndPoint).AsString();
            }
            catch (SocketException)
            {
            }

            // it needs to be disposed of here
            _tello.Dispose();

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

            _caduhdApp = new CaduhdApp(null, skinColorHandsDetectorMock.Object, _tello, keyInputEvaluator, handsInputEvaluator);

            string response = string.Empty;
            _caduhdApp.Input(BgrImage.GetBlank(1, 1, Color.Black));

            try
            {
                response = _udpClient.Receive(ref _anyEndPoint).AsString();
            }
            catch (SocketException)
            {
            }

            // it needs to be disposed of here
            _tello.Dispose();

            Assert.Equal($"rc 0 0 -{_tello.Speed} 0", response);
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

            _caduhdApp = new CaduhdApp(null, skinColorHandsDetectorMock.Object, _tello, keyInputEvaluator, handsInputEvaluator);

            _caduhdApp.Input(new KeyInfo(Key.Left, KeyState.Down));
            _udpClient.Client.ReceiveTimeout = 1000;

            string response = string.Empty;
            bool handsInputIgnored = false;

            try
            {
                response = _udpClient.Receive(ref _anyEndPoint).AsString();
            }
            catch (SocketException)
            {
            }

            _caduhdApp.Input(BgrImage.GetBlank(1, 1, Color.Black));

            try
            {
                response = _udpClient.Receive(ref _anyEndPoint).AsString();
            }
            catch (SocketException e)
            {
                if (e.SocketErrorCode == SocketError.TimedOut)
                {
                    handsInputIgnored = true;
                }

            }

            _tello.Dispose();
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

            _caduhdApp = new CaduhdApp(null, skinColorHandsDetectorMock.Object, _tello, keyInputEvaluator, handsInputEvaluator);

            _caduhdApp.Input(new KeyInfo(Key.Space, KeyState.Down));
            _udpClient.Client.ReceiveTimeout = 1000;

            string response = string.Empty;
            bool handsInputIgnored = false;

            try
            {
                response = _udpClient.Receive(ref _anyEndPoint).AsString();
            }
            catch (SocketException)
            {
            }

            _caduhdApp.Input(BgrImage.GetBlank(1, 1, Color.Black));

            try
            {
                response = _udpClient.Receive(ref _anyEndPoint).AsString();
            }
            catch (SocketException e)
            {
                if (e.SocketErrorCode == SocketError.TimedOut)
                {
                    handsInputIgnored = true;
                }

            }

            _tello.Dispose();
            Assert.False(handsInputIgnored);
            Assert.Equal($"rc 0 0 {_tello.Speed} 0", response);
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

            _caduhdApp = new CaduhdApp(null, skinColorHandsDetectorMock.Object, _tello, keyInputEvaluator, handsInputEvaluator);

            _caduhdApp.Input(BgrImage.GetBlank(1, 1, Color.Black));
            _udpClient.Client.ReceiveTimeout = 1000;

            string response = string.Empty;
            bool handsInputIgnored = false;

            try
            {
                response = _udpClient.Receive(ref _anyEndPoint).AsString();
                Assert.Equal($"rc 0 0 {_tello.Speed} 0", response);
            }
            catch (SocketException)
            {
            }

            _caduhdApp.Input(new KeyInfo(Key.D, KeyState.Down));

            try
            {
                response = _udpClient.Receive(ref _anyEndPoint).AsString();
            }
            catch (SocketException e)
            {
                if (e.SocketErrorCode== SocketError.TimedOut)
                {
                    handsInputIgnored = true;
                }

            }

            _tello.Dispose();
            Assert.False(handsInputIgnored);
            Assert.Equal($"rc 0 0 0 {_tello.Speed}", response);
        }

        [Fact]
        public void SameKeyInputTwwice_OnlySentOnce()
        {
            var skinColorHandsDetector = new SkinColorHandsDetector();
            var keyInputEvaluator = new GeneralDroneKeyInputEvaluator();
            var handsInputEvaluator = new DroneControllerHandsInputEvaluator();

            _caduhdApp = new CaduhdApp(null, skinColorHandsDetector, _tello, keyInputEvaluator, handsInputEvaluator);

            _udpClient.Client.ReceiveTimeout = 1000;

            var keyInfo = new KeyInfo(Key.Right, KeyState.Down);

            _caduhdApp.Input(keyInfo);
            _caduhdApp.Input(keyInfo);

            int counter = 0;

            try
            {
                string response = _udpClient.Receive(ref _anyEndPoint).AsString();
                Assert.Equal($"rc {_tello.Speed} 0 0 0", response);
                counter++;
                response = _udpClient.Receive(ref _anyEndPoint).AsString();
                counter++;
            }
            catch (SocketException)
            {
            }

          
            _tello.Dispose();
            Assert.Equal(1, counter);
        }

        [Fact]
        public void DifferentKeyInputsAfterEachOther_BothSent()
        {
            var skinColorHandsDetector = new SkinColorHandsDetector();
            var keyInputEvaluator = new GeneralDroneKeyInputEvaluator();
            var handsInputEvaluator = new DroneControllerHandsInputEvaluator();

            _caduhdApp = new CaduhdApp(null, skinColorHandsDetector, _tello, keyInputEvaluator, handsInputEvaluator);

            _udpClient.Client.ReceiveTimeout = 1000;

            _caduhdApp.Input(new KeyInfo(Key.Right, KeyState.Down));
            _caduhdApp.Input(new KeyInfo(Key.Up, KeyState.Down));

            int counter = 0;

            try
            {
                string response = _udpClient.Receive(ref _anyEndPoint).AsString();
                Assert.Equal($"rc {_tello.Speed} 0 0 0", response);
                counter++;
                response = _udpClient.Receive(ref _anyEndPoint).AsString();
                Assert.Equal($"rc {_tello.Speed} {_tello.Speed} 0 0", response);
                counter++;
            }
            catch (SocketException)
            {
            }

            _tello.Dispose();
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

            _caduhdApp = new CaduhdApp(null, skinColorHandsDetectorMock.Object, _tello, keyInputEvaluator, handsInputEvaluator);

            _caduhdApp.Input(BgrImage.GetBlank(1, 1, Color.Black));
            _caduhdApp.Input(BgrImage.GetBlank(1, 1, Color.Black));

            _udpClient.Client.ReceiveTimeout = 1000;
            int counter = 0;

            try
            {
                string response = _udpClient.Receive(ref _anyEndPoint).AsString();
                Assert.Equal($"rc -{_tello.Speed} 0 0 0", response);
                counter++;
                response = _udpClient.Receive(ref _anyEndPoint).AsString();
                counter++;
            }
            catch (SocketException)
            {
            }

            _tello.Dispose();
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

            _caduhdApp = new CaduhdApp(null, skinColorHandsDetectorMock.Object, _tello, keyInputEvaluator, handsInputEvaluator);

            _caduhdApp.Input(BgrImage.GetBlank(1, 1, Color.Black));
            _caduhdApp.Input(BgrImage.GetBlank(1, 1, Color.Black));

            _udpClient.Client.ReceiveTimeout = 1000;
            int counter = 0;

            try
            {
                string response = _udpClient.Receive(ref _anyEndPoint).AsString();
                Assert.Equal($"rc 0 0 {_tello.Speed} 0", response);
                counter++;
                response = _udpClient.Receive(ref _anyEndPoint).AsString();
                Assert.Equal($"rc 0 0 -{_tello.Speed} 0", response);
                counter++;
            }
            catch (SocketException)
            {
            }

            _tello.Dispose();
            Assert.Equal(2, counter);
        }
    }
}
