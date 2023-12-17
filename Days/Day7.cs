using AdventOfCode2023.Utilities;
using AdventOfCode2023.Utilities.Contracts;

namespace AdventOfCode2023.Days;

[Solution(7)]
public class Day7(string path) : ISolution
{
    public string GetPath()
    {
        return path;
    }

    public async Task<Dictionary<string, string>> Solve()
    {
        var fileService = new FileService();
        var input = await fileService.ReadFileAsync(path);
        return await Solution(input);
    }

    public static Task<Dictionary<string, string>> Solution(string input)
    {
        CamelCards cardGame = new(input);
        Dictionary<string, string> result = new()
        {
            { "Part 1", cardGame.Part1().ToString() },
            { "Part 2", cardGame.Part2().ToString() }
        };

        return Task.FromResult(result);
    }
}

// This one was hard to solve, but I'm not sure if I like the solution.
public class CamelCards(string input)
{
    private const string Part1Cards = "123456789TJQKA";
    private const string Part2Cards = "J123456789TQKA";

    public string Input { private get; set; } = input;
    public int Part1() => Solve(Part1Points);
    public int Part2() => Solve(Part2Points);

    private static (long, long) Part1Points(string hand)
    {
        return (PatternValue(hand), CardValue(hand, Part1Cards));
    }

    private static (long, long) Part2Points(string hand) {
        var pattern = Part1Cards.Select(ch => PatternValue(hand.Replace('J', ch))).Max();
        return (pattern, CardValue(hand, Part2Cards));
    }

    private static long CardValue(string hand, string cardOrder)
    {
        return Pack(hand.Select(card => cardOrder.IndexOf(card)));
    }

    private static long PatternValue(string hand)
    {
        return Pack(hand.Select(card => hand.Count(x => x == card)).OrderDescending());
    }

    private static long Pack(IEnumerable<int> numbers)
    {
        return numbers.Aggregate(1L, (a, v) => a * 256 + v);
    }

    private int Solve(Func<string, (long, long)> points) {
        var bids = Input.Split("\n").Select(line => new { line, hand = line.Split(" ")[0] }).Select(t => new { t, bid = int.Parse(t.line.Split(" ")[1]) }).OrderBy(t => points(t.t.hand)).Select(t => t.bid);
        

        return bids.Select((bid, rank) => (rank + 1) * bid).Sum();
    }


}

