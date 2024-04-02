namespace Sphagnum.Common.Contracts.Messaging
{
    public readonly struct RoutingKey
    {
        public readonly byte Part1;
        public readonly byte Part2;
        public readonly byte Part3;

        public bool IsEmpry => Part1 == 0 && Part2 == 0 && Part3 == 0;

        public RoutingKey(byte part1, byte part2, byte part3)
        {
            Part1 = part1;
            Part2 = part2;
            Part3 = part3;
        }
    }
}
