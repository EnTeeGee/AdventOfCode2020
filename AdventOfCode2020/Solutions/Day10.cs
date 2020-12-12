using AdventOfCode2020.Common;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020.Solutions
{
    class Day10
    {
        [Solution(10, 1)]
        public int Solution1(string input)
        {
            var adapters = Parser.ToArrayOfInt(input).Concat(new[] { 0 }).OrderBy(it => it).ToArray();
            var diffs = adapters.Zip(adapters.Skip(1), (a, b) => b - a).ToArray();

            return diffs.Where(it => it == 1).Count() * (diffs.Where(it => it == 3).Count() + 1);
        }

        [Solution(10, 2)]
        public long Solution2(string input)
        {
            var adapters = Parser.ToArrayOfInt(input);
            adapters = adapters.Concat(new[] { 0, adapters.Max() + 3 }).OrderBy(it => it).ToArray();

            var list = new LinkedList<int>(adapters);
            var found = new Dictionary<int, long>();
            return GetValidConnections(list.First(), list.First.Next, found);
        }

        private long GetValidConnections(int current, LinkedListNode<int> remaining, Dictionary<int, long> found)
        {
            if (found.ContainsKey(current))
                return found[current];

            var sum = 0L;

            while (remaining.Value - current <= 3)
            {
                if (remaining.Next == null)
                {
                    ++sum;
                    break;
                }
                else
                {
                    sum += GetValidConnections(remaining.Value, remaining.Next, found);
                    remaining = remaining.Next;
                }
            }

            found[current] = sum;

            return sum;
        }
    }
}
