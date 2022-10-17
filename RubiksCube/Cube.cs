using System;
using System.Collections.Generic;

namespace RubiksCube
{
    public class Cube
    {
        public Cube(int size)
        {
            Size = size;

            foreach (Position position in Enum.GetValues(typeof(Position)))
            {
                if (position != Position.Unknown)
                    Faces[position] = new Face(size, position);
            }
        }

        public void Rotate(Position position, Direction direction, int numRows = 1)
        {
            Console.WriteLine("Rotating cube (Position: " + position.ToString() + ", Direction: " + direction.ToString() + ", Rows: " + numRows.ToString() + ")");

            // for cases front or back, spin so that we can simply use the right hand direction
            Position originalPosition = position;
            switch(position)
            {
                case Position.Front:
                    Spin(Position.Right);
                    position = Position.Right;
                    break;
                case Position.Back:
                    Spin(Position.Left);
                    position = Position.Right;
                    break;
            }

            // first, rotate the face itself
            Faces[position].Rotate(direction);

            // then deal with the edges
            var surroundingPositions = Utils.GetAdjacentPositionsClockwise(position);
            Position firstSurroundingPosition = surroundingPositions[0];
            Position lastSurroundingPosition = surroundingPositions[surroundingPositions.Length - 1];
            switch (direction)
            {
                case Direction.Clockwise:
                {
                    Colour[] lastFaceRow = Faces[lastSurroundingPosition].GetRows(position, numRows);
                    for (int i = surroundingPositions.Length - 2; i >= 0; i--)
                        Faces[surroundingPositions[i + 1]].SetRows(Faces[surroundingPositions[i]], position, numRows);

                    Faces[firstSurroundingPosition].SetRows(lastFaceRow, position, numRows);
                    break;
                }
                case Direction.Anticlockwise:
                {
                    Colour[] firstFaceRow = Faces[firstSurroundingPosition].GetRows(position, numRows);
                    for (int i = 1; i < surroundingPositions.Length; i++)
                        Faces[surroundingPositions[i - 1]].SetRows(Faces[surroundingPositions[i]], position, numRows);

                    Faces[surroundingPositions[surroundingPositions.Length - 1]].SetRows(firstFaceRow, position, numRows);
                    break;
                }
            }

            // for cases front or back, spin back
            switch (originalPosition)
            {
                case Position.Front:
                    Spin(Position.Right);
                    break;
                case Position.Back:
                    Spin(Position.Left);
                    break;
            }
        }

        public void Spin(Position position)
        {
            Console.WriteLine("Spinning cube: " + position.ToString());

            Position adjFace = Position.Unknown;
            switch (position)
            {
                case Position.Right:
                    adjFace = Position.Up;
                    break;
                case Position.Left:
                    adjFace = Position.Down;
                    break;
                case Position.Up:
                    adjFace = Position.Right;
                    break;
                case Position.Down:
                    adjFace = Position.Left;
                    break;
                case Position.Front:
                case Position.Back:
                    adjFace = position;
                    break;
                default:
                    throw new Exception("Cannot spin: " + position.ToString());
            }

            var surroundingFaces = Utils.GetAdjacentPositionsClockwise(adjFace);

            var lastSurroundingFace = Faces[surroundingFaces[surroundingFaces.Length - 1]].Clone();
            for (int i = surroundingFaces.Length - 2; i >= 0; i--)
                Faces[surroundingFaces[i + 1]].SetColours(Faces[surroundingFaces[i]]);

            Faces[surroundingFaces[0]].SetColours(lastSurroundingFace);

            Faces[adjFace].Rotate(Direction.Clockwise);
            Faces[Utils.GetOpposite(adjFace)].Rotate(Utils.GetOpposite(Direction.Clockwise));
        }

        public void Scramble()
        {
            for (int i = 0; i < 100; i++)
            {
                int rows = Utils.RandomNumber(1, Size);
                Position position = Utils.RandomEnum<Position>(Position.Unknown);
                Direction direction = Utils.RandomEnum<Direction>(Direction.Unknown);
                Rotate(position, direction, rows);
            }
        }

        public void Print()
        {
            Console.WriteLine("---------------------");
            foreach (var face in Faces)
            {
                int idx = 0;
                Console.WriteLine(face.Key.ToString());
                foreach(var colour in face.Value.Colours)
                {
                    Console.Write(colour.ToString());
                    Console.Write("\t");
                    if ((idx+1) % Size == 0)
                        Console.WriteLine();
                    idx++;
                }
                Console.WriteLine();
            }
            Console.WriteLine("---------------------");
        }

        public Dictionary<Position, Face> Faces { get; } = new Dictionary<Position, Face>();
        public int Size { get; }
    }
}
