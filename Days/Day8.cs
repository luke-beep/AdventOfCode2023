using AdventOfCode2023.Utilities;
using AdventOfCode2023.Utilities.Contracts;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.Days;

internal class Day8(string path) : ISolution
{
    public string GetPath() => path;

    public async Task<Dictionary<string, string>> Solve()
    {
        var fileService = new FileService();
        var input = await fileService.ReadAllLinesAsync(GetPath());
        return await Solution(input);
    }

    public static Task<Dictionary<string, string>> Solution(string[] input)
    {
        //var part1 = Part1(input);
        var part2 = Part2(input);

        Dictionary<string, string> result = new()
        {
            { "Part 1", "e" },
            { "Part 2", part2.ToString() }
        };

        return Task.FromResult(result);
       
    }

    public static int Part1(string[] input)
    {
        var Node = new Nodes();
        var instructions = input[0].ToCharArray();
        var nodes = input[1..].ToList();

        // RL
        foreach (var item in instructions)
        {
            var inst = new Instructions
            {
                Direction = item
            };
            Node.instructions.Add(inst);
        }

        // AAA = (BBB, CCC)
        for (var i = 0; i < nodes.Count; i++)
        {
            Node node = new();
            var name = nodes[i].Split(" = ")[0];
            var firstElement = nodes[i].Split(" = ")[1].Split(", ")[0].Replace("(", "");
            var secondElement = nodes[i].Split(" = ")[1].Split(", ")[1].Replace(")", "");
            node.Name = name;
            node.Elements.Add(firstElement, secondElement);
            Node.nodes.Add(node);
        }

        return Node.Part1();
    }

    public static int Part2(string[] input)
    {
        var Node = new Nodes();
        var instructions = input[0].ToCharArray();
        var nodes = input[1..].ToList();

        // RL
        foreach (var item in instructions)
        {
            var inst = new Instructions
            {
                Direction = item
            };
            Node.instructions.Add(inst);
        }

        // AAA = (BBB, CCC)
        for (var i = 0; i < nodes.Count; i++)
        {
            Node node = new();
            var name = nodes[i].Split(" = ")[0];
            var firstElement = nodes[i].Split(" = ")[1].Split(", ")[0].Replace("(", "");
            var secondElement = nodes[i].Split(" = ")[1].Split(", ")[1].Replace(")", "");
            node.Name = name;
            node.Elements.Add(firstElement, secondElement);
            Node.nodes.Add(node);
        }

        return Node.Part2();
    }
}

public class Nodes()
{
    public List<Instructions> instructions = [];
    public List<Node> nodes = [];

    public int Part1()
    {
        string currentElement = "AAA";
        int instructionIndex = 0;
        int steps = 0;

        while (currentElement != "ZZZ")
        {
            Node currentNode = nodes.Find(n => n.Name == currentElement);
            if (currentNode == null)
            {
                throw new Exception($"Node {currentElement} not found");
            }

            char currentInstruction = instructions[instructionIndex].Direction;
            currentElement = currentInstruction == 'L' ? currentNode.Elements.Keys.First() : currentNode.Elements.Values.First();

            instructionIndex = (instructionIndex + 1) % instructions.Count;
            steps++;
        }

        return steps;
    }

    public int Part2()
    {
        var beginningNodes = nodes.Where(n => n.Name.EndsWith("A")).ToList();
        var endingNodes = nodes.Where(n => n.Name.EndsWith("Z")).ToList();

        var steps = 0;
        var currentElements = new ConcurrentQueue<string>(beginningNodes.Select(n => n.Name));

        while (currentElements.Any(e => !endingNodes.Any(n => n.Name == e)))
        {
            var newElements = new ConcurrentQueue<string>();
            Parallel.ForEach(currentElements, element =>
            {
                currentElements.TryDequeue(out element);
                var currentNode = nodes.Find(n => n.Name == element);
                if (currentNode == null)
                {
                    throw new Exception($"Node {element} not found");
                }

                var currentInstruction = instructions[steps % instructions.Count].Direction;
                var newElement = currentInstruction == 'L' ? currentNode.Elements.Keys.First() : currentNode.Elements.Values.First();
                newElements.Enqueue(newElement);
            });

            currentElements = newElements;
            steps++;
        }
        return steps;
    }






}

public class Node()
{
    public string Name { get; set; }
    public Dictionary<string, string> Elements { get; set; } = new();
}

public class Instructions()
{
    public char Direction { get; set; }
}

