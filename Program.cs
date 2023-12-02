using AdventOfCode2023.Days;
using AdventOfCode2023.Days.Contracts;

namespace AdventOfCode2023;

internal class Program
{
    private static async Task Main(string[] args)
    {
        IDay2 day2 = new Day2();
        await day2.Solve();
    }
}
