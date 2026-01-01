namespace AdventOfCode;

public class Day1_2025 : DayGeneric
{
    public static string Name = "-- Day 1: Secret Entrance --";
    private static int lockState;

    public static void Day1Main()
    {
        lockState = 50;
        int password = Part1();
        Console.WriteLine($"The lock hits zero {password} times, making that the password.");
        lockState = 50;
        int crossesPassword = Part2();
        Console.WriteLine($"With the new crossing zero system, the password is {crossesPassword}.");
    }

    public static int Part1()
    {
        int zeroes = 0;
        StreamReader reader = new StreamReader(Client.filePrefix + "Day 1 - Secret Entrance.txt");
        while (!reader.EndOfStream)
        {
            string step = reader.ReadLine();
            int rotationValue = ((step[0] == 'R') ? int.Parse(step.Substring(1)) : -1 * int.Parse(step.Substring(1))) % 100;
            if (lockState + rotationValue > 99) lockState = lockState + rotationValue - 100;
            else if (lockState + rotationValue < 0) lockState = lockState + rotationValue + 100;
            else lockState = lockState + rotationValue;

            if (lockState == 0) zeroes++;
        }
        
        return zeroes;
    }
    
    public static int Part2()
    {
        int zeroes = 0;
        StreamReader reader = new StreamReader(Client.filePrefix + "Day 1 - Secret Entrance.txt");
        while (!reader.EndOfStream)
        {
            string step = reader.ReadLine();
            int rotationValue = (step[0] == 'R') ? int.Parse(step.Substring(1)) : -1 * int.Parse(step.Substring(1));
            
            zeroes += CalcCrossings(rotationValue);
            rotationValue %= 100;
            if (lockState + rotationValue > 99) lockState = lockState + rotationValue - 100;
            else if (lockState + rotationValue < 0) lockState = lockState + rotationValue + 100;
            else lockState = lockState + rotationValue;

            //if (lockState == 0) zeroes++;
        }
        
        return zeroes;
    }

    public static int CalcCrossings(int rotationValue)
    {
        int zeroes = 0;
        if (rotationValue > 0)
        {
            if (lockState + rotationValue < 100) return 0;
            zeroes += rotationValue / 100;
            rotationValue %= 100;
            if (lockState + rotationValue > 99) zeroes += 1;
        }
        else if (rotationValue < 0)
        {
            if (lockState + rotationValue > 0) return 0;
            zeroes += Math.Abs(rotationValue) / 100;
            rotationValue %= 100;
            if (lockState == 0) lockState = 100;
            if (lockState + rotationValue <= 0) zeroes += 1;
        }

        return zeroes;
    }

}
