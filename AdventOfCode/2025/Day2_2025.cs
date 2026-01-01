namespace AdventOfCode;

public class Day2_2025 : DayGeneric
{
    public static string Name = "-- Day 2: Gift Shop --";
    //public static List<ulong> invalids = new List<ulong>();

    public static void Day2Main()
    {
        ulong invalidSum = Part1();
        Console.WriteLine($"The invalid IDs sum to {invalidSum}.");
        ulong pt2InvalidSum = Part2();
        Console.WriteLine($"Checking for more patterns, the invalid IDs sum to {pt2InvalidSum}.");
    }

    public static ulong Part1()
    {
        ulong invalidSum = 0;
        StreamReader reader = new StreamReader(Client.filePrefix + "Day 2 - Gift Shop.txt");
        while (!reader.EndOfStream)
        {
            string[] input = reader.ReadLine().Split(',');
            foreach (string item in input)
            {
                if (item.Equals("")) continue;
                string[] indices = item.Split('-');
                invalidSum += FindInvalids(ulong.Parse(indices[0]), ulong.Parse(indices[1]));
            }
        }

        return invalidSum;
    }

    public static ulong Part2()
    {
        ulong invalidSum = 0;
        StreamReader reader = new StreamReader(Client.filePrefix + "Day 2 - Gift Shop.txt");
        while (!reader.EndOfStream)
        {
            string[] input = reader.ReadLine().Split(',');
            foreach (string item in input)
            {
                if (item.Equals("")) continue;
                string[] indices = item.Split('-');
                invalidSum += FindInvalidsPt2(ulong.Parse(indices[0]), ulong.Parse(indices[1]));
            }
        }

        return invalidSum;
    }

    public static ulong FindInvalids(ulong first, ulong last)
    {
        ulong invalidSum = 0;
        for (ulong i = first; i <= last; i++)
        {
            string id = i.ToString();
            if (id.Length == 1) continue;
            string lastHalf = id.Substring(id.Length / 2);
            if (lastHalf.Equals(id.Substring(0, id.Length / 2)))
                invalidSum += i;
        }

        return invalidSum;
    }
    
    public static ulong FindInvalidsPt2(ulong first, ulong last)
    {
        ulong invalidSum = 0;
        for (ulong i = first; i <= last; i++)
        {
            string id = i.ToString();
            
            for (int partition = 1; partition <= id.Length / 2; partition++)
            {
                if (id.Length % partition != 0) continue;
            
                List<string> slices = new List<string>();
                for (int j = 0; j < id.Length; j += partition)
                    slices.Add(id.Substring(j, partition));
            
                if (CheckSlices(slices))
                {
                    //invalids.Add(i);
                    invalidSum += i;
                    break;
                }
            }
        }

        return invalidSum;
    }

    public static bool CheckSlices(List<string> slices)
    {
        for (int i = 1; i < slices.Count; i++)
        {
            if (slices[i] != slices[0]) return false;
        }

        return true;
    }
}
