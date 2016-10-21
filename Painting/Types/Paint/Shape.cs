using System.Drawing;

namespace Painting.Types.Paint
{
    public class Shape
    {
        public Shape (Coordinate position, Coordinate size, Colour mainColour)
        {
            Size = size;
            Position = position;
            MainColour = mainColour;
        }

        protected bool Equals (Shape other) => other != null && Equals (Size, other.Size) && Equals (Position, other.Position) && Equals (MainColour, other.MainColour);

        public override bool Equals (object obj) => obj is Shape && Equals ((Shape) obj);

        public override int GetHashCode ()
        {
            unchecked
            {
                return ((((Size?.GetHashCode () ?? 0) * 397) ^ (Position?.GetHashCode () ?? 0)) * 397) ^ MainColour.GetHashCode ();
            }
        }

        public override string ToString () => $"Position: {Position}; Size:{Size}";

        public virtual Colour MainColour { get; set; }
        public virtual Coordinate Position { get; set; }
        public virtual Coordinate Size { get; set; }
    }
}