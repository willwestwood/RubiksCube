using System;

namespace RubiksCube
{
    public enum Direction
    {
        Unknown,
        Clockwise,
        Anticlockwise
    }

    public static partial class Utils
    {
        public static Direction GetOpposite(Direction direction)
        {
            switch(direction)
            {
                case Direction.Clockwise: return Direction.Anticlockwise;
                case Direction.Anticlockwise: return Direction.Clockwise;
                default: throw new System.Exception("Invalid direction");
            }
        }

        public static Direction ToDirection(string input)
        {
            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
                if (direction.ToString().ToUpperInvariant() == input.ToUpperInvariant())
                    return direction;
            return Direction.Unknown;
        }
    }
}
