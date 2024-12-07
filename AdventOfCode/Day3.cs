using System.Text.RegularExpressions;

namespace AdventOfCode;

public class Day3
{
    public static string Name = "-- Day 3: Mull It Over --";
    
    public static void Day3Main()
    {
        string data = "";
        StreamReader reader = new StreamReader(Client.filePrefix + "Day 3 - MullItOver.txt");
        while (!reader.EndOfStream)
        {
            data += reader.ReadLine();
        }

        for (int i = 0; i < 2; i++)
        {
            int total = 0;
            string[] ops = (i == 0) ? ParseInput(data) : ParseInputWithDo(data);

            foreach (string op in ops)
            {
                total += doMul(op);
            }
            string run = (i == 0) ? "" : " respecting do() commands";
            Console.WriteLine($"Total multiplied value{run}: {total}");
        }
    }

    public static string[] ParseInput(string line)
    {
        Regex parserRegex = new Regex("(mul\\(\\d*,\\d*\\))");
        MatchCollection matches = parserRegex.Matches(line);
        return(matches.Select(m => m.Value).ToArray());
    }
    
    public static string[] ParseInputWithDo(string line)
    {
        Regex doParserRegex = new Regex("(?<=(do\\(\\))|^).*?(?=don't\\(\\))");
        string[] splitInstrs = doParserRegex.Matches(line).Select(m => m.Value).ToArray();
        List<string> result = new List<string>();
        foreach (string s in splitInstrs)
        {
            result.AddRange(ParseInput(s).ToList());
        }

        return result.ToArray();
    }

    public static int doMul(string input)
    {
        input = input.Remove(input.Length - 1).Substring(input.IndexOf('(') + 1);
        int[] toMultiply = input.Split(',').Select(int.Parse).ToArray();
        return toMultiply[0] * toMultiply[1];
    }
}