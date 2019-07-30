using FluentAssertions;
using Solver;
using Solver.Solvers;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Sdk;

namespace Tests
{
    public class SolverTests : IClassFixture<TestOutputHelper>
    {
        private readonly TestOutputHelper _outputHelper;

        public SolverTests(TestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        [Fact]
        public void Solve_OneCellEmpty_DoSolves()
        {
            var field = new Field();

            field[0] = new[] { 0, 8, 9, 0, 0, 0, 0, 0, 0 };
            field[1] = new[] { 0, 3, 5, 6, 0, 0, 8, 0, 0 };
            field[2] = new[] { 6, 0, 0, 0, 0, 7, 9, 3, 0 };

            field[3] = new[] { 0, 0, 2, 7, 6, 9, 4, 0, 3 };
            field[4] = new[] { 0, 0, 0, 8, 0, 5, 0, 0, 0 };
            field[5] = new[] { 5, 0, 7, 3, 1, 4, 6, 0, 0 };

            field[6] = new[] { 0, 7, 6, 2, 0, 0, 0, 0, 9 };
            field[7] = new[] { 0, 0, 3, 0, 0, 6, 7, 8, 0 };
            field[8] = new[] { 0, 0, 0, 0, 0, 0, 2, 1, 0 };

            Print(field);

            var solver = new CompositeSolver(new ISolverInstance[]{new CrossingSolver(), new OccupationSolver()});

            var solvedField = solver.Solve(field);

            PrintZeroMetric(solvedField);
            Print(solvedField);

            solvedField.RowWithDuplicatesIndex().Should().Be(-1);
            solvedField.ColumnWithDuplicatesIndex().Should().Be(-1);
            solvedField.SquareWithDuplicatesIndex().Should().Be(-1);

            for (int row = 0; row < Constraints.Size; row++)
            {
                solvedField[row].Distinct().Should().BeEquivalentTo(Enumerable.Range(1, 9));
            }

            for (int col = 0; col < 9; col++)
            {
                var aColumn = new List<int>(9);
                for (int row = 0; row < 9; row++)
                {
                    aColumn.Add(solvedField[row][col]);
                }

                aColumn.Should().BeEquivalentTo(Enumerable.Range(1, 9));
            }
        }

        private void PrintZeroMetric(Field solvedField)
        {
            var zeroCounter = 0;
            for (int i = 0; i < 81; i++)
            {
                if (!solvedField[i / 9][i % 9].IsAssigned())
                    zeroCounter++;
            }

            _outputHelper.WriteLine($"there are {zeroCounter} zeros in the result");
        }

        private void Print(Field field)
        {
            for (int i = 0; i < 9; i++)
            {
                var line = string.Join(" ", field[i].Select((n, col) => col % 3 == 2 ? $"{n}|" : $"{n}"));
                _outputHelper.WriteLine($"{i+1}: {line}");

                if (i % 3 == 2)
                {
                    _outputHelper.WriteLine(string.Join("_", Enumerable.Range(1,9).Select(n => "_")));
                }
            }
        }
    }
}
