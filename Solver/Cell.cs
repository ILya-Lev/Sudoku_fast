using System.Collections.Generic;

namespace Solver
{
    internal class Cell
    {
        public Cell(int row, int column, int value)
        {
            Row = row;
            Column = column;
            Value = value;
        }

        public Cell(int row, int column, HashSet<int> possibleValues)
            : this(row, column, 0)
        {
            PossibleValues = new HashSet<int>(possibleValues);
        }

        public int Row { get; }
        public int Column { get; }
        public int Value { get; }
        public HashSet<int> PossibleValues { get; }
    }
}