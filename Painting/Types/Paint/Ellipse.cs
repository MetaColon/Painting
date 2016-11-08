using System;
using System.Drawing;

namespace Painting.Types.Paint
{
    public class Ellipse : Shape
    {
        public Ellipse(float width, Colour lineColour, Coordinate position, Coordinate size, Colour mainColour, float rotation)
            : base(position, size, mainColour, rotation)
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

        public override bool Equals(object obj) => obj is Ellipse && Equals((Ellipse) obj) && Math.Abs(Rotation - ((Ellipse)obj).Rotation) < 0.001;

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = base.GetHashCode();
                hashCode = (hashCode*397) ^ (LineColour?.GetHashCode() ?? 0);
                hashCode = (hashCode*397) ^ Width.GetHashCode();
                hashCode = (hashCode*397) ^ Rotation.GetHashCode();
                return hashCode;
            }
        }

        public void Paint(Graphics p, Coordinate rotationCenterPointFromPosition)
        {
            var trans = p.Transform.Clone();
            if (Rotation != 0)
            {
                p.TranslateTransform(rotationCenterPointFromPosition.X, rotationCenterPointFromPosition.Y);
                p.RotateTransform(Rotation);
                p.TranslateTransform(-rotationCenterPointFromPosition.X, -rotationCenterPointFromPosition.Y);
            }
            if (MainColour.Visible)
                p.FillEllipse(new SolidBrush(MainColour.Color), Position.X, Position.Y, UnturnedSize.X, UnturnedSize.Y);
            if (LineColour.Visible)
                p.DrawEllipse(new Pen(LineColour.Color, Width), Position.X, Position.Y, UnturnedSize.X, UnturnedSize.Y);
            p.Transform = trans;
        }
    }
}