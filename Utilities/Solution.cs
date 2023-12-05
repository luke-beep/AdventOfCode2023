namespace AdventOfCode2023.Utilities;

public class Solution : Attribute
{
    public int Day
    {
        get;
    }

    public Solution(int day)
    {
        Day = day;
    }
}