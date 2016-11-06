using System.Drawing;
using System.Linq;
using Painting.Types.Paint;

namespace Designer
{
    public class Util
    {
        public static string GetColorString(Color color) => !color.IsEmpty
            ? $"{color.ToString().Split('[').Last().Split(']').First()})"
            : "Invisible)";

        public static Colour GetColourToColor(Color color)
            => !color.IsEmpty ? new Colour(color) : Colour.Invisible;
    }
}