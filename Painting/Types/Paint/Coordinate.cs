using System;
using System.Drawing;
using Painting.Util;

namespace Painting.Types.Paint
{
    public class Coordinate : IComparable
    {
        public Coordinate(Coordinate coordinate)
        {
            X = coordinate.X;
            Y = coordinate.Y;
        }

        public Coordinate(float x, float y)
        {
            X = x;
            Y = y;
        }

        public Coordinate(Point point)
        {
            X = point.X;
            Y = point.Y;
        }

        public float X { get; set; }
        public float Y { get; set; }

        public int CompareTo(object obj) => obj is Coordinate ? CompareTo((Coordinate) obj) : -2;

        private bool Equals(Coordinate other)
            => (other != null) && (Math.Abs(X - other.X) < 0.001) && (Math.Abs(Y - other.Y) < 0.001);

        public Coordinate Add(Coordinate a) => new Coordinate(X + a.X, Y + a.Y);
        public Coordinate Add(float a) => new Coordinate(X + a, Y + a);

        public int CompareTo(Coordinate obj) =>
            (obj.X > X) && (obj.Y > Y)
                ? -1
                : ((Math.Abs(obj.X - X) < 0.001) && (Math.Abs(obj.Y - Y) < 0.001)
                    ? 0
                    : ((obj.X < X) && (obj.Y < Y)
                        ? 1
                        : /*(obj.X + obj.Y).CompareTo(obj.X + obj.Y)))*/0));

        public Coordinate Div(double d) => new Coordinate(X/(float) d, Y/(float) d);
        public Coordinate Div(Coordinate d) => new Coordinate(X/d.X, Y/d.Y);

        public override bool Equals(object obj) => obj is Coordinate && Equals((Coordinate) obj);

        public override int GetHashCode()
        {
            unchecked
            {
                return (X.GetHashCode()*397) ^ Y.GetHashCode();
            }
        }

        public PointF GetPointF() => new PointF(X, Y);

        public Coordinate Mult(double m) => new Coordinate(X*(float) m, Y*(float) m);
        public Coordinate Mult(Coordinate m) => new Coordinate(X*m.X, Y*m.Y);

        public float Pyth() => (float)Math.Sqrt(X * X + Y * Y);

        public Coordinate Sub(Coordinate s) => Add(s.Mult(-1));

        public override string ToString() => $"X: {X}; Y:{Y}";

        public bool Symmetric() => Math.Abs(X - Y) < 0.001;

        public float Min() => X > Y ? Y : X;

        public float Max() => X > Y ? X : Y;

        public float Atan() => (float)Physomatik.ToDegree(Math.Atan(Y/X));

        public Coordinate Dif(Coordinate value) => Sub(value).Abs();

        public Coordinate Abs() => new Coordinate(Math.Abs(X), Math.Abs(Y));

        public bool Is0() => Math.Abs(X) < 0.0001 && Math.Abs(Y) < 0.0001;

        public Coordinate QuadraticForm() => new Coordinate(Max(), Max());
    }
}