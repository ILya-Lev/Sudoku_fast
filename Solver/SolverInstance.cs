using System;
using System.Collections.Generic;
using System.Linq;

namespace Solver
{
    public class SolverInstance
    {
        public Field Solve(Field rawField)
        {
            var (oneStepForwardField, isChanged) = MoveOneStepTowardCrossingSolution(rawField);
            while (isChanged)
            {
                (oneStepForwardField, isChanged) = MoveOneStepTowardCrossingSolution(oneStepForwardField);
            }

            return oneStepForwardField;
        }

        private (Field, bool) MoveOneStepTowardCrossingSolution(Field rawField)
        {
            var rows = GetRows(rawField);
            var columns = GetColumns(rawField);
            var squares = GetSquares(rawField);

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

        private static Line[] GetSquares(Field rawField)
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
                        square.Add(rawField[row + rowShift][col + colShift]);
                    }
                }

                squares[squareIndex] = new Line(square.ToArray());
            }

            return squares;
        }

        private static Line[] GetColumns(Field rawField)
        {
            var columns = new Line[Constraints.Size];
            for (int columnIndex = 0; columnIndex < Constraints.Size; columnIndex++)
            {
                var column = new List<int>(9);
                for (int rowIndex = 0; rowIndex < Constraints.Size; rowIndex++)
                {
                    column.Add(rawField[rowIndex][columnIndex]);
                }

                columns[columnIndex] = new Line(column.ToArray());
            }

            return columns;
        }

        private static Line[] GetRows(Field rawField)
        {
            var rows = new Line[Constraints.Size];
            for (int i = 0; i < Constraints.Size; i++)
            {
                rows[i] = new Line(rawField[i]);
            }

            return rows;
        }
    }
}