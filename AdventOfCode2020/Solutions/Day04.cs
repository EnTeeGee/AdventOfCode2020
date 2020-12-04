using AdventOfCode2020.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020.Solutions
{
    class Day04
    {
        private readonly char[] hexadecimals = new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };
        private readonly string[] eyeColours = new[] { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };

        [Solution(4, 1)]
        public int Solution1(string input)
        {
            var requiredKeys = new []{ "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid" };

            var blocks = input.Split(new[] { "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            var valid = 0;
            foreach (var block in blocks)
            {
                var items = block.Split((char[])null, StringSplitOptions.RemoveEmptyEntries).Select(it => it.Split(':')[0]).ToArray();

                if (requiredKeys.All(it => items.Contains(it)))
                    valid++;
            }

            return valid;
        }

        [Solution(4, 2)]
        public int Solution2(string input)
        {
            var requiredKeys = new Dictionary<string, Func<string, bool>>
            {
                { "byr", (it) => it.All(c => char.IsNumber(c)) && Convert.ToInt32(it) >= 1920 && Convert.ToInt32(it) <= 2002 },
                { "iyr", (it) => it.All(c => char.IsNumber(c)) && Convert.ToInt32(it) >= 2010 && Convert.ToInt32(it) <= 2020 },
                { "eyr", (it) => it.All(c => char.IsNumber(c)) && Convert.ToInt32(it) >= 2020 && Convert.ToInt32(it) <= 2030 },
                { "hgt", (it) => IsValidHeight(it) },
                { "hcl", (it) => it[0] == '#' && it.Skip(1).All(c => hexadecimals.Contains(c)) },
                { "ecl", (it) => eyeColours.Contains(it) },
                { "pid", (it) => it.Length == 9 && it.All(c => char.IsNumber(c)) }
            };

            var blocks = input.Split(new[] { "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            var valid = 0;
            foreach (var block in blocks)
            {
                var items = block.Split((char[])null, StringSplitOptions.RemoveEmptyEntries).Select(it => it.Split(':')).ToDictionary(it => it[0], it => it[1]);

                if (requiredKeys.All(it => items.ContainsKey(it.Key) && it.Value.Invoke(items[it.Key])))
                    valid++;
            }

            return valid;
        }

        private static bool IsValidHeight(string input)
        {
            if (input.Take(input.Length - 2).Any(it => !char.IsDigit(it)))
                return false;
            var value = Convert.ToInt32(new string(input.Take(input.Length - 2).ToArray()));

            return
                (input.EndsWith("cm") && value >= 150 && value <= 193) ||
                (input.EndsWith("in") && value >= 59 && value <= 76); 
        }
    }
}
