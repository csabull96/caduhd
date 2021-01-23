using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caduhd.HandDetector.Model
{
    public class Hands
    {
        public Hand Left { get; private set; }
        public Hand Right { get; private set; }

        public Hands(Hand left, Hand right)
        {
            Left = left;
            Right = right;
        }
    }
}
