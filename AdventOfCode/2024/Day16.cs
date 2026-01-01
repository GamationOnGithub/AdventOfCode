using System.Collections;

namespace AdventOfCode;

public class Day16 : DayGeneric
{
    public static string Name = "-- Day 16: Reindeer Maze -- ";
    
    public static void Day16Main()
    {
        char[,] map = ParseInput("Day 16 - ReindeerMaze.txt");
        var ((startRow, startCol), (endRow, endCol)) = FindTargets(map);
        (int score, int uniquePoints) = Dijkstra(map, startRow, startCol, endRow, endCol);
        Console.WriteLine($"Solved the reindeer race with a lowest score of {score}.");
        Console.WriteLine($"{uniquePoints} unique points lie upon the fastest routes.");
    }

    public static char[,] ParseInput(string filename)
    {
        string[] input = File.ReadAllLines(Client.filePrefix + filename);
        int gridWidth = input[0].Length;
        int gridHeight = input.Length;
        
        char[,] map = new char[gridHeight, gridWidth];

        for (int i = 0; i < input.Length; i++)
        {
            if (i < gridHeight)
            {
                for (int j = 0; j < input[i].Length; j++)
                {
                    map[i, j] = input[i][j];
                }
            }
        }
        
        return map;
    }
    
    public static ((int, int), (int, int)) FindTargets(char[,] map)
    {
        var (startRow, startCol) = (0, 0);
        var (endRow, endCol) = (0, 0);

        // Find the start and end
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                if (map[i, j] == 'S') (startRow, startCol) = (i, j);
                else if (map[i, j] == 'E') (endRow, endCol) = (i, j);
            }
        }

        return ((startRow, startCol), (endRow, endCol));
    }

    public static (int, int) Dijkstra(char[,] map, int startRow, int startCol, int endRow, int endCol)
    {
        // Welcome to tuple hell
        PriorityQueue<(int dist, (int row, int col) pos, Directions dir, List<(int row, int col)> path), int> queue = new();
        Dictionary<((int row, int col) pos, Directions dir), int> seen = new();
        HashSet<(int row, int col)> optimalPoints = new HashSet<(int row, int col)>();
        
        seen[((startRow, startCol), Directions.Right)] = 0;
        queue.Enqueue((0, (startRow, startCol), Directions.Right, new List<(int row, int col)>() { (startRow, startCol) }), 0);

        while (queue.Count > 0)
        {
            var (dist, (row, col), dir, path) = queue.Dequeue();
            if (row == endRow && col == endCol)
            {
                // Okay, stop, we're at the end
                if (!seen.TryGetValue(((endRow, endCol), Directions.None), out int prevDist))
                {
                    seen[((endRow, endCol), Directions.None)] = dist;
                }
                else
                {
                    if (dist < prevDist) seen[((endRow, endCol), Directions.None)] = dist;
                }
                
                foreach (var (pRow, pCol) in path)
                {
                    optimalPoints.Add((pRow, pCol));
                }

                continue;
            }

            if (seen.GetValueOrDefault(((row, col), dir), int.MaxValue) < dist) continue;
            seen[((row, col), dir)] = dist;
            
            // Check up
            if (map[row - 1, col] != '#')
            {
                int weight = (dir == Directions.Up ? 1 : 1001);
                List<(int, int)> newPath = new(path);
                newPath.Add((row - 1, col));
                queue.Enqueue((dist + weight, (row - 1, col), Directions.Up, newPath), dist + weight);
            }
            
            // Check down
            if (map[row + 1, col] != '#')
            {
                int weight = (dir == Directions.Down ? 1 : 1001);
                List<(int, int)> newPath = new(path);
                newPath.Add((row + 1, col));
                queue.Enqueue((dist + weight, (row + 1, col), Directions.Down, newPath), dist + weight);
            }
            
            // Check right
            if (map[row, col + 1] != '#')
            {
                int weight = (dir == Directions.Right ? 1 : 1001);
                List<(int, int)> newPath = new(path);
                newPath.Add((row, col + 1));
                queue.Enqueue((dist + weight, (row, col + 1), Directions.Right, newPath), dist + weight);
            }
            
            // Check left
            if (map[row, col - 1] != '#')
            {
                int weight = (dir == Directions.Left ? 1 : 1001);
                List<(int, int)> newPath = new(path);
                newPath.Add((row, col - 1));
                queue.Enqueue((dist + weight, (row, col - 1), Directions.Left, newPath), dist + weight);
            }
        }
        
        return (seen[((endRow, endCol), Directions.None)], optimalPoints.Count);
    }
    
    public static void PrintGrid(char[,] map)
    {
        string[] output = new string[map.GetLength(0)];
        for (int i = 0; i < map.GetLength(0); i++)
        {
            string line = "";
            for (int j = 0; j < map.GetLength(1); j++)
            {
                line += map[i, j].ToString();
            }
            output[i] = line;
        }
        
        File.WriteAllLines(Client.filePrefix + "output.txt", output);
    }
}