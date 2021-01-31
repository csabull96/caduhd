using Caduhd.HandDetector.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caduhd.Controller
{
    public class HandsDetectedEventArgs : EventArgs
    {

        public Hands Hands { get; private set; }
        public string Movement { get; private set; }

        public HandsDetectedEventArgs(Hands hands, string movement)
        {
            Hands = hands;
            Movement = movement;
        }
    }
}
