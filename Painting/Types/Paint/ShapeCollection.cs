using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using Painting.Util;

namespace Painting.Types.Paint
{
    public class ShapeCollection : Shape
    {
        private Coordinate _position;

        private Coordinate _size;
        private float _rotation;

        public ShapeCollection(ObservableCollection<Shape> shapes)
            : base(
                shapes.Select(shape => shape.Position).Min(),
                shapes.Select(shape => shape.Size).Max(),
                shapes.FirstOrDefault(shape => shape.MainColour.Visible)?.MainColour)

        {
            Shapes = new ObservableCollection<Shape>(shapes);
        }

        public float Rotation //alpha'
        {
            get { return _rotation; }
            set
            {
                var mx = Size.X / 2 + Position.X;
                var my = Size.Y / 2 + Position.Y;

                foreach (var shape in Shapes)
                {
                    var mp = shape.Position.Add(shape.Size.Div(2));
                    var r2 = Math.Sqrt(Physomatik.Quadr(mp.Y - my) + Physomatik.Quadr(mp.X - mx));

                    shape.Position = new Coordinate((float) (Math.Cos(Physomatik.ToRadian(value))*r2 - shape.Size.X / 2 + mx),
                        (float) (Math.Sin(Physomatik.ToRadian(value))*r2 - shape.Size.Y / 2 + my));


                    var line = shape as Line;
                    var polygon = shape as Polygon;
                    var ellipse = shape as Ellipse;
                    if (line != null)
                        line.Rotation += value - _rotation;
                    else if (polygon != null)
                        polygon.Rotation += value - _rotation;
                    else if (ellipse != null)
                        ellipse.Rotation += value - _rotation;
                    else
                        throw new NotImplementedException();
                    
                }
                _rotation = value;
            }
        }

        public ObservableCollection<Shape> Shapes { get; set; }

        public override Coordinate Position
        {
            get { return _position; }
            set
            {
                if ((Shapes != null) && (_position != null))
                    foreach (var shape in Shapes)
                        shape.Position = shape.Position.Add(value.Sub(_position));
                _position = value;
            }
        }

        public override Coordinate Size
        {
            get { return _size; }
            set
            {
                if ((Shapes != null) && (_size != null))
                    foreach (var shape in Shapes)
                    {
                        var fac = value.Div(_size);
                        shape.Size = shape.Size.Mult(fac);
                        var dif = shape.Position.Sub(Position);
                        dif = dif.Mult(fac);
                        shape.Position = dif.Add(Position);
                    }
                _size = value;
            }
        }

        public void Paint(Graphics p)
        {
            foreach (var shape in Shapes.Reverse())
            {
                (shape as Ellipse)?.Paint(p, shape.Size.Div(2));
                (shape as DefinedPolygon)?.Paint(p);
                (shape as DefinedShape)?.Paint(p);
                (shape as Line)?.Paint(p);
                (shape as Polygon)?.Paint(p);
                (shape as Rectangle)?.Paint(p);
                (shape as ShapeCollection)?.Paint(p);
            }
        }

        public override bool Equals(object obj) => obj is ShapeCollection && Equals((ShapeCollection)obj);

        protected bool Equals(ShapeCollection other) => other != null && base.Equals(other) && _position.Equals (other._position) && _size.Equals (other._size) && _rotation.Equals(other._rotation) && Shapes.Equals (other.Shapes);

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = base.GetHashCode();
                hashCode = (hashCode*397) ^ (_position?.GetHashCode() ?? 0);
                hashCode = (hashCode*397) ^ (_size?.GetHashCode() ?? 0);
                hashCode = (hashCode*397) ^ _rotation.GetHashCode();
                hashCode = (hashCode*397) ^ (Shapes?.GetHashCode() ?? 0);
                return hashCode;
            }
        }
    }
}