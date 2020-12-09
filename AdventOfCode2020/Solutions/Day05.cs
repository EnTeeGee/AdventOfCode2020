using AdventOfCode2020.Common;
using System;
using System.Linq;

namespace AdventOfCode2020.Solutions
{
    class Day05
    {
        [Solution(5, 1)]
        public int Solution1(string input)
        {
            var passes = Parser.ToArrayOfString(input);

            return passes.Select(it => ToSeatId(it)).Max();
        }

        [Solution(5, 2)]
        public int Solution2(string input)
        {
            var seatIds = Parser.ToArrayOfString(input).Select(ToSeatId).ToHashSet();
            var validSeatIds = Enumerable.Range(1, seatIds.Max());

            return validSeatIds.First(it => !seatIds.Contains(it) && seatIds.Contains(it - 1) && seatIds.Contains(it + 1));
        }

        private int ToSeatId(string input)
        {
            var row = Convert.ToInt32(input.Substring(0, 7).Replace('F', '0').Replace('B', '1'), 2);
            var column = Convert.ToInt32(input.Substring(7).Replace('L', '0').Replace('R', '1'), 2);

            return (row * 8) + column;
        }
    }
}
