using System;

namespace RubiksCube
{
    class MainClass
    {
        public static void RightyAlg(Cube cube)
        {
            cube.Rotate(Position.Right, Direction.Clockwise);
            cube.Rotate(Position.Up, Direction.Clockwise);
            cube.Rotate(Position.Right, Direction.Anticlockwise);
            cube.Rotate(Position.Up, Direction.Anticlockwise);
        }

        public static void LeftyAlg(Cube cube)
        {
            cube.Rotate(Position.Left, Direction.Anticlockwise);
            cube.Rotate(Position.Up, Direction.Anticlockwise);
            cube.Rotate(Position.Left, Direction.Clockwise);
            cube.Rotate(Position.Up, Direction.Clockwise);
        }

        public static void InverseRightyAlg(Cube cube)
        {
            cube.Rotate(Position.Up, Direction.Clockwise);
            cube.Rotate(Position.Right, Direction.Clockwise);
            cube.Rotate(Position.Up, Direction.Anticlockwise);
            cube.Rotate(Position.Right, Direction.Anticlockwise);
        }

        public static void InverseLeftyAlg(Cube cube)
        {
            cube.Rotate(Position.Up, Direction.Anticlockwise);
            cube.Rotate(Position.Left, Direction.Anticlockwise);
            cube.Rotate(Position.Up, Direction.Clockwise);
            cube.Rotate(Position.Left, Direction.Clockwise);
        }

        public static void Main(string[] args)
        {
            Cube cube = new Cube(3);
            cube.Print();

            RightyAlg(cube);
            LeftyAlg(cube);

            for (int i = 0; i < 5; i++)
                RightyAlg(cube);

            for(int i = 0; i < 5; i++)
                LeftyAlg(cube);

            cube.Print();


            RightyAlg(cube);
            LeftyAlg(cube);
            InverseRightyAlg(cube);
            InverseLeftyAlg(cube);

            cube.Print();

            RightyAlg(cube);
            LeftyAlg(cube);
            InverseRightyAlg(cube);
            InverseLeftyAlg(cube);

            cube.Print();

            //cube.Rotate(Position.Front, Direction.Clockwise);
            //cube.Rotate(Position.Up, Direction.Clockwise);
            //cube.Rotate(Position.Right, Direction.Anticlockwise);
            //cube.Rotate(Position.Left, Direction.Clockwise);

            //cube.Rotate(Position.Back, Direction.Clockwise);
            //cube.Rotate(Position.Left, Direction.Clockwise);
            //cube.Rotate(Position.Up, Direction.Anticlockwise);
            //cube.Rotate(Position.Right, Direction.Clockwise);

            //cube.Rotate(Position.Front, Direction.Clockwise);
            //cube.Rotate(Position.Up, Direction.Clockwise);
            //cube.Rotate(Position.Right, Direction.Anticlockwise);
            //cube.Rotate(Position.Left, Direction.Clockwise);

            //for (int i = 0; i < 6; i++)
            //{
            //    cube.Rotate(Position.Right, Direction.Clockwise);
            //    cube.Rotate(Position.Up, Direction.Clockwise);
            //    cube.Rotate(Position.Right, Direction.Anticlockwise);
            //    cube.Rotate(Position.Up, Direction.Anticlockwise);

            //    cube.Print();
            //}
        }
    }
}
