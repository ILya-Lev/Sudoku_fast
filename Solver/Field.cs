using System;

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
    }
}
