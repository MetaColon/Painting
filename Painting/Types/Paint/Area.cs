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
            switch (Start.CompareTo(End))
            {
                case -1:
                    Start = a;
                    End = b;
                    break;
                case 1:
                    Start = b;
                    End = a;
                    break;
                default:
                    throw new Exception("The given Coordinates are not valid for an area!");
            }
        }

        public bool IsCoordinateInArea(Coordinate coord) => coord.CompareTo(Start) == 1 && coord.CompareTo(End) == -1;
    }
}