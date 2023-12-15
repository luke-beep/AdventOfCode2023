using AdventOfCode2023.Utilities;
using AdventOfCode2023.Utilities.Contracts;

namespace AdventOfCode2023.Days;

[Solution(2)]
public class Day2 : ISolution
{
    private readonly string _path;
    public Day2(string path)
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

    public Task<Dictionary<string, string>> Solution(string[] input)
    {
        var games = new List<Game>();
        for (var i = 0; i < input.Length; i++)
        {
            var replace = input[i].Replace($"Game {i + 1}: ", "");
            var rounds = replace.Split(";");

            var game = new Game
            {
                Id = i + 1,
                Reds = new List<int>(),
                Greens = new List<int>(),
                Blues = new List<int>()
            };

            foreach (var round in rounds)
            {
                var replace2 = round.Replace(",", "");
                var split = replace2.Split(" ").Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
                for (var j = 0; j < split.Length; j += 2)
                {
                    var color = split[j + 1];
                    var count = int.Parse(split[j]);
                    switch (color)
                    {
                        case "red":
                            game.Reds.Add(count);
                            break;
                        case "green":
                            game.Greens.Add(count);
                            break;
                        case "blue":
                            game.Blues.Add(count);
                            break;
                    }
                }
            }

            games.Add(game);
        }

        var possibleGames = games.Where(g => g is { MinReds: <= 12, MinGreens: <= 13, MinBlues: <= 14 });
        var sumOfIds = possibleGames.Sum(g => g.Id);
        var sumOfPowers = games.Sum(g => g.Power);
        return Task.FromResult(new Dictionary<string, string>
        {
            { "Part 1", sumOfIds.ToString() },
            { "Part 2", sumOfPowers.ToString() }
        });
    }
}

public class Game
{
    public int Id
    {
        get; set;
    }
    public List<int> Reds
    {
        get; set;
    }
    public List<int> Greens
    {
        get; set;
    }
    public List<int> Blues
    {
        get; set;
    }

    public int MinReds
    {
        get { return Reds.Max(); }
    }

    public int MinGreens
    {
        get { return Greens.Max(); }
    }

    public int MinBlues
    {
        get { return Blues.Max(); }
    }

    public int Power
    {
        get { return MinReds * MinGreens * MinBlues; }
    }
}