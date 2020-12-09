using AdventOfCode2020.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020.Solutions
{
    class Day07
    {
        [Solution(7, 1)]
        public int Solution1(string input)
        {
            var bags = Parser.ToArrayOf(input, it => new BagInfo(it));

            return GetBagsThatContain("shiny gold", bags).Count();
        }

        [Solution(7, 2)]
        public int Solution2(string input)
        {
            var bags = Parser.ToArrayOf(input, it => new BagInfo(it));

            return GetCountOfBagsInside(bags.First(it => it.Kind == "shiny gold"), bags);
        }

        private HashSet<string> GetBagsThatContain(string kind, BagInfo[] bags)
        {
            var contain = bags.Where(it => it.SubBags.Any(b => b.kind == kind)).ToArray();

            var output = new HashSet<string>(contain.Select(it => it.Kind));
            foreach(var item in contain)
            {
                output.UnionWith(GetBagsThatContain(item.Kind, bags));
            }

            return output;
        }

        private int GetCountOfBagsInside(BagInfo info, BagInfo[] bags)
        {
            if (!info.SubBags.Any())
                return 0;

            return info.SubBags.Select(it => it.count + (it.count * GetCountOfBagsInside(bags.First(b => b.Kind == it.kind), bags))).Sum();
        }

        private class BagInfo
        {
            public string Kind { get; }

            public (string kind, int count)[] SubBags;

            public BagInfo(string input)
            {
                var sections = input.Split(new[] { " contain " }, StringSplitOptions.RemoveEmptyEntries);
                Kind = sections[0].Substring(0, sections[0].Length - 5);

                if (sections[1].StartsWith("no"))
                {
                    SubBags = new(string kind, int count)[0];

                    return;
                }

                SubBags = sections[1]
                    .Split(new[] { ',', '.' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(it => it.Trim())
                    .Select(it => it.Split((char[])null, StringSplitOptions.RemoveEmptyEntries))
                    .Select(it => (kind: $"{it[1]} {it[2]}", count: int.Parse(it[0])))
                    .ToArray();
            }
        }
    }
}
