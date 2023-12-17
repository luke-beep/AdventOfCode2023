using System.Text.RegularExpressions;
using AdventOfCode2023.Utilities;
using AdventOfCode2023.Utilities.Contracts;

namespace AdventOfCode2023.Days;

[Solution(9)]
public class Day9(string path) : ISolution
{
    public string GetPath() => path;

    public async Task<Dictionary<string, string>> Solve()
    {
        var fileService = new FileService();
        var input = await fileService.ReadAllLinesAsync(GetPath());
        return await Solution(input);
    }

    public static Task<Dictionary<string, string>> Solution(string[] input)
    {
        var part1 = Part1(input);
        var part2 = Part2(input);

        Dictionary<string, string> result = new()
        {
            { "Part 1", part1.ToString() },
            { "Part 2", part2.ToString() }
        };

        return Task.FromResult(result);
    }

    public static int Part1(string[] input)
    {
        var sum = 0;
        foreach (var line in input)
        {
            var values = line.Split(' ').Select(int.Parse).ToArray();
            sum += Extrapolate(values, true);
        }

        return sum;
    }


    public static int Part2(string[] input)
    {
        var sum = 0;
        foreach (var line in input)
        {
            var values = line.Split(' ').Select(int.Parse).ToArray();
            sum += Extrapolate(values, false);
        }

        return sum;
    }

    public static int Extrapolate(int[] values, bool positive)
    {
        var sequences = new List<int[]>();
        var initial = new int[values.Length];

        switch (positive)
        {
            case true:
                Array.Copy(values, initial, values.Length);
                break;
            default:
            {
                for (var i = 0; i < values.Length; i++)
                {
                    initial[i] = values[values.Length - 1 - i];
                }

                break;
            }
        }

        sequences.Add(initial);

        while (Array.Exists(sequences[sequences.Count - 1], val => val != 0))
        {
            var newSequence = new int[sequences[sequences.Count - 1].Length - 1];
            for (var i = 0; i < newSequence.Length; i++)
            {
                newSequence[i] = sequences[sequences.Count - 1][i + 1] - sequences[sequences.Count - 1][i];
            }
            sequences.Add(newSequence);
        }

        var result = 0;
        for (var i = sequences.Count - 2; i >= 0; i--)
        {
            result += sequences[i][sequences[i].Length - 1];
        }

        return result;
    }
}
