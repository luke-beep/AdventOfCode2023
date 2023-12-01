using AdventOfCode2023.Day1.Contracts;

namespace AdventOfCode2023;

internal class Program
{
    private static async Task Main(string[] args)
    {
        IDay1 day1 = new Day1.Day1();
        await day1.Solve();
    }
}
