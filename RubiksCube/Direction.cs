namespace RubiksCube
{
    public enum Direction
    {
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
    }
}
