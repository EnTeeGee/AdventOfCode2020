using AdventOfCode2020.Common;

namespace AdventOfCode2020.Solutions
{
    class Day12
    {
        [Solution(12, 1)]
        public int Solution1(string input)
        {
            var actions = Parser.ToArrayOf(input, it => (action: it[0], distance: int.Parse(it.Substring(1))));

            var currentDirection = Direction.East;
            var currentPosition = new Point();

            foreach(var item in actions)
            {
                var action = item.action;

                if(action == 'F')
                {
                    action = currentDirection == Direction.North ? 'N' :
                        currentDirection == Direction.East ? 'E' :
                        currentDirection == Direction.South ? 'S' :
                        'W';
                }

                switch (action)
                {
                    case 'N':
                        currentPosition = new Point(currentPosition.X, currentPosition.Y - item.distance);
                        break;
                    case 'E':
                        currentPosition = new Point(currentPosition.X + item.distance, currentPosition.Y);
                        break;
                    case 'S':
                        currentPosition = new Point(currentPosition.X, currentPosition.Y + item.distance);
                        break;
                    case 'W':
                        currentPosition = new Point(currentPosition.X - item.distance, currentPosition.Y);
                        break;
                    case 'L':
                        currentDirection = currentDirection.AntiClockwise(item.distance / 90);
                        break;
                    case 'R':
                        currentDirection = currentDirection.Clockwise(item.distance / 90);
                        break;
                }
            }

            return currentPosition.ManDistanceTo(new Point());
        }

        [Solution(12, 2)]
        public int Solution2(string input)
        {
            var actions = Parser.ToArrayOf(input, it => (action: it[0], distance: int.Parse(it.Substring(1))));

            var waypointOffset = new Point(10, -1);
            var currentPosition = new Point();

            foreach(var item in actions)
            {
                switch (item.action)
                {
                    case 'N':
                        waypointOffset = new Point(waypointOffset.X, waypointOffset.Y - item.distance);
                        break;
                    case 'E':
                        waypointOffset = new Point(waypointOffset.X + item.distance, waypointOffset.Y);
                        break;
                    case 'S':
                        waypointOffset = new Point(waypointOffset.X, waypointOffset.Y + item.distance);
                        break;
                    case 'W':
                        waypointOffset = new Point(waypointOffset.X - item.distance, waypointOffset.Y);
                        break;
                    case 'L':
                        var remainingLeft = item.distance / 90;
                        for(var i = 0; i < remainingLeft; i++)
                            waypointOffset = new Point(waypointOffset.Y, -waypointOffset.X);
                        break;
                    case 'R':
                        var remainingRight = item.distance / 90;
                        for (var i = 0; i < remainingRight; i++)
                            waypointOffset = new Point(-waypointOffset.Y, waypointOffset.X);
                        break;
                    case 'F':
                        currentPosition = new Point(currentPosition.X + (waypointOffset.X * item.distance), currentPosition.Y + (waypointOffset.Y * item.distance));
                        break;
                }
            }

            return currentPosition.ManDistanceTo(new Point());
        }
    }
}
