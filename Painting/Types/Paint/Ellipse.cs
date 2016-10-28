using System;
using System.Drawing;

namespace Painting.Types.Paint
{
    public class Ellipse : Shape
    {
        public Ellipse(float width, Colour lineColour, Coordinate position, Coordinate size, Colour mainColour, float rotation)
            : base(position, size, mainColour)
        {
            Width = width;
            LineColour = lineColour;
            Rotation = rotation;
        }

        public Colour LineColour { get; set; }
        public float Width { get; set; }
        public float Rotation { get; set; }

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
            p.TranslateTransform(Position.X + rotationCenterPointFromPosition.X, Position.Y + rotationCenterPointFromPosition.Y);
            p.RotateTransform(Rotation);
            if (MainColour.Visible)
                p.FillEllipse(new SolidBrush(MainColour.Color), -rotationCenterPointFromPosition.X, -rotationCenterPointFromPosition.Y, Size.X, Size.Y);
            if (LineColour.Visible)
                p.DrawEllipse(new Pen(LineColour.Color, Width), -rotationCenterPointFromPosition.X, -rotationCenterPointFromPosition.Y, Size.X, Size.Y);
            p.RotateTransform(-Rotation);
            p.TranslateTransform (-Position.X - rotationCenterPointFromPosition.X, -rotationCenterPointFromPosition.Y);
        }
    }
}