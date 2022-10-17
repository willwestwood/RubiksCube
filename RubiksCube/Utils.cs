using System.Linq;

namespace RubiksCube
{
    public static partial class Utils
    {
        public static void RotateMatrix<T>(ref T[,] matrix, Direction direction)
        {
            T[,] newMatrix = new T[matrix.GetLength(1), matrix.GetLength(0)];

            int newColumn, newRow = 0;
            for (int oldColumn = direction == Direction.Clockwise ? 0 : matrix.GetLength(1) - 1;
                direction == Direction.Clockwise ? oldColumn < matrix.GetLength(1) : oldColumn >= 0;)
            {
                newColumn = 0;
                for (int oldRow = direction == Direction.Clockwise ? matrix.GetLength(0) -1 : 0;
                    direction == Direction.Clockwise ? oldRow >= 0 : oldRow < matrix.GetLength(0);)
                {
                    newMatrix[newRow, newColumn] = matrix[oldRow, oldColumn];
                    newColumn++;

                    // increase or decrease row index
                    switch (direction)
                    {
                        case Direction.Clockwise: oldRow--; break;
                        case Direction.Anticlockwise: oldRow++; break;
                    }
                }
                newRow++;

                // increase or decrease column index
                switch (direction)
                {
                    case Direction.Clockwise: oldColumn++; break;
                    case Direction.Anticlockwise: oldColumn--; break;
                }
            }
            matrix = newMatrix;
        }

        public static T[,] ArrayToMatrix2D<T>(T[] arr, int size)
        {
            return ArrayToMatrix2D(arr, size, size);
        }

        public static T[,] ArrayToMatrix2D<T>(T[] arr, int rowSize, int colSize)
        {
            var ret = new T[rowSize, colSize];

            int rowIdx = 0, colIdx = 0;
            foreach (var a in arr)
            {
                if (colIdx >= colSize)
                {
                    rowIdx++;
                    colIdx = 0;
                }

                ret[rowIdx, colIdx] = a;

                colIdx++;
            }

            return ret;
        }

        public static T[] Matrix2DToArray<T>(T[,] matrix)
        {
            var ret = new T[matrix.GetLength(0) * matrix.GetLength(1)];

            int idx = 0;
            foreach(var m in matrix)
            {
                ret[idx] = m;
                idx++;
            }

            return ret;
        }

        public static void ReverseArraySubSections<T>(ref T[] arr, int sectionSize)
        {
            T[] newArr = new T[0];
            for(int i = 0; i < sectionSize; i++)
            {
                var subSection = arr.Skip(i * sectionSize).Take(sectionSize).ToArray();
                subSection = subSection.Reverse().ToArray();
                newArr = newArr.Concat(subSection).ToArray();
            }
            arr = newArr;
        }

        public static void ReverseArrayInSections<T>(ref T[] arr, int sectionSize)
        {
            T[] newArr = new T[0];
            for (int i = 0; i < sectionSize; i++)
            {
                var subSection = arr.Skip(i * sectionSize).Take(sectionSize).ToArray();
                newArr = subSection.Concat(newArr).ToArray();
            }
            arr = newArr;
        }
    }
}
