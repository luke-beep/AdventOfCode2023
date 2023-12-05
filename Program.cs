using AdventOfCode2023.Days;
using AdventOfCode2023.Days.Contracts;

namespace AdventOfCode2023;

internal class Program
{
    private static async Task Main(string[] args)
    {
        IDay3 day3 = new Day3();
        await day3.Solve();
    }
}
