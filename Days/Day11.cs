using AdventOfCode2023.Utilities;
using AdventOfCode2023.Utilities.Contracts;

namespace AdventOfCode2023.Days;

[Solution(11)]
public class Day11(string path) : ISolution
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
        var map = Universe.ExpandUniverse(input.ToArray(), 2);
        var part1 = map.GetShortestDistances();
        var map2 = Universe.ExpandUniverse(input.ToArray(), 1000000);
        var part2 = map2.GetShortestDistances();

        var result = new Dictionary<string, string>
        {
            { "Part1", part1.ToString() },
            { "Part2", part2.ToString() }
        };
        return Task.FromResult(result);
    }
}

public class Universe(IList<Vector> universes)
{
    public static Universe ExpandUniverse(IList<string> grid, long space)
    {
        var rowsToAdd = new List<int>();
        for (var row = 0; row < grid.Count; row++)
        {
            var allEmpty = true;
            for (var col = 0; col < grid[row].Length; col++)
            {
                if (grid[row][col] == '.') continue;
                allEmpty = false;
                break;
            }
            if (allEmpty)
            {
                rowsToAdd.Add(row);
            }
        }

        var colsToAdd = new List<int>();
        for (var col = 0; col < grid[0].Length; col++)
        {
            var allEmpty = true;
            foreach (var t in grid)
            {
                if (t[col] == '.') continue;
                allEmpty = false;
                break;
            }
            if (allEmpty)
            {
                colsToAdd.Add(col);
            }
        }

        var galaxies = new List<Vector>();
        for (var row = 0; row < grid.Count; row++)
        {
            var rowOffset = 0;
            foreach (var t in rowsToAdd)
            {
                if (t <= row)
                {
                    rowOffset += (int)(space - 1);
                }
            }

            for (var col = 0; col < grid[0].Length; col++)
            {
                if (grid[row][col] != '#')
                {
                    continue;
                }

                var colOffset = 0;
                foreach (var t in colsToAdd)
                {
                    if (t <= col)
                    {
                        colOffset += (int)(space - 1);
                    }
                }

                galaxies.Add(new Vector(row + rowOffset, col + colOffset));
            }
        }

        return new Universe(galaxies);
    }

    public void PrintUniverse()
    {
        var minRow = universes.Min(u => u.Row);
        var maxRow = universes.Max(u => u.Row);
        var minCol = universes.Min(u => u.Col);
        var maxCol = universes.Max(u => u.Col);

        for (var row = minRow; row <= maxRow; row++)
        {
            for (var col = minCol; col <= maxCol; col++)
            {
                var galaxy = universes.FirstOrDefault(u => u.Row == row && u.Col == col);
                Console.Write(galaxy == null ? '.' : '#');
            }

            Console.WriteLine();
        }
    }

    public long GetShortestDistances()
    {
        var pairs = new List<(Vector, Vector)>();
        for (var i = 0; i < universes.Count; i++)
        {
            for (var j = i + 1; j < universes.Count; j++)
            {
                var g1 = universes[i];
                var g2 = universes[j];
                pairs.Add((g1, g2));
            }
        }

        var sum = 0L;
        foreach (var pair in pairs)
        {
            var primary = pair.Item1;
            var secondary = pair.Item2;
            sum += primary.VectorTo(secondary).NumberOfSteps;
        }

        return sum;
    }
}

public record Vector(long Row, long Col)
{
    public Vector VectorTo(Vector x) => new(x.Row - Row, x.Col - Col);
    public long NumberOfSteps { get; } = Math.Abs(Row) + Math.Abs(Col);
}