using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using PhysomatikLibrary;

namespace Painting.Types.Paint
{
    public class Polygon : Shape
    {
        public int AngleCount { get; set; }
        public float Width { get; set; }
        public Colour LineColour { get; set; }
        public float TurningAngle { get; set; }

        public Polygon(int angleCount, float width, Colour lineColour, Coordinate position, Coordinate size,
            Colour mainColour, float turningAngle) : base(position, size, mainColour)
        {
            AngleCount = angleCount;
            Width = width;
            LineColour = lineColour;
            TurningAngle = turningAngle;
        }

        public override bool Equals(object obj) => obj is Polygon && Equals((Polygon) obj);

        protected bool Equals(Polygon other)
            =>
            other != null && base.Equals(other) && AngleCount == other.AngleCount && Width.Equals(other.Width) &&
            Equals(LineColour, other.LineColour) && Math.Abs(TurningAngle - other.TurningAngle) < 0.001;

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = base.GetHashCode();
                hashCode = (hashCode*397) ^ AngleCount;
                hashCode = (hashCode*397) ^ Width.GetHashCode();
                hashCode = (hashCode*397) ^ (LineColour?.GetHashCode() ?? 0);
                return hashCode;
            }
        }

        public void Paint(Graphics p)
        {
            var points = GetPoints().Select(coordinate => coordinate.GetPointF()).ToArray();
            if (points.Length <= 0)
                return;
            if(MainColour.Visible)
                p.FillPolygon(new SolidBrush(MainColour.Color), points);
            if (points.Length <= 1)
                return;
            if(LineColour.Visible)
                p.DrawPolygon(new Pen(LineColour.Color, Width), points);
        }

        public List<Coordinate> GetPoints()
        {
            var m = Position.add(Size.mult(0.5));
            var fin = new List<Coordinate>();
            if (AngleCount == 0)
                return fin;
            for (var i = TurningAngle; i < 360 + TurningAngle; i += (float)360/AngleCount)
                fin.Add(m.add(new Coordinate((float) Math.Cos(Physomatik.ToRadian(i))*(Size.X/2),
                    (float) Math.Sin(Physomatik.ToRadian(i))*(Size.Y/2))));
            return fin;
        }
    }
}