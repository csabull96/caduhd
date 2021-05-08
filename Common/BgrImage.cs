using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing;

namespace Caduhd.Common
{
    public class BgrImage 
    {
        private readonly Image<Bgr, byte> _image;

        public const int FILL_DRAWING = -1;

        public int Width => _image.Width;

        public int Height => _image.Height;

        public Rectangle Roi 
        {
            get => _image.ROI;
            set => _image.ROI = value;
        }

        public Bitmap Bitmap => _image.Bitmap;

        public BgrImage(Mat mat) : this(mat.ToImage<Bgr, byte>()) { }

        public BgrImage(Image<Bgr, byte> image)
        {
            _image = image;
        }

        public BgrPixel GetPixel(int x, int y) =>     
            new BgrPixel(
                _image.Data[_image.ROI.Y + y, _image.ROI.X + x, 0],
                _image.Data[_image.ROI.Y + y, _image.ROI.X + x, 1],
                _image.Data[_image.ROI.Y + y, _image.ROI.X + x, 2]);

        public void SetPixel(BgrPixel pixel, int x, int y)
        {
            _image.Data[_image.ROI.Y + y, _image.ROI.X + x, 0] = (byte)pixel.Blue;
            _image.Data[_image.ROI.Y + y, _image.ROI.X + x, 1] = (byte)pixel.Green;
            _image.Data[_image.ROI.Y + y, _image.ROI.X + x, 2] = (byte)pixel.Red;
        }

        //public void DrawCircle(int x, int y, Color color, double radius, int thickness)
        //{
        //    Point center = new Point(x, y);
        //    CircleF circle = new CircleF(center, (float)radius);
        //    _image.Draw(circle, new Bgr(color), thickness);
        //}

        public void DrawCircle(double normalizedX, double normalizedY, Color color, double radius, int thickness)
        {
            PointF center = new PointF(
                (float)normalizedX * this.Width,
                (float)normalizedY * this.Height);
            CircleF circle = new CircleF(center, (float)radius);
            _image.Draw(circle, new Bgr(color), thickness);
        }

        public void DrawLineSegment(double normalizedX0, double normalizedY0, double normalizedX1, double normalizedY1, Color color, int thickness)
        {
            _image.Draw(
                new LineSegment2DF(
                    new PointF((float)normalizedX0 * this.Width, (float)normalizedY0 * this.Height), 
                    new PointF((float)normalizedX1 * this.Width, (float)normalizedY1 * this.Height)), 
                new Bgr(color), thickness);
        }

        public void DrawRectangle(Rectangle rectangle, Color color, int thickness)
        {
            _image.Draw(rectangle, new Bgr(color), thickness);
        }

        //public void DrawRectangle(double normalizedX0, double normalizedY0, double normalizedX1, double normalizedY1, Color color, int thickness)
        //{
        //    double smallerX = Math.Min(normalizedX0, normalizedX1);
        //    double greaterX = Math.Max(normalizedX0, normalizedX1);
            
        //    double smallerY = Math.Min(normalizedY0, normalizedY1);
        //    double greaterY = Math.Max(normalizedY0, normalizedY1);

        //    int x = Convert.ToInt32(smallerX * this.Width);
        //    int y = Convert.ToInt32(smallerY * this.Height);
        //    int width = Convert.ToInt32((greaterX - smallerX) * this.Width);
        //    int height = Convert.ToInt32((greaterY - smallerY) * this.Width);

        //    _image.Draw(new Rectangle(x, y, width, height), new Bgr(color), thickness);
        //}

        public BgrImage Merge(BgrImage right)
        {
            BgrImage merged = right.Copy();
            Rectangle roi = new Rectangle(0, 0, merged.Width / 2, merged.Height);
            merged.Roi = roi;
            this.Roi = roi;
            this.CopyTo(merged);
            merged.Roi = Rectangle.Empty;
            this.Roi = Rectangle.Empty;
            return merged;
        }

        //public static BgrImage GetBlank(Color color) =>
        //    new BgrImage(new Image<Bgr, byte>(1920, 1080, new Bgr(color)));

        public BgrImage Copy() => new BgrImage(_image.Copy());

        public void CopyTo(BgrImage destination) => this._image.CopyTo(destination._image);
    }
}
