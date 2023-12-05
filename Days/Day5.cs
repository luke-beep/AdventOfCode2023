using AdventOfCode2023.Days.Contracts;
using AdventOfCode2023.Utilities;

namespace AdventOfCode2023.Days;

[Solution(5)]
public class Day5 : ISolution
{
    private readonly string _path;
    public Day5(string path)
    {
        _path = path;
    }
    public string GetPath() => _path;

    public Task<Dictionary<string, string>> Solve()
    {
        Dictionary<string, string> result = new()
        {
            { "Part 1", "1" },
            { "Part 2", "2" }
        };
        return Task.FromResult(result);
    }
}