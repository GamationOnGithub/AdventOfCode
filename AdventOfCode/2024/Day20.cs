namespace AdventOfCode;

public class Day20 : DayGeneric
{
    public static string Name = "-- Day 20: Day 20 -- ";

    public static char[,] mazeMap;
    public static Dictionary<(int row, int col), int> startDistance = new();
    public static Dictionary<(int row, int col), int> endDistance = new();
    
    public static void Day20Main()
    {
        // Setup
        mazeMap = ParseInput("Day 20 - RaceCondition.txt");
        (int startRow, int startCol, int endRow, int endCol) = FindEndpoints();
        BFS(startRow, startCol, startDistance);
        BFS(endRow, endCol, endDistance);
        
        // Part 1
        int validCheats = 0;
        foreach (var (row, col) in startDistance.Keys)
        {
            validCheats += CheckCheats(row, col, startRow, startCol, 2);
        }
        Console.WriteLine($"Found {validCheats} 100-nanosecond-saving ways to cheat.");
        
        // Part 2
        int altValidCheats = 0;
        foreach (var (row, col) in startDistance.Keys)
        {
            altValidCheats += CheckCheats(row, col, startRow, startCol, 20);
        }
        Console.WriteLine($"Found {altValidCheats} 100-nanosecond-saving ways to cheat with the new rules.");
    }

    public static char[,] ParseInput(string filename)
    {
        string[] input = File.ReadAllLines(Client.filePrefix + filename);
        char[,] mazeMap = new char[input.Length, input[0].Length];
        for (int i = 0; i < input.Length; i++)
        {
            for (int j = 0; j < input[i].Length; j++) mazeMap[i, j] = input[i][j];
        }

        return mazeMap;
    }

    // There has to be a faster way to do this with linq, but I refuse to use that out of principle
    public static (int sRow, int sCol, int eRow, int eCol) FindEndpoints()
    {
        int sRow = -1, sCol = -1, eRow = -1, eCol = -1;
        for (int row = 0; row < mazeMap.GetLength(0); row++)
        {
            for (int col = 0; col < mazeMap.GetLength(1); col++)
            {
                if (mazeMap[row, col] == 'S')
                {
                    sRow = row;
                    sCol = col;
                } else if (mazeMap[row, col] == 'E')
                {
                    eRow = row;
                    eCol = col;
                }
            }
        }
        
        return (sRow, sCol, eRow, eCol);
    }
    
    public static void BFS(int startRow, int startCol, Dictionary<(int row, int col), int> toUpdate)
    {
        // I'm getting tired of writing and commenting BFSes
        Queue<(int row, int col, int distance)> queue = new();
        queue.Enqueue((startRow, startCol, 0));

        while (queue.Count > 0)
        {
            (int row, int col, int distance) = queue.Dequeue();
            
            // This check is useless because it's meant to handle branching paths in the maze
            // ...which don't happen in the actual input
            if (toUpdate.ContainsKey((row, col)))
            {
                if (toUpdate[(row, col)] > distance) toUpdate[(row, col)] = distance;
                else continue;
            }
            
            toUpdate[(row, col)] = distance;

            // god i love GetAdjacent why didn't i start using this sooner
            foreach (var (dRow, dCol) in GetAdjacent())
            {
                if (mazeMap[row + dRow, col + dCol] != '#') queue.Enqueue((row + dRow, col + dCol, distance + 1));
            }
        }
    }

    public static int CheckCheats(int row, int col, int startRow, int startCol, int radius)
    {
        int validCheats = 0;
        
        for (int dRow = -radius; dRow <= radius; dRow++)
        {
            for (int dCol = -radius; dCol <= radius; dCol++)
            {
                int manhattanDistance = Math.Abs(dRow) + Math.Abs(dCol);
                // This isn't a valid point to check if true
                if (manhattanDistance > radius) continue;

                // This point is inside a barrier if true, and thus useless
                if (!CheckBounds(row + dRow, col + dCol) || mazeMap[row + dRow, col + dCol] == '#') continue;
                
                // If the sum of (the distance from the start, the distance of the point post-cheat from the end,
                // and the distance of the cheat) is 100 less than the original distance, we did it
                int cheatedDistance = startDistance[(row, col)] + endDistance[(row + dRow, col + dCol)] + manhattanDistance;
                if (cheatedDistance + 100 <= endDistance[(startRow, startCol)]) validCheats++;
            }
        }

        return validCheats;
    }
    
    public static bool CheckBounds(int row, int col)
    {
        return row >= 0 && row < mazeMap.GetLength(0) && col >= 0 && col < mazeMap.GetLength(1);
    }
}