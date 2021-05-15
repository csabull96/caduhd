namespace Caduhd.Common.Tests
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Emgu.CV;
    using Emgu.CV.CvEnum;
    using Emgu.CV.Structure;
    using Xunit;

    public class BgrImageTests
    {
        private readonly Bitmap bitmap;
        private readonly Image<Bgr, byte> emguImage;
        private readonly BgrImage bgrImage;

        public BgrImageTests()
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            string resource = executingAssembly.GetManifestResourceNames()
                .FirstOrDefault(r => r.Contains("colorful_houses.jpg"));
            Stream resourceStream = executingAssembly.GetManifestResourceStream(resource);

            this.bitmap = new Bitmap(resourceStream);
            this.emguImage = new Image<Bgr, byte>(this.bitmap);
            this.bgrImage = new BgrImage(this.emguImage);
        }

        public static IEnumerable<object[]> RoiTestData => new List<object[]>()
        {
            new object[] { 123, 98, 205, 335 },
            new object[] { 77, 145, 111, 200 },
            new object[] { 200, 200, 720, 360 },
        };

        public static IEnumerable<object[]> GetPixelTestData => new List<object[]>()
        {
            new object[] { 23, 680 },
            new object[] { 0, 0 },
            new object[] { 789, 301 },
        };

        public static IEnumerable<object[]> SetPixelTestData => new List<object[]>()
        {
            new object[] { 23, 680, 28, 0, 255 },
            new object[] { 23, 680, 123, 78, 201 },
            new object[] { 0, 0, 63, 179, 11 },
            new object[] { 0, 0, 227, 33, 19 },
            new object[] { 789, 301, 245, 255, 233 },
            new object[] { 789, 301, 1, 9, 7 },
        };

        public static IEnumerable<object[]> DrawCircleTestData => new List<object[]>()
        {
            new object[] { 0.2, 0.3, 223, 179, 23, 11.2, 3 },
            new object[] { 0.44, 0.78, 111, 178, 137, 6.2, 3 },
            new object[] { 0.72, 0.13, 18, 157, 99, 9.78, -1 },
        };

        public static IEnumerable<object[]> DrawLineSegmentTestData => new List<object[]>()
        {
            new object[] { 0.2, 0.6, 0.3, 0.9, 23, 142, 78, 11 },
            new object[] { 0.923, 0.476, 0.71, 0.253, 29, 42, 3, -1 },
            new object[] { 0.5456, 0.988, 0.33, 0.35, 241, 207, 113, 9 },
        };

        public static IEnumerable<object[]> DrawRectangleTestData => new List<object[]>()
        {
            new object[] { 2, 20, 100, 210, 32, 214, 78, -1 },
            new object[] { 23, 376, 71, 253, 29, 42, 3, 7 },
            new object[] { 456, 156, 330, 235, 241, 207, 113, 9 },
        };

        [Fact]
        public void WidthGetter_GetsCorrectWidth()
        {
            Assert.Equal(this.emguImage.Width, this.bgrImage.Width);
        }

        [Fact]
        public void HeightGetter_GetsCorrectHeight()
        {
            Assert.Equal(this.bitmap.Height, this.bgrImage.Height);
        }

        [Theory]
        [MemberData(nameof(RoiTestData))]
        public void RoiGetter_GetsCorrectRoi(int x, int y, int width, int height)
        {
            Rectangle roi = new Rectangle(x, y, width, height);
            this.emguImage.ROI = roi;
            Assert.Equal(roi, this.bgrImage.Roi);
        }

        [Theory]
        [MemberData(nameof(RoiTestData))]
        public void RoiSetter_SetsRoiCorrectly(int x, int y, int width, int height)
        {
            Rectangle roi = new Rectangle(x, y, width, height);
            this.bgrImage.Roi = roi;
            Assert.Equal(roi, this.emguImage.ROI);
        }

        [Theory]
        [MemberData(nameof(GetPixelTestData))]
        public void GetPixel_GetsTheCorrectPixel(int x, int y)
        {
            Assert.True(this.BgrEqualsBgrPixel(this.emguImage[y, x], this.bgrImage.GetPixel(x, y)));
        }

        [Theory]
        [MemberData(nameof(SetPixelTestData))]
        public void SetPixel_SetsPixelCorrectly(int x, int y, int blue, int green, int red)
        {
            BgrPixel pixel = new BgrPixel(blue, green, red);
            this.bgrImage.SetPixel(pixel, x, y);
            Assert.True(this.BgrEqualsBgrPixel(this.emguImage[y, x], pixel));
        }

        [Theory]
        [MemberData(nameof(DrawCircleTestData))]
        public void DrawCircle_CircleDrawnCorrectly(int normalizedX, int normalizedY, int blue, int green, int red, double radius, int thickness)
        {
            PointF absoluteCenter = new PointF(
                (float)normalizedX * this.emguImage.Width,
                (float)normalizedY * this.emguImage.Height);
            Color color = Color.FromArgb(red, green, blue);
            CircleF circle = new CircleF(absoluteCenter, (float)radius);

            this.emguImage.Draw(circle, new Bgr(color), thickness);
            this.bgrImage.DrawCircle(normalizedX, normalizedY, color, radius, thickness);

            Assert.True(this.CompareImagesPixelByPixel(this.emguImage, this.bgrImage));
        }

        [Theory]
        [MemberData(nameof(DrawLineSegmentTestData))]
        public void DrawLineSegment_LineSegmentDrawnCorrectly(int normalizedX0, int normalizedY0, int normalizedX1, int normalizedY1, int blue, int green, int red, int thickness)
        {
            Color color = Color.FromArgb(red, green, blue);

            this.emguImage.Draw(
               new LineSegment2DF(
                   new PointF((float)normalizedX0 * this.emguImage.Width, (float)normalizedY0 * this.emguImage.Height),
                   new PointF((float)normalizedX1 * this.emguImage.Width, (float)normalizedY1 * this.emguImage.Height)),
               new Bgr(color),
               thickness);

            this.bgrImage.DrawLineSegment(normalizedX0, normalizedY0, normalizedX1, normalizedY1, color, thickness);

            Assert.True(this.CompareImagesPixelByPixel(this.emguImage, this.bgrImage));
        }

        [Theory]
        [MemberData(nameof(DrawRectangleTestData))]
        public void DrawRectangle_RectangleDrawnCorrectly(int x, int y, int width, int height, int blue, int green, int red, int thickness)
        {
            Rectangle rectangle = new Rectangle(x, y, width, height);
            Color color = Color.FromArgb(red, green, blue);

            this.emguImage.Draw(rectangle, new Bgr(color), thickness);
            this.bgrImage.DrawRectangle(rectangle, color, thickness);

            Assert.True(this.CompareImagesPixelByPixel(this.emguImage, this.bgrImage));
        }

        [Fact]
        public void Merge_TheMergedImageHasTheCorrectSize()
        {
            BgrImage bgrImageUpsideDown = new BgrImage(this.emguImage.Flip(FlipType.Horizontal));
            BgrImage imageMerged = bgrImageUpsideDown.Merge(this.bgrImage);

            Assert.Equal(this.bgrImage.Width, imageMerged.Width);
            Assert.Equal(this.bgrImage.Height, imageMerged.Height);
        }

        [Fact]
        public void Merge_BgrImagesMergedCorrectly()
        {
            BgrImage bgrImageUpsideDown = new BgrImage(this.emguImage.Flip(FlipType.Horizontal));
            BgrImage imageMerged = bgrImageUpsideDown.Merge(this.bgrImage);

            Assert.False(this.CompareImagesPixelByPixel(this.bgrImage, imageMerged));

            int half = imageMerged.Width / 2;
            bool success = true;
            int row = 0;
            int column = 0;

            while (row < imageMerged.Height && success)
            {
                while (column < imageMerged.Width && success)
                {
                    if (column < half)
                    {
                        if (!this.AreBgrPixelsTheSame(bgrImageUpsideDown.GetPixel(column, row), imageMerged.GetPixel(column, row)))
                        {
                            success = false;
                        }
                    }
                    else
                    {
                        if (!this.AreBgrPixelsTheSame(this.bgrImage.GetPixel(column, row), imageMerged.GetPixel(column, row)))
                        {
                            success = false;
                        }
                    }

                    column++;
                }

                column = 0;
                row++;
            }

            Assert.True(success);
        }

        [Fact]
        public void Copy_CopyingBgrImage_ReferencesAreDifferemt()
        {
            Assert.False(object.ReferenceEquals(this.bgrImage, this.bgrImage.Copy()));
        }

        [Fact]
        public void Copy_DrawingOnOriginalBgrImageAfterCopying_ImagesAreDifferent()
        {
            BgrImage bgrImageCopy = this.bgrImage.Copy();
            this.bgrImage.DrawCircle(0.3, 0.4, Color.Red, 11, -1);
            Assert.False(this.CompareImagesPixelByPixel(this.bgrImage, bgrImageCopy));
        }

        [Fact]
        public void CopyTo_CopyingBgrImageToDestination_ReferencesAreDifferent()
        {
            BgrImage bgrImageCopy = new BgrImage(this.emguImage.CopyBlank());
            this.bgrImage.CopyTo(bgrImageCopy);
            Assert.False(object.ReferenceEquals(this.bgrImage, bgrImageCopy));
        }

        [Fact]
        public void CopyTo_DrawingOnOriginalBgrImageAfterCopying_ImagesAreDifferent()
        {
            BgrImage bgrImageCopy = new BgrImage(this.emguImage.CopyBlank());
            this.bgrImage.CopyTo(bgrImageCopy);
            this.bgrImage.DrawCircle(0.3, 0.4, Color.Red, 11, -1);
            Assert.False(this.CompareImagesPixelByPixel(this.bgrImage, bgrImageCopy));
        }

        private bool CompareImagesPixelByPixel(Image<Bgr, byte> emguImage, BgrImage bgrImage)
        {
            if (emguImage.Width != bgrImage.Width || emguImage.Height != bgrImage.Height)
            {
                return false;
            }

            for (int y = 0; y < emguImage.Height; y++)
            {
                for (int x = 0; x < bgrImage.Width; x++)
                {
                    if (!this.BgrEqualsBgrPixel(emguImage[y, x], bgrImage.GetPixel(x, y)))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private bool CompareImagesPixelByPixel(BgrImage a, BgrImage b)
        {
            if (a.Width != b.Width || a.Height != b.Height)
            {
                return false;
            }

            for (int y = 0; y < a.Height; y++)
            {
                for (int x = 0; x < b.Width; x++)
                {
                    if (!this.AreBgrPixelsTheSame(a.GetPixel(x, y), b.GetPixel(x, y)))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private bool BgrEqualsBgrPixel(Bgr emguPixel, BgrPixel bgrPixel) =>
             emguPixel.Blue == bgrPixel.Blue &&
             emguPixel.Green == bgrPixel.Green &&
             emguPixel.Red == bgrPixel.Red;

        private bool AreBgrPixelsTheSame(BgrPixel a, BgrPixel b) =>
             a.Blue == b.Blue &&
             a.Green == b.Green &&
             a.Red == b.Red;
    }
}
