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
}