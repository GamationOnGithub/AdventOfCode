using System.Collections.Immutable;

namespace AdventOfCode;

public class Day23 : DayGeneric
{
    public static string Name = "-- Day 23: LAN Party -- ";
    
    public static Dictionary<string, List<string>> computers = new();

    public static void Day23Main()
    {
        // Part 1
        ParseInput("Day 23 - LANParty.txt");
        HashSet<(string, string, string)> connections = FindConnections();
        int count = FindTComputers(connections);
        Console.WriteLine($"Found {connections.Count} sets of 3 computers, {count} of which have at least one computer starting with t.");
        
        // Part 2
        string password = FindLargestNetwork();
        Console.WriteLine($"The password to the LAN Party is {password}.");
    }

    public static void ParseInput(string filename)
    {
        string[] input = File.ReadAllLines(Client.filePrefix + filename);
        foreach (string line in input)
        {
            string[] pcs = line.Split('-');
            if (computers.ContainsKey(pcs[0])) computers[pcs[0]].Add(pcs[1]);
            else computers.Add(pcs[0], new List<string>() { pcs[1] });
            if (computers.ContainsKey(pcs[1])) computers[pcs[1]].Add(pcs[0]);
            else computers.Add(pcs[1], new List<string>() { pcs[0] });
        }
    }

    public static HashSet<(string, string, string)> FindConnections()
    {
        HashSet<(string, string, string)> allConnections = new();
        foreach (string comp1 in computers.Keys)
        {
            if (computers[comp1].Count > 1)
            {
                for (int i = 1; i < computers[comp1].Count; i++)
                {
                    List<string> connections = computers[comp1];
                    string comp2 = connections[i - 1];
                    for (int j = 1; j < connections.Count; j++)
                    {
                        string comp3 = connections[j];
                        if (computers[comp2].Contains(comp3))
                        {
                            // lol. lmao
                            List<string> sorted = new List<string>() { comp1, comp2, comp3 };
                            sorted.Sort();
                            allConnections.Add((sorted[0], sorted[1], sorted[2]));
                        }
                    }
                }
            }
        }
        
        return allConnections;
    }

    public static int FindTComputers(HashSet<(string, string, string)> connections)
    {
        int count = 0;
        foreach (var (comp1, comp2, comp3) in connections)
        {
            if (comp1[0] == 't' || comp2[0] == 't' || comp3[0] == 't') count++;
        }

        return count;
    }

    public static string FindLargestNetwork()
    {
        int largestNetworkSize = 0;
        List<string> largestNetwork = new();
        
        foreach (string comp1 in computers.Keys)
        {
            // thank you stackoverflow gods for writing this GetCombinations for me
            // python gets it for free with itertools i'm allowed to take it from the internet
            var combinations = GetCombinations(computers[comp1]).ToList();
            foreach (var possibleNetwork in combinations)
            {
                if (CheckNetwork(possibleNetwork.ToList()) && largestNetworkSize < possibleNetwork.Length)
                {
                    largestNetworkSize = possibleNetwork.Length;
                    largestNetwork = possibleNetwork.ToList();
                    largestNetwork.Add(comp1);
                } 
            }
        }
        
        largestNetwork.Sort();
        return string.Join(',', largestNetwork);
    }

    public static bool CheckNetwork(List<string> possibleNetwork)
    {
        foreach (string comp1 in possibleNetwork)
        {
            foreach (string comp2 in possibleNetwork)
            {
                if (comp1 != comp2 && !computers[comp1].Contains(comp2)) return false;
            }
        }

        return true;
    }
    
}