namespace AdventOfCode;

public class Day2 : DayGeneric
{
    public static string Name = "-- Day 2: Red-Nosed Reports --";
    
    public static void Day2Main()
    {
        for (int i = 0; i < 2; i++)
        {
            int safeCounter = 0;
            try
            {
                StreamReader reader = new StreamReader(Client.filePrefix + "Day 2 - RedNosedReports.txt");
                while (!reader.EndOfStream)
                {
                    List<int> nums = reader.ReadLine().Split().Select(int.Parse).ToList();
                    bool safe = (i == 0) ? CheckSafety(nums) : CheckSafetyWithTolerance(nums);
                    if (safe) safeCounter++;
                }

                string tolerance = i == 0 ? "without tolerance" : "with tolerance";
                Console.WriteLine($"Number of safe instructions {tolerance}: {safeCounter}");

            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File not found");
            }
        }
    }

    public static bool CheckSafety(List<int> nums)
    {
        bool isIncreasing = nums[1] > nums[0];

        for (int i = 1; i < nums.Count; i++)
        {
            int diff = nums[i] - nums[i - 1];
            // Check if the diff is 0 or greater than 3, since those are banned
            if (diff == 0 || Math.Abs(diff) > 3) return false;
            
            // Make sure the always-increasing / decreasing rules are being followed
            if (isIncreasing && diff < 0) return false;
            if (!isIncreasing && diff > 0) return false;
        }

        return true;
    }

    public static bool CheckSafetyWithTolerance(List<int> nums)
    {
        // If the result is true without any tolerance, great, return it
        if (CheckSafety(nums)) return true;
        
        // Check if removing an index would fix things
        for (int i = 0; i < nums.Count; i++)
        {
            List<int> copy = new List<int>(nums);
            copy.RemoveAt(i);
            if (CheckSafety(copy)) return true;
        }
        
        // Nope, no matter what index we remove we're screwed, cool
        return false;
    }
}