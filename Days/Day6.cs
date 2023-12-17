using AdventOfCode2023.Utilities;
using AdventOfCode2023.Utilities.Contracts;

namespace AdventOfCode2023.Days;

[Solution(6)]
public class Day6(string path) : ISolution
{
    public string GetPath()
    {
        return path;
    }

    public async Task<Dictionary<string, string>> Solve()
    {
        var fileService = new FileService();
        var input = await fileService.ReadAllLinesAsync(path);
        return await Solution(input);
    }

    public Task<Dictionary<string, string>> Solution(string[] input)
    {
        var boatGame = new BoatGame();

        var time = input[0].Split(":")[1].Trim();
        var distance = input[1].Split(":")[1].Trim();

        List<int> timeList = new();
        List<int> distanceList = new();

        foreach (var item in time.Split("     "))
        {
            timeList.Add(int.Parse(item));
        }
        foreach (var item in distance.Split("   "))
        {
            distanceList.Add(int.Parse(item));
        }

        boatGame.Time = timeList;
        boatGame.Distance = distanceList;

        var part1 = boatGame.Part1();
        var part2 = boatGame.Part2();

        Dictionary<string, string> result = new()
        {
            { "Part 1", part1.ToString() },
            { "Part 2", part2.ToString() }
        };

        return Task.FromResult(result);
    }
}

public class BoatGame
{
    public List<int> Time { get; set; }
    public List<int> Distance { get; set; }

    public int Part1()
    {
        var totalWays = 1;
        for (var i = 0; i < Time.Count; i++)
        {
            var ways = 0;
            for (var j = 1; j < Time[i]; j++)
            {
                var tmp = j * (Time[i] - j);
                if (tmp > Distance[i])
                {
                    ways++;
                }
            }
            totalWays *= ways;
        }
 
        // var totalWaysLinq = Time.Select((t, i) => Enumerable.Range(1, t - 1).Count(j => j * (t - j) > Distance[i])).Aggregate(1, (acc, ways) => acc * ways);
        return totalWays;
    }

    public int Part2()
    {
        var time = long.Parse(string.Join("", Time));
        var distance = long.Parse(string.Join("", Distance));

        var totalWays = 0;
        for (var i = 1; i < time; i++)
        {
            var tmp = i * (time - i);
            if (tmp > distance)
            {
                totalWays++;
            }
        }

        // var totalWaysLinq = Enumerable.Range(1, (int)time - 1).Count(i => i * (time - i) > distance);
        return totalWays;
    }


}