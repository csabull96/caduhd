namespace Caduhd.Common
{
    using System.Drawing;
    using Emgu.CV;
    using Emgu.CV.Structure;

    /// <summary>
    /// A wrapper class around the Emgu.CV.Image class.
    /// </summary>
    public class BgrImage
    {
        public const int FILL_DRAWING = -1;

        private readonly Image<Bgr, byte> image;

        /// <summary>
        /// Gets the width of the image.
        /// </summary>
        public int Width => this.image.Width;

        /// <summary>
        /// Gets the height of the image.
        /// </summary>
        public int Height => this.image.Height;

        /// <summary>
        /// Gets or sets the region of interest of the image.
        /// </summary>
        public Rectangle Roi
        {
            get => this.image.ROI;
            set => this.image.ROI = value;
        }

        /// <summary>
        /// Gets the image as Bitmap.
        /// </summary>
        public Bitmap Bitmap => this.image.Bitmap;

        /// <summary>
        /// Initializes a new instance of the <see cref="BgrImage"/> class.
        /// </summary>
        /// <param name="mat">The Emgu.CV.Mat object that the BgrImage is going to be constructed of.</param>
        public BgrImage(Mat mat) : this(mat.ToImage<Bgr, byte>()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BgrImage"/> class.
        /// </summary>
        /// <param name="image">The Emgu.CV.Image object that the BgrImage is going to be constructed of.</param>
        public BgrImage(Image<Bgr, byte> image)
        {
            this.image = image;
        }

        /// <summary>
        /// Gets a blank BgrImage with the specified width, height and color.
        /// </summary>
        /// <param name="width">The desired width of the blank BgrImage.</param>
        /// <param name="height">The desired height of the blank BgrImage.</param>
        /// <param name="color">The desired color of the blank BgrImage.</param>
        /// <returns>The blank BgrImage with the desired width, height and color.</returns>
        public static BgrImage GetBlank(int width, int height, Color color) =>
            new BgrImage(new Image<Bgr, byte>(width, height, new Bgr(color)));

        /// <summary>
        /// Gets a BgrPixel of BgrImage at a specified position.
        /// </summary>
        /// <param name="x">The column from which the BgrPixel is requested.</param>
        /// <param name="y">The row from which the BgrPixel is requested.</param>
        /// <returns>The BgrPixel of the BgrImage in the specified position.</returns>
        public BgrPixel GetPixel(int x, int y) =>
            new BgrPixel(
                this.image.Data[this.image.ROI.Y + y, this.image.ROI.X + x, 0],
                this.image.Data[this.image.ROI.Y + y, this.image.ROI.X + x, 1],
                this.image.Data[this.image.ROI.Y + y, this.image.ROI.X + x, 2]);

        /// <summary>
        /// Sets a BgrPixel of BgrImage at a specified position.
        /// </summary>
        /// <param name="pixel">The BgrPixel to set.</param>
        /// <param name="x">The column in which the BgrPixel to set is located.</param>
        /// <param name="y">The row in which the BgrPixel to set is located.</param>
        public void SetPixel(BgrPixel pixel, int x, int y)
        {
            this.image.Data[this.image.ROI.Y + y, this.image.ROI.X + x, 0] = (byte)pixel.Blue;
            this.image.Data[this.image.ROI.Y + y, this.image.ROI.X + x, 1] = (byte)pixel.Green;
            this.image.Data[this.image.ROI.Y + y, this.image.ROI.X + x, 2] = (byte)pixel.Red;
        }

        /// <summary>
        /// Draws a circle.
        /// </summary>
        /// <param name="normalizedX">The normalized value of X.</param>
        /// <param name="normalizedY">The normalized value of Y.</param>
        /// <param name="color">The color to paint with.</param>
        /// <param name="radius">The absulote value of the radius.</param>
        /// <param name="thickness">The thicknes of the outline. Set it to -1 to fill the circle with the chosen color.</param>
        public void DrawCircle(double normalizedX, double normalizedY, Color color, double radius, int thickness)
        {
            PointF center = new PointF(
                (float)normalizedX * this.Width,
                (float)normalizedY * this.Height);
            CircleF circle = new CircleF(center, (float)radius);
            this.image.Draw(circle, new Bgr(color), thickness);
        }

        /// <summary>
        /// Draws a line segment based on two points.
        /// </summary>
        /// <param name="normalizedX0">The normalized X value of the first point.</param>
        /// <param name="normalizedY0">The normalized Y value of the first point.</param>
        /// <param name="normalizedX1">The normalized X value of the second point.</param>
        /// <param name="normalizedY1">The normalized Y value of the second point.</param>
        /// <param name="color">The color to paint with.</param>
        /// <param name="thickness">The thicknes of the line segment.</param>
        public void DrawLineSegment(double normalizedX0, double normalizedY0, double normalizedX1, double normalizedY1, Color color, int thickness)
        {
            var first = new PointF((float)normalizedX0 * this.Width, (float)normalizedY0 * this.Height);
            var second = new PointF((float)normalizedX1 * this.Width, (float)normalizedY1 * this.Height);
            this.image.Draw(new LineSegment2DF(first, second), new Bgr(color), thickness);
        }

        /// <summary>
        /// Draws a rectangle.
        /// </summary>
        /// <param name="rectangle">The rectangle to draw.</param>
        /// <param name="color">The color to paint with.</param>
        /// <param name="thickness">The thicknes of the outline. Set it to -1 to fill the rectangle with the chosen color.</param>
        public void DrawRectangle(Rectangle rectangle, Color color, int thickness)
        {
            this.image.Draw(rectangle, new Bgr(color), thickness);
        }

        /// <summary>
        /// Merges two BgrImages horizontally in the ratio of 1:1 by taking the left half of this BgrImage and the right half of the BgrImage that is passed as a parameter.
        /// </summary>
        /// <param name="right">The BgrImage to merge this BgrImage with.</param>
        /// <returns>The merged BgrImage.</returns>
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

        /// <summary>
        /// Copying this BgrImage.
        /// </summary>
        /// <returns>The copied BgrImage.</returns>
        public BgrImage Copy() => new BgrImage(this.image.Copy());

        /// <summary>
        /// Copies this BgrImage to a specified BgrImage.
        /// </summary>
        /// <param name="destination">The BgrImage to copy this one to.</param>
        public void CopyTo(BgrImage destination) => this.image.CopyTo(destination.image);
    }
}
