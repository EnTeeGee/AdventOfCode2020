using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020.Common
{
    class HyperVox
    {
        public int W { get;  }

        public int X { get; }

        public int Y { get; }

        public int Z { get; }

        public HyperVox() { }

        public HyperVox(int w, int x, int y, int z)
        {
            W = w;
            X = x;
            Y = y;
            Z = z;
        }

        public HyperVox[] GetSurrounding()
        {
            var steps = new[] { -1, 0, 1 };
            var output = new List<HyperVox>();

            foreach(var newW in steps)
                foreach (var newX in steps)
                    foreach (var newY in steps)
                        foreach (var newZ in steps)
                            if (newW != 0 || newX != 0 || newY != 0 || newZ != 0)
                                output.Add(new HyperVox(W + newW, X + newX, Y + newY, Z + newZ));

            return output.ToArray();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is HyperVox))
                return false;

            var castObj = obj as HyperVox;

            return W == castObj.W && X == castObj.X && Y == castObj.Y && Z == castObj.Z;
        }

        public override int GetHashCode()
        {
            return W ^ X ^ Y ^ Z;
        }
    }
}
