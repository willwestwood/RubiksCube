using System;

namespace RubiksCube
{
    using Coordinate = Tuple<int, int>;

    public class Solver
    {
        public Solver(ref Cube cube)
        {
            Cube = cube;
        }

        private void MoveFaceToFront(Face face)
        {
            switch (face.Position)
            {
                case Position.Left:
                    Cube.Spin(Position.Right);
                    break;
                case Position.Right:
                    Cube.Spin(Position.Left);
                    break;
                case Position.Up:
                    Cube.Spin(Position.Down);
                    break;
                case Position.Down:
                    Cube.Spin(Position.Up);
                    break;
                case Position.Back:
                    Cube.Spin(Position.Up);
                    Cube.Spin(Position.Up);
                    break;
            }
        }

        private bool TryGetEdgePiece(Colour colour, Face face, ref Coordinate coordinate)
        {
            for (int row = 0; row < face.Size; row++)
            {
                for (int col = 0; col < face.Size; col++)
                {
                    if (face.Colours[col + (row * face.Size)] == colour)
                    {
                        Coordinate candidate = new Coordinate(col, row);
                        if (face.IsEdge(candidate))
                        {
                            coordinate = candidate;
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private void GetYellowCross()
        {
            Face yellowFace = null;
            Coordinate centreCoordinate = new Coordinate(Cube.Size / 2, Cube.Size / 2);
            foreach(var face in Cube.Faces)
                if (face.Value.GetSquare(centreCoordinate) == Colour.Yellow)
                    yellowFace = face.Value;

            if (yellowFace == null)
                throw new Exception("No yellow face found");

            MoveFaceToFront(yellowFace);

            foreach (var face in Cube.Faces)
            {
                if (face.Value.GetSquare(centreCoordinate) != Colour.Yellow)
                {
                    Coordinate edge = new Coordinate(-1, -1);
                    if (TryGetEdgePiece(Colour.Yellow, face.Value, ref edge))
                    {

                    }
                }
            }
        }

        public void Solve()
        {

        }

        Cube Cube { get; }
    }
}
