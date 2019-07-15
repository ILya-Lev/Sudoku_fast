using System;
using System.Collections.Generic;
using System.Linq;

namespace Solver
{
    public class Line
    {
        private readonly int[] _values = new int[Constraints.Size];

        public HashSet<int> PresentValues { get; } = new HashSet<int>();

        public HashSet<int> PossibleValues { get; } = new HashSet<int>();

        public Line(int[] aLine)
        {
            Array.Copy(aLine, 0, _values, 0, Constraints.Size);
            foreach (var value in _values)
            {
                if (value.IsAssigned())
                    PresentValues.Add(value);
            }

            foreach (var absentValue in Enumerable.Range(1, Constraints.Size).Where(n => !PresentValues.Contains(n)))
            {
                PossibleValues.Add(absentValue);
            }
        }

        public bool IsComplete => PossibleValues.Count == 0;

        public int this[int index]
        {
            get => index.IsIndexInRange()
                ? _values[index]
                : throw new Exception($"{nameof(index)} is out of range {index}");
            set
            {
                if (!index.IsIndexInRange())
                {
                    throw new Exception($"{nameof(index)} is out of range {index}");
                }

                if (_values[index] == value)
                    return;

                if (_values[index].IsAssigned())
                    throw new Exception($"Cannot modify already set value {_values[index]} at index {index}");

                if (value.IsAssigned())
                {
                    _values[index] = value;
                    PresentValues.Add(value);
                    PossibleValues.Remove(value);
                }
            }
        }
    }
}