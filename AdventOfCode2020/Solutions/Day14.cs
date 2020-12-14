using AdventOfCode2020.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020.Solutions
{
    class Day14
    {
        [Solution(14, 1)]
        public long Solution1(string input)
        {
            var memory = new Dictionary<int, string>();
            var mask = new string(Enumerable.Repeat('X', 36).ToArray());
            var lines = Parser.ToArrayOfString(input);

            foreach(var item in lines)
            {
                if(item.StartsWith("mask"))
                {
                    mask = item.Substring(7);
                    continue;
                }

                var info = item.Substring(4).Split(new[] { "] = " }, StringSplitOptions.RemoveEmptyEntries).Select(it => long.Parse(it)).ToArray();
                var newValue = Convert.ToString(info[1], 2).PadLeft(36, '0');

                memory[(int)info[0]] = new string(newValue.Zip(mask, (a, b) => b == 'X' ? a : b).ToArray());
            }

            return memory.Sum(it => Convert.ToInt64(it.Value, 2));
        }

        [Solution(14, 2)]
        public long Solution2(string input)
        {
            var memory = new Dictionary<string, long>();
            var mask = new string(Enumerable.Repeat('X', 36).ToArray());
            var lines = Parser.ToArrayOfString(input);

            foreach (var item in lines)
            {
                if (item.StartsWith("mask"))
                {
                    mask = item.Substring(7);
                    continue;
                }

                var info = item.Substring(4).Split(new[] { "] = " }, StringSplitOptions.RemoveEmptyEntries).Select(it => long.Parse(it)).ToArray();
                var newAddress = Convert.ToString(info[0], 2).PadLeft(36, '0');

                newAddress = new string(newAddress.Zip(mask, (a, b) => b == '0' ? a : b == '1' ? '1' : 'X').ToArray());
                var addresses = GetPermutations(newAddress);

                foreach (var address in addresses)
                    memory[address] = info[1];
            }

            return memory.Sum(it => it.Value);
        }

        private string[] GetPermutations(string input)
        {
            var firstX = input.IndexOf('X');

            if (firstX == -1)
                return new[] { input };

            var permutations = GetPermutations(input.Substring(firstX + 1));

            return permutations
                .Select(it => input.Substring(0, firstX) + "0" + it)
                .Concat(permutations.Select(it => input.Substring(0, firstX) + "1" + it))
                .ToArray();
        }
    }
}
