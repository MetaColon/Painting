using System.Drawing;

namespace Painting.Types.Paint
{
    public class DefinedShape : Shape
    {
        public DefinedShape(Pixel[,] pixels, Coordinate size, Coordinate position, Colour mainColour, float rotation = 0)
            : base(position, size, mainColour, rotation)
        {
            Pixels = pixels;
        }

        public Pixel[,] Pixels { get; set; }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        protected bool Equals(DefinedShape other)
            => (other != null) && base.Equals(other) && Equals(Pixels, other.Pixels);

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode()*397) ^ (Pixels?.GetHashCode() ?? 0);
            }
        }

        public override string ToString() => "A lot of Pixels.";

        public void Paint(Graphics p)
        {
            for (var y = 0; y < Pixels.GetLength(0); y++)
                for (var x = 0; x < Pixels.GetLength(1); x++)
                    Pixels[x, y].Paint(p);
        }
    }
}