using System;
using Painting.Util;

namespace Painting.Types.Paint
{
    public class Shape
    {
        public Shape()
        {
        }

        public Shape(Coordinate position, Coordinate unturnedSize, Colour mainColour, float rotation)
        {
            UnturnedSize = unturnedSize;
            Position = position;
            MainColour = mainColour;
            Rotation = rotation;
        }

        public virtual Colour MainColour { get; set; }
        public virtual Coordinate Position { get;
            set; }

        public virtual Coordinate UnturnedSize { get; set; } //If avoidable, don't change the unturnedSize :)

        protected bool Equals(Shape other)
            =>
            (other != null) && UnturnedSize != null && UnturnedSize.Equals(other.UnturnedSize) && Position != null && Position.Equals (other.Position) && MainColour != null &&
            MainColour.Equals(other.MainColour);

        public override bool Equals(object obj) => obj is Shape && Equals((Shape) obj);

        public override int GetHashCode()
        {
            unchecked
            {
                return ((((UnturnedSize?.GetHashCode() ?? 0)*397) ^ (Position?.GetHashCode() ?? 0))*397) ^
                       MainColour.GetHashCode();
            }
        }

        public override string ToString() => $"Position: {Position}; UnturnedSize:{UnturnedSize}";

        public bool IsCoordinateInThis(Coordinate coordinate)
            =>
            (coordinate.X >= Position.X) && (coordinate.X <= Position.X + UnturnedSize.X) && (coordinate.Y >= Position.Y) &&
            (coordinate.Y <= Position.Y + UnturnedSize.Y);

        public Coordinate CenterPosition => Position.Add(UnturnedSize.Div(2));
        public virtual float Rotation { get; set; }
    }
}