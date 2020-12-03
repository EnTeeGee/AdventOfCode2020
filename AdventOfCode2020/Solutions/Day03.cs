using AdventOfCode2020.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020.Solutions
{
    class Day03
    {
        [Solution(3, 1)]
        public int Solution1(string input)
        {
            var width = input.IndexOf('\n') - 1;
            var trees = Parser.ToArrayOf(input, it => GetTrees(it));
            var currentX = 0;
            var hitTrees = 0;

            foreach(var item in trees)
            {
                if (item.Contains(currentX))
                    hitTrees++;

                currentX += 3;
                currentX %= width;
            }

            return hitTrees;
        }

        [Solution(3, 2)]
        public long Solution2(string input)
        {
            var width = input.IndexOf('\n') - 1;
            var trees = Parser.ToArrayOf(input, it => GetTrees(it));

            return GetHitTrees(trees, 1, 1, width) *
                GetHitTrees(trees, 3, 1, width) *
                GetHitTrees(trees, 5, 1, width) *
                GetHitTrees(trees, 7, 1, width) *
                GetHitTrees(trees, 1, 2, width);
        }

        private long GetHitTrees(int[][] trees, int xStep, int yStep, int width)
        {
            var currentX = 0;
            var hitTrees = 0;

            for(var i = 0; i < trees.Length; i += yStep)
            {
                var item = trees[i];

                if (item.Contains(currentX))
                    hitTrees++;

                currentX += xStep;
                currentX %= width;
            }

            return hitTrees;
        }

        private int[] GetTrees(string input)
        {
            return input
                .Zip(Enumerable.Range(0, input.Length), (a, b) => new { a, b })
                .Where(it => it.a == '#')
                .Select(it => it.b)
                .ToArray();
        }
    }
}
