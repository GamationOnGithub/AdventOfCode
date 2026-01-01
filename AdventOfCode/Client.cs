global using System.IO;

namespace AdventOfCode;

public class Client
{
    public static string filePrefix = "D:/AdventOfCode/Inputs/";
    
    public static void Main(string[] args)
    {
        Console.WriteLine("Welcome to Advent of Code!");
        Console.Write("Choose a year and day (space separated) to display results from: ");
        string? input = Console.ReadLine();
        
        while (input != "exit" && input != "")
        {
            string[] inputs = input.Split(' ');
            int year = int.Parse(inputs[0]);
            int day = int.Parse(inputs[1]);
            
            Console.WriteLine();
            try
            {
                string file = (year == 2024) ? "" : "_" + year.ToString();
                Type t = Type.GetType("AdventOfCode.Day" + day + file);
                Console.WriteLine(t.GetField("Name").GetValue(null).ToString());
                t.GetMethod("Day" + day + "Main").Invoke(null, null);

                Console.WriteLine();
                Console.WriteLine("Enter another number to see another day, or type 'exit'.");
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("That's not a valid day! Try again, or type 'exit'.");
            }

            input = Console.ReadLine();
        }
    }
}