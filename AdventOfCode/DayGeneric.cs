namespace AdventOfCode;

public class DayGeneric
{
    public static string Name = "-- Day 0: The Generic Day --";

    public enum Directions { Up, Down, Left, Right, None }
    
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
    
    // -- MATH --

    public static double[,] RowReduce(double[,] matrix, float tolerance)
    {
        int pivotRow = 0;
        int pivotCol = 0;

        // Row reduction algorithm adapted from wikipedia
        while (pivotRow < matrix.GetLength(0) - 1 && pivotCol < matrix.GetLength(1) - 1)
        {
            // Find the next pivot
            int largestIndex = 0;
            for (int i = pivotRow; i < matrix.GetLength(0); i++)
            {
                if (matrix[i, pivotCol] > matrix[largestIndex, pivotCol])
                {
                    largestIndex = i;
                }
            }
            
            // If true, this is a column of all zeros and thus not a pivot
            if (matrix[largestIndex, pivotCol] == 0) pivotCol++;
            else
            {
                for (int i = pivotRow + 1; i < matrix.GetLength(0); i++)
                {
                    // Calculate the scaling factor required to cancel out the row below
                    double scalar = matrix[i, pivotCol] / matrix[pivotRow, pivotCol];
                    // Very slightly more efficient to set this to 0 rather than doing math
                    matrix[i, pivotCol] = 0;
                    // Add the correctly scaled value to the other columns
                    for (int j = pivotCol + 1; j < matrix.GetLength(1); j++)
                    {
                        matrix[i, j] -= matrix[pivotRow, j] * scalar;
                    }
                }
            }
        }
        
        // Now correct for float imprecision
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                if (Math.Abs(matrix[i, j] - Math.Floor(matrix[i, j])) < tolerance) matrix[i, j] = Math.Floor(matrix[i, j]);
                else if (Math.Abs(Math.Ceiling(matrix[i, j]) - matrix[i, j]) < tolerance) matrix[i, j] = Math.Ceiling(matrix[i, j]);
            }
        }
        
        return matrix;
    }

    public static double[,] rref(double[,] matrix, float tolerance)
    {
        matrix = RowReduce(matrix, tolerance);
        
        int pivotRow = 0;
        int pivotCol = 0;
        
        // Finding the final pivot is a little trickier
        // After row reduction it must be in either the last row or last column depending on the dimensions of the matrix
        // First the easy square case:
        if (matrix.GetLength(0) == matrix.GetLength(1))
        {
            pivotRow = matrix.GetLength(0) - 1;
            pivotCol = matrix.GetLength(1) - 1;
        }
        // Then, if we have more rows than columns, it must be in the final column
        if (matrix.GetLength(0) > matrix.GetLength(1))
        {
            pivotCol = matrix.GetLength(1) - 1;
            pivotRow = matrix.GetLength(1) - 1;
        }
        // And if we have more columns than rows, it must be in the final row
        if (matrix.GetLength(0) < matrix.GetLength(1))
        {
            pivotRow = matrix.GetLength(0) - 1;
            pivotCol = matrix.GetLength(1) - 1;
        }
        
        // TODO: finish this
        return matrix;
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