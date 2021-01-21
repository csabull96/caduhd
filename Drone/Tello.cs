﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Caduhd.Drone
{
    class Tello : IDrone, IStreamable
    {
        private IDroneMovement m_droneMovement;
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

        public void SetMovement(IDroneMovement movement)
        {
            // 1) check the values (-100; 100)
            m_droneMovement = movement;
            UdpClient udpClient = new UdpClient();
            byte[] sendbuf = Encoding.ASCII.GetBytes($"rc {m_droneMovement.Horizontal} {m_droneMovement.Longitudinal} {m_droneMovement.Vertical} {m_droneMovement.Yaw}");
            udpClient.Send(sendbuf, sendbuf.Length, new IPEndPoint(IPAddress.Parse("192.168.10.1"), 8889));
        }

        public void StartVideoStream()
        {
            UdpClient udpClient = new UdpClient();
            byte[] sendbuf = Encoding.ASCII.GetBytes("streamon");
            udpClient.Send(sendbuf, sendbuf.Length, new IPEndPoint(IPAddress.Parse("192.168.10.1"), 8889));
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
