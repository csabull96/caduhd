using System.Drawing;

namespace Ksvydo.HandDetector.Model
{
    public class Hand
    {
        private int m_xTotal = 0;
        private int m_yTotal = 0;
        private int m_weight = 0;

        public Point Position => m_weight <= 0 ? new Point(0, 0) : new Point(m_xTotal / m_weight, m_yTotal / m_weight);
        
        public int Weight => m_weight;
    
        public void Extend(int x, int y)
        {
            m_xTotal += x;
            m_yTotal += y;
            m_weight++;
        }
    }
}
