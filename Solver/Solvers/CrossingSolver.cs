using System.Collections.Generic;
using System.Linq;

namespace Solver.Solvers
{
    public class CrossingSolver : ISolverInstance
    {
        public bool IsFieldModified { get; private set; }

        public Field Solve(Field rawField)
        {
            var (oneStepForwardField, isChanged) = MoveOneStepTowardCrossingSolution(rawField);
            IsFieldModified = isChanged;
            return oneStepForwardField;
        }
        
        private (Field, bool) MoveOneStepTowardCrossingSolution(Field rawField)
        {
            var rows = rawField.GetRows();
            var columns = rawField.GetColumns();
            var squares = rawField.GetSquares();

            var oneStepFurtherField = new Field();
            bool assignCrossingValue = true;
            for (int row = 0; row < Constraints.Size; row++)
            {
                var furtherRow = new List<int>(9);
                for (int col = 0; col < Constraints.Size; col++)
                {
                    if (rows[row][col].IsAssigned())
                    {
                        furtherRow.Add(rows[row][col]);
                        continue;
                    }

                    if (assignCrossingValue)
                    {
                        var cellPossibleValues = rows[row].PossibleValues
                        .Intersect(columns[col].PossibleValues)
                        .Intersect(squares[row / 3 * 3 + col / 3].PossibleValues)
                        .ToArray();
                        if (cellPossibleValues.Length == 1)
                        {
                            furtherRow.Add(cellPossibleValues[0]);
                            assignCrossingValue = false;
                        }
                        else
                            furtherRow.Add(0);

                    }
                    else
                        furtherRow.Add(0);
                }

                oneStepFurtherField[row] = furtherRow.ToArray();
            }

            return (oneStepFurtherField, !assignCrossingValue);
        }
    }
}