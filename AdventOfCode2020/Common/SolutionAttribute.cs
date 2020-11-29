using System;

namespace AdventOfCode2020.Common
{
    [AttributeUsage(AttributeTargets.Method)]
    class SolutionAttribute : Attribute
    {
        public int Day { get; }
        public int Problem { get; }

        public SolutionAttribute(int day, int problem)
        {
            Day = day;
            Problem = problem;
        }
    }
}
