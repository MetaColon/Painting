using System;
using System.Drawing;

namespace Painting.Types.Paint
{
    public class Rectangle : Shape
    {
        public float Width { get; set; }
        public Colour LineColour { get; set; }

        public Rectangle(float width, Colour lineColour, Coordinate position, Coordinate size, Colour mainColour)
            : base (position, size, mainColour)
        {
            Width = width;
            LineColour = lineColour;
        }

        public override bool Equals(object obj) => obj is Rectangle && Equals((Rectangle) obj);

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = base.GetHashCode();
                hashCode = (hashCode*397) ^ Width.GetHashCode();
                hashCode = (hashCode*397) ^ (LineColour?.GetHashCode() ?? 0);
                return hashCode;
            }
        }

        protected bool Equals(Rectangle other) => other != null && base.Equals(other) && Math.Abs(Width - other.Width) < 0.001 && Equals(LineColour, other.LineColour);

        public void Paint(Graphics p)
        {
                if (MainColour.Visible)
                    p.FillRectangle(new SolidBrush(MainColour.Color), Position.X, Position.Y, Size.X, Size.Y);
                if (LineColour.Visible)
                    p.DrawRectangle(new Pen(LineColour.Color, Width), Position.X, Position.Y, Size.X, Size.Y);
                return;
        }
    }
}