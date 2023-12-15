using AdventOfCode2023.Utilities;
using AdventOfCode2023.Utilities.Contracts;

namespace AdventOfCode2023.Days;

[Solution(1)]
public class Day1 : ISolution
{
    private readonly string _path;
    public Day1(string path)
    {
        _path = path;
    }

    public string GetPath()
    {
        return _path;
    }
    public async Task<Dictionary<string, string>> Solve()
    {
        IFileService fileService = new FileService();
        var input = await fileService.ReadAllLinesAsync(_path);
        return await Solution(input);
    }

    private async Task<Dictionary<string, string>> Solution(string[] input)
    {
        Dictionary<string, string> result = new();
        var p1 = await Part1(input);
        var p2 = await Part2(input);
        result.Add("Part 1", p1);
        result.Add("Part 2", p2);
        return result;
    }

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

    private static Task<string> Part1(IEnumerable<string> input)
    {
        var sum = 0;
        foreach (var s in input)
        {
            sum += Index(s, P1);
        }

        return Task.FromResult(sum.ToString());
    }
    private static Task<string> Part2(IEnumerable<string> input)
    {
        var sum = 0;
        foreach (var s in input)
        {
            sum += Index(s, P2);
        }

        return Task.FromResult(sum.ToString());
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