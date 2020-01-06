using System;

namespace RubiksCube
{
    public class Cube
    {
        public Cube(int size)
        {
            Size = size;

            Faces = new Face[Config.NumberOfFaces];
            for (int i = 0; i < Config.NumberOfFaces; i++)
            {
                Faces[i] = new Face(size, (Position)i);
            }
        }

        public void Rotate(Position position, Direction direction)
        {
            Console.WriteLine("Rotating cube (Position: " + position.ToString() + ", Direction: " + direction.ToString() + ")");

            // for cases front or back, spin so that we can simply use the right hand direction
            Position originalPosition = position;
            switch(position)
            {
                case Position.Front:
                    Spin(Direction.Anticlockwise);
                    position = Position.Right;
                    break;
                case Position.Back:
                    Spin(Direction.Clockwise);
                    position = Position.Right;
                    break;
            }

            // first, rotate the face itself
            Faces[(int)position].Rotate(direction);

            // then deal with the edges
            var surroundingPositions = Utils.GetSurroundingPositions(position);
            Position firstSurroundingPosition = surroundingPositions[0];
            Position lastSurroundingPosition = surroundingPositions[surroundingPositions.Length - 1];
            switch (direction)
            {
                case Direction.Clockwise:
                {
                    Colour[] lastFaceRow = Faces[(int)lastSurroundingPosition].GetEdgeBlock(position);
                    for (int i = surroundingPositions.Length - 2; i >= 0; i--)
                    {
                        var surroundingPosition = surroundingPositions[i];
                        Faces[(int)surroundingPositions[i + 1]].SetEdgeBlock(Faces[(int)surroundingPosition], position);
                    }
                    Faces[(int)firstSurroundingPosition].SetEdgeBlock(lastFaceRow, position);
                    break;
                }
                case Direction.Anticlockwise:
                {
                    Colour[] firstFaceRow = Faces[(int)surroundingPositions[0]].GetEdgeBlock(position);
                    for (int i = 1; i < surroundingPositions.Length; i++)
                    {
                        Faces[(int)surroundingPositions[i - 1]].SetEdgeBlock(Faces[(int)surroundingPositions[i]], position);
                    }
                    Faces[(int)surroundingPositions[surroundingPositions.Length - 1]].SetEdgeBlock(firstFaceRow, position);
                    break;
                }
            }

            // for cases front or back, spin back
            switch (originalPosition)
            {
                case Position.Front:
                    Spin(Direction.Clockwise);
                    break;
                case Position.Back:
                    Spin(Direction.Anticlockwise);
                    break;
            }
        }

        public void Spin(Direction direction)
        {
            Console.WriteLine("Spinning cube: " + direction.ToString() + ") around the Y axis");

            var surroundingFaces = Utils.GetSurroundingPositions(Position.Up);            
            switch (direction)
            {
                case Direction.Clockwise:
                    {
                        var lastSurroundingFace = Faces[(int)surroundingFaces[surroundingFaces.Length - 1]].Clone();
                        for(int i = surroundingFaces.Length - 2; i >= 0; i--)
                        {
                            Faces[(int)surroundingFaces[i + 1]].SetColours(Faces[(int)surroundingFaces[i]]);
                        }
                        Faces[(int)surroundingFaces[0]].SetColours(lastSurroundingFace);
                        break;
                    }
                case Direction.Anticlockwise:
                    {
                        var firstSurroundingFace = Faces[(int)surroundingFaces[0]].Clone();
                        for (int i = 1; i < surroundingFaces.Length; i++)
                        {
                            Faces[(int)surroundingFaces[i - 1]].SetColours(Faces[(int)surroundingFaces[i]]);
                        }
                        Faces[(int)surroundingFaces[surroundingFaces.Length - 1]].SetColours(firstSurroundingFace);
                        break;
                    }
            }

            Faces[(int)Position.Up].Rotate(direction);
            Faces[(int)Position.Down].Rotate(Utils.GetOpposite(direction));
        }

        public void Print()
        {
            Console.WriteLine("---------------------");
            foreach (var face in Faces)
            {
                int idx = 0;
                Console.WriteLine(face.Position.ToString());
                foreach(var colour in face.Colours)
                {
                    Console.Write(colour.ToString());
                    Console.Write("\t");
                    if ((idx+1) % Size == 0)
                        Console.Write("\n");
                    idx++;
                }
                Console.WriteLine();
            }
            Console.WriteLine("---------------------");
        }

        public Face[] Faces { get; }
        public int Size { get; }
    }
}
