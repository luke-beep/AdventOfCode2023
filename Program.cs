using System.Reflection;
using AdventOfCode2023.Days;
using AdventOfCode2023.Utilities;
using AdventOfCode2023.Utilities.Contracts;
using Spectre.Console;

namespace AdventOfCode2023;

internal class Program
{
    private const string Path1 = @"D:\Source\luke-beep\AdventOfCode2023\Inputs\Day1.txt";
    private const string Path2 = @"D:\Source\luke-beep\AdventOfCode2023\Inputs\Day2.txt";
    private const string Path3 = @"D:\Source\luke-beep\AdventOfCode2023\Inputs\Day3.txt";
    private const string Path4 = @"D:\Source\luke-beep\AdventOfCode2023\Inputs\Day4.txt";
    private const string Path5 = @"D:\Source\luke-beep\AdventOfCode2023\Inputs\Day5.txt";
    private const string Path6 = @"D:\Source\luke-beep\AdventOfCode2023\Inputs\Day6.txt";
    private const string Path7 = @"D:\Source\luke-beep\AdventOfCode2023\Inputs\Day7.txt";
    private const string Path8 = @"D:\Source\luke-beep\AdventOfCode2023\Inputs\Day8.txt";
    private const string Path9 = @"D:\Source\luke-beep\AdventOfCode2023\Inputs\Day9.txt";

    private static readonly ISolution Day1 = new Day1(Path1);
    private static readonly ISolution Day2 = new Day2(Path2);
    private static readonly ISolution Day3 = new Day3(Path3);
    private static readonly ISolution Day4 = new Day4(Path4);
    private static readonly ISolution Day5 = new Day5(Path5);
    private static readonly ISolution Day6 = new Day6(Path6);
    private static readonly ISolution Day7 = new Day7(Path7);
    private static readonly ISolution Day8 = new Day8(Path8);
    private static readonly ISolution Day9 = new Day9(Path9);

    private static readonly Table PathTable = new();
    private static readonly Table RootTable = new();

    private static readonly Calendar AdventCalendar = new(2023, 12);

    private static readonly Panel AdventCalendarPanel = new(Align.Center(AdventCalendar));
    private static readonly Panel PathPanel = new(Align.Center(PathTable));
    private static readonly Panel RootPanel = new(Align.Center(RootTable));

    private static readonly List<int> Ticked = new();

    private static async Task Main(string[] args)
    {
        var debug = args.Length > 0 && args[0] == "debug";
        var debugDay = args.Length > 1 ? int.Parse(args[1]) : 0;
        if (debug)
        {
            await Debug(debugDay);
        }
        else
        {
            await InitializeConsoleAsync();
            await ConfigurePanelsAsync();
            await InitializeTablesAsync();
            await ConfigureTablesAsync();
            await ConfigureCalendarAsync();
            await InitializeLayoutAsync();
        }

    }

    private static async Task Debug(int day)
    {
        var tmp = new Dictionary<string, string>();
        switch (day)
        {
            case 1:
                tmp = await Day1.Solve();
                break;
            case 2:
                tmp = await Day2.Solve();
                break;
            case 3:
                tmp = await Day3.Solve();
                break;
            case 4:
                tmp = await Day4.Solve();
                break;
            case 5:
                tmp = await Day5.Solve();
                break;
            case 6:
                tmp = await Day6.Solve();
                break;
            case 7:
                tmp = await Day7.Solve();
                break;
            case 8:
                tmp = await Day8.Solve();
                break;
            case 9:
                tmp = await Day9.Solve();
                break;
        }

        foreach (var kvp in tmp)
        {
            AnsiConsole.WriteLine($"{kvp.Key}: {kvp.Value}");
        }
    }

    private static async Task InitializeLayoutAsync()
    {
        await InitializeConsoleAsync();

        var layout = new Layout("Root")
            .SplitColumns(
                new Layout("Left"),
                new Layout("Right").SplitColumns(new Layout("Top"), new Layout("Bottom")));



        layout["Left"].Update(RootPanel);
        layout["Right"]["Top"].Update(AdventCalendarPanel);
        layout["Right"]["Bottom"].Update(PathPanel);

        AnsiConsole.Write(layout);
    }

