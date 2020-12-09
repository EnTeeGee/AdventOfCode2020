using AdventOfCode2020.Common;
using System;
using System.Linq;

namespace AdventOfCode2020.Solutions
{
    class Day09
    {
        const int PreambleSize = 25;

        [Solution(9, 1)]
        public long Solution1(string input)
        {
            var numbers = Parser.ToArrayOf(input, it => long.Parse(it));

            var buffer = numbers.Take(PreambleSize).ToArray();
            var index = 0;
            var pos = PreambleSize;

            while(pos < numbers.Length)
            {
                var foundValid = false;

                for(var i = 0; i < PreambleSize; i++)
                {
                    for(var j = i + 1; j < PreambleSize; j++)
                    {
                        if(buffer[i] + buffer[j] == numbers[pos] && buffer[i] != buffer[j])
                        {
                            buffer[index] = numbers[pos];
                            index++;
                            index %= PreambleSize;
                            pos++;
                            foundValid = true;
                            break;
                        }
                    }

                    if (foundValid)
                        break;
                }

                if (!foundValid)
                    return numbers[pos];
            }

            throw new Exception("No invalid number found");
        }

        [Solution(9, 2)]
        public long Solution2(string input)
        {
            var target = Solution1(input);
            var numbers = Parser.ToArrayOf(input, it => long.Parse(it));

            var start = 0;
            var length = 1;

            while(start < numbers.Length)
            {
                var sum = numbers.Skip(start).Take(length).Sum();

                if (sum < target)
                    length++;
                else if (sum > target)
                {
                    start++;
                    length--;
                }
                else
                {
                    var items = numbers.Skip(start).Take(length).ToArray();

                    return items.Max() + items.Min();
                }
            }

            throw new Exception("Unable to find range");
        }
    }
}
