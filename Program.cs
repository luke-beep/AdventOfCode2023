using System.Reflection;
using AdventOfCode2023.Days;
using AdventOfCode2023.Days.Contracts;
using AdventOfCode2023.Utilities;
using Spectre.Console;

namespace AdventOfCode2023;

internal class Program
{
    private static async Task Main(string[] args)
    {
        AnsiConsole.Write(new FigletText("Advent of Code 2023").Color(Color.Green).Centered());
        AnsiConsole.Write(new Markup("[link]https://github.com/luke-beep/AdventOfCode2023[/]").Centered());
        AnsiConsole.Background = Color.Black;

        var path1 = @"F:\source\luke-beep\AdventOfCode2023\Inputs\Day1.txt";
        var path2 = @"F:\source\luke-beep\AdventOfCode2023\Inputs\Day2.txt";
        var path3 = @"F:\source\luke-beep\AdventOfCode2023\Inputs\Day3.txt";
        var path4 = @"F:\source\luke-beep\AdventOfCode2023\Inputs\Day4.txt";
        var path5 = @"F:\source\luke-beep\AdventOfCode2023\Inputs\Day5.txt";

        ISolution day1 = new Day1(path1);
        ISolution day2 = new Day2(path2);
        ISolution day3 = new Day3(path3);
        ISolution day4 = new Day4(path4);
        ISolution day5 = new Day5(path5);

        var solutions = new List<ISolution> { day1, day2, day3, day4, day5 };

        // Tables
        var pathTable = new Table
        {
            Border = TableBorder.Double,
            BorderStyle = null,
            UseSafeBorder = false,
            ShowHeaders = false,
            ShowRowSeparators = false,
            ShowFooters = false,
            Expand = false,
            Width = null,
            Title = null,
            Caption = null
        };
        pathTable.AddColumn("Paths");

        // Root
        var root = new Table();

        List<Text> columns = new();
        List<Text> rows = new();

        // Solve all solutions
        var lastSolution = 1;
        foreach (var solution in solutions)
        {
            var solve = await solution.Solve();
            var solutionName = solution.GetType().GetCustomAttribute<Solution>()?.Day;
            var path = new TextPath(solution.GetPath());
            pathTable.AddRow(path);
            if (solutionName != null)
            {
                lastSolution = solutionName.Value;
                foreach (var kvp in solve)
                {
                    columns.Add(new Text($"Day {solutionName} | {kvp.Key}"));
                    rows.Add(new Text(kvp.Value));
                }
            }
            else
            {
                throw new Exception("Solution name is null");
            }
        }
        columns.ForEach(s => root.AddColumn(new TableColumn(s)));
        root.AddRow(rows);


        // Calender
        var calender = new Calendar(2023, 12)
        {
            Day = 0,
            Border = TableBorder.Double,
            UseSafeBorder = false,
            BorderStyle = null,
            Culture = null,
            HighlightStyle = Color.Red,
            ShowHeader = true,
            HeaderStyle = null
        }.AddCalendarEvent(2023, 12, lastSolution).HideHeader().Alignment(Justify.Center);


        // Panels
        var calenderPanel = new Panel(Align.Center(calender))
        {
            Header = new PanelHeader("Calender", Justify.Center),
            Border = BoxBorder.Double,
            UseSafeBorder = false,
            BorderStyle = null,
            Expand = false,
            Padding = null,
            Height = null,
            Width = null
        };
        var pathPanel = new Panel(Align.Center(pathTable))
        {
            Header = new PanelHeader("Paths", Justify.Right),
            Border = BoxBorder.Double,
            UseSafeBorder = false,
            BorderStyle = null,
            Expand = false,
            Padding = null,
            Height = null,
            Width = null

        };
        var rootPanel = new Panel(Align.Center(new Columns(root)))
        {
            Header = new PanelHeader("Solutions"),
            Border = BoxBorder.Double,
            UseSafeBorder = false,
            BorderStyle = null,
            Expand = false,
            Padding = null,
            Height = null,
            Width = null
        };

        // Layout
        var layout = new Layout("Root")
            .SplitColumns(
                new Layout("Left"),
                new Layout("Right").SplitColumns(new Layout("Top"), new Layout("Bottom")));



        layout["Left"].Update(rootPanel);
        layout["Right"]["Top"].Update(calenderPanel);
        layout["Right"]["Bottom"].Update(pathPanel);
        
        AnsiConsole.Write(layout);
    }

 

}
