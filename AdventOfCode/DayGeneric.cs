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

    public static char[][] ParseInputAsMap(string filename, char _)
    {
        string[] areaMapUnparsed = File.ReadAllLines(Client.filePrefix + filename);
        char[][] areaMap = areaMapUnparsed.Select(s => s.ToCharArray()).ToArray();
        return areaMap;
    }

    public static int[,] ParseInputAsMap(string filename, int _)
    {
        char[][] areaMapChars = ParseInputAsMap(filename, '_');
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

    public static int[,] PadMap(int[,] map, int padding)
    {
        int[,] paddedMap = new int[map.GetLength(0) + 2, map.GetLength(1) + 2];
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                paddedMap[i + 1, j + 1] = map[i, j];
            }
        }

        for (int i = 0; i < paddedMap.GetLength(0); i++)
        {
            for (int j = 0; j < paddedMap.GetLength(1); j++)
            {
                if (i == 0 || i == paddedMap.GetLength(0) - 1 || j == 0 || j == paddedMap.GetLength(1) - 1) paddedMap[i, j] = padding;
            }
        }
        
        return paddedMap;
    }
    
    // TODO: Fix this
    /*
    public static char[][] PadMap(char[][] map, char padding)
    {
        char[][] paddedMap = new char[map.Length + 2][];
        for (int i = 0; i < paddedMap.Length; i++)
        {
            char[] row = new char[paddedMap.Length];
            for (int j = 0; j < map[0].Length; j++)
            {
                if (j == 0 || j == paddedMap.Length - 1) row[j] = padding;
                else row[j] = map[i][j];
            }
            paddedMap[i] = row;
        }

        return paddedMap;
    }
    */
}