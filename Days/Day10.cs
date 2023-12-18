using AdventOfCode2023.Utilities;
using AdventOfCode2023.Utilities.Contracts;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2023.Days;

public partial class Day10(string path) : ISolution
{

    [GeneratedRegex("S|-|J|7")]
    private static partial Regex Right();

    [GeneratedRegex("S|-|F|L")]
    private static partial Regex Left();

    [GeneratedRegex("S|\\||J|L")]
    private static partial Regex Down();

    [GeneratedRegex("S|\\||7|F")]
    private static partial Regex Up();

    public string GetPath() => path;

    public async Task<Dictionary<string, string>> Solve()
    {
        var fileService = new FileService();
        var input = await fileService.ReadAllLinesAsync(GetPath());
        return await Solution(input);
    }

    public Task<Dictionary<string, string>> Solution(string[] input)
    {
        var inputs = ConvertTo2DArray(input);

        var u = new Tuple<int, int, Regex, string, string>(-1, 0, Up(), "u", "d");
        var d = new Tuple<int, int, Regex, string, string>(1, 0, Down(), "d", "u");
        var l = new Tuple<int, int, Regex, string, string>(0, -1, Left(), "l", "r");
        var r = new Tuple<int, int, Regex, string, string>(0, 1, Right(), "r", "l");

        var validation = new Dictionary<char, Tuple<int, int, Regex, string, string>[]>
        {
            { '|', new[] { u, d } },
            { '-', new[] { l, r } },
            { 'J', new[] { u, l } },
            { '7', new[] { d, l } },
            { 'F', new[] { d, r } },
            { 'L', new[] { u, r } }
        };

        var start = FindStartPosition(inputs);
        var destination = DetermineInitialDirection(inputs, start, u, d, l, r);

        var i = 0;
        var pos = start;


        while (true)
        {
            i++;
            pos = [pos[0] + destination.Item1, pos[1] + destination.Item2];
            var currentChar = inputs[pos[0], pos[1]];

            if (currentChar == 'S') break;

            var moves = validation[currentChar];
            destination = destination.Item5 != moves[0].Item4 ? moves[0] : moves[1];
        }



        Dictionary<string, string> result = new()
            {
                { "Part 1", (i/2).ToString() },
                { "Part 2", "0" }
            };

        return Task.FromResult(result);
    }

    private static int[] FindStartPosition(char[,] inputs)
    {
        for (var y = 0; y < inputs.GetLength(0); y++)
        {
            for (var x = 0; x < inputs.GetLength(1); x++)
            {
                if (inputs[y, x] == 'S')
                {
                    return [y, x];
                }
            }
        }

        return null;
    }

    private static Tuple<int, int, Regex, string, string> DetermineInitialDirection(char[,] inputs, int[] start, Tuple<int, int, Regex, string, string> u, Tuple<int, int, Regex, string, string> d, Tuple<int, int, Regex, string, string> l, Tuple<int, int, Regex, string, string> r)
    {
        if (start[0] > 0 && u.Item3.IsMatch(inputs[start[0] - 1, start[1]].ToString())) return u;
        if (start[0] < inputs.GetLength(0) - 1 && d.Item3.IsMatch(inputs[start[0] + 1, start[1]].ToString())) return d;
        if (start[1] > 0 && l.Item3.IsMatch(inputs[start[0], start[1] - 1].ToString())) return l;
        if (start[1] < inputs.GetLength(1) - 1 && r.Item3.IsMatch(inputs[start[0], start[1] + 1].ToString())) return r;
        return null;
    }

    public static char[,] ConvertTo2DArray(string[] input)
    {
        var width = input[0].Length;
        var height = input.Length;
        var result = new char[height, width];

        for (var y = 0; y < height; y++)
        {
            var line = input[y];
            for (var x = 0; x < width; x++)
            {
                result[y, x] = line[x];
            }
        }

        return result;
    }
}
