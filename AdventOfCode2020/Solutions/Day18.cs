using AdventOfCode2020.Common;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020.Solutions
{
    class Day18
    {
        [Solution(18, 1)]
        public long Solution1(string input)
        {
            var lines = Parser.ToArrayOfString(input).Select(it => it.Replace(" ", string.Empty)).ToArray();
            var sum = 0L;

            foreach(var item in lines)
            {
                sum += CaculateLine(item);
            }

            return sum;
        }

        [Solution(18, 2)]
        public long Solution2(string input)
        {
            var lines = Parser.ToArrayOfString(input).Select(it => it.Replace(" ", string.Empty)).ToArray();
            var sum = 0L;

            foreach (var item in lines)
            {
                sum += CaculateLine2(item);
            }

            return sum;
        }

        private long CaculateLine(string line)
        {
            var stack = new Stack<List<string>>();
            var currentSection = new List<string>();

            foreach(var item in line)
            {
                if(item == '(')
                {
                    stack.Push(currentSection);
                    currentSection = new List<string>();
                } 
                else if (item == ')')
                {
                    currentSection = stack.Pop().Concat(currentSection).ToList();
                }
                else
                {
                    currentSection.Add(item.ToString());
                }

                if(currentSection.Count == 3)
                {
                    var first = long.Parse(currentSection[0]);
                    var second = long.Parse(currentSection[2]);

                    currentSection = new List<string> { currentSection[1] == "+" ? (first + second).ToString() : (first * second).ToString() };
                }
            }

            return long.Parse(currentSection[0]);
        }

        private long CaculateLine2(string line)
        {
            var stack = new Stack<List<string>>();
            var currentSection = new List<string>();

            foreach (var item in line)
            {
                if (item == '(')
                {
                    stack.Push(currentSection);
                    currentSection = new List<string>();
                }
                else if (item == ')')
                {
                    currentSection = new[] { CaculateSection(currentSection).ToString() }.ToList();
                    currentSection = stack.Pop().Concat(currentSection).ToList();
                }
                else
                {
                    currentSection.Add(item.ToString());
                }
            }

            return CaculateSection(currentSection);
        }

        private long CaculateSection(List<string> input)
        {
            while (true)
            {
                var index = input.IndexOf("+");
                if (index == -1)
                    break;

                var first = long.Parse(input[index - 1]);
                var second = long.Parse(input[index + 1]);

                input = input.Take(index - 1).Concat(new[] { (first + second).ToString() }).Concat(input.Skip(index + 2)).ToList();
            }

            while (input.Count > 1)
            {
                var first = long.Parse(input[0]);
                var second = long.Parse(input[2]);

                input = new[] { (first * second).ToString() }.Concat(input.Skip(3)).ToList();
            }

            return long.Parse(input[0]);
        }
    }
}
