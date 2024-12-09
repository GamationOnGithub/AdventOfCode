namespace AdventOfCode;

public class Day9 : DayGeneric
{
    public static string Name = "-- Day 9: Disk Fragmenter --";

    public static void Day9Main()
    {
        string input = File.ReadAllText(Client.filePrefix + "Day 9 - DiskFragmenter.txt");
        List<int> unsortedList = ParseFragment(input);
        List<int> sortedList = SortList(unsortedList);
        List<int> blockedList = SortListBlocks(unsortedList);
        long checksum = CalculateSum(sortedList);
        long blockChecksum = CalculateSum(blockedList);
        
        Console.WriteLine($"Calculated a checksum of {checksum}.");
        Console.WriteLine($"Calculated a checksum of {blockChecksum} by sorting whole blocks.");
    }

    public static List<int> ParseFragment(string input)
    {
        Dictionary<int, (int digitCount, int whitespace)> idPairs = new();
        string line = input;
        
        // Account for the last thing not having any whitespace
        line = line.Trim();
        line += "0";
        //File.WriteAllText(Client.filePrefix + "stringOutput.txt", line);
        
        
        for (int i = 0; i < line.Length; i += 2)
        {
            string temp = line.Substring(i, 2);
            
            idPairs.Add(i / 2, (int.Parse(temp[0].ToString()), int.Parse(temp[1].ToString())));
            
        }

        List<int> unsortedList = new();
        //Console.WriteLine(idPairs[idPairs.Count - 1]);
        foreach (var key in idPairs)
        {
            for (int i = 0; i < key.Value.digitCount; i++) unsortedList.Add(key.Key);
            for (int i = 0; i < key.Value.whitespace; i++) unsortedList.Add(-1);
        }

        return unsortedList;
    }

    public static List<int> SortList(List<int> list)
    {
        List<int> sortedList = new(list);
        
        int backwardsIndex = sortedList.Count - 1;
        for (int forwardsIndex = 0; forwardsIndex < list.Count; forwardsIndex++)
        {
            if (sortedList[forwardsIndex] == -1)
            {
                sortedList[forwardsIndex] = sortedList[backwardsIndex];
                // Yes I'm using -2 as a dummy value. Can you tell it's late?
                sortedList[backwardsIndex] = -2;
                // Reset our backwards mover to the next farthest int
                backwardsIndex = sortedList.Count - 1;
                while (sortedList[backwardsIndex] <= 0) backwardsIndex--;
            }
        }
        // This system will miss the very last int, so let's move it manually
        while (sortedList[backwardsIndex] <= 0) backwardsIndex--;
        int finalIndex = 0;
        while (sortedList[finalIndex] >= 0) finalIndex++;
        sortedList[finalIndex] = sortedList[backwardsIndex];
        sortedList[backwardsIndex] = 0;
        
        // Clear out dummy values and replace with 0s
        for (int i = 0; i < sortedList.Count; i++) if (sortedList[i] == -2) sortedList[i] = 0;
        
        return sortedList;
    }

    public static List<int> SortListBlocks(List<int> list)
    {
        int blockSize = 0; int blockValue = list[list.Count - 1];
        int whitespaceSize = 0;
        List<int> blockedList = new(list);
        List<(int pos, int size)> whitespaceList = new List<(int pos, int size)>();
        int startIndex = 0;

        // Cache all whitespace in our list and its size
        for (int i = 1; i < list.Count - 1; i++)
        {
            if (list[i] == -1)
            {
                if (list[i - 1] != -1)
                {
                    startIndex = i;
                }
                whitespaceSize++;
            }
            else if (whitespaceSize > 0)
            {
                whitespaceList.Add((startIndex, whitespaceSize));
                whitespaceSize = 0;
            }
        }

        int backwardsIndex = list.Count - 1;
        // yum spaghetti
        while (backwardsIndex > 0)
        {
            // First, find the last block
            // Move to the next number
            while (blockedList[backwardsIndex] == -1) backwardsIndex--;
            
            // Then calculate the size of the block
            blockSize = 0;
            blockValue = blockedList[backwardsIndex];
            while (blockedList[backwardsIndex] == blockValue)
            {
                blockSize++;
                if (backwardsIndex > 0) backwardsIndex--;
                else break;
            }
            
            // Now find the first bit of whitespace in our cache that will hold it
            for (int i = 0; i < whitespaceList.Count; i++)
            {
                // Can't be moving blocks to the right
                if (backwardsIndex > whitespaceList[i].pos)
                {
                    // If true, we found one! Excellent
                    if (whitespaceList[i].size >= blockSize)
                    {
                        for (int j = 0; j < blockSize; j++)
                        {
                            blockedList[whitespaceList[i].pos + j] = blockValue;
                            blockedList[backwardsIndex + 1 + j] = -1;
                        }
                        
                        // Adjust the whitespace cache to reflect the block now there
                        int newPos = whitespaceList[i].pos + blockSize;
                        int newSize = whitespaceList[i].size - blockSize;
                        whitespaceList.RemoveAt(i);
                        whitespaceList.Insert(i, (newPos, newSize));
                        break;
                    }
                }
            }
        }
        
        // Clear out whitespace values and replace with 0s for summation later
        for (int i = 0; i < blockedList.Count; i++)
        {
            if (blockedList[i] == -1) blockedList[i] = 0;
        }

        return blockedList;
    }
    
    public static long CalculateSum(List<int> list)
    {
        long sum = 0;
        for (int i = 0; i < list.Count; i++)
        {
            sum += list[i] * i;
        }

        return sum;
    }
}