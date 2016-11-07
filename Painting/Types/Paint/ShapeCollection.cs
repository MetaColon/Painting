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
                new Coordinate(shapes.Count > 0 ? shapes.Select(shape => shape.Position.X).Min() : 0, shapes.Count > 0 ? shapes.Select(shape => shape.Position.Y).Min() : 0),
                new Coordinate(shapes.Count > 0 ? shapes.Select(shape => shape.Size.X).Max() : 0, shapes.Count > 0 ? shapes.Select(shape => shape.Size.Y).Max() : 0),
                shapes.FirstOrDefault(shape => shape.MainColour.Visible)?.MainColour)

        {
            Shapes = new ObservableCollection<Shape>(shapes);
        }

        public float Rotation
        {
            get { return _rotation; }
            set
            {
                _rotation = value;
            }
        }

        public ObservableCollection<Shape> Shapes { get; set; }

        public override Coordinate Position
        {
            get { return _position; }
            set
            {
                if (Position != null && Position.Equals(value))
                    return;
                if (Shapes != null)
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
                if (Size != null && Size.Equals (value))
                    return;
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

        public void Paint(Graphics p, Coordinate rotationCenterPointFromPosition)
        {
            foreach (var shape in Shapes.Reverse())
            {
                var trans = p.Transform.Clone();
                if (Rotation != 0)
                {
                    p.TranslateTransform(rotationCenterPointFromPosition.X, rotationCenterPointFromPosition.Y);
                    p.RotateTransform(Rotation);
                    p.TranslateTransform(-rotationCenterPointFromPosition.X, -rotationCenterPointFromPosition.Y);
                }
                (shape as Ellipse)?.Paint(p, shape.CenterPosition);
                (shape as DefinedPolygon)?.Paint(p);
                (shape as DefinedShape)?.Paint(p);
                (shape as Line)?.Paint(p);
                (shape as Polygon)?.Paint(p, shape.CenterPosition);
                (shape as Rectangle)?.Paint(p, shape.CenterPosition);
                (shape as ShapeCollection)?.Paint(p, shape.CenterPosition);
                p.Transform = trans;
            }
        }

        public override bool Equals(object obj) => obj is ShapeCollection && Equals((ShapeCollection)obj);

        protected bool Equals(ShapeCollection other) => other != null && base.Equals(other) && _position != null && _position.Equals (other._position) && _size != null && _size.Equals (other._size) && _rotation.Equals(other._rotation) && Shapes != null && Shapes.SequenceEqual(other.Shapes);

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