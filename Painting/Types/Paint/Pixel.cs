using System;
using System.Drawing;

namespace Painting.Types.Paint
{
    public class Pixel
    {
        public Colour PixelColour { get; set; }
        public Coordinate Coordinate { get; set; }
        public float Size { get; set; }

        public Pixel(Colour pixelColour, Coordinate coordinate, float size)
        {
            PixelColour = pixelColour;
            Coordinate = coordinate;
            Size = size;
        }

        public override bool Equals(object obj) => obj is Pixel && Equals((Pixel) obj);

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = PixelColour?.GetHashCode() ?? 0;
                hashCode = (hashCode*397) ^ (Coordinate?.GetHashCode() ?? 0);
                hashCode = (hashCode*397) ^ Size.GetHashCode();
                return hashCode;
            }
        }

        protected bool Equals(Pixel other) => other != null && PixelColour.Equals(other.PixelColour) && Coordinate.Equals(other.Coordinate) && Math.Abs(Size - other.Size) < 0.001;

        public void Paint(Graphics p)
        {
            if (PixelColour.Visible)
                p.FillEllipse(new SolidBrush(PixelColour.Color), Coordinate.X, Coordinate.Y, Size, Size);
        }
    }
}