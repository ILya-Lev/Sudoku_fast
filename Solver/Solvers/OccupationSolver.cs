namespace Solver.Solvers
{
    public class OccupationSolver : ISolverInstance
    {
        public bool IsFieldModified { get; private set; }
        public Field Solve(Field raw)
        {
            var modified = MoveOneStepTowardOccupationSolution(raw);
            IsFieldModified = modified != null;
            return modified ?? raw;
        }

        private Field MoveOneStepTowardOccupationSolution(Field rawField)
        {
            var rows = rawField.GetRows();
            var columns = rawField.GetColumns();
            var squares = rawField.GetSquares();

            var cells = new Cell[Constraints.Size, Constraints.Size];
            for (int row = 0; row < rows.Length; row++)
            {
                for (int col = 0; col < columns.Length; col++)
                {
                    if (rows[row][col].IsAssigned())
                        cells[row, col] = new Cell(row, col, rows[row][col]);
                    else
                    {
                        cells[row, col] = new Cell(row, col, rows[row].PossibleValues);
                        cells[row, col].PossibleValues.IntersectWith(columns[col].PossibleValues);
                        cells[row, col].PossibleValues.IntersectWith(squares[row / 3 * 3 + col / 3].PossibleValues);
                    }
                }
            }

            for (int row = 0; row < rows.Length; row++)
            {
                foreach (var possibleValue in rows[row].PossibleValues)
                {
                    var (popularity, colIdx) = GetPopularityInRow(cells, row, possibleValue);
                    if (popularity == 1 && colIdx != -1)
                    {
                        var nextStepField = rawField.Clone();
                        nextStepField[row][colIdx] = possibleValue;
                        return nextStepField;
                    }
                }
            }

            for (int col = 0; col < rows.Length; col++)
            {
                foreach (var possibleValue in columns[col].PossibleValues)
                {
                    var (popularity, rowIdx) = GetPopularityInColumn(cells, col, possibleValue);
                    if (popularity == 1 && rowIdx != -1)
                    {
                        var nextStepField = rawField.Clone();
                        nextStepField[rowIdx][col] = possibleValue;
                        return nextStepField;
                    }
                }
            }

            return null;
        }

        private (int, int) GetPopularityInColumn(Cell[,] cells, int col, int possibleValue)
        {
            int popularity = 0;
            int rowIndex = -1;
            for (int row = 0; row < Constraints.Size; row++)
            {
                if (cells[row, col].PossibleValues?.Contains(possibleValue) ?? false)
                {
                    popularity++;
                    rowIndex = row;
                }
            }

            return (popularity, rowIndex);
        }

        private (int, int) GetPopularityInRow(Cell[,] cells, int row, int possibleValue)
        {
            int popularity = 0;
            int column = -1;
            for (int col = 0; col < Constraints.Size; col++)
            {
                if (cells[row, col].PossibleValues?.Contains(possibleValue) ?? false)
                {
                    popularity++;
                    column = col;
                }
            }

            return (popularity, column);
        }
    }
}