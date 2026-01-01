namespace AdventOfCode;

public class Day13 : DayGeneric
{
    public static string Name = "-- Day 13: Claw Contraption -- ";

    public static void Day13Main()
    {
        List<double[,]> matrices = new();
        
        StreamReader reader = new StreamReader(Client.filePrefix + "Day 13 - ClawContraption.txt");
        while (!reader.EndOfStream)
        {
            string[] input = new string[] {reader.ReadLine(), reader.ReadLine(), reader.ReadLine()};
            matrices.Add(ToMatrix(input));
            if (!reader.EndOfStream) reader.ReadLine();
        }

        List<double[,]> matricesPt1 = new();
        foreach (double[,] m in matrices)
        {
            matricesPt1.Add(m.Clone() as double[,]);
        }
        ulong totalCost = CalculateCost(matricesPt1, 0.0001f);
        
        Console.WriteLine($"Total cost to get all prizes: {totalCost}");
        
        foreach (double[,] m in matrices)
        {
            m[0, 2] = m[0, 2] + 10000000000000;
            m[1, 2] += 10000000000000;
        }

        totalCost = CalculateCost(matrices, 0.01f);
        Console.WriteLine($"Total cost to get all prizes with new coordinates: {totalCost}");


    }

    public static double[,] ToMatrix(string[] input)
    {
        double[,] matrix = new double[2, 3];
        for (int i = 0; i < input.Length; i++)
        {
            string[] col = input[i].Substring(input[i].IndexOf('X')).Split(", ");
            matrix[0, i] = float.Parse(col[0].Substring(2));
            matrix[1, i] = float.Parse(col[1].Substring(2));
        }

        return matrix;
    }

    public static double[,] RowReduce(double[,] matrix)
    {
        // Very hardcoded Gauss-Jordan elimination for our use case.
        double scalar1 = matrix[1, 0] / matrix[0, 0];
        for (int i = 0; i < matrix.GetLength(1); i++)
        {
            matrix[1, i] -= matrix[0, i] * scalar1;
        }

        double scalar2 = matrix[0, 1] / matrix[1, 1];
        for (int i = 0; i < matrix.GetLength(1); i++)
        {
            matrix[0, i] -= matrix[1, i] * scalar2;
        }

        double row1NormalScalar = 1f / matrix[0, 0];
        double row2NormalScalar = 1f / matrix[1, 1];
        for (int i = 0; i < matrix.GetLength(1); i++)
        {
            matrix[0, i] = matrix[0, i] * row1NormalScalar;
            matrix[1, i] = matrix[1, i] * row2NormalScalar;
        }
        
        return matrix;
    }
    
    public static (long, long) CheckValidity(double[,] matrix, float tolerance)
    {
        long aPresses = -1;
        long bPresses = -1;
        
        if (Math.Abs(matrix[0, 2] - Math.Floor(matrix[0, 2])) < tolerance) aPresses = (long)Math.Floor(matrix[0, 2]);
        else if (Math.Abs(Math.Ceiling(matrix[0, 2]) - matrix[0,2]) < tolerance) aPresses = (long)Math.Ceiling(matrix[0,2]);
        
        if (Math.Abs(matrix[1, 2] - Math.Floor(matrix[1, 2])) < tolerance) bPresses = (long)Math.Floor(matrix[1, 2]);
        else if (Math.Abs(Math.Ceiling(matrix[1, 2]) - matrix[1,2]) < tolerance) bPresses = (long)Math.Ceiling(matrix[1,2]);
        
        return (aPresses, bPresses);
    }

    public static ulong CalculateCost(List<double[,]> matrices, float tolerance)
    {
        ulong totalCost = 0;
        for (int i = 0; i < matrices.Count; i++)
        {
            matrices[i] = RowReduce(matrices[i]);
            var (aPress, bPress) = CheckValidity(matrices[i], tolerance);
            if (aPress != -1 && bPress != -1)
            {
                totalCost += (ulong)((aPress * 3) + bPress);
            }
        }

        return totalCost;
    }
}