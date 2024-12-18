namespace AdventOfCode;

public class Day18 : DayGeneric
{
    public static string Name = "-- Day 18: RAM Run --";

    public static char[,] areaMap;

    public static void Day18Main()
    {
        // Part 1
        string[] allBlocks = File.ReadAllLines(Client.filePrefix + "Day 18 - RAMRun.txt");
        int max = 1024;
        List<(int row, int col)> blocks = GetFirstBlocks(allBlocks, max);
        SetupMaze(blocks, 71);
        int shortestPath = SolveMaze();
        Console.WriteLine($"Found a way out in {shortestPath} steps minimum.");
        
        // Part 2
        max++;
        while (shortestPath != -1)
        {
            CleanMaze();
            AddBlock(allBlocks, max);
            shortestPath = SolveMaze();
            max++;
        }
        
        int[] lastBlock = allBlocks[max - 1].Split(',').Select(int.Parse).ToArray();
        Console.WriteLine($"It becomes impossible to escape after the block at index {max - 1} with position ({lastBlock[0]}, {lastBlock[1]}) is placed.");
    }
    
    public static List<(int, int)> GetFirstBlocks(string[] input, int max)
    {
        List<(int, int)> blocks = new List<(int, int)>();
        for (int i = 0; i < max; i++)
        {
            int[] block = input[i].Split(',').Select(int.Parse).ToArray();
            blocks.Add((block[0], block[1]));
        }
        
        return blocks;
    }

    public static void AddBlock(string[] lines, int index)
    {
        int[] block = lines[index].Split(',').Select(int.Parse).ToArray();
        areaMap[block[0], block[1]] = '#';
    }
    
    public static void SetupMaze(List<(int, int)> blocks, int dim)
    {
        areaMap = new char[dim, dim];
        for (int i = 0; i < areaMap.GetLength(0); i++)
        {
            for (int j = 0; j < areaMap.GetLength(1); j++)
            {
                if (blocks.Contains((i, j))) areaMap[i, j] = '#';
                else areaMap[i, j] = '.';
            }
        }
    }
    
    public static int SolveMaze()
    {
        Queue<(int row, int col, List<(int, int)> parents)> queue = new();
        
        queue.Enqueue((0, 0, new List<(int, int)>()));
        while (queue.Count > 0)
        {
            (int row, int col, List<(int, int)> parents) = queue.Dequeue();

            if (row == areaMap.GetLength(0) - 1 && col == areaMap.GetLength(1) - 1) return parents.Count;

            foreach (var (dRow, dCol) in GetAdjacent())
            {
                if (CheckBounds(row + dRow, col + dCol) && areaMap[row + dRow, col + dCol] == '.')
                {
                    List<(int row, int col)> updatedParents = new(parents) { (row, col) };
                    areaMap[row + dRow, col + dCol] = 'X';
                    queue.Enqueue((row + dRow, col + dCol, updatedParents));
                }
            }
        }

        return -1;
    }

    public static void CleanMaze()
    {
        for (int i = 0; i < areaMap.GetLength(0); i++)
        {
            for (int j = 0; j < areaMap.GetLength(1); j++)
            {
                if (areaMap[i, j] == 'X') areaMap[i, j] = '.';
            }
        }
    }
    
    public static IEnumerable<(int dRow, int dCol)> GetAdjacent()
    {
        yield return (1, 0);
        yield return (0, 1);
        yield return (-1, 0);
        yield return (0, -1);
    }

    public static bool CheckBounds(int row, int col)
    {
      return (row >= 0 && row < areaMap.GetLength(0) && col >= 0 && col < areaMap.GetLength(1));  
    }
}