global using System.IO;

namespace AdventOfCode;

public class Client
{
    public static string filePrefix = "C:/Users/noahb/Documents/AoC2024/";
    
    public static void Main(string[] args)
    {
        Console.WriteLine("Welcome to Advent of Code 2024!");
        Console.Write("Choose a day to display results from: ");
        string? input = Console.ReadLine();
        
        while (input != "exit" && input != "")
        {
            int day = int.Parse(input);
            
            Console.WriteLine();
            try
            {
                Type t = Type.GetType("AdventOfCode.Day" + day);
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