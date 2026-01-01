namespace AdventOfCode;

public class Day17 : DayGeneric
{
    public static string Name = "-- Day 17: Chronospatial Computer -- ";

    public static long RegisterA;
    public static long RegisterB;
    public static long RegisterC;
    
    public static List<int> instructions = new List<int>();

    public static Dictionary<int, long> ComboOperands = new Dictionary<int, long>();
    
    public static void Day17Main()
    {
        string pt1Output = Write(Solve(-1));
        Console.WriteLine($"The computer output {pt1Output} for the default A-Value.");


        List<long> dupes = FindDupeButDFS(0, 1);

        Console.WriteLine($"The computer will match its input for an A-Value of {dupes.Min()}, generating {Write(Solve(dupes.Min()))}.");

    }

    public static List<long> FindDupeButDFS(long AValue, int digToMatch)
    {
        List<long> workingAValues = new List<long>();
        long testAValue = AValue << 3;
        for (int offset = 0; offset < 8; offset++)
        {
            if (Solve(testAValue + offset).SequenceEqual(instructions.TakeLast(digToMatch)))
            {
                if (digToMatch == instructions.Count) workingAValues.Add(testAValue + offset);
                else workingAValues.AddRange(FindDupeButDFS(testAValue + offset, digToMatch + 1));
            }
        }
        
        return workingAValues;
    }
    
    public static List<int> Solve(long aValue)
    {
        ParseInput("Day 17 - ChronospatialComputer.txt", aValue);
        return Operate();
    }

    public static string Write(List<int> output)
    {
        string outputStr = "";
        foreach (int o in output)
        {
            outputStr += o + ",";
        }
        // The above loop adds an extra comma which I am too lazy to properly fix
        return outputStr.Trim(',');
    }
    
    public static void ParseInput(string filename, long aValue = -1)
    {
        string[] input = File.ReadAllLines(Client.filePrefix + filename);
        RegisterA = (aValue == -1) ? int.Parse(input[0].Substring(input[0].IndexOf(':') + 2)) : aValue;
        RegisterB = int.Parse(input[1].Substring(input[1].IndexOf(':') + 2));
        RegisterC = int.Parse(input[2].Substring(input[2].IndexOf(':') + 2));
        
        instructions = input[4].Substring(input[4].IndexOf(':') + 2).Split(',').Select(int.Parse).ToList();

        for (int i = 0; i < 4; i++) ComboOperands[i] = i;
        UpdateComboOperands();
    }
    
    public static List<int> Operate()
    {
        int pointer = 0;
        List<int> output = new List<int>();

        while (pointer < instructions.Count - 1)
        {
            int opcode = instructions[pointer];
            int operand = instructions[pointer + 1];
            switch (opcode)
            {
                case 0:
                    RegisterA = (long)(RegisterA / Math.Pow(2, ComboOperands[operand]));
                    pointer += 2;
                    break;
                case 1:
                    RegisterB ^= operand;
                    pointer += 2;
                    break;
                case 2:
                    RegisterB = ComboOperands[operand] % 8;
                    pointer += 2;
                    break;
                case 3:
                    if (RegisterA == 0) pointer += 2;
                    else pointer = operand;
                    break;
                case 4:
                    RegisterB ^= RegisterC;
                    pointer += 2;
                    break;
                case 5:
                    output.Add((int)(ComboOperands[operand] % 8));
                    pointer += 2;
                    break;
                case 6:
                    RegisterB = (long)(RegisterA / Math.Pow(2, ComboOperands[operand]));
                    pointer += 2;
                    break;
                case 7:
                    RegisterC = (long)(RegisterA / Math.Pow(2, ComboOperands[operand]));
                    pointer += 2;
                    break;
            }

            UpdateComboOperands();
        }

        return output;
    }

    public static void UpdateComboOperands()
    {
        ComboOperands[4] = RegisterA;
        ComboOperands[5] = RegisterB;
        ComboOperands[6] = RegisterC;
    }
}