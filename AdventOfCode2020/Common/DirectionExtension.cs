namespace AdventOfCode2020.Common
{
    static class DirectionExtension
    {
        public static Direction Clockwise(this Direction direction, int steps = 1)
        {
            return (Direction)(((int)direction + steps) % 4);
        }

        public static Direction AntiClockwise(this Direction direction, int steps = 1)
        {
            var result = (int)direction - steps;
            while (result < 0)
                result += 4;

            return (Direction)result;
        }
    }
}
