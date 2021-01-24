using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Caduhd.Drone
{
    public class Tello : IDrone, IStreamable
    {
        private IDroneMovement m_movement;

        public event DroneVideoEventHandler Feed;

        public DroneState State { get; set; }

        public Tello()
        {

            // Where shoudl I initialize the m_droneMovement?
            //

            State = DroneState.Grounded;
        }

        public void Connect()
        {
            UdpClient udpClient = new UdpClient();
            byte[] sendbuf = Encoding.ASCII.GetBytes("command");
            udpClient.Send(sendbuf, sendbuf.Length, new IPEndPoint(IPAddress.Parse("192.168.10.1"), 8889));
        }

        public void Disconnect()
        {
            throw new NotImplementedException();
        }

        public void TakeOff()
        {
            State = DroneState.Flying;

            UdpClient udpClient = new UdpClient();
            byte[] sendbuf = Encoding.ASCII.GetBytes("takeoff");
            udpClient.Send(sendbuf, sendbuf.Length, new IPEndPoint(IPAddress.Parse("192.168.10.1"), 8889));
        }

        public void Land()
        {
            State = DroneState.Grounded;

            UdpClient udpClient = new UdpClient();
            byte[] sendbuf = Encoding.ASCII.GetBytes("land");
            udpClient.Send(sendbuf, sendbuf.Length, new IPEndPoint(IPAddress.Parse("192.168.10.1"), 8889));
        }

        public void Move(IDroneMovement movement)
        {
            // 1) check the values (-100; 100)
            m_movement = movement;
            UdpClient udpClient = new UdpClient();
            byte[] sendbuf = Encoding.ASCII.GetBytes($"rc {m_movement.Lateral} {m_movement.Longitudinal} {m_movement.Vertical} {m_movement.Yaw}");
            udpClient.Send(sendbuf, sendbuf.Length, new IPEndPoint(IPAddress.Parse("192.168.10.1"), 8889));
        }

        public void StartVideoStream()
        {
            UdpClient udpClient = new UdpClient();
            byte[] sendbuf = Encoding.ASCII.GetBytes("streamon");
            udpClient.Send(sendbuf, sendbuf.Length, new IPEndPoint(IPAddress.Parse("192.168.10.1"), 8889));

            Task.Run(() =>
            {
                VideoCapture vc = new VideoCapture("udp://0.0.0.0:11111");
                while (true)
                {
                    var frameCount = vc.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameCount);
                    Mat frame = new Mat();
                    vc.Read(frame);

                        Feed?.Invoke(this, new DroneVideoEventArgs(frame.Bitmap));

                }
            });
        }

        public void StopVideoStream()
        {
            UdpClient udpClient = new UdpClient();
            byte[] sendbuf = Encoding.ASCII.GetBytes("streamoff");
            udpClient.Send(sendbuf, sendbuf.Length, new IPEndPoint(IPAddress.Parse("192.168.10.1"), 8889));
        }

        public string GetVideoStreamAddress()
        {
            // this should return a IPEndPoint (ip address + port)
            return "udp://0.0.0.0:11111";
        }

    }
}
