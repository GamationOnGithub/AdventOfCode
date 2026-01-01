namespace AdventOfCode;

public class Day21 : DayGeneric
{
    public static string Name = "-- Day 21: Keypad Conundrum -- ";

    public static Dictionary<string, long> seen = new();

    public static Dictionary<string, List<string>> keypadResults = new()
    {
        // Doubles
        { "AA", new() { "AA" }},
        { "^^", new() { "AA" } },
        { ">>", new() { "AA" } },
        { "<<", new() { "AA" } },
        { "vv", new() { "AA" } },
        
        // A-Starts
        { "A<", new() { "Av<<A", "A<v<A" } },
        { "Av", new() { "Av<A", "A<vA" } },
        { "A>", new() { "AvA" } },
        { "A^", new() { "A<A" } },
        
        // <-Starts
        { "<^", new() { "A>^A" } },
        { "<v", new() { "A>A" } },
        { "<>", new() { "A>>A" } },
        { "<A", new() { "A>>^A", "A>^>A" } },
        
        // ^-Starts
        { "^<", new() { "Av<A" } },
        { "^v", new() { "AvA" } },
        { "^>", new() { "Av>A", "A>vA" } },
        { "^A", new() { "A>A" } },
        
        // v-Starts
        { "v<", new() { "A<A" } },
        { "v>", new() { "A>A" } },
        { "v^", new() { "A^A" } },
        { "vA", new() { "A>^A", "A^>A" } },
        
        // >-Starts
        { "><", new() { "A<<A" } },
        { ">v", new() { "A<A" } },
        { ">^", new() { "A<^A", "A^<A" } },
        { ">A", new() { "A^A" } }
    };
    
    public static char[,] initKeypad = { {'7', '8', '9' }, {'4', '5', '6'}, {'1', '2', '3'}, {'#', '0', 'A'} };

    public static void Day21Main()
    {
        // I don't usually do this, but my solution methods for the two parts are going to be disparate
        // So, I'm splitting them into 2 methods
        Console.WriteLine("Part 1 may take a moment.");
        Day21Part1();
        Day21Part2();
    }

    public static void Day21Part1()
    {
        string[] keypadSequences = File.ReadAllLines(Client.filePrefix + "Day 21 - KeypadConundrum.txt");

        long complexity = 0;
        foreach (string keypadSequence in keypadSequences)
        {
            List<string> fastestSequences = EncodeSequence(keypadSequence, initKeypad);

            char[,] dirKeypad = { { '#', '^', 'A' }, { '<', 'v', '>' } };
            for (int i = 0; i < 2; i++)
            {
                List<string> nextLevelSequences = new();
                int shortestSequence = fastestSequences.Min(sequence => sequence.Length);
                foreach (string sequence in fastestSequences.Where(sequence => sequence.Length == shortestSequence))
                    nextLevelSequences.AddRange(EncodeSequence(sequence, dirKeypad));
                fastestSequences = nextLevelSequences;
            }
            
            fastestSequences.Sort((x, y) => x.Length.CompareTo(y.Length));
            complexity += fastestSequences[0].Length * int.Parse(keypadSequence.Substring(0, 3));
            //Console.WriteLine($"Calculated a complexity of {complexity} for sequence {keypadSequence}.");
        }

        Console.WriteLine($"Calculated a complexity of {complexity} for all sequences at depth 2.");
    }

    public static void Day21Part2()
    {
        string[] keypadSequences = File.ReadAllLines(Client.filePrefix + "Day 21 - KeypadConundrum.txt");
        long complexity = 0;
        
        foreach (string keypadSequence in keypadSequences)
        {
            List<string> buttonSequences = EncodeSequence(keypadSequence, initKeypad);
            long result = long.MaxValue;
            foreach (string possibleFirstSequence in buttonSequences)
            {
                long sum = 0;
                
                // i forgot i have to parse the first sequence into a form my chunk parser likes. oops
                // what writing one part of the program 12 hours after the other does to a man
                string[] steps = possibleFirstSequence.Split('A').ToList().Where(x => x != "").ToArray();
                for (int i = 0; i < steps.Length; i++) steps[i] = "A" + steps[i] + "A";
                
                foreach (string step in steps)
                {
                    for (int i = 0; i < step.Length - 1; i++)
                    {
                        string chunk = step[i].ToString() + step[i + 1];
                        sum += CalcNext(chunk, 0, 25);
                    }
                }

                result = Math.Min(result, sum);
            }
            
            //Console.WriteLine($"Calculated a complexity of {result} for the sequence {keypadSequence} at depth 25.");
            complexity += int.Parse(keypadSequence.Substring(0, 3)) * result;
        }
        
        Console.WriteLine($"Calculated a complexity of {complexity} for all sequences at depth 25.");
    }

