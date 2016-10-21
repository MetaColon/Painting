using System;
using System.Drawing;

namespace Painting.Types.Paint
{
    public class Coordinate : IComparable
    {
        public float X { get; set; }
        public float Y { get; set; }

        public Coordinate()
        {

        }

        public Coordinate(float x, float y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object obj) => obj is Coordinate && Equals((Coordinate) obj);

        public override int GetHashCode()
        {
            unchecked
            {
                return (X.GetHashCode()*397) ^ Y.GetHashCode();
            }
        }

        public int CompareTo(object obj) => obj is Coordinate ? CompareTo((Coordinate) obj) : -2;

        public int CompareTo(Coordinate obj) =>
            obj.X > X && obj.Y > Y
                ? -1
                : (Math.Abs(obj.X - X) < 0.001 && Math.Abs(obj.Y - Y) < 0.001
                    ? 0
                    : (obj.X < X && obj.Y < Y
                        ? 1
                        : (obj.X + obj.Y).CompareTo(obj.X + obj.Y)));

        protected bool Equals(Coordinate other)
            => other != null && Math.Abs(X - other.X) < 0.001 && Math.Abs(Y - other.Y) < 0.001;

        public override string ToString() => $"X: {X}; Y:{Y}";

        public Coordinate add(Coordinate a) => new Coordinate(X + a.X, Y + a.Y);
        public Coordinate sub(Coordinate s) => add(s.mult(-1));

        public Coordinate mult(double m) => new Coordinate(X*(float) m, Y*(float) m);
        public Coordinate mult(Coordinate m) => new Coordinate(X*m.X, Y*m.Y);

        public PointF GetPointF() => new PointF(X, Y);
    }
}