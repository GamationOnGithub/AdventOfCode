namespace AdventOfCode;

public class Day5 : DayGeneric
{
    public static string Name = "-- Day 5: Print Queue --";
    
    public static void Day5Main()
    {
        string[] input = File.ReadAllLines(Client.filePrefix + "Day 5 - PrintQueue.txt");
        var (rules, queues) = Parse(input);
        int sum = 0, orderedSum = 0;
        foreach (int[] queue in queues)
        {
            bool ordered = true;
            for (int currentPage = 0; currentPage < queue.Length; currentPage++)
            {
                for (int pageBefore = 0; pageBefore < currentPage; pageBefore++)
                {
                    // Check for if a page that must appear after our current page is appearing before it.
                    // If this were true, we would have a rule violation for (currentPage | pageBefore).
                    if (rules.Contains((queue[currentPage], queue[pageBefore])))
                    {
                        ordered = false;
                    }
                }

                for (int pageAfter = currentPage + 1; pageAfter < queue.Length; pageAfter++)
                {
                    // Check for if a page that must appear before our current page is appearing before it.
                    // If this were true, we would have a rule violation for (pageAfter | currentPage).
                    if (rules.Contains((queue[pageAfter], queue[currentPage])))
                    {
                        ordered = false;
                    }
                }
            }
            
            // Hooray, we survived the checks!
            if (ordered) sum += queue[queue.Length / 2];
            else
            {
                // Just kidding! Back into the mines for you
                List<int> orderedQueue = queue.ToList();
                for (int page = 0; page < orderedQueue.Count; page++)
                {
                    for (int toSort = page + 1; toSort < orderedQueue.Count; toSort++)
                    {
                        // If our current page must come after a page it currently comes before, uh oh!
                        // Move it before our current index.
                        if (rules.Contains((orderedQueue[toSort], orderedQueue[page])))
                        {
                            int toInsert = orderedQueue[toSort];
                            orderedQueue.RemoveAt(toSort);
                            orderedQueue.Insert(page, toInsert);
                        }
                    }
                }

                orderedSum += orderedQueue[orderedQueue.Count / 2];
            }
        }
        
        Console.WriteLine($"The sum of the ordered lines' midpoints is {sum}.");
        Console.WriteLine($"The sum of the unordered lines' ordered midpoints is {orderedSum}.");
    }

    public static (HashSet<(int, int)>, List<int[]>) Parse(string[] input)
    {
        HashSet<(int, int)> rules = new();
        List<int[]> queues = new();
        int midpoint = 0;

        for (int i = 0; i < input.Length; i++)
        {
            // Make sure we're still reading the rules
            if (input[i] == "")
            {
                midpoint = i;
                break;
            }

            var (before, after) = SplitRule(input[i]);
            rules.Add((before, after));
        }
        
        // Create a list of all our print queues
        for (int i = midpoint + 1; i < input.Length; i++)
        {
            string[] singleQueueAsString = input[i].Split(',');
            List<int> singleQueue = new();
            foreach (string data in singleQueueAsString)
            {
                singleQueue.Add(int.Parse(data));
            }
            queues.Add(singleQueue.ToArray());
        }

        return (rules, queues);
    }

    public static (int, int) SplitRule(string input)
    {
        string[] splitInput = input.Split("|");
        return (int.Parse(splitInput[0]), int.Parse(splitInput[1]));
    }

}