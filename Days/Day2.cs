using AdventOfCode2023.Days.Contracts;
using AdventOfCode2023.Utilities;
using AdventOfCode2023.Utilities.Contracts;

namespace AdventOfCode2023.Days;

public class Day2 : IDay2
{
    public async Task Solve()
    {
        IFileService fileService = new FileService();
        var input = await fileService.ReadAllLinesAsync(@"F:\source\luke-beep\AdventOfCode2023\Inputs\Day2.txt");
        await Solution(input);
    }

    public async Task Solution(string[] input)
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
                Greens = new(),
                Blues = new()
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
        await Console.Out.WriteLineAsync(sumOfIds.ToString());

        var sumOfPowers = games.Sum(g => g.Power);
        await Console.Out.WriteLineAsync(sumOfPowers.ToString());
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

    public int MinReds => Reds.Max();
    public int MinGreens => Greens.Max();
    public int MinBlues => Blues.Max();

    public int Power => MinReds * MinGreens * MinBlues;
}