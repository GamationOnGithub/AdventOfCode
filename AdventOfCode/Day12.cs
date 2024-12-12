using System.Collections.Immutable;
using System.Data.Common;

namespace AdventOfCode;

public class Day12 : DayGeneric
{
    public static string Name = "-- Day 12: Garden Groups --";
    
    public static List<(int row, int col)> filledPoints = new();
    
    // Lord forgive me
    public static List<(int row, int col)> sidesUp = new();
    public static List<(int row, int col)> sidesDown = new();
    public static List<(int row, int col)> sidesRight = new();
    public static List<(int row, int col)> sidesLeft = new();
    
    static char[][] areaMap = ParseInputAsMap("Day 12 - GardenGroups.txt", '_');
    
    public static void Day12Main()
    {
        areaMap = FloodFill();
        Dictionary<char, List<(int row, int col)>> regions = GetRegions();
        (int price, int pt2Price) = CalculatePrice(regions);
        
        Console.WriteLine($"Calculated a total price of {price}.");
        Console.WriteLine($"Calculated a total price of {pt2Price} with side discounts.");
    }

    public static char[][] FloodFill()
    {
        char id = 'a';
        
        for (int row = 0; row < areaMap.Length; row++)
        {
            for (int col = 0; col < areaMap[row].Length; col++)
            {
                if (!filledPoints.Contains((row, col)) && areaMap[row][col] != id)
                {
                    areaMap = Fill(row, col, id);
                    id++;
                }
            }
        }

        return areaMap;
    }

    public static char[][] Fill(int startingRow, int startingCol, char fill)
    {
        Queue<(int row, int col)> queue = new();
        char initial = areaMap[startingRow][startingCol];
        queue.Enqueue((startingRow, startingCol));

        while (queue.Count > 0)
        {
            (int row, int col) = queue.Dequeue();
            if (!filledPoints.Contains((row, col)))
            {
                areaMap[row][col] = fill;
                filledPoints.Add((row, col));
                if (row > 0 && areaMap[row - 1][col] == initial) queue.Enqueue((row - 1, col));
                if (row < areaMap.Length - 1 && areaMap[row + 1][col] == initial) queue.Enqueue((row + 1, col));
                if (col > 0 && areaMap[row][col - 1] == initial) queue.Enqueue((row, col - 1));
                if (col < areaMap[0].Length - 1 && areaMap[row][col + 1] == initial) queue.Enqueue((row, col + 1));
            }
        }

        return areaMap;
    }

    public static Dictionary<char, List<(int row, int col)>> GetRegions()
    {
        Dictionary<char, List<(int row, int col)>> regions = new();
        for (int i = 0; i < areaMap.Length; i++)
        {
            for (int j = 0; j < areaMap[i].Length; j++)
            {
                if (!regions.ContainsKey(areaMap[i][j]))
                {
                    regions.Add(areaMap[i][j], new List<(int row, int col)>());
                }
                regions[areaMap[i][j]].Add((i, j));
            }
        }
        
        return regions;
    }

    public static (int, int) CalculatePrice(Dictionary<char, List<(int row, int col)>> regions)
    {
        int price = 0;
        int altPrice = 0;
        foreach (char c in regions.Keys)
        {
            int area = 0;
            area += regions[c].Count;

            int perimeter = 0;
            int sides = 0;
            sidesUp.Clear();
            sidesDown.Clear();
            sidesLeft.Clear();
            sidesRight.Clear();
            for (int i = 0; i < regions[c].Count; i++)
            {
                perimeter += CalculatePerimeter(regions[c][i].row, regions[c][i].col);
            }

            CalculateSides();
            sides = sidesUp.Count + sidesDown.Count + sidesRight.Count + sidesLeft.Count;
            
            price += area * perimeter;
            altPrice += area * sides;
        }
        
        return (price, altPrice);
    }

    public static int CalculatePerimeter(int row, int col)
    {
        int perimeter = 0;
        int sides = 0;
        if (row == 0 || areaMap[row - 1][col] != areaMap[row][col])
        {
            perimeter++;
            sidesUp.Add((row, col));
        }
        if (row == areaMap.Length - 1 || areaMap[row + 1][col] != areaMap[row][col]) 
        {
            perimeter++;
            sidesDown.Add((row, col));
        }

        if (col == 0 || areaMap[row][col - 1] != areaMap[row][col]) 
        {
            perimeter++;
            sidesLeft.Add((row, col));
        }
        if (col == areaMap[0].Length - 1 || areaMap[row][col + 1] != areaMap[row][col])
        {
            perimeter++;
            sidesRight.Add((row, col));
        }
        
        return perimeter;
    }

    public static void CalculateSides()
    {
        // I hate it here.
        
        // Up
        for (int i = 0; i < sidesUp.Count; i++)
        {
            (int row, int col) = sidesUp[i];
            int index = 1;
            while (sidesUp.Contains((row, col + index)))
            {
                sidesUp.Remove((row, col + index));
                index++;
            }
        }
        
        // Down
        for (int i = 0; i < sidesDown.Count; i++)
        {
            (int row, int col) = sidesDown[i];
            int index = 1;
            while (sidesDown.Contains((row, col + index)))
            {
                sidesDown.Remove((row, col + index));
                index++;
            }
        }

        // Right
        for (int i = 0; i < sidesRight.Count; i++)
        {
            (int row, int col) = sidesRight[i];
            int index = 1;
            while (sidesRight.Contains((row + index, col)))
            {
                sidesRight.Remove((row + index, col));
                index++;
            }
        }
        
        // Left
        for (int i = 0; i < sidesLeft.Count; i++)
        {
            (int row, int col) = sidesLeft[i];
            int index = 1;
            while (sidesLeft.Contains((row + index, col)))
            {
                sidesLeft.Remove((row + index, col));
                index++;
            }
        }
    }
}