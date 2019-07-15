using System.Collections.Generic;
using System.Linq;

namespace Solver
{
    public static class DuplicateChecker
    {
        public static int RowWithDuplicatesIndex(this Field field)
        {
            for (int rowIndex = 0; rowIndex < Constraints.Size; rowIndex++)
            {
                var rowValues = field[rowIndex].Where(n => n.IsAssigned()).ToArray();
                if (rowValues.Length != rowValues.Distinct().Count())
                    return rowIndex;
            }

            return -1;
        }

        public static int ColumnWithDuplicatesIndex(this Field field)
        {
            for (int columnIndex = 0; columnIndex < Constraints.Size; columnIndex++)
            {
                var columnValues = new List<int>(9);
                for (int rowIndex = 0; rowIndex < Constraints.Size; rowIndex++)
                {
                    if (field[rowIndex][columnIndex].IsAssigned())
                        columnValues.Add(field[rowIndex][columnIndex]);
                }

                if (columnValues.Count != columnValues.Distinct().Count())
                    return columnIndex;
            }

            return -1;
        }

        public static int SquareWithDuplicatesIndex(this Field field)
        {
            for (int squareIndex = 0; squareIndex < Constraints.Size; squareIndex++)
            {
                var square = new List<int>(9);
                var rowShift = squareIndex / 3 * 3;
                var colShift = squareIndex % 3 * 3;

                for (int row = 0; row < 3; row++)
                {
                    for (int col = 0; col < 3; col++)
                    {
                        square.Add(field[row + rowShift][col + colShift]);
                    }
                }

                square = square.Where(n => n.IsAssigned()).ToList();
                if (square.Count != square.Distinct().Count())
                    return squareIndex;
            }

            return -1;
        }
    }
}