using System;
using System.Collections.Generic;

namespace Solver.Solvers
{
    public class CompositeSolver : ISolverInstance
    {
        private readonly IReadOnlyList<ISolverInstance> _solvers;

        public CompositeSolver(IReadOnlyList<ISolverInstance> solvers)
        {
            if (solvers == null || solvers.Count ==0)
                throw new ArgumentException(nameof(solvers), "Cannot solve a field without any solver!");

            _solvers = solvers;
        }

        public bool IsFieldModified { get; private set; }

        public Field Solve(Field raw)
        {
            IsFieldModified = false;
            var candidate = raw;

            for (var i = 0; i < _solvers.Count; i++)
            {
                //'intermediateResult' could be null or something else which will screw up everything
                var intermediateResult = _solvers[i].Solve(candidate);
                
                if (_solvers[i].IsFieldModified)
                {
                    IsFieldModified = true;
                    candidate = intermediateResult;
                    //go through whole solvers pipeline once again! i++ in 'for' will set 'i' to 0
                    i = -1;
                }
            }

            return candidate;
        }
    }
}