using System;

namespace Painting.Types.Paint
{
    public class Area
    {
        public Coordinate Start { get; }
        public Coordinate End { get; }
        /// <summary>
        /// Automatically chooses the correct Coordinates if possible
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public Area(Coordinate a, Coordinate b)
        {
            Start = new Coordinate(Math.Min(a.X,b.X), Math.Min(a.Y, b.Y));
            End = new Coordinate(Math.Max(a.X, b.X), Math.Max(a.Y, b.Y));
        }

        public bool IsCoordinateInArea(Coordinate coord) => coord.CompareTo(Start) == 1 && coord.CompareTo(End) == -1;
    }
}