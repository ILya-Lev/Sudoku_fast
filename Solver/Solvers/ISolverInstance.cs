namespace Solver.Solvers
{
    public interface ISolverInstance
    {
        Field Solve(Field raw);
        bool IsFieldModified { get; }
    }
}