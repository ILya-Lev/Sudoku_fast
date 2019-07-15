namespace Solver
{
    public static class Constraints
    {
        public const int Size = 9;
        public static bool IsIndexInRange(this int index) => 0 <= index && index < 9;
        public static bool IsAssigned(this int value) => 1 <= value && value <= 9;
    }
}