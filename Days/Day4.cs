using System.Collections.Specialized;
using AdventOfCode2023.Utilities;
using AdventOfCode2023.Utilities.Contracts;

namespace AdventOfCode2023.Days;

[Solution(4)]
public class Day4 : ISolution
{
    private readonly string _path;
    public Day4(string path)
    {
        _path = path;
    }

    public string GetPath()
    {
        return _path;
    }

    public async Task<Dictionary<string, string>> Solve()
    {
        var fileService = new FileService();
        var input = await fileService.ReadAllLinesAsync(_path);
        return await Solution(input);
    }

    private static Task<Dictionary<string, string>> Solution(string[] input)
    {
        List<CardGame> games = new();
        foreach (var line in input)
        {
            var game = new CardGame();
            var playerCards = new Dictionary<int, int>();
            var winningCards = new Dictionary<int, int>();

            var gameCard = line.Remove(0, 9);
            var cards = gameCard.Split(" | ");
            var winningCard = cards[0];
            var playerCard = cards[1];

            var winningCardSplit = winningCard.Split(" ");
            var playerCardSplit = playerCard.Split(" ");
            for (var i = 0; i < winningCardSplit.Length; i++)
            {
                var numberTrimmed = winningCardSplit[i].Trim();
                var parsed= int.TryParse(numberTrimmed, out var numberInt);
                if (!parsed)
                {
                    continue;
                }

                winningCards.Add(i, numberInt);
            }
            for (var i = 0; i < playerCardSplit.Length; i++)
            {
                var numberTrimmed = playerCardSplit[i].Trim();
                var parsed = int.TryParse(numberTrimmed, out var numberInt);
                if (!parsed)
                {
                    continue;
                }

                playerCards.Add(i, numberInt);
            }
            game.WinningCards = winningCards;
            game.PlayerCards = playerCards;
            games.Add(game);
        }

        var sum = games.Sum(x => x.CalculateCardPoints());
        Dictionary<string, string> result = new()
        {
            { "Part 1", $"{sum}" },
            { "Part 2", $"{CalculateScratchPoints(games)}" }
        };
        return Task.FromResult(result);
    }

    public static int CalculateScratchPoints(List<CardGame> games)
    {
        var totalCards = new Dictionary<int, int>();

        for (var i = 0; i < games.Count; i++)
        {
            totalCards[i] = 1; 
        }

        for (var i = 0; i < games.Count; i++)
        {
            var matches = games[i].CalculateMatches();

            for (var j = 1; j <= matches; j++)
            {
                var nextCardIndex = i + j;
                if (nextCardIndex >= games.Count)
                {
                    continue;
                }
                totalCards.TryAdd(nextCardIndex, 0);
                totalCards[nextCardIndex] += totalCards[i];
            }
        }

        return totalCards.Values.Sum();
    }


}

public class CardGame
{
    public Dictionary<int, int> WinningCards
    {
        get;
        set;
    } = new();

    public Dictionary<int, int> PlayerCards
    {
        get;
        set;
    } = new();

    public int CalculateCardPoints()
    {
        return CalculateMatches() == 0 ? 0 : (int)Math.Pow(2, CalculateMatches() - 1);
    }

    public int CalculateMatches()
    {
        return WinningCards.Values.Intersect(PlayerCards.Values).Count();
    }
}