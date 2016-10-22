using System;
using System.Drawing;

namespace Painting.Types.Paint
{
    public class Line : Shape
    {
        private Coordinate _end;
        public Coordinate End
        {
            get
            {
                return _end;
            }
            set
            {
                _end = value;
                if (Position != null && value != null && !Size.Equals(End.Sub(Position)))
                    Size = End.Sub (Position);
            }
        }

        private Coordinate _position;
        public override Coordinate Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
                if (End != null && Position != null && !Size.Equals(End.Sub(Position)))
                    Size = End.Sub (Position);
            }
        }

        public float Width { get; set; }

        public Line (Coordinate start, Coordinate end, Colour lineColour, float width) : base (start, end.Sub (start), lineColour)
        {
            End = end;
            Width = width;
        }

        public override bool Equals (object obj) => obj is Line && Equals ((Line) obj);

        public override int GetHashCode ()
        {
            unchecked
            {
                var hashCode = base.GetHashCode ();
                hashCode = (hashCode * 397) ^ (End?.GetHashCode () ?? 0);
                hashCode = (hashCode * 397) ^ Width.GetHashCode ();
                return hashCode;
            }
        }

        protected bool Equals (Line other) => other != null && Equals (End, other.End) && MainColour.Equals (other.MainColour) && Equals (Size, other.Size) && Math.Abs (Width - other.Width) < 0.001;

        public void Paint (Graphics p)
        {
            if (MainColour.Visible)
                p.DrawLine (new Pen (MainColour.Color, Width), Position.X, Position.Y, End.X, End.Y);
        }
    }
}