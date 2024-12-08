namespace AdventOfCode;

public class Day1 : DayGeneric
{
    public static string Name = "-- Day 1: Historian Hysteria --";
    static List<List<int>> nums = new List<List<int>>();

    public static void Day1Main()
    {
        ReadInput();
        int distance = CalculateDistances();
        int similarity = CalculateSimilarity();
        Console.WriteLine($"Total distance: {distance}");
        Console.WriteLine($"Total similarity: {similarity}");
    }

    public static void ReadInput()
    {
        List<int> left = new List<int>();
        List<int> right = new List<int>();

        try
        {
            StreamReader sr = new StreamReader(Client.filePrefix + "Day 1 - HistorianHysteria.txt");
            string? line = sr.ReadLine();
            while (line != null)
            {
                string[] values = line.Split("   ");
                for (int i = 0; i < values.Length; i++) values[i] = values[i].Trim();
                left.Add(int.Parse(values[0]));
                right.Add(int.Parse(values[1]));
                line = sr.ReadLine();
            }
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("File not found");
        }

        left.Sort();
        right.Sort();
        nums.Add(left);
        nums.Add(right);
    }

    public static int CalculateDistances()
    {
        int distance = 0;

        for (int i = 0; i < nums[0].Count; i++)
        {
            distance += Math.Abs(nums[0][i] - nums[1][i]);
        }

        return distance;
    }

    public static int CalculateSimilarity()
    {
        int similarity = 0;
        int repeatCount = 0;

        for (int i = 0; i < nums[0].Count; i++)
        {
            repeatCount = 0;
            // Loop through our right list; check if the value is the same as the current one in the left list
            foreach (int num in nums[1]) if (num == nums[0][i]) repeatCount++;
            similarity += nums[0][i] * repeatCount;
        }

        return similarity;
    }
}
