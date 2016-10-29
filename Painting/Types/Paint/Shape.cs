namespace Painting.Types.Paint
{
    public class Shape
    {
        public Shape()
        {
        }

        public Shape(Coordinate position, Coordinate size, Colour mainColour)
        {
            Size = size;
            Position = position;
            MainColour = mainColour;
        }

        public virtual Colour MainColour { get; set; }
        public virtual Coordinate Position { get;
            set; }

        public virtual Coordinate Size { get; set; } //If avoidable, don't change the size :)

        protected bool Equals(Shape other)
            =>
            (other != null) && Size != null && Size.Equals(other.Size) && Position != null && Position.Equals (other.Position) && MainColour != null &&
            MainColour.Equals(other.MainColour);

        public override bool Equals(object obj) => obj is Shape && Equals((Shape) obj);

        public override int GetHashCode()
        {
            unchecked
            {
                return ((((Size?.GetHashCode() ?? 0)*397) ^ (Position?.GetHashCode() ?? 0))*397) ^
                       MainColour.GetHashCode();
            }
        }

        public override string ToString() => $"Position: {Position}; Size:{Size}";

        public bool IsCoordinateInThis(Coordinate coordinate)
            =>
            (coordinate.X >= Position.X) && (coordinate.X <= Position.X + Size.X) && (coordinate.Y >= Position.Y) &&
            (coordinate.Y <= Position.Y + Size.Y);
    }
}