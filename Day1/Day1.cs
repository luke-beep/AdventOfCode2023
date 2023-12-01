using AdventOfCode2023.Day1.Contracts;
using AdventOfCode2023.Utilities;
using AdventOfCode2023.Utilities.Contracts;

namespace AdventOfCode2023.Day1;

public class Day1 : IDay1
{
    private static readonly Dictionary<string, int> P1 = new()
    {
        {"0", 0},
        {"1", 1},
        {"2", 2},
        {"3", 3},
        {"4", 4},
        {"5", 5},
        {"6", 6},
        {"7", 7},
        {"8", 8},
        {"9", 9},
    };

    private static readonly Dictionary<string, int> P2 = new(P1)
    {
        { "one", 1 },
        { "two", 2 },
        { "three", 3 },
        { "four", 4 },
        { "five", 5 },
        { "six", 6 },
        { "seven", 7 },
        { "eight", 8 },
        { "nine", 9 },
    };

    public async Task Solve()
    {
        IFileService fileService = new FileService();

        var input = await fileService.ReadFileAsLinesAsync(@"F:\source\luke-beep\AdventOfCode2023\Inputs\Day1.txt");

        Part1(input);
        Part2(input);
    }

    private static void Part1(IEnumerable<string> input)
    {
        var sum = 0;
        foreach (var s in input)
        {
            sum += Index(s, P1);
        }

        Console.WriteLine(sum);
    }
    private static void Part2(IEnumerable<string> input)
    {
        var sum = 0;
        foreach (var s in input)
        {
            sum += Index(s, P2);
        }

        Console.WriteLine(sum);
    }

    private static int Index(string line, Dictionary<string, int> dict)
    {
        var minIndex = int.MaxValue;
        var minValue = 0;
        var maxIndex = int.MinValue;
        var maxValue = 0;

        foreach (var kvp in dict)
        {
            var currentIndex = line.IndexOf(kvp.Key, StringComparison.Ordinal);
            if (currentIndex >= 0 && currentIndex < minIndex)
            {
                minIndex = currentIndex;
                minValue = kvp.Value;
            }

            currentIndex = line.LastIndexOf(kvp.Key, StringComparison.Ordinal);
            if (currentIndex > 0 || currentIndex <= maxIndex)
            {
                continue;
            }

            maxIndex = currentIndex;
            maxValue = kvp.Value;
        }

        var firstIndex = minValue * 10;
        var sum = firstIndex + maxValue;

        return sum;
    }
}