    public static List<string> EncodeSequence(string sequence, char[,] keypad)
    {
        // Okay, find the starting pos first, since it differs (ugh)
        (int row, int col) = (-1, -1);
        for (int i = 0; i < keypad.GetLength(0); i++)
        {
            for (int j = 0; j < keypad.GetLength(1); j++)
            {
                if (keypad[i, j] == 'A')
                {
                    row = i;
                    col = j;
                    break;
                }
            }
        }
        
        // Now the real fun
        (int targetRow, int targetCol) = (-1, -1);
        List<string> allPaths = new();
        foreach (char c in sequence)
        {
            // First, find where it's located
            for (int dRow = 0; dRow < keypad.GetLength(0); dRow++)
            {
                for (int dCol = 0; dCol < keypad.GetLength(1); dCol++)
                {
                    if (c == keypad[dRow, dCol])
                    {
                        targetRow = dRow;
                        targetCol = dCol;
                        break;
                    }
                }
            }
            
            // Then get all of the shortest paths that lead to it
            List<string> stepPaths = BFS(keypad, (row, col), (targetRow, targetCol));
            
            // Now concatenate each new path with every existing one
            if (allPaths.Count == 0)
            {
                foreach (string stepPath in stepPaths) allPaths.Add(stepPath);
            }
            else
            {
                int initialPaths = allPaths.Count;
                List<string> toAdd = new();
                for (int i = 0; i < initialPaths; i++)
                {
                    foreach (string stepPath in stepPaths)
                    {
                        toAdd.Add(allPaths[i] + stepPath);
                    }
                }

                allPaths.Clear();
                allPaths.AddRange(toAdd);
            }
            
            row = targetRow;
            col = targetCol;
        }

        return allPaths;
    }
    
    public static List<string> BFS(char[,] keypad, (int sRow, int sCol) sPos, (int tRow, int tCol) tPos)
    {
        List<List<Directions>> workingPaths = new List<List<Directions>>();
        Queue<(int row, int col, int distance, List<Directions> pastMoves)> queue = new();
        Dictionary<(int row, int col), int> distanceDict = new Dictionary<(int row, int col), int>();
        
        queue.Enqueue((sPos.sRow, sPos.sCol, 0, new List<Directions>()));
        
        while (queue.Count > 0)
        {
            (int row, int col, int distance, List<Directions> pastMoves) = queue.Dequeue();

            if (row == tPos.tRow && col == tPos.tCol &&
                distanceDict.GetValueOrDefault((row, col), int.MaxValue) >= distance)
            {
                workingPaths.Add(pastMoves);
                distanceDict[(row, col)] = distance;
                continue;
            }

            if (keypad[row, col] == '#')
            {
                // This is inaccessible
                distanceDict[(row, col)] = int.MaxValue;
                continue;
            }
            
            if (distanceDict.TryGetValue((row, col), out int prevDistance) && prevDistance < distance) continue;
            
            // this sucks lol
            if (CheckBounds(row - 1, col, keypad)) 
                queue.Enqueue((row - 1, col, distance + 1, new List<Directions>(pastMoves) { Directions.Up }));
            
            if (CheckBounds(row, col + 1, keypad)) 
                queue.Enqueue((row, col + 1, distance + 1, new List<Directions>(pastMoves) { Directions.Right }));
            
            if (CheckBounds(row + 1, col, keypad)) 
                queue.Enqueue((row + 1, col, distance + 1, new List<Directions>(pastMoves) { Directions.Down }));
            
            if (CheckBounds(row, col - 1, keypad)) 
                queue.Enqueue((row, col - 1, distance + 1, new List<Directions>(pastMoves) { Directions.Left }));

            distanceDict[(row, col)] = distance;
        }

        List<string> sequences = new List<string>();
        foreach (List<Directions> moveList in workingPaths)
        {
            string seq = "";
            foreach (Directions dir in moveList)
            {
                switch (dir)
                {
                    case Directions.Up: seq += '^';
                        break;
                    case Directions.Right: seq += '>';
                        break;
                    case Directions.Down: seq += 'v';
                        break;
                    case Directions.Left: seq += '<';
                        break;
                }
            }

            seq += 'A';
            sequences.Add(seq);
        }

        return sequences;
    }

    public static long CalcNext(string sequence, int depth, int finalDepth)
    {
        if (depth == finalDepth) return 1;
        
        // Go go gadget memoization
        if (seen.ContainsKey(sequence + depth)) return seen[sequence + depth];
        
        long result = long.MaxValue;
        foreach (string nextPath in keypadResults[sequence])
        {
            long combinationSum = 0;
            for (int i = 0; i < nextPath.Length - 1; i++)
            {
                string chunk = nextPath[i].ToString() + nextPath[i + 1];
                // I think I finally understand recursion
                combinationSum += CalcNext(chunk, depth + 1, finalDepth);
            }
            result = Math.Min(result, combinationSum);
        }

        seen[sequence + depth] = result;
        return result;
    }
    
    public static bool CheckBounds(int row, int col, char[,] keypad)
    {
        return (row >= 0 && row < keypad.GetLength(0) && col >= 0 && col < keypad.GetLength(1));
    }

}