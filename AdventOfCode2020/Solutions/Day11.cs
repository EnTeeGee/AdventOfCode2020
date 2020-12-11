using AdventOfCode2020.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020.Solutions
{
    class Day11
    {
        [Solution(11, 1)]
        public int Solution1(string input)
        {
            var lines = Parser.ToArrayOfString(input);
            var seats = new List<Point>();

            for(var i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                for(var j = 0; j < line.Length; j++)
                {
                    if (line[j] == 'L')
                        seats.Add(new Point(j, i));
                }
            }

            var hasChanged = true;
            var takenSeats = new HashSet<Point>();

            while (hasChanged)
            {
                var newTakenSeats = new HashSet<Point>();
                hasChanged = false;

                foreach(var item in seats)
                {
                    var surrounding = item.GetSurrounding8().Where(it => takenSeats.Contains(it)).Count();
                    var taken = takenSeats.Contains(item);
                    var newState = taken;

                    if(taken && surrounding >= 4)
                    {
                        hasChanged = true;
                        newState = false;
                    }
                    else if(!taken && surrounding == 0)
                    {
                        hasChanged = true;
                        newState = true;
                    }

                    if (newState)
                        newTakenSeats.Add(item);
                }

                takenSeats = newTakenSeats;
            }

            return takenSeats.Count();
        }

        [Solution(11, 2)]
        public int Solution2(string input)
        {
            var lines = Parser.ToArrayOfString(input);
            var seats = new List<Point>();

            for (var i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                for (var j = 0; j < line.Length; j++)
                {
                    if (line[j] == 'L')
                        seats.Add(new Point(j, i));
                }
            }

            var hasChanged = true;
            var takenSeats = new HashSet<Point>();
            var surroundingSeats = GenerateSurrounding(seats);

            while (hasChanged)
            {
                var newTakenSeats = new HashSet<Point>();
                hasChanged = false;

                foreach (var item in seats)
                {
                    var surrounding = surroundingSeats[item].Where(it => takenSeats.Contains(it)).Count();
                    var taken = takenSeats.Contains(item);
                    var newState = taken;

                    if (taken && surrounding >= 5)
                    {
                        hasChanged = true;
                        newState = false;
                    }
                    else if (!taken && surrounding == 0)
                    {
                        hasChanged = true;
                        newState = true;
                    }

                    if (newState)
                        newTakenSeats.Add(item);
                }

                takenSeats = newTakenSeats;
            }

            return takenSeats.Count();
        }

        private Dictionary<Point, Point[]> GenerateSurrounding(List<Point> points)
        {
            var bounding = new Point(points.Max(it => it.X), points.Max(it => it.Y));
            var set = new HashSet<Point>(points);

            var output = new Dictionary<Point, Point[]>();

            foreach(var item in points)
                output.Add(item, GetSurroundingForPoint(item, set, bounding));

            return output;
        }

        private Point[] GetSurroundingForPoint(Point point, HashSet<Point> points, Point bounding)
        {
            var steppers = new List<Func<Point, Point>>{
                it => new Point(it.X, it.Y - 1),
                it => new Point(it.X + 1, it.Y - 1),
                it => new Point(it.X + 1, it.Y),
                it => new Point(it.X + 1, it.Y + 1),
                it => new Point(it.X, it.Y + 1),
                it => new Point(it.X - 1, it.Y + 1),
                it => new Point(it.X - 1, it.Y),
                it => new Point(it.X - 1, it.Y - 1)
            };

            var output = new List<Point>();
            foreach(var item in steppers)
            {
                var next = point;

                while(next.X >= 0 && next.X <= bounding.X && next.Y >= 0 && next.Y <= bounding.Y)
                {
                    next = item.Invoke(next);
                    if (points.Contains(next))
                    {
                        output.Add(next);
                        break;
                    }
                }
            }

            return output.ToArray();
        }
    }
}
