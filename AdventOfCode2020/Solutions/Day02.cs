using AdventOfCode2020.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020.Solutions
{
    class Day02
    {
        [Solution(2, 1)]
        public string Solution1(string input)
        {
            return Parser.ToArrayOf<PasswordInfo>(input, it => new PasswordInfo(it)).Where(it => it.PassesTask1()).Count().ToString();
        }

        [Solution(2, 2)]
        public string Solution2(string input)
        {
            return Parser.ToArrayOf<PasswordInfo>(input, it => new PasswordInfo(it)).Where(it => it.PassesTask2()).Count().ToString();
        }

        private class PasswordInfo
        {

            public int RangeLow { get; }
            
            public int RangeHigh { get; }

            public char Letter { get; }

            public string Password { get; }

            public PasswordInfo(string input)
            {
                var items = input.Split(new[] { '-', ' ', ':' }, StringSplitOptions.RemoveEmptyEntries);
                RangeLow = Convert.ToInt32(items[0]);
                RangeHigh = Convert.ToInt32(items[1]);
                Letter = Convert.ToChar(items[2]);
                Password = items[3];
            }

            public bool PassesTask1()
            {
                var count = Password.Where(it => it == Letter).Count();

                return count >= RangeLow && count <= RangeHigh;
            }

            public bool PassesTask2()
            {
                return Password[RangeLow - 1] == Letter ^ Password[RangeHigh - 1] == Letter;
            }
        }
    }
}
