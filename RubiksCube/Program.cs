namespace RubiksCube
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Cube cube = new Cube(3);
            cube.Print();

            cube.Rotate(Position.Front, Direction.Clockwise);
            cube.Rotate(Position.Up, Direction.Clockwise);
            cube.Rotate(Position.Right, Direction.Anticlockwise);
            cube.Rotate(Position.Left, Direction.Clockwise);

            cube.Rotate(Position.Back, Direction.Clockwise);
            cube.Rotate(Position.Left, Direction.Clockwise);
            cube.Rotate(Position.Up, Direction.Anticlockwise);
            cube.Rotate(Position.Right, Direction.Clockwise);

            cube.Rotate(Position.Front, Direction.Clockwise);
            cube.Rotate(Position.Up, Direction.Clockwise);
            cube.Rotate(Position.Right, Direction.Anticlockwise);
            cube.Rotate(Position.Left, Direction.Clockwise);

            cube.Print();

            for (int i = 0; i < 0; i++)
            {
                cube.Rotate(Position.Right, Direction.Clockwise);
                cube.Rotate(Position.Up, Direction.Clockwise);
                cube.Rotate(Position.Right, Direction.Anticlockwise);
                cube.Rotate(Position.Up, Direction.Anticlockwise);
            }

            cube.Print();
        }
    }
}
