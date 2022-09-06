using System;

namespace RubiksCube
{
    public enum Position
    {
        Unknown,
        Up,
        Down,
        Right,
        Left,
        Front,
        Back
    }

    public static partial class Utils
    {
        public static Position[] GetAdjacentPositionsClockwise(Position position)
        {
            switch (position)
            {
                case Position.Up: return new Position[] { Position.Front, Position.Left, Position.Back, Position.Right };
                case Position.Down: return new Position[] { Position.Front, Position.Right, Position.Back, Position.Left };
                case Position.Right: return new Position[] { Position.Front, Position.Up, Position.Back, Position.Down };
                case Position.Left: return new Position[] { Position.Front, Position.Down, Position.Back, Position.Up };
                case Position.Front: return new Position[] { Position.Left, Position.Up, Position.Right, Position.Down };
                case Position.Back: return new Position[] { Position.Left, Position.Down, Position.Right, Position.Up };
                default: throw new System.Exception("Cannot get surrounding positions from " + position.ToString());
            }
        }

        public static Position GetOpposite(Position position)
        {
            switch(position)
            {
                case Position.Up: return Position.Down;
                case Position.Down: return Position.Up;
                case Position.Right: return Position.Left;
                case Position.Left: return Position.Right;
                case Position.Front: return Position.Back;
                case Position.Back: return Position.Front;
                default: return position;
            }
        }

        public static Position ToPosition(string input)
        {
            foreach (Position position in Enum.GetValues(typeof(Position)))
                if (position.ToString().ToUpperInvariant() == input.ToUpperInvariant())
                    return position;
            return Position.Unknown;
        }
    }
}
