using System.Text.RegularExpressions;
using AdventOfCode2023.Days.Contracts;
using AdventOfCode2023.Utilities;
using static AdventOfCode2023.Days.Parts;

namespace AdventOfCode2023.Days;

[Solution(3)]
public partial class Day3 : ISolution
{
    private readonly string _path;
    public Day3(string path)
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

    public static Task<Dictionary<string, string>> Solution(string[] input)
    {
        var symbols = new List<Symbol>();
        var numbers = new List<Number>();

        var lineNum = 0;
        foreach (var line in input)
        {
            ProcessNumbers(line, numbers, lineNum);
            ProcessSymbols(line, symbols, lineNum);

            lineNum++;
        }

        var result = new Dictionary<string, string>
        {
            { "Part1", $"{Sum(numbers, symbols)}" },
            { "Part2", $"{Ratio(numbers, symbols)}" }
        };
        return Task.FromResult(result);
    }

    private static void ProcessNumbers(string line, ICollection<Number> numbers, int lineNum)
    {
        foreach (var match in NumberRegex().Matches(line).Cast<Match>())
        {
            var isNumber = int.TryParse(match.Value, out var value);
            if (!isNumber)
            {
                continue;
            }
            var position = new Position(lineNum, match.Index, match.Index + match.Length - 1);
            var number = new Number(value, position);
            numbers.Add(number);
        }
    }

    private static void ProcessSymbols(string line, ICollection<Symbol> symbols, int lineNum)
    {
        foreach (var match in SymbolRegex().Matches(line).Cast<Match>())
        {
            var position = new Position(lineNum, match.Index, match.Index);
            var symbol = new Symbol(match.Value, position);
            symbols.Add(symbol);
        }
    }

    [GeneratedRegex(@"(\d)+")]
    private static partial Regex NumberRegex();
    [GeneratedRegex(@"[^\.\d\n]")]
    private static partial Regex SymbolRegex();
}

public class Parts
{
    public class Position()
    {
        public Position(int row, int columnStart, int columnEnd) : this()
        {
            Row = row;
            ColumnStart = columnStart;
            ColumnEnd = columnEnd;
        }

        public bool IsAdjacent(Number number)
        {
            if (Row < number.Pos.Row - 1 || Row > number.Pos.Row + 1)
            {
                return false;
            }
            var isAdjacent = ColumnStart >= number.Pos.ColumnStart - 1 && ColumnStart <= number.Pos.ColumnEnd + 1;
            return isAdjacent;
        }

        public int Row
        {
            get;
        }

        public int ColumnStart
        {
            get;
        }

        public int ColumnEnd
        {
            get;
        }
    };

    public class Symbol()
    {
        public Symbol(string smValue, Position pos) : this()
        {
            Value = smValue;
            Pos = pos;
        }

        public string Value
        {
            get;
        }

        public Position Pos
        {
            get;
        }
    }

    public class Number()
    {
        public Number(int value, Position pos) : this()
        {
            Value = value;
            Pos = pos;
        }

        public int Value
        {
            get;
        }

        public Position Pos
        {
            get;
        }
    }

    public static int Sum(List<Number> numbers, List<Symbol> symbols)
    {
        var sum = 0;
        foreach (var n in numbers)
        {
            var isAdjacent = false;
            foreach (var s in symbols)
            {
                if (!s.Pos.IsAdjacent(n))
                {
                    continue;
                }

                isAdjacent = true;
                break;
            }
            if (isAdjacent)
            {
                sum += n.Value;
            }
        }
        return sum;
    }

    public static int Ratio(List<Number> numbers, List<Symbol> symbols)
    {
        var sum = 0;
        foreach (var g in symbols)
        {
            var adjacentNumbers = new List<Number>();
            foreach (var n in numbers)
            {
                if (g.Pos.IsAdjacent(n))
                {
                    adjacentNumbers.Add(n);
                }
            }

            if (adjacentNumbers.Count != 2)
            {
                continue;
            }

            {
                var product = 1;
                foreach (var n in adjacentNumbers)
                {
                    product *= n.Value;
                }
                sum += product;
            }
        }
        return sum;
    }
}


