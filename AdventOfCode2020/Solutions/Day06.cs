using AdventOfCode2020.Common;
using System.Linq;

namespace AdventOfCode2020.Solutions
{
    class Day06
    {
        [Solution(6, 1)]
        public int Solution1(string input)
        {
            return Parser.ToArrayOfGroups(input).Select(it => it.Where(g => char.IsLetter(g)).Distinct().Count()).Sum();
        }

        [Solution(6, 2)]
        public int Solution2(string input)
        {
           return Parser.ToArrayOfGroups(input)
                .Select(it => (lines: Parser.ToArrayOfString(it).Length, groups: it.Where(c => char.IsLetter(c)).GroupBy(c => c)))
                .Select(it => it.groups.Where(g => g.Count() == it.lines).Count())
                .Sum();
        }
    }
}
