namespace AdventOfCode2023.Utilities.Contracts;

public interface IFileService
{
    Task<string> ReadFileAsync(string path);
    Task<string[]> ReadAllLinesAsync(string path);
}