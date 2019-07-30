using System;
using System.Collections.Generic;

namespace Solver
{
    public class Field
    {
        private readonly int[][] _values = new int[Constraints.Size][];
        public int[] this[int rowIndex]
        {
            get => rowIndex.IsIndexInRange()
                ? _values[rowIndex]
                : throw new Exception($"Row index should be in range [0; 8], but is {rowIndex}");
            set
            {
                if (rowIndex.IsIndexInRange())
                    _values[rowIndex] = value;
                else
                    throw new Exception($"Row index should be in range [0;8] but is {rowIndex}");
            }
        }

        public Field Clone()
        {
            var clone = new Field();
            for (int row = 0; row < Constraints.Size; row++)
            {
                var aRow = new int[Constraints.Size];
                for (int col = 0; col < Constraints.Size; col++)
                {
                    aRow[col] = _values[row][col];
                }

                clone[row] = aRow;
            }

            return clone;
        }

        public Line[] GetSquares()
        {
            var squares = new Line[Constraints.Size];
            for (int squareIndex = 0; squareIndex < Constraints.Size; squareIndex++)
            {
                var square = new List<int>(9);
                var rowShift = squareIndex / 3 * 3;
                var colShift = squareIndex % 3 * 3;
                for (int row = 0; row < 3; row++)
                {
                    for (int col = 0; col < 3; col++)
                    {
                        square.Add(this[row + rowShift][col + colShift]);
                    }
                }

                squares[squareIndex] = new Line(square.ToArray());
            }

            return squares;
        }

        public Line[] GetColumns()
        {
            var columns = new Line[Constraints.Size];
            for (int columnIndex = 0; columnIndex < Constraints.Size; columnIndex++)
            {
                var column = new List<int>(9);
                for (int rowIndex = 0; rowIndex < Constraints.Size; rowIndex++)
                {
                    column.Add(this[rowIndex][columnIndex]);
                }

                columns[columnIndex] = new Line(column.ToArray());
            }

            return columns;
        }

        public Line[] GetRows()
        {
            var rows = new Line[Constraints.Size];
            for (int i = 0; i < Constraints.Size; i++)
            {
                rows[i] = new Line(this[i]);
            }

            return rows;
        }
    }
}
