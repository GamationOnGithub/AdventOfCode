namespace AdventOfCode;

public class Day24 : DayGeneric
{
    public static string Name = "-- Day 24: Crossed Wires --";
    
    public enum Operations { AND, OR, XOR }
    
    public static SortedDictionary<string, bool> wires = new();
    public static List<(string i1, string i2, string res, Operations op)> operations = new();

    public static void Day24Main()
    {
        ParseInput("Day 24 - CrossedWires.txt");
        Evaluate();
        long zCount = AssembleResult('z');
        Console.WriteLine($"The gates produce a value of {zCount}.");
        
        // Part 2
        List<string> toSwap = IdentifyBadZ(zCount);
        Console.WriteLine($"To make the adder function, swap {string.Join(",", toSwap)}.");
    }

    public static void ParseInput(string filename)
    {
        string[] input = File.ReadAllLines(Client.filePrefix + filename);
        int line = 0;
        while (input[line] != "") 
        {
            string[] parts = input[line].Split(": ");
            wires.Add(parts[0], Convert.ToBoolean(int.Parse(parts[1])));
            line++;
        }

        line++;
        while (line < input.Length)
        {
            string[] parts = input[line].Split();
            Operations op = (Operations)Enum.Parse(typeof(Operations), parts[1]);
            operations.Add((parts[0], parts[2], parts[4], op));
            line++;
        }
    }

    public static void Evaluate()
    {
        int pointer = 0;
        while (pointer < operations.Count)
        {
            var (i1, i2, res, op) = operations[pointer];
            if (wires.ContainsKey(i1) && wires.ContainsKey(i2))
            {
                operations.RemoveAt(pointer);
                if (!wires.ContainsKey(res)) wires.Add(res, false);
                switch (op)
                {
                    case Operations.AND:
                        wires[res] = wires[i1] && wires[i2];
                        break;
                    case Operations.OR:
                        wires[res] = wires[i1] || wires[i2];
                        break;
                    case Operations.XOR:
                        wires[res] = wires[i1] ^ wires[i2];
                        break;
                }

                pointer = 0;
            }
            else pointer++;
        }
    }

    public static long AssembleResult(char toBuild)
    {
        List<string> keys = wires.Keys.Where(x => x[0] == toBuild).ToList();
        keys.Sort();
        string result = "";
        foreach (string key in keys)
        {
            result = Convert.ToInt32(wires[key]) + result;
        }

        return Convert.ToInt64(result, 2);
    }

    public static List<string> IdentifyBadZ(long zCount)
    {
        long target = AssembleResult('x') + AssembleResult('y');
        string diff = Convert.ToString(target ^ zCount, 2);
        string realBase2 = Convert.ToString(zCount, 2);
        string targetBase2 = Convert.ToString(target, 2);
        
        while (diff.Length < targetBase2.Length) diff = "0" + diff;
        List<string> toView = new List<string>() {realBase2, targetBase2, diff};
        
        // Solved by hand using Notepad++ and a schematic of a full adder stolen from wikipedia. Results can be verified using the above diff
        List<string> wiresList = new() { "drg", "jbp", "jgc", "gvw", "qjb", "z35", "z15", "z22" };
        wiresList.Sort();
        return wiresList;
    }
}