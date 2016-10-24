using System;
using System.Drawing;
using PhysomatikLibrary;

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
                if (Position == null || value == null) return;
                if (!Size.Equals(End.Sub(Position)))
                    Size = End.Sub(Position);
                if (Size == null)
                    return;
                var n = (float)Physomatik.ToDegree(Math.Asin(Size.Y/Length));
                if (Size.X < 0)
                    n = 180 - n;
                if (Math.Abs(Rotation - n) > 0.001)
                    Rotation = n;
            }
        }

        public float Rotation
        {
            get { return _rotation; }
            set
            {
                _rotation = value;
                if (Position == null || End == null || Size == null) return;
                var n = new Coordinate((float) Math.Cos(Physomatik.ToRadian(value)) * Length,
                    (float) Math.Sin(Physomatik.ToRadian(value))*Length).Add(Position);
                if (!End.Equals(n))
                    End = n;
            }
        }

        private Coordinate _position;
        private Coordinate _size;
        private float _rotation;

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
                    End = Position.Add(Size);
            }
        }

        public float Length => Size.Pyth();

        public override Coordinate Size
        {
            get { return _size; }
            set
            {
                _size = value;
                if (Size != null && End != null && !End.Equals (Position.Add (Size)))
                    End = Position.Add (Size);
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