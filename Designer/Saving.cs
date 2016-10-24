using System.Collections.Generic;
using System.Drawing;
using Painting.Types.Paint;

namespace Designer
{
    public class Saving
    {
        public static string GetSaveCode(ShapeCollection collection)
        {
            var fin = "var foo = new ShapeCollection(new List<Shape> {";
            foreach (var shape in collection.Shapes)
            {
                var ellipse = shape as Ellipse;
                if (ellipse != null)
                    fin +=
                        $"new Ellipse({ellipse.Width}, {GetColourString(ellipse.LineColour)}, {GetCoordinate(ellipse.Position)}, {GetCoordinate(ellipse.Size)}, {GetColourString(ellipse.MainColour)}),";
                var line = shape as Line;
                if (line != null)
                    fin +=
                        $"new Line({GetCoordinate(shape.Position)}, {GetCoordinate(line.End)}, {GetColourString(shape.MainColour)}, {line.Width}),";
                var polygon = shape as Polygon;
                if (polygon != null)
                    fin +=
                        $"new Polygon({polygon.AngleCount}, {polygon.Width}, {GetColourString(polygon.LineColour)}, {GetCoordinate(shape.Position)}, {GetCoordinate(shape.Size)}, {GetColourString(shape.MainColour)}, {polygon.Rotation}),";
                var rectangle = shape as Painting.Types.Paint.Rectangle;
                if (rectangle != null)
                    fin +=
                        $"new Rectangle({rectangle.Width}, {GetColourString(rectangle.LineColour)}, {GetCoordinate(shape.Position)}, {GetCoordinate(shape.Size)}, {GetColourString(shape.MainColour)}),";
            }
            fin += "}){Position = new Coordinate(0, 0)};";
            return fin;
        }

        public static string GetColourString(Colour c)
            => $"new Colour ({(c.Visible ? $"Color.FromArgb({c.Color.ToArgb()})" : "Color.Empty")})";

        public static string GetCoordinate(Coordinate c) => $"new Coordinate({c.X},{c.Y})";
    }
}