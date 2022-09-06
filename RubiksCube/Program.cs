using System;

namespace RubiksCube
{
    class MainClass
    {
        public static void RightyAlg(Cube cube, int rows = 1)
        {
            cube.Rotate(Position.Right, Direction.Clockwise, rows);
            cube.Rotate(Position.Up, Direction.Clockwise, rows);
            cube.Rotate(Position.Right, Direction.Anticlockwise, rows);
            cube.Rotate(Position.Up, Direction.Anticlockwise, rows);
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

        public static void Interact(Cube cube)
        {
            while (true)
            {
                var input = Console.ReadLine();
                if (input.ToUpperInvariant() == "EXIT")
                    break;

                var parts = input.Split(' ');
                if (parts.Length < 2)
                {
                    Console.WriteLine("Specify at least position and direction");
                    continue;
                }

                Direction dir = Direction.Unknown;
                Position pos = Position.Unknown;
                int rows = 1;
                foreach (var part in parts)
                {
                    if (dir == Direction.Unknown)
                    {
                        Direction dirCandidate = Utils.ToDirection(part);
                        if (dirCandidate != Direction.Unknown)
                            dir = dirCandidate;
                    }
                    if (pos == Position.Unknown)
                    {
                        Position posCandidate = Utils.ToPosition(part);
                        if (posCandidate != Position.Unknown)
                            pos = posCandidate;
                    }

                    if (int.TryParse(part, out rows))
                    {
                        if (rows < 1)
                            rows = 1;
                    }
                    else
                        rows = 1;
                }

                if (dir == Direction.Unknown)
                {
                    Console.WriteLine("Direction not specified");
                    continue;
                }

                if (pos == Position.Unknown)
                {
                    Console.WriteLine("Position not specified");
                    continue;
                }

                cube.Rotate(pos, dir, rows);
                cube.Print();
                Console.WriteLine();
            }
        }

        public static void Main(string[] args)
        {
            WebSocketService webSocketServer = new WebSocketService();
            webSocketServer.Start();

            Cube cube = new Cube(3);
            cube.Print();

            //cube.Scramble();

            Interact(cube);

            //cube.Spin(Position.Front);
            //cube.Print();
            //cube.Spin(Position.Back);
            cube.Print();

            Instruction instruction1 = new Instruction(Instruction.InstructionType.Spin, Position.Right, Direction.Clockwise, 1, 1);
            Instruction instruction2 = new Instruction(Instruction.InstructionType.Rotate, Position.Right, Direction.Clockwise, 1, 1);

            instruction1.Print();
            instruction2.Print();

            //for (int i = 0; i < 6; i++)
            //    RightyAlg(cube, 3);

            //cube.Print();

            //cube.Rotate(Position.Right, Direction.Clockwise);
            //cube.Print();

            //cube.Rotate(Position.Up, Direction.Anticlockwise, 2);
            //cube.Print();


            //RightyAlg(cube);
            //LeftyAlg(cube);

            //for (int i = 0; i < 5; i++)
            //    RightyAlg(cube);

            //for(int i = 0; i < 5; i++)
            //    LeftyAlg(cube);

            //cube.Print();


            //RightyAlg(cube);
            //LeftyAlg(cube);
            //InverseRightyAlg(cube);
            //InverseLeftyAlg(cube);

            //cube.Print();

            //RightyAlg(cube);
            //LeftyAlg(cube);
            //InverseRightyAlg(cube);
            //InverseLeftyAlg(cube);

            //cube.Print();

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
