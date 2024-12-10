namespace AdventOfCode;

public class Day10
{
    public static string Name = "-- Day 10: Hoof It --";
    static List<(int row, int col)> queue = new(); 

    public static void Day10Main()
    {
        int[,] areaMap = DayGeneric.ParseInputAsIntMap("Day 10 - HoofIt.txt");
        int trailCount = CountTrails(areaMap, false);
        int totalDistance = CountTrails(areaMap, true);
        
        Console.WriteLine($"Calculated a total score of {trailCount}.");
        Console.WriteLine($"Calculated a total distance of {totalDistance}.");
    }

    public static int CountTrails(int[,] areaMap, bool calcDistance)
    {
        int trailCount = 0;
        List<(int row, int col)> trailheads = new();
        
        // First, find all 0s (trailheads)
        for (int row = 0; row < areaMap.GetLength(0); row++)
        {
            for (int col = 0; col < areaMap.GetLength(1); col++)
            {
                if (areaMap[row,col] == 0) trailheads.Add((row, col));
            }     
        }

        // Then for each of them, do a breadth-first search
        for (int i = 0; i < trailheads.Count; i++)
        {
            List<(int row, int col)> visited = new();
            queue.Add(trailheads[i]);
            int score = 0;
            while (queue.Count > 0)
            {
                (int row, int col) = queue[0];
                if (areaMap[row, col] == 9)
                {
                    // If we're calculating score, we need to make sure we're only counting unique 9s
                    if (calcDistance || !visited.Contains((row, col))) score++;
                    visited.Add((row, col));
                }
                else
                {
                    if (row - 1 >= 0 && areaMap[row - 1, col] == areaMap[row, col] + 1) queue.Add((row - 1, col));
                    if (row + 1 <= areaMap.GetLength(0) - 1 && areaMap[row + 1, col] == areaMap[row, col] + 1)
                        queue.Add((row + 1, col));
                    if (col - 1 >= 0 && areaMap[row, col - 1] == areaMap[row, col] + 1)
                        queue.Add((row, col - 1));
                    if (col + 1 <= areaMap.GetLength(1) - 1 && areaMap[row, col + 1] == areaMap[row, col] + 1)
                        queue.Add((row, col + 1));
                }

                queue.Remove((row, col));
            }

            trailCount += score;
        }

        return trailCount;
    }
}