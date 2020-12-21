using AdventOfCode2020.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020.Solutions
{
    class Day20
    {
        [Solution(20, 1)]
        public long Solution1(string input)
        {
            var tiles = Parser.ToArrayOfGroups(input).Select(it => new Tile(it)).ToArray();
            var product = 1L;
            for(var i = 0; i < tiles.Length; i++)
            {
                var tile = tiles[i];
                var sides = tile.GetSides();
                sides = new[] { sides[0], sides[2], sides[4], sides[6] };
                var totalMatches = 0;
                for(var j = 0; j < tiles.Length; j++)
                {
                    if (i == j)
                        continue;

                    var checkingSides = tiles[j].GetSides();
                    foreach(var item in sides)
                    {
                        if (checkingSides.Any(it => AreSameList(item, it)))
                            ++totalMatches;
                    }
                }

                if (totalMatches == 2)
                    product *= tile.Id;
            }

            return product;
        }

        [Solution(20, 2)]
        public long Solution2(string input)
        {
            var tiles = Parser.ToArrayOfGroups(input).Select(it => new Tile(it)).ToArray();

            for (var i = 0; i < tiles.Length; i++)
            {
                var tile = tiles[i];
                var sides = tile.GetSides();
                sides = new[] { sides[0], sides[2], sides[4], sides[6] };
                for (var j = 0; j < tiles.Length; j++)
                {
                    if (i == j)
                        continue;

                    var checkingSides = tiles[j].GetSides();
                    foreach (var item in sides)
                    {
                        if (checkingSides.Any(it => AreSameList(item, it)))
                            tile.JoinedTo = tile.JoinedTo.Concat(new[] { tiles[j] }).ToArray();
                    }
                }
            }

            var grid = MapToGrid(tiles);

            var points = MapToPointBag(grid);
            var monsters = new[]
            {
                GetSeaMonster(),
                GetSeaMonster().Select(it => new Point(-it.X, it.Y)).ToArray(),
                GetSeaMonster().Select(it => new Point(it.X, -it.Y)).ToArray(),
                GetSeaMonster().Select(it => new Point(-it.X, -it.Y)).ToArray()
            };

            for(var i = 0; i < 4; i++)
            {
                foreach(var monster in monsters)
                {
                    var count = 0;

                    foreach (var point in points)
                    {
                        if (IsSeaMonsterAtPoint(points, point, monster))
                            ++count;
                    }

                    if(count > 0)
                    {
                        return points.Count - (monster.Length * count);
                    }
                }

                monsters = monsters.Select(it => it.Select(mp => new Point(mp.Y, -mp.X)).ToArray()).ToArray();
            }

            throw new Exception("Failed to find any monsters");
        }

        private Tile[,] MapToGrid(Tile[] tiles)
        {
            var size = (int)Math.Sqrt(tiles.Length);
            var corner = tiles.First(it => it.JoinedTo.Length == 2);
            var joined = corner.JoinedTo;
            var leftPermutations = joined[0].GetPermutations();
            var leftRightLink = corner.GetPermutations().Where(it => leftPermutations.Any(left => it.Matches(left, Direction.East))).ToList();
            var upDownLink = leftRightLink.Where(it => joined[1].GetPermutations().Any(down => it.Matches(down, Direction.South))).ToList();
            var output = new Tile[size, size];
            output[0, 0] = upDownLink.First();



            for(var i = 0; i < size; i++)
            {
                if (i != 0)
                {
                    var above = output[0, i - 1];
                    foreach(var tile in above.JoinedTo.SelectMany(it => it.GetPermutations()))
                    {
                        if(above.Matches(tile, Direction.South))
                        {
                            output[0, i] = tile;
                            break;
                        }
                    }
                }

                for(var j = 1; j < size; j++)
                {
                    var validTiles = output[j - 1, i].JoinedTo;
                    if(i != 0)
                    {
                        validTiles = validTiles.Where(it => output[j, i - 1].JoinedTo.Select(jt => jt.Id).Contains(it.Id)).ToArray();
                    }

                    validTiles = validTiles.SelectMany(it => it.GetPermutations()).ToArray();

                    var matching = validTiles.First(it => output[j - 1, i].Matches(it, Direction.East));
                    output[j, i] = matching;
                }
            }

            return output;
        }

        private HashSet<Point> MapToPointBag(Tile[,] input)
        {
            var size = input.GetLength(0);
            var output = new HashSet<Point>();
            var stepSize = 8;

            for(var i = 0; i < size; i++)
            {
                for(var j = 0; j < size; j++)
                {
                    var tile = input[j, i];
                    var points = tile.Points.Where(it => !(it.X == 0 || it.Y == 0 || it.X == Tile.Size || it.Y == Tile.Size)).ToArray();

                    points = points.Select(it => new Point((j * stepSize) + (it.X - 1), (i * stepSize) + (it.Y - 1))).ToArray();
                    foreach (var item in points)
                        output.Add(item);
                }
            }

            return output;
        }

        private Point[] GetSeaMonster()
        {
            return new[]
            {
                new Point(0, 0),
                new Point(1, 1),
                new Point(4, 1),
                new Point(5, 0),
                new Point(6, 0),
                new Point(7, 1),
                new Point(10, 1),
                new Point(11, 0),
                new Point(12, 0),
                new Point(13, 1),
                new Point(16, 1),
                new Point(17, 0),
                new Point(18, 0),
                new Point(19, 0),
                new Point(18, -1)
            };
        }

        private bool IsSeaMonsterAtPoint(HashSet<Point> points, Point targetPoint, Point[] monster)
        {
            return monster.All(it => points.Contains(new Point(it.X + targetPoint.X, it.Y + targetPoint.Y)));
        }

        private Dictionary<Point, Tile> TryPlaceTile(Tile tile, Point[] positions, Tile[] remaining, Dictionary<Point, Tile> placed)
        {
            var permutations = tile.GetPermutations();
            var position = positions.First();

            foreach(var item in permutations)
            {
                bool fits = true;
                var north = new Point(position.X, position.Y - 1);
                if (placed.ContainsKey(north) && !placed[north].Matches(item, Direction.South))
                    fits = false;
                var east = new Point(position.X + 1, position.Y);
                if (fits && placed.ContainsKey(east) && !placed[east].Matches(item, Direction.West))
                    fits = false;
                var south = new Point(position.X, position.Y + 1);
                if (fits && placed.ContainsKey(south) && !placed[south].Matches(item, Direction.North))
                    fits = false;
                var west = new Point(position.X - 1, position.Y);
                if (fits && placed.ContainsKey(west) && !placed[west].Matches(item, Direction.East))
                    fits = false;

                if (!fits)
                    continue;

                var newPlaced = new Dictionary<Point, Tile>(placed);
                newPlaced.Add(position, item);

                if (!remaining.Any())
                    return newPlaced;

                var remainingPositions = positions.Skip(1).ToArray();

                foreach(var next in remaining)
                {
                    var nextRemaining = remaining.Where(it => it != next).ToArray();
                    var result = TryPlaceTile(next, remainingPositions, nextRemaining, newPlaced);
                    if (result != null)
                        return result;
                }
            }

            return null;
        }

        private bool AreSameList(int[] first, int[] second)
        {
            return first.Length == second.Length && first.Zip(second, (a, b) => a - b).All(it => it == 0);
        }

        private class Tile
        {
            public static readonly int Size = 9;

            public int Id { get; }
            public Point[] Points;

            private Tile[] permutations;

            public Tile[] JoinedTo { get; set; }

            public Tile(string input)
            {
                var lines = Parser.ToArrayOfString(input);
                Id = int.Parse(lines[0].Substring(5, 4));
                var pointBuilder = new List<Point>();
                JoinedTo = new Tile[0];

                for(var i = 1; i < lines.Length; i++)
                {
                    var line = lines[i];
                    for(var j = 0; j < line.Length; j++)
                    {
                        if (line[j] == '#')
                            pointBuilder.Add(new Point(j, i - 1));
                    }
                }

                Points = pointBuilder.ToArray();
            }

            public Tile(int id, Point[] points, Tile[] joined)
            {
                Id = id;
                Points = points;
                JoinedTo = joined;
            }

            public Tile[] GetPermutations()
            {
                if(permutations == null)
                    permutations = GetFlipped().SelectMany(it => it.GetRotations()).ToArray();

                return permutations;
            }

            public Tile[] GetRotations()
            {
                var clockwiseOnce = new Tile(Id, Points.Select(it => new Point(-it.Y + Size, it.X)).ToArray(), JoinedTo);
                var clockwiseTwice = new Tile(Id, clockwiseOnce.Points.Select(it => new Point(-it.Y + Size, it.X)).ToArray(), JoinedTo);
                var clockwiseThrice = new Tile(Id, clockwiseTwice.Points.Select(it => new Point(-it.Y + Size, it.X)).ToArray(), JoinedTo);

                return new[] { this, clockwiseOnce, clockwiseTwice, clockwiseThrice };
            }

            public Tile[] GetFlipped()
            {
                var flippedX = new Tile(Id, Points.Select(it => new Point(-it.X + Size, it.Y)).ToArray(), JoinedTo);
                var flippedY = new Tile(Id, Points.Select(it => new Point(it.X, -it.Y + Size)).ToArray(), JoinedTo);

                return new[] { this, flippedX, flippedY };
            }

            public bool Matches(Tile target, Direction direction)
            {
                var sourcePoints = new int[0];
                var targetPoints = new int[0];
                switch (direction)
                {
                    case Direction.North:
                        sourcePoints = Points.Where(it => it.Y == 0).Select(it => it.X).ToArray();
                        targetPoints = target.Points.Where(it => it.Y == Size).Select(it => it.X).ToArray();
                        break;
                    case Direction.East:
                        sourcePoints = Points.Where(it => it.X == Size).Select(it => it.Y).ToArray();
                        targetPoints = target.Points.Where(it => it.X == 0).Select(it => it.Y).ToArray();
                        break;
                    case Direction.South:
                        sourcePoints = Points.Where(it => it.Y == Size).Select(it => it.X).ToArray();
                        targetPoints = target.Points.Where(it => it.Y == 0).Select(it => it.X).ToArray();
                        break;
                    case Direction.West:
                        sourcePoints = Points.Where(it => it.X == 0).Select(it => it.Y).ToArray();
                        targetPoints = target.Points.Where(it => it.X == Size).Select(it => it.Y).ToArray();
                        break;
                }

                sourcePoints = sourcePoints.OrderBy(it => it).ToArray();
                targetPoints = targetPoints.OrderBy(it => it).ToArray();

                return sourcePoints.Length == targetPoints.Length && sourcePoints.Zip(targetPoints, (a, b) => a - b).All(it => it == 0);
            }

            public int[][] GetSides()
            {
                var left = Points.Where(it => it.X == 0).Select(it => it.Y).ToArray();
                var right = Points.Where(it => it.X == Size).Select(it => it.Y).ToArray();
                var top = Points.Where(it => it.Y == 0).Select(it => it.X).ToArray();
                var bottom = Points.Where(it => it.Y == Size).Select(it => it.X).ToArray();

                return new[]
                {
                    left.OrderBy(it => it).ToArray(),
                    left.Select(it => -it + Size).OrderBy(it => it).ToArray(),
                    right.OrderBy(it => it).ToArray(),
                    right.Select(it => -it + Size).OrderBy(it => it).ToArray(),
                    top.OrderBy(it => it).ToArray(),
                    top.Select(it => -it + Size).OrderBy(it => it).ToArray(),
                    bottom.OrderBy(it => it).ToArray(),
                    bottom.Select(it => -it + Size).OrderBy(it => it).ToArray()
                };
            }

            public int[] GetSide(Direction direction)
            {
                switch (direction)
                {
                    case Direction.North:
                        return Points.Where(it => it.Y == 0).Select(it => it.X).ToArray();
                    case Direction.East:
                        return Points.Where(it => it.X == Size).Select(it => it.Y).ToArray();
                    case Direction.South:
                        return Points.Where(it => it.Y == Size).Select(it => it.X).ToArray();
                    case Direction.West:
                        return Points.Where(it => it.X == 0).Select(it => it.Y).ToArray();
                }

                throw new Exception("Unexpected direction");
            }
        }
    }
}
