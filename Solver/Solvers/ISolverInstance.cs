namespace Solver.Solvers
{
    public interface ISolverInstance
    {
        /// <summary>
        /// tries to fill in at least one cell with valid value (in range 1 to 9)
        /// </summary>
        /// <param name="raw">initial <see cref="Field"/></param>
        /// <returns>either more complete <see cref="Field"/>, or <paramref name="raw"/> or null</returns>
        Field Solve(Field raw);
        
        /// <summary>
        /// should be used to notify outer world if <see cref="Solve"/> method succeeded in filling in at least one cell of the <see cref="Field"/> otherwise should return false!
        /// </summary>
        bool IsFieldModified { get; }
    }
}