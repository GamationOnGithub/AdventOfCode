namespace AdventOfCode;

public class Day4 : DayGeneric
{
    public static string Name = "-- Day 4: Ceres Search --";
    static char[][] wordSearch;
    
    public static void Day4Main()
    {
        string[] wordSearchUnparsed = File.ReadAllLines(Client.filePrefix + "Day 4 - CeresSearch.txt");
        wordSearch = wordSearchUnparsed.Select(s => s.ToCharArray()).ToArray();
        FindChristmas();
    }

    public static void FindChristmas()
    {
        int xmasCount = 0;
        for (int part = 1; part <= 2; part++) {
            xmasCount = 0;
            // We have to loop through the whole thing regardless
            for (int i = 0; i < wordSearch.Length; i++)
            {
                for (int j = 0; j < wordSearch[i].Length; j++)
                {
                    if (part == 1)
                    {
                        // For optimization's sake, just don't bother unless we're starting on the right char
                        if (wordSearch[i][j] == 'X')
                        {
                            string[] dirs = BoundaryCheck(i, j, 3);

                            foreach (string dir in dirs)
                            {
                                if (CheckChristmas(dir, i, j)) xmasCount++;
                            }
                        }
                    }
                    else
                    {
                        // Only the As on the inside of our word search are safe to check
                        if (wordSearch[i][j] == 'A'
                            && i != 0 && j != 0
                            && i != wordSearch[i].Length - 1 && j != wordSearch[i].Length - 1)
                        { 
                            if(CheckXMas(i, j)) xmasCount++;
                        }
                    }
                }
            }

            string puzzlePart = (part == 1) ? "Christmases" : "X-Mas-es";
            Console.WriteLine($"Found {xmasCount} {puzzlePart} in the word search.");
        }
    }

    public static string[] BoundaryCheck(int i, int j, int tolerance)
    {
        List<string> validDirs = new();
        if (i - tolerance >= 0) validDirs.Add("left");
        if (i + tolerance < wordSearch.Length) validDirs.Add("right");
        if (j - tolerance >= 0)
        {
            validDirs.Add("up");
            if (validDirs.Contains("left")) validDirs.Add("upleft");
            if (validDirs.Contains("right")) validDirs.Add("upright");
        }
        if (j + tolerance < wordSearch[i].Length)
        {
            validDirs.Add("down");
            if (validDirs.Contains("left")) validDirs.Add("downleft");
            if (validDirs.Contains("right")) validDirs.Add("downright");
        }
        return validDirs.ToArray();
    }

    public static bool CheckChristmas(string dir, int i, int j)
    {
        var (iMult, jMult) = GetIJMults(dir);
        return (wordSearch[i + 1 * iMult][j + 1 * jMult] == 'M' 
                && wordSearch[i + 2 * iMult][j + 2 * jMult] == 'A' 
                && wordSearch[i + 3 * iMult][j + 3 * jMult] == 'S');
    }
    
    public static bool CheckXMas(int row, int col)
    {
        int xCount = 0;
        for (int i = -1; i <= 1; i += 2)
        {
            for (int j = -1; j <= 1; j += 2)
            {
                if (wordSearch[row + i][col + j] == 'M' && wordSearch[row - i][col - j] == 'S') xCount++;
            }
        }

        return (xCount >= 2);
    }

    public static (int, int) GetIJMults(string dir)
    {
        int iMult = 0;
        int jMult = 0;

        switch (dir)
        {
            case "left":
                iMult = -1;
                break;
            case "right":
                iMult = 1;
                break;
            case "up":
                jMult = -1;
                break;
            case "down":
                jMult = 1;
                break;
            case "upleft":
                iMult = -1;
                jMult = -1;
                break;
            case "upright":
                iMult = 1;
                jMult = -1;
                break;
            case "downleft":
                iMult = -1;
                jMult = 1;
                break;
            case "downright":
                iMult = 1;
                jMult = 1;
                break;
        }
        
        return (iMult, jMult);
    }
}