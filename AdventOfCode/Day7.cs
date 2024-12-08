namespace AdventOfCode;

public class Day7 : DayGeneric
{
    public static string Name = "-- Day 7: Bridge Repair --";

    public static void Day7Main()
    {
        List<long[]> allSequences = ParseInput();
        long sumValid = 0;
        long sumValidWithConcat = 0;
        foreach (long[] sequence in allSequences)
        {
            // Part 1 - assume no concatenation
            if(CheckValidity(sequence, sequence[0], sequence.Length - 1, false))
                sumValid += sequence[0];
            // Part 2, with concatenation this time
            if(CheckValidity(sequence, sequence[0], sequence.Length - 1, true))
                sumValidWithConcat += sequence[0];
        }
        
        Console.WriteLine($"Calculated sum of valid sequences without concatenation as {sumValid}.");
        Console.WriteLine($"Calculated sum of valid sequences with concatenation as {sumValidWithConcat}.");
    }

    public static List<long[]> ParseInput()
    {
        string[] sequencesUnparsed = File.ReadAllLines(Client.filePrefix + "Day 7 - BridgeRepair.txt");
        List<long[]> allSequences = new List<long[]>();
        foreach (string line in sequencesUnparsed)
        {
            string[] sequence = line.Replace(':', ' ').Split(' ');
            List<long> numbers = new List<long>();
            foreach (string num in sequence)
            {
                if (num != "") numbers.Add(long.Parse(num));
            }
            allSequences.Add(numbers.ToArray());
        }
        return allSequences;
    }

    public static bool CheckValidity(long[] sequence, long result, int pos, bool concatenate)
    {
        // If true, we've gone through the whole sequence: is our sum correct?
        if (pos <= 1) return (result == sequence[1]);

        // Check subtraction
        // First make sure we haven't subtracted too much and undershot
        // Then, check if we could get a valid sequence after the subtraction happens
        if (result > sequence[pos] && CheckValidity(sequence, result - sequence[pos], pos - 1, concatenate)) return true;
        
        // Check division
        // First make sure that there's no remainder since these are ints. I forgot this for 20 minutes i swear to god
        // Then, check if we could get a valid sequence after the division
        if (result % sequence[pos] == 0 && CheckValidity(sequence, result / sequence[pos], pos - 1, concatenate)) return true;

        if (concatenate)
        {
            // thanks wolfram https://mathworld.wolfram.com/Concatenation.html
            long concatConstant = (long)Math.Pow(10, Math.Floor(Math.Log10(sequence[pos])) + 1);
            
            // comment this later
            if (result % concatConstant == sequence[pos] && CheckValidity(sequence, result / concatConstant, pos - 1, concatenate)) return true;
        }
        
        return false;
    }
}