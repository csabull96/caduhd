namespace Caduhd.Common
{
    public interface IHistogram
    {
        double Smallest { get; }
       
        double Greatest { get; }

        double[] Normalized { get; }

        void Insert(double value);

        bool TryInsert(double value);
    }
}
