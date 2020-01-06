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

        public Colour[] GetEdgeBlock(Position position)
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
                    ret = Colours.Take(Size).ToArray();
                    break;
                case Position.Down:
                    ret = Colours.Skip(Size * (Size - 1)).Take(Size).ToArray();
                    break;
                case Position.Left:
                    ret = Colours.Where((x, i) => i % Size == 0).ToArray();
                    break;
                case Position.Right:
                    ret = Colours.Where((x, i) => (i + 1) % Size == 0).ToArray();
                    break;
                default:
                    throw new System.Exception("Cannot get face edge block from position: " + position.ToString());
            }

            // back face orientation correction
            if (Position == Position.Back && (position == Position.Left || position == Position.Right))
                return ret.Reverse().ToArray();

            return ret;
        }

        public void SetEdgeBlock(Colour[] colours, Position position)
        {
            // back face orientation correction
            if (Position == Position.Back)
            {
                if (position == Position.Right)
                    position = Position.Left;
                else if (position == Position.Left)
                    position = Position.Right;

                if (position == Position.Right || position == Position.Left)
                    colours = colours.Reverse().ToArray();
            }

            int[] idxs = new int[Size];
            switch (position)
            {
                case Position.Up:
                    idxs = Enumerable.Range(0, Size).ToArray();
                    break;
                case Position.Down:
                    idxs = Enumerable.Range(Size * (Size-1), Size).ToArray();
                    break;
                case Position.Left:
                    idxs = Enumerable.Range(0, Size * Size).Where(x => x % Size == 0).ToArray();
                    break;
                case Position.Right:
                    idxs = Enumerable.Range(0, Size * Size).Where(x => (x + 1) % Size == 0).ToArray();
                    break;
                default:
                    throw new System.Exception("Cannot get face edge block from position: " + position.ToString());
            }

            for(int i = 0; i < colours.Length; i++)
            {
                Colours[idxs[i]] = colours[i];
            }
        }

        public void SetEdgeBlock(Face face, Position position)
        {
            SetEdgeBlock(face.GetEdgeBlock(position), position);
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
