using System.Drawing;

namespace Painting.Types.Paint
{
    public class Colour
    {
        public Color Color { get; set; }
        public bool Visible { get; set; }

        public Colour()
        {
            
        }

        public Colour(Color color, bool visible)
        {
            Color = color;
            Visible = visible;
        }

        public Colour(Color color)
        {
            Color = color;
            Visible = !color.IsEmpty;
        }

        public override bool Equals(object obj) => obj is Colour && Equals((Colour) obj);

        protected bool Equals(Colour other) => other != null && Color.Equals(other.Color) && Visible == other.Visible;

        public override int GetHashCode()
        {
            unchecked
            {
                return (Color.GetHashCode()*397) ^ Visible.GetHashCode();
            }
        }

        public static Colour Invisible () => new Colour(new Color(), false);
    }
}