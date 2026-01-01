using System.Text.RegularExpressions;

namespace AdventOfCode;

public class Day14 : DayGeneric
{
    internal class Robot
    {
        public int row;
        public int col;
        public int dRow;
        public int dCol;

        public Robot(int startingCol, int startingRow, int dCol, int dRow)
        {
            (this.row, this.col) = (startingRow, startingCol);
            (this.dRow, this.dCol) = (dRow, dCol);
        }

        public Robot(int[] data) : this(data[0], data[1], data[2], data[3]) {}
        
        public void Update(int rowBound, int colBound)
        {
            row += dRow;
            col += dCol;
            if (row < 0) row += rowBound;
            if (row > rowBound - 1) row -= rowBound;
            if (col < 0) col += colBound;
            if (col > colBound - 1) col -= colBound;
        }
        
        public (int row, int col) GetPosition() => (this.row, this.col);
    }

    public static string Name = "-- Day 14: Restroom Redoubt --";
    static List<Robot> robots = new List<Robot>();

    public static void Day14Main()
    {
        ParseInput();
        for (int i = 0; i < 6979; i++)
        {
            foreach (Robot robot in robots)
            {
                robot.Update(103,101);
            }

            if (i > 6874 && i < 6979) PrintOutput(i, 103, 101);
        }

        int safety = CalcSafety(103, 101);
        Console.WriteLine($"Calculated a safety score of {safety}.");
    }

    public static void ParseInput()
    {
        string[] input = File.ReadAllLines(Client.filePrefix + "Day 14 - RestroomRedoubt.txt");
        Regex numberRegex = new Regex("-?(\\d)+");
        
        for (int i = 0; i < input.Length; i++)
        {
            int[] data = numberRegex.Matches(input[i]).Select(x => int.Parse(x.Value)).ToArray();
            robots.Add(new Robot(data));
        }
    }

    public static int CalcSafety(int rowBound, int colBound)
    {
        int q1Count = 0, q2Count = 0, q3Count = 0, q4Count = 0;
        foreach (Robot robot in robots)
        {
            var (row, col) = robot.GetPosition();
            if (row < rowBound / 2 && col < colBound / 2) q1Count++;
            if (row < rowBound / 2 && col > colBound / 2) q2Count++;
            if (row > rowBound / 2 && col < colBound / 2) q3Count++;
            if (row > rowBound / 2 && col > colBound / 2) q4Count++;
        }
        
        return q1Count * q2Count * q3Count * q4Count;
    }
    
    public static void PrintOutput(int index, int rowBound, int colBound)
    {
        char[,] outputArray = new char[rowBound, colBound];
        
        // Initialize as all 0s
        for (int i = 0; i < outputArray.GetLength(0); i++)
        {
            for (int j = 0; j < outputArray.GetLength(1); j++)
            {
                outputArray[i, j] = '.';
            }
        }

        foreach (Robot robot in robots)
        {
            var (row, col) = robot.GetPosition();
            outputArray[row, col] = '#';
        }
        
        string[] outputArrayStrings = new string[outputArray.GetLength(0)];
        for (int i = 0; i < outputArrayStrings.GetLength(0); i++)
        {
            for (int j = 0; j < outputArray.GetLength(1); j++)
            {
                outputArrayStrings[i] += outputArray[i, j].ToString();
            }
        }

        // Empirically, weird behavior occurs at index % 103 == 77.
        // Using this observation, I found an instance of a christmas tree at index 6876, and from there built a regex to identify other ones
        // (It just checks for 30 #s in a row)
        Regex christmasRegex = new Regex("\\#{30}");
        foreach (string str in outputArrayStrings)
        {
            MatchCollection matches = christmasRegex.Matches(str);
            if (matches.Count > 0) Console.WriteLine($"Found a likely christmas tree at index {index}.");
        }
        
        File.WriteAllLines(Client.filePrefix + "output.txt", outputArrayStrings);
    }
}