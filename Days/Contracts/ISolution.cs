namespace AdventOfCode2023.Days.Contracts;

public interface ISolution
{
    Task<Dictionary<string, string>> Solve();

    string GetPath();
}