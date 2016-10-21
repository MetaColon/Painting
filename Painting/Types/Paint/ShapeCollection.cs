using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Painting.Types.Paint
{
    public class ShapeCollection : Shape
    {
        public List<Shape> Shapes { get; set; }

        private Coordinate _position;
        public override Coordinate Position
        {
            get
            {
                return _position;
            }
            set
            {
                if (Shapes != null)
                    foreach (var shape in Shapes)
                        shape.Position = shape.Position.add (value.sub (_position));
                _position = value;
            }
        }

        public ShapeCollection (List<Shape> shapes)
            : base (
                shapes.Select (shape => shape.Position).Min (),
                shapes.Select (shape => shape.Size).Max ().sub (shapes.Select (shape => shape.Position).Min ()), shapes.FirstOrDefault (shape => shape.MainColour.Visible)?.MainColour)

        {
            Shapes = new List<Shape> (shapes);
        }

        public void Paint (Graphics p)
        {
            foreach (var shape in Shapes)
            {
                (shape as Ellipse)?.Paint (p);
                (shape as DefinedPolygon)?.Paint (p);
                (shape as DefinedShape)?.Paint (p);
                (shape as Line)?.Paint (p);
                (shape as Polygon)?.Paint (p);
                (shape as Rectangle)?.Paint (p);
            }
        }
    }
}