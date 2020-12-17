using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020.Common
{
    class Voxel
    {
        public int X { get; }

        public int Y { get; }

        public int Z { get;  }

        public Voxel() { }

        public Voxel(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Voxel[] GetSurrounding()
        {
            var steps = new[] { -1, 0, 1 };
            var output = new List<Voxel>();

            foreach(var newX in steps)
            {
                foreach(var newY in steps)
                {
                    foreach(var newZ in steps)
                    {
                        if (newX != 0 || newY != 0 || newZ != 0)
                            output.Add(new Voxel(X + newX, Y + newY, Z + newZ));
                    }
                }
            }

            return output.ToArray();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Voxel))
                return false;

            var castObj = obj as Voxel;

            return X == castObj.X && Y == castObj.Y && Z == castObj.Z;
        }

        public override int GetHashCode()
        {
            return X ^ Y ^ Z;
        }
    }
}
