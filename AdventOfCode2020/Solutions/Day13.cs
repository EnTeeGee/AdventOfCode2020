using AdventOfCode2020.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020.Solutions
{
    class Day13
    {
        [Solution(13, 1)]
        public int Solution1(string input)
        {
            var lines = Parser.ToArrayOfString(input);
            var time = int.Parse(lines[0]);
            var busses = lines[1].Split(',').Where(it => it != "x").Select(it => int.Parse(it)).ToArray();
            var firstBus = busses.Select(it => (id: it, timeTo: it - (time % it))).OrderBy(it => it.timeTo).First();

            return firstBus.id * firstBus.timeTo;
        }

        [Solution(13, 2)]
        public long Solution2(string input)
        {
            var busStrings = Parser.ToArrayOfString(input).Last().Split(',').ToArray();
            var busInfo = new List<(int id, int offset)>();

            for(var i = 0; i < busStrings.Length; i++)
            {
                if (busStrings[i] == "x")
                    continue;

                busInfo.Add((id: int.Parse(busStrings[i]), offset: i));
            }

            var firstInstance = 0L;
            var repeats = (long)busInfo[0].id;

            foreach(var item in busInfo.Skip(1))
            {
                var currentOffset = firstInstance;

                while((currentOffset + item.offset) % item.id != 0)
                    currentOffset += repeats;

                firstInstance = currentOffset;
                if (item == busInfo.Last())
                    return firstInstance;

                currentOffset += repeats;
                while ((currentOffset + item.offset) % item.id != 0)
                    currentOffset += repeats;

                repeats = currentOffset - firstInstance;
            }

            throw new Exception("Unexpected escape from loop");
        }
    }
}
