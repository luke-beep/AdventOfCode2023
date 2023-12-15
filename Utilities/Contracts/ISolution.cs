namespace AdventOfCode2023.Utilities.Contracts;

public interface ISolution
{
    Task<Dictionary<string, string>> Solve();

    string GetPath();
}