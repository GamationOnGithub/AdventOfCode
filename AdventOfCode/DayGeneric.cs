namespace AdventOfCode;

public class DayGeneric
{
    public static string Name = "-- Day 0: The Generic Day --";

    public static void DayMain()
    {
        Console.WriteLine("You're in the generic class. Try an actual day.");
    }
    
    public static void GenerateMapOutput(char[][] map, string filename = "output")
    {
        List<string> finishedMap = new();
        for (int i = 0; i < map.Length; i++)
        {
            string s = new string(map[i]);
            finishedMap.Add(s);
        }
        File.WriteAllLines($"{Client.filePrefix}{filename}.txt", finishedMap.ToArray());
    }

    public static char[][] ParseInputAsMap(string filename)
    {
        string[] areaMapUnparsed = File.ReadAllLines(Client.filePrefix + filename);
        char[][] areaMap = areaMapUnparsed.Select(s => s.ToCharArray()).ToArray();
        return areaMap;
    }
    
    public static int[,] ParseInputAsIntMap(string filename)
    {
        // TODO: This sucks. Make it better with generics later
        string[] areaMapUnparsed = File.ReadAllLines(Client.filePrefix + filename);
        char[][] areaMapChars = areaMapUnparsed.Select(s => s.ToCharArray()).ToArray();
        int[,] areaMap = new int[areaMapChars.Length, areaMapChars[0].Length];
        for (int i = 0; i < areaMapChars.Length; i++)
        {
            for (int j = 0; j < areaMapChars[i].Length; j++)
            {
                areaMap[i, j] = int.Parse(areaMapChars[i][j].ToString());
            }
        }

        return areaMap;
    }
}