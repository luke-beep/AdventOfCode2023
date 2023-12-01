using AdventOfCode2023.Utilities.Contracts;

namespace AdventOfCode2023.Utilities;
internal class FileService : IFileService
{
    public async Task<string> ReadFileAsync(string path)
    {
        return await File.ReadAllTextAsync(path);
    }

    public async Task<string[]> ReadFileAsLinesAsync(string path)
    {
        return await File.ReadAllLinesAsync(path);
    }
}
