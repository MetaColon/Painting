using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Painting.Types.Paint
{
    public class DefinedPolygon : Shape
    {
        public DefinedPolygon(List<Coordinate> edges, int width, Colour lineColour, Colour mainColour)
            : base(edges.Min(), edges.Max().Sub(edges.Min()), mainColour)
        {
            Edges = edges;
            Width = width;
            LineColour = lineColour;
        }

        public List<Coordinate> Edges { get; set; }
        public int Width { get; set; }
        public Colour LineColour { get; set; }

        public override bool Equals(object obj) => Equals(obj as DefinedPolygon);

        protected bool Equals(DefinedPolygon other)
            =>
            (other != null) && base.Equals(other) && Equals(Edges, other.Edges) && (Width == other.Width) &&
            Equals(LineColour, other.LineColour);

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = base.GetHashCode();
                hashCode = (hashCode*397) ^ (Edges?.GetHashCode() ?? 0);
                hashCode = (hashCode*397) ^ Width;
                hashCode = (hashCode*397) ^ (LineColour?.GetHashCode() ?? 0);
                return hashCode;
            }
        }

        public void Paint(Graphics p)
        {
            var points = Edges.Select(coordinate => coordinate.GetPointF()).ToArray();
            if (MainColour.Visible)
                p.FillPolygon(new SolidBrush(MainColour.Color), points);
            if (LineColour.Visible)
                p.DrawPolygon(new Pen(LineColour.Color), points);
        }
    }
}