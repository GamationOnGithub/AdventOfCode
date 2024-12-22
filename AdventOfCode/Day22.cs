namespace AdventOfCode;

public class Day22 : DayGeneric
{
    public static string Name = "-- Day 22: Day 22 --";

    public static Dictionary<long, long> seen = new();
    public static Dictionary<string, int> sellsFor = new();

    public static void Day22Main()
    {
        // Me forcing myself to use LINQ exhibit 1
        List<long> initSecrets = File.ReadAllLines(Client.filePrefix + "Day 22 - MonkeyMarket.txt").Select(x => long.Parse(x)).ToList();
        long sum = 0;
        foreach (long secret in initSecrets)
        {
            List<int> prices = new() { (int)(secret % 10) };
            
            // Part 1
            long finalSecret = secret;
            for (int i = 0; i < 2000; i++)
            {
                finalSecret = Sequence(finalSecret);
                // This bit is for part 2 but don't tell anyone
                prices.Add((int)(finalSecret % 10));
            }
            //Console.WriteLine($"After 2000 iterations, secret {secret} becomes {finalSecret}.");
            sum += finalSecret;

            // Part 2
            // bro this code is an abomination
            List<int> dPrices = new();
            HashSet<string> seenSequences = new();
            
            for (int i = 1; i < prices.Count; i++)
            {
                dPrices.Add(prices[i] - prices[i - 1]);
            }
            
            for (int i = 4; i < dPrices.Count; i++)
            {
                string sequence = string.Join(',', dPrices.GetRange(i - 4, 4));
                if (!seenSequences.Contains(sequence))
                {
                    seenSequences.Add(sequence);
                    if (!sellsFor.ContainsKey(sequence)) sellsFor.Add(sequence, 0);
                    sellsFor[sequence] += prices[i];
                }
            }
        }
        
        Console.WriteLine($"After 2000 iterations, all secrets sum to {sum}.");
        Console.WriteLine($"The best sequence produces the value {sellsFor.Values.Max()}.");
        
    }

    public static long Sequence(long secret)
    {
        // Go go gadget memoization
        if (seen.ContainsKey(secret)) return seen[secret];

        long newSecret = secret;
        newSecret = Prune(Mix(newSecret, newSecret * 64));
        newSecret = Prune(Mix(newSecret, newSecret / 32));
        newSecret = Prune(Mix(newSecret, newSecret * 2048));
        
        seen[secret] = newSecret;
        return newSecret;
    }

    public static long Mix(long secret, long toMix)
    {
        return secret ^ toMix;
    }

    public static long Prune(long secret)
    {
        return secret % 16777216;
    }
}