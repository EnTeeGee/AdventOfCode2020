using AdventOfCode2020.Common;

namespace AdventOfCode2020.Solutions
{
    class Day01
    {
        [Solution(1, 1)]
        public string Solution1(string input)
        {
            var lines = Parser.ToArrayOfInt(input);

            for(var i = 0; i < lines.Length; i++)
            {
                for(var j = i + 1; j < lines.Length; j++)
                {
                    if (lines[i] + lines[j] == 2020)
                        return (lines[i] * lines[j]).ToString();
                }
            }

            return null;
        }

        [Solution(1, 2)]
        public string Solution2(string input)
        {
            var lines = Parser.ToArrayOfInt(input);

            for (var i = 0; i < lines.Length; i++)
            {
                for (var j = i + 1; j < lines.Length; j++)
                {
                    for(var k = j + 1; k < lines.Length; k++)

                    if (lines[i] + lines[j] + lines[k] == 2020)
                        return (lines[i] * lines[j] * lines[k]).ToString();
                }
            }

            return null;
        }
    }
}
