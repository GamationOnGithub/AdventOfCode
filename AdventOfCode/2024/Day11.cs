namespace AdventOfCode;

public class Day11 : DayGeneric
{
    public static string Name = "-- Day 11: Plutonian Pebbles --";

    public static void Day11Main()
    {
        string[] input = File.ReadAllLines(Client.filePrefix + "Day 11 - PlutonianPebbles.txt");
        //string[] input = ["125 17"];
        string[] inputsSplit = input[0].Trim().Split(' ');
        Dictionary<long, long> stones = new();
        for (int i = 0; i < inputsSplit.Length; i++)
        {
            stones.Add(long.Parse(inputsSplit[i]), 1);
        }
        
        for (int i = 0; i < 75; i++)
        {
            stones = Blink(stones);
            if (i == 24)
            {
                long stoneCount = 0;
                foreach (var kvp in stones)
                {
                    stoneCount += kvp.Value;
                } 
                Console.WriteLine($"Counted {stoneCount} stones after 25 blinks.");
            }
        }
        
        long stoneCountFinal = 0;
        foreach (var kvp in stones)
        {
            stoneCountFinal += kvp.Value;
        } 
        Console.WriteLine($"Counted {stoneCountFinal} stones after 75 blinks.");

    }

    public static Dictionary<long, long> Blink(Dictionary<long, long> stones)
    {
        Dictionary<long, long> result = new Dictionary<long, long>();
        
        foreach (var kvp in stones)
        {
            long[] stoneIteration = Blink(kvp.Key);
            foreach (long newStone in stoneIteration)
            {
                if (result.ContainsKey(newStone)) result[newStone] += kvp.Value;
                else result.Add(newStone, kvp.Value);
            }
        }

        return result;
    }
    
    public static long[] Blink(long stone)
    {
        // First condition: if 0, process it as a 1 for the next count - 1 iterations
        if (stone == 0) return [1];
        
        // Second condition: if even length, process both halves 
        if (stone.ToString().Length % 2 == 0)
        {
            string stoneString = stone.ToString();
            long stone1 = long.Parse(stoneString.Substring(0, stoneString.Length / 2));
            long stone2 = long.Parse(stoneString.Substring(stoneString.Length / 2));
            return [stone1, stone2];
        }
        
        // Else multiply by 2024 and process that
        return [2024 * stone];
    }
    
}