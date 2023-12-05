using System.Linq;
using System.Runtime.CompilerServices;
using AdventOfCode2023.Days.Contracts;
using AdventOfCode2023.Utilities;

namespace AdventOfCode2023.Days;

[Solution(5)]
public class Day5 : ISolution
{
    private readonly string _path;
    public Day5(string path)
    {
        _path = path;
    }
    public string GetPath() => _path;

    public async Task<Dictionary<string, string>> Solve()
    {
        var fileService = new FileService();
        var input = await fileService.ReadAllLinesAsync(_path);
        return await Solution(input);
    }

    private Task<Dictionary<string, string>> Solution(string[] input)
    {
        Almanac almanac = new();

        var seeds = input[0];
        var seedList = seeds.Replace("seeds: ", "");
        var seedListSplit = seedList.Split(" ");
        var seedListInt = seedListSplit.Select(Int128.Parse).ToList();
        
        almanac.Seeds = seedListInt;

        List<Int128[]> currentMap = null;


        foreach (var line in input.Skip(1))
        {
            if (line.StartsWith("seed-to-soil map:"))
            {
                currentMap = almanac.SeedToSoilMap;
            }
            else if (line.StartsWith("soil-to-fertilizer map:"))
            {
                currentMap = almanac.SoilToFertilizerMap;
            }
            else if (line.StartsWith("fertilizer-to-water map:"))
            {
                currentMap = almanac.FertilizerToWaterMap;
            }
            else if (line.StartsWith("water-to-light map:"))
            {
                currentMap = almanac.WaterToLightMap;
            }
            else if (line.StartsWith("light-to-temperature map:"))
            {
                currentMap = almanac.LightToTemperatureMap;
            }
            else if (line.StartsWith("temperature-to-humidity map:"))
            {
                currentMap = almanac.TemperatureToHumidityMap;
            }
            else if (line.StartsWith("humidity-to-location map:"))
            {
                currentMap = almanac.HumidityToLocationMap;
            }
            else if (!string.IsNullOrWhiteSpace(line) && currentMap != null)
            {
                var numbers = line.Split(' ').Select(Int128.Parse).ToArray();
                currentMap.Add(numbers);
            }
        }

        var lowest = GetLowestLocation(almanac);
        Dictionary<string, string> result = new()
        {
            { "Part 1", lowest.ToString() },
            { "Part 2", "2" }
        };
        return Task.FromResult(result);
    }

    public Int128 GetLowestLocation(Almanac almanac)
    {
        var locationNumbers = new List<Int128>();
        foreach (var seed in almanac.Seeds)
        {
            var soil = MapNumber(almanac.SeedToSoilMap, seed);
            var fertilizer = MapNumber(almanac.SoilToFertilizerMap, soil);
            var water = MapNumber(almanac.FertilizerToWaterMap, fertilizer);
            var light = MapNumber(almanac.WaterToLightMap, water);
            var temperature = MapNumber(almanac.LightToTemperatureMap, light);
            var humidity = MapNumber(almanac.TemperatureToHumidityMap, temperature);
            var location = MapNumber(almanac.HumidityToLocationMap, humidity);
            locationNumbers.Add(location);
        }
        return locationNumbers.Min();
    }

    public Int128 MapNumber(List<Int128[]> map, Int128 number)
    {
        if (map == null || map.Count == 0)
        {
            return number;
        }

        foreach (var mapping in map)
        {
            if (number >= mapping[1] && number < mapping[1] + mapping[2])
            {
                return mapping[0] + (number - mapping[1]);
            }
        }
        return number;
    }
}

public class Almanac
{
    public List<Int128> Seeds
    {
        get; set;
    }

    public List<Int128[]> SeedToSoilMap
    {
        get; set;
    }

    public List<Int128[]> SoilToFertilizerMap
    {
        get; set;
    }

    public List<Int128[]> FertilizerToWaterMap
    {
        get; set;
    }

    public List<Int128[]> WaterToLightMap
    {
        get; set;
    }

    public List<Int128[]> LightToTemperatureMap
    {
        get; set;
    }

    public List<Int128[]> TemperatureToHumidityMap
    {
        get; set;
    }

    public List<Int128[]> HumidityToLocationMap
    {
        get; set;
    }
}