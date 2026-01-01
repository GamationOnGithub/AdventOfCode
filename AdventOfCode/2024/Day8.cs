namespace AdventOfCode;

public class Day8 : DayGeneric
{
    public static string Name = "-- Day 8: Resonant Collinearity --";

    public static char[][] areaMap;

    public static void Day8Main()
    {
        areaMap = ParseInputAsMap("Day 8 - ResonantCollinearity.txt", '_');
        
        Dictionary<char, List<(int x, int y)>> frequencies = FindFrequencies();
        int uniqueAntinodes = PlaceAntinodes(frequencies, false);
        GenerateMapOutput(areaMap, "output1");
        int uniqueAntinodesOnLine = PlaceAntinodes(frequencies, true);
        GenerateMapOutput(areaMap, "output2");
        
        Console.WriteLine($"Found {uniqueAntinodes} unique antinode positions.");
        Console.WriteLine($"Found {uniqueAntinodesOnLine} unique antinode positions with respect to each full line.");
    }

    public static Dictionary<char, List<(int x, int y)>> FindFrequencies()
    {
        Dictionary<char, List<(int x, int y)>> frequencies = new Dictionary<char, List<(int x, int y)>>();
        for (int i = 0; i < areaMap.Length; i++)
        {
            for (int j = 0; j < areaMap[i].Length; j++)
            {
                if (areaMap[i][j] != '.')
                {
                    if (!frequencies.ContainsKey(areaMap[i][j])) frequencies.Add(areaMap[i][j], new List<(int x, int y)>());
                    frequencies[areaMap[i][j]].Add((i, j));
                }
            }
        }
        
        return frequencies;
    }

    public static int PlaceAntinodes(Dictionary<char, List<(int x, int y)>> frequencies, bool repeatOnLine)
    {
        List<(int x, int y)> antinodes = new List<(int x, int y)>();
        
        foreach (var (key, value) in frequencies)
        {
            for (int i = 0; i < value.Count; i++)
            {
                for (int j = i + 1; j < value.Count; j++)
                {
                    bool withinGrid = true;
                    int scale = (repeatOnLine) ? 0 : 1;
                    while (withinGrid)
                    {
                        int dx = (value[j].x - value[i].x) * scale;
                        int dy = (value[j].y - value[i].y) * scale;
                        int inBoundsCounter = 2;
                        if (CheckBounds(value[i].x - dx, value[i].y - dy))
                        {
                            if (!antinodes.Contains((value[i].x - dx, value[i].y - dy)))
                                antinodes.Add((value[i].x - dx, value[i].y - dy));
                            if (areaMap[value[i].x - dx][value[i].y - dy] == '.')
                                areaMap[value[i].x - dx][value[i].y - dy] = '#';
                        }
                        else inBoundsCounter--;

                        if (CheckBounds(value[j].x + dx, value[j].y + dy))
                        {
                            if (!antinodes.Contains((value[j].x + dx, value[j].y + dy)))
                                antinodes.Add((value[j].x + dx, value[j].y + dy));
                            if (areaMap[value[j].x + dx][value[j].y + dy] == '.')
                                areaMap[value[j].x + dx][value[j].y + dy] = '#';
                        } else inBoundsCounter--;

                        if (!repeatOnLine) withinGrid = false;
                        if (repeatOnLine && inBoundsCounter == 0) withinGrid = false;
                        scale++;
                    }
                }
            }
        }
        
        return antinodes.Count;
    }

    public static bool CheckBounds(int x, int y)
    {
        return x >= 0 && x < areaMap.Length && y >= 0 && y < areaMap[0].Length;
    }
}