namespace AdventOfCode;

public class Day25 : DayGeneric
{
    public static string Name = "-- Day 25: Code Chronicle --";
    public static List<char[,]> locks = new();
    public static List<char[,]> keys = new();

    public static void Day25Main()
    {
        ParseInput("Day 25 - CodeChronicle.txt");

        int workingLocks = 0;
        for (int i = 0; i < locks.Count; i++)
        {
            workingLocks += TestLock(i);
        }
        Console.WriteLine($"Found {workingLocks} lock-key combinations.");
        Console.WriteLine("Merry Christmas! See you in 2025!");
    }

    public static void ParseInput(string filename)
    {
        StreamReader sr = File.OpenText(Client.filePrefix + filename);
        while (!sr.EndOfStream)
        {
            string line = sr.ReadLine();
            if (line != "")
            {
                char[,] block = new char[7, 5];
                int row = 0;
                while (line != "" && line != "\n" && !sr.EndOfStream)
                {
                    for (int col = 0; col < line.Length; col++)
                    {
                        block[row, col] = line[col];
                    }
                    row++;
                    line = sr.ReadLine();
                }

                // I hate this. Merry christmas
                if (block[0, 0] == '#')
                {
                    if (sr.EndOfStream)
                    {
                        for (int col = 0; col < 5; col++)
                        {
                            block[block.GetLength(0) - 1, col] = '.';
                        }
                    }
                    locks.Add(block);
                }
                else
                {
                    if (sr.EndOfStream)
                    {
                        for (int col = 0; col < 5; col++)
                        {
                            block[block.GetLength(0) - 1, col] = '#';
                        }
                    }
                    keys.Add(block);
                }
            }
        }
        
        
    }

    public static int TestLock(int index)
    {
        int[] colSize = CountColumnHeight(locks[index]);
        int maxColSize = locks[index].GetLength(0);
        int workingKeys = 0;
        foreach (var key in keys)
        {
            bool broken = false;
            int[] keyCols = CountColumnHeight(key);
            for (int col = 0; col < keyCols.Length; col++)
            {
                if (colSize[col] + keyCols[col] > maxColSize)
                {
                    broken = true;
                    break;
                }
            }
            
            if (!broken) workingKeys++;
        }
        
        return workingKeys;
    }

    public static int[] CountColumnHeight(char[,] toCount)
    {
        int[] colSize = new int[5];
        for (int i = 0; i < toCount.GetLength(0); i++)
        {
            for (int j = 0; j < toCount.GetLength(1); j++)
            {
                if (toCount[i, j] == '#') colSize[j]++;
            }
        }

        return colSize;
    }
}