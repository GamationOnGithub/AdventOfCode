namespace AdventOfCode;

public class Day19 : DayGeneric
{
    public static string Name = "-- Day 19: Linen Layout --";
    public static HashSet<string> towelSet = new();
    public static List<string> toMatch = new();
    public static Dictionary<string, long> seen = new();

    public static void Day19Main()
    {
        ParseInput("Day 19 - LinenLayout.txt");
        // For part 1
        int workingCount = 0;
        // For part 2
        long totalComboCount = 0;
        
        foreach (string towel in toMatch)
        {
            long comboCount = TowelDFS(towel);
            if (comboCount > 0)
            {
                workingCount++;
                totalComboCount += comboCount;
            }
        }
        
        // Part 1
        Console.WriteLine($"Found {workingCount} working towel patterns.");
        // Part 2
        Console.WriteLine($"Found {totalComboCount} ways to create working towels.");
    }

    public static void ParseInput(string filename)
    {
        string[] input = File.ReadAllLines(Client.filePrefix + filename);
        
        // Get the towels
        List<string> towels = new();
        int line = 0;
        while (input[line] != "")
        {
            towelSet.UnionWith(input[line].Split(", ").ToHashSet());
            line++;
        }
        towelSet.RemoveWhere(x => x == "" || x == ", ");
        
        // Get all patterns to construct
        // Move past the newline first though
        line++;
        for (int i = line; i < input.Length; i++)
        {
            toMatch.Add(input[i]);
        }
    }

    public static long TowelDFS(string toMatch)
    {
        if (toMatch.Length == 0) return 1;
        // Go go gadget memoization
        if (seen.ContainsKey(toMatch)) return seen[toMatch];

        long result = 0;
        foreach (string towel in towelSet)
        {
            if (toMatch.StartsWith(towel)) result += TowelDFS(toMatch.Substring(towel.Length));
        }
        
        seen[toMatch] = result;
        return result;
    }
}