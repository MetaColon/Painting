using System;
using System.Drawing;

namespace Painting.Types.Paint
{
    public class Ellipse : Shape
    {
        public Ellipse(float width, Colour lineColour, Coordinate position, Coordinate size, Colour mainColour)
            : base(position, size, mainColour)
        {
            Width = width;
            LineColour = lineColour;
        }

        public Colour LineColour { get; set; }
        public float Width { get; set; }

        protected bool Equals(Ellipse other)
            =>
            (other != null) && base.Equals(other) && (Math.Abs(Width - other.Width) < 0.001) &&
            LineColour.Equals(other.LineColour);

        public override bool Equals(object obj) => obj is Ellipse && Equals((Ellipse) obj);

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = base.GetHashCode();
                hashCode = (hashCode*397) ^ Width.GetHashCode();
                hashCode = (hashCode*397) ^ LineColour.GetHashCode();
                return hashCode;
            }
        }

        public void Paint(Graphics p)
        {
            if (MainColour.Visible)
                p.FillEllipse(new SolidBrush(MainColour.Color), Position.X, Position.Y, Size.X, Size.Y);
            if (LineColour.Visible)
                p.DrawEllipse(new Pen(LineColour.Color, Width), Position.X, Position.Y, Size.X, Size.Y);
        }
    }
}