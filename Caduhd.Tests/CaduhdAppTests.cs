using Caduhd.Common;
using Caduhd.Controller.InputEvaluator;
using Caduhd.Drone;
using Caduhd.Drone.Dji;
using Caduhd.HandsDetector;
using Caduhd.Input.Keyboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xunit;

namespace Caduhd.Tests
{
    public class CaduhdAppTests
    {
        string ip = "127.0.0.1";
        int port = 8889;

        private CaduhdApp _caduhdApp;
        private AbstractDrone _tello;

        public CaduhdAppTests()
        {
            _tello = new Tello(ip, port);
        }

        [Fact]
        public void test1()
        {
            var keyInputEvaluator = new TelloKeyInputEvaluator();
            var handsInputEvaluator = new DroneControllerHandsInputEvaluator();

            StartFakeDroneListener();
            ISkinColorHandsDetector obj = new SkinColorHandsDetector();
            
            _caduhdApp = new CaduhdApp(null, null, _tello, keyInputEvaluator, handsInputEvaluator);

        
            _caduhdApp.Input(new KeyInfo(Key.D0, KeyState.Down));
            
        }


        private async void StartFakeDroneListener()
        {
            await Task.Run(() => 
            {
                UdpClient udpClient = new UdpClient(new IPEndPoint(IPAddress.Parse(ip), port));
                IPEndPoint anyIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                bool done = false;
                while (!done)
                {
                    byte[] result = udpClient.Receive(ref anyIpEndPoint);
                    string characters = result.AsString();
                    done = true;
                }
                _tello.Dispose();
            });
        }
    }
}
