using AdventOfCode2020.Common;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020.Solutions
{
    class Day17
    {
        [Solution(17, 1)]
        public int Solution1(string input)
        {
            var activeCubes = new HashSet<Voxel>();
            var lines = Parser.ToArrayOfString(input);

            for (var i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                for(var j = 0; j < line.Length; j++)
                {
                    if (line[j] == '#')
                        activeCubes.Add(new Voxel(j, i, 0));
                }
            }

            for(var i = 0; i < 6; i++)
            {
                var toTest = activeCubes.SelectMany(it => it.GetSurrounding()).Distinct().ToArray();
                var newActive = new HashSet<Voxel>();

                foreach(var item in toTest)
                {
                    var isActive = activeCubes.Contains(item);
                    var surrounding = item.GetSurrounding().Where(it => activeCubes.Contains(it)).Count();

                    if ((isActive && (surrounding == 2 || surrounding == 3)) || (!isActive && surrounding == 3))
                        newActive.Add(item);
                }

                activeCubes = newActive;
            }

            return activeCubes.Count();
        }

        [Solution(17, 2)]
        public int Solution2(string input)
        {
            var activeCubes = new HashSet<HyperVox>();
            var lines = Parser.ToArrayOfString(input);

            for (var i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                for (var j = 0; j < line.Length; j++)
                {
                    if (line[j] == '#')
                        activeCubes.Add(new HyperVox(j, i, 0, 0));
                }
            }

            for (var i = 0; i < 6; i++)
            {
                var toTest = activeCubes.SelectMany(it => it.GetSurrounding()).Distinct().ToArray();
                var newActive = new HashSet<HyperVox>();

                foreach (var item in toTest)
                {
                    var isActive = activeCubes.Contains(item);
                    var surrounding = item.GetSurrounding().Where(it => activeCubes.Contains(it)).Count();

                    if ((isActive && (surrounding == 2 || surrounding == 3)) || (!isActive && surrounding == 3))
                        newActive.Add(item);
                }

                activeCubes = newActive;
            }

            return activeCubes.Count();
        }
    }
}
