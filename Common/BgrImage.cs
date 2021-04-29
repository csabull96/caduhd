using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing;

namespace Caduhd.Common
{
    public class BgrImage 
    {
        private Image<Bgr, byte> _image;

        public int Width => _image.Width;
        public int Height => _image.Height;
        public Rectangle Roi 
        {
            get => _image.ROI;
            set => _image.ROI = value;
        }
        public Bitmap Bitmap => _image.Bitmap;

        public BgrImage(Mat mat)
        {
            _image = mat.ToImage<Bgr, byte>();
        }

        public BgrImage(Image<Bgr, byte> image)
        {
            _image = image;
        }

        public BgrPixel GetPixel(int x, int y) =>
            new BgrPixel(_image.Data[y, x, 0], _image.Data[y, x, 1], _image.Data[y, x, 2]);

        public void SetPixel(BgrPixel pixel, int x, int y)
        {
            _image.Data[y, x, 0] = (byte)pixel.Blue;
            _image.Data[y, x, 1] = (byte)pixel.Green;
            _image.Data[y, x, 2] = (byte)pixel.Red;
        }

        public void DrawCircle(double x, double y, Color color, double radius, int thickness)
        {
            PointF center = new PointF((int)x, (int)y);
            CircleF circle = new CircleF(center, (float)radius);
            _image.Draw(circle, new Bgr(color), thickness);
        }

        public void DrawRectangle(Rectangle rectangle, Color color, int thickness)
        {
            _image.Draw(rectangle, new Bgr(color), thickness);
        }

        public BgrImage Merge(BgrImage right)
        {
            BgrImage merged = right.Copy();
            Rectangle roi = new Rectangle(0, 0, merged.Width / 2, merged.Height);
            this.Roi = roi;
            merged.Roi = roi;
            this.CopyTo(merged);
            this.Roi = Rectangle.Empty;
            merged.Roi = Rectangle.Empty;
            return merged;
        }

        public BgrImage Copy() => new BgrImage(_image.Copy());

        public void CopyTo(BgrImage destination) => this._image.CopyTo(destination._image);
    }
}
