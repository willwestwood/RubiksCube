using System.Linq;

namespace RubiksCube
{
    public class Face
    {
        public Face(int size, Position position)
        {
            Position = position;
            Size = size;

            Colour colour;
            switch (position)
            {
                case Position.Down:
                    colour = Colour.White;
                    break;
                case Position.Up:
                    colour = Colour.Yellow;
                    break;
                case Position.Right:
                    colour = Colour.Green;
                    break;
                case Position.Left:
                    colour = Colour.Blue;
                    break;
                case Position.Front:
                    colour = Colour.Red;
                    break;
                case Position.Back:
                    colour = Colour.Orange;
                    break;
                default:
                    colour = Colour.Unknown;
                    break;
            }
            Colours = Enumerable.Repeat(colour, size * size).ToArray();
        }

        public Face Clone()
        {
            Face clone = new Face(Size, Position);
            clone.Colours = (Colour[])Colours.Clone();
            return clone;
        }

        public void Rotate(Direction direction)
        {
            var matrix = Utils.ArrayToMatrix2D(Colours, Size);
            Utils.RotateMatrix(ref matrix, direction);
            Colours = Utils.Matrix2DToArray(matrix);
        }

        public Colour[] GetRows(Position position, int n = 1)
        {
            // back face orientation correction
            if (Position == Position.Back)
            {
                if (position == Position.Right)
                    position = Position.Left;
                else if (position == Position.Left)
                    position = Position.Right;
            }

            Colour[] ret;
            switch (position)
            {
                case Position.Up:
                    ret = Colours.Take(Size * n).ToArray();
                    break;
                case Position.Down:
                    ret = Colours.Skip(Size * (Size - n)).Take(Size * n).ToArray();
                    break;
                case Position.Left:
                    ret = new Colour[] {};
                    for(int i = 0; i < n; i++)
                        ret = ret.Concat(Colours.Where((x, j) => (j - i) % Size == 0).ToArray()).ToArray();
                    break;
                case Position.Right:
                    ret = new Colour[] { };
                    for (int i = 0; i < n; i++)
                        ret = ret.Concat(Colours.Where((x, j) => (j + i + 1) % Size == 0).ToArray()).ToArray();
                    break;
                default:
                    throw new System.Exception("Cannot get face edge block from position: " + position.ToString());
            }

            // back face orientation correction
            if (Position == Position.Back && (position == Position.Left || position == Position.Right))
                Utils.ReverseArraySubSections(ref ret, Size);

            return ret;
        }

        public void SetRows(Colour[] colours, Position position, int n = 1)
        {
            if (colours.Length != Size * n)
                throw new System.Exception("Invalid number of colours");

            // back face orientation correction
            if (Position == Position.Back)
            {
                if (position == Position.Right)
                    position = Position.Left;
                else if (position == Position.Left)
                    position = Position.Right;

                if (position == Position.Right || position == Position.Left)
                    Utils.ReverseArraySubSections(ref colours, Size);
            }

            int[] idxs = new int[0];
            switch (position)
            {
                case Position.Up:
                    idxs = Enumerable.Range(0, Size * n).ToArray();
                    break;
                case Position.Down:
                    idxs = Enumerable.Range(Size * (Size-n), Size * n).ToArray();
                    break;
                case Position.Left:
                    for (int i = 0; i < n; i++)
                        idxs = idxs.Concat(Enumerable.Range(0, Size * Size).Where(x => (x - i) % Size == 0).ToArray()).ToArray();
                    break;
                case Position.Right:
                    for (int i = 0; i < n; i++)
                        idxs = idxs.Concat(Enumerable.Range(0, Size * Size).Where(x => (x + i + 1) % Size == 0).ToArray()).ToArray();
                    break;
                default:
                    throw new System.Exception("Cannot get face edge block from position: " + position.ToString());
            }

            for(int i = 0; i < colours.Length; i++)
                Colours[idxs[i]] = colours[i];
        }

        public void SetRows(Face face, Position position, int n = 1)
        {
            SetRows(face.GetRows(position, n), position, n);
        }

        public void SetColours(Face face)
        {
            Colours = face.Colours;
        }

        public int Size { get; }
        public Position Position { get; }
        public Colour[] Colours { get; private set; }
    }
}
