namespace AdventOfCode;

public class Day6 : DayGeneric
{
    public static string Name = "-- Day 6: Guard Gallivant --";

    static char[][] areaMap;
    private static int loopCount;
    private static int startingRow = 0;
    private static int startingCol = 0;

    public enum Directions
    {
        Up = 0, 
        Right = 1, 
        Down = 2, 
        Left = 3
    }

    public enum MoveEndings
    {
        HitEdge = 0,
        HitBarrier = 1,
        InfiniteLoop = 2,
    }

    public static void Day6Main()
    {
        areaMap = ParseInputAsMap("Day 6 - GuardGallivant.txt", '_');
        (startingRow, startingCol) = FindStart();

        var (moveCount, steps) = Move(Directions.Up, startingRow, startingCol, new HashSet<(int, int, Directions)>());
        SolvePart2(steps);
        
        GenerateMapOutput(areaMap);
        Console.WriteLine($"Covered {moveCount} unique tiles.");
        Console.WriteLine($"Detected {loopCount} unique ways of making infinite loops.");
    }
    
    public static (MoveEndings ending, int moveCount, int nextRow, int nextCol) MoveUntilBarrier
        (Directions dir, int nextRow, int nextCol, int xStep, int yStep, HashSet<(int row, int col, Directions orientation)> steps)
    {
        int moveCount = 0;
        MoveEndings ending = MoveEndings.HitEdge;

        while ((nextRow < areaMap.Length && nextRow >= 0) && (nextCol < areaMap[0].Length && nextCol >= 0))
        {
            if (steps.Contains((nextRow, nextCol, dir)))
            {
                // We're in an infinite loop! Let's get out of here
                ending = MoveEndings.InfiniteLoop;
                break;
            }
            
            // Then, check if we're gonna hit a barrier
            if (areaMap[nextRow][nextCol] != '#')
            {
                // Check if we've already been here
                if (areaMap[nextRow][nextCol] != 'X')
                {
                    // If not, increase move count and mark it as visited
                    moveCount++;
                    areaMap[nextRow][nextCol] = 'X';
                }
                
                steps.Add((nextRow, nextCol, dir));
                nextCol += xStep;
                nextRow += yStep;
            }
            else
            {
                
                // Take a step back to before the barrier
                nextCol -= xStep;
                nextRow -= yStep;
                ending = MoveEndings.HitBarrier;
                // Break to receive the new direction
                break;
            }
        }

        return (ending, moveCount, nextRow, nextCol);
    }
    
    public static (int, HashSet<(int row, int col, Directions orientation)>) Move
        (Directions dir, int row, int col, HashSet<(int row, int col, Directions orientation)> steps)
    {
        int totalMoveCount = 0;
        Directions nextDir = dir;
        var (xStep, yStep) = CalculateNextStepValues(nextDir);
        
        // Fencepost
        var (ending, moveCount, nextRow, nextCol) = MoveUntilBarrier(nextDir, row, col, xStep, yStep,  steps);
        totalMoveCount += moveCount;
        nextDir = (Directions)(((int)nextDir + 1) % 4);
        (xStep, yStep) = CalculateNextStepValues(nextDir);
        
        while (ending != MoveEndings.HitEdge)
        {
            // Move until we hit a barrier and count how many moves it was
            (ending, moveCount, nextRow, nextCol) = MoveUntilBarrier(nextDir, nextRow, nextCol, xStep, yStep, steps);

            // Check if the last run ended in an infinite loop, and if so, return early
            if (ending == MoveEndings.InfiniteLoop) return (-1, steps);
            
            totalMoveCount += moveCount;
            
            // Reorient 90 degrees to the right
            nextDir = (Directions)(((int)nextDir + 1) % 4);
            (xStep, yStep) = CalculateNextStepValues(nextDir);
        }
        
        return (totalMoveCount, steps);
    }

    public static void SolvePart2(HashSet<(int row, int col, Directions orientation)> steps)
    {
        // Collect a big list of all the valid locations to place barriers, so we can check for any duplicates
        // Notably orientation doesn't matter here - it's a barrier regardless of the direction you hit it
        HashSet<(int, int)> validBarriers = new();
        for (int i = 0; i < steps.Count; i++)
        {
            int row = steps.ElementAt(i).row;
            int col = steps.ElementAt(i).col;
            Directions dir = steps.ElementAt(i).orientation;
            
            // Save the previous value of the space so we can reinstate it later
            char prevValue = areaMap[row][col];
            areaMap[row][col] = '#';

            // The trap has been set
            HashSet<(int row, int col, Directions orientation)> simulationSteps = new();
            int result = Move(Directions.Up, startingRow, startingCol, simulationSteps).Item1;
            if (result == -1 && !validBarriers.Contains((row, col)))
            { 
                validBarriers.Add((row, col)); 
                loopCount++;
            }
            
            // Return the character to its previous state, or a O if it was a valid location
            areaMap[row][col] = (result == -1) ? 'O' : prevValue;
        }
        
        // Go go gadget hardcoding!
        if (validBarriers.Contains((startingRow, startingCol)))
        {
            validBarriers.Remove((startingRow, startingCol));
            loopCount--;
        }
    }
    
    public static (int, int) CalculateNextStepValues(Directions dir)
    {
        int xStep = 0, yStep = 0;
        switch (dir)
        {
            case Directions.Up:
                xStep = 0;
                yStep = -1;
                break;
            case Directions.Right:
                xStep = 1;
                yStep = 0;
                break;
            case Directions.Down:
                xStep = 0;
                yStep = 1;
                break;
            case Directions.Left:
                xStep = -1;
                yStep = 0;
                break;
        }
        
        return (xStep, yStep);
    }
    
    public static (int, int) FindStart()
    {
        for (int i = 0; i < areaMap.Length; i++)
        {
            for (int j = 0; j < areaMap[i].Length; j++)
            {
                if (areaMap[i][j] == '^') return (i, j);
            }
        }
        
        return (-1, -1);
    }
}