    private static async Task ConfigurePanelsAsync()
    {
        RootPanel.Header = new PanelHeader("Solutions");
        RootPanel.Border = BoxBorder.Double;
        RootPanel.UseSafeBorder = false;
        RootPanel.Expand = false;

        PathPanel.Header = new PanelHeader("Paths");
        PathPanel.Border = BoxBorder.Double;
        PathPanel.UseSafeBorder = false;
        PathPanel.Expand = false;

        AdventCalendarPanel.Header = new PanelHeader("Calender");
        AdventCalendarPanel.Border = BoxBorder.Double;
        AdventCalendarPanel.UseSafeBorder = false;
        AdventCalendarPanel.Expand = false;

        await Task.CompletedTask;
    }

    private static async Task InitializeTablesAsync()
    {
        var solutions = await PromptSolutions();

        PathTable.AddColumn("Paths");

        List<Text> columns = new();
        List<Text> rows = new();

        foreach (var solution in solutions)
        {
            var solve = await solution.Solve();
            var solutionName = solution.GetType().GetCustomAttribute<Solution>()?.Day;
            var path = new TextPath(solution.GetPath());
            PathTable.AddRow(path);
            if (solutionName != null)
            {
                Ticked.Add(solutionName.Value);
                foreach (var kvp in solve)
                {
                    columns.Add(new Text($"Day {solutionName}, {kvp.Key}"));
                    rows.Add(new Text(kvp.Value));
                }
            }
            else
            {
                throw new Exception("Solution name is null");
            }
        }
        columns.ForEach(s => RootTable.AddColumn(new TableColumn(s)));
        RootTable.AddRow(rows);
    }

    private static async Task ConfigureTablesAsync()
    {
        RootTable.Border = TableBorder.Double;
        RootTable.Expand = false;

        PathTable.Border = TableBorder.Double;
        PathTable.Expand = false;

        await Task.CompletedTask;
    }

    private static async Task ConfigureCalendarAsync()
    {
        AdventCalendar.Border = TableBorder.Double;
        AdventCalendar.HighlightStyle = new Style(Color.Red);

        foreach(var i in Ticked)
        {
            AdventCalendar.AddCalendarEvent(2023, 12, i).HideHeader().Alignment(Justify.Center);
        }

        await Task.CompletedTask;
    }

    private static Task<List<ISolution>> PromptSolutions()
    {
        var solutionList = new List<string>
        {
            "Day 1",
            "Day 2",
            "Day 3",
            "Day 4",
            "Day 5",
            "Day 6",
            "Day 7",
            "Day 8",
            "Day 9"
        };

        var solutionStrings = AnsiConsole.Prompt(
            new MultiSelectionPrompt<string>().Title("Which [green]Advent of Code[/] solutions would you like to see?").PageSize(10).Required().MoreChoicesText("[grey](Move up and down to reveal more solutions)[/]").InstructionsText("[grey](Press [blue]<space>[/] to toggle a solution, " +
                "[green]<enter>[/] to accept)[/]").AddChoices(solutionList));

        var solutions = new List<ISolution>();
        foreach (var solutionString in solutionStrings)
        {
            switch (solutionString)
            {
                case "Day 1":
                    solutions.Add(Day1);
                    break;
                case "Day 2":
                    solutions.Add(Day2);
                    break;
                case "Day 3":
                    solutions.Add(Day3);
                    break;
                case "Day 4":
                    solutions.Add(Day4);
                    break;
                case "Day 5":
                    solutions.Add(Day5);
                    break;
                case "Day 6":
                    solutions.Add(Day6);
                    break;
                case "Day 7":
                    solutions.Add(Day7);
                    break;
                case "Day 8":
                    solutions.Add(Day8);
                    break;
                case "Day 9":
                    solutions.Add(Day9);
                    break;
            }
        }
        return Task.FromResult(solutions);
    }

    private static async Task InitializeConsoleAsync()
    {
        AnsiConsole.Clear();

        AnsiConsole.Write(new FigletText("Advent of Code 2023").Color(Color.Green).Centered());
        AnsiConsole.Write(new Markup("[link]https://github.com/luke-beep/AdventOfCode2023[/]").Centered());
        AnsiConsole.Write(new Markup("[link]https://adventofcode.com/2023[/]").Centered());

        AnsiConsole.Background = Color.Black;

        await WriteEmptyLinesAsync(4);
    }

    private static async Task WriteEmptyLinesAsync(int count)
    {
        for (var i = 0; i < count; i++)
        {
            AnsiConsole.WriteLine();
        }

        await Task.CompletedTask;
    }
}
