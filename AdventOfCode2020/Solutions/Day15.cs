using AdventOfCode2020.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020.Solutions
{
    class Day15
    {
        [Solution(15, 1)]
        public int Solution1(string input)
        {
            return RunFor(input, 2020);
        }

        [Solution(15, 2)]
        public int Solution2(string input)
        {
            return RunFor(input, 30000000);
        }

        private int RunFor(string input, int cycles)
        {
            var items = input.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(it => int.Parse(it)).ToArray();
            var seen = new Dictionary<int, int>();
            foreach (var item in items.Zip(Enumerable.Range(1, items.Length - 1), (a, b) => new { a, b }))
                seen.Add(item.a, item.b);

            var lastSpoken = items.Last();

            for (var turn = items.Length + 1; turn <= cycles; turn++)
            {
                var nextSpoken = seen.ContainsKey(lastSpoken) ? (turn - 1) - seen[lastSpoken] : 0;
                seen[lastSpoken] = turn - 1;
                lastSpoken = nextSpoken;
            }

            return lastSpoken;
        }
    }
}
