namespace AdventOfCode;

public class Day15 : DayGeneric
{
    internal class Robot
    {
        public int row, col;
        public char[,] grid;
        
        public Robot(int row, int col, char[,] grid)
        {
            this.row = row;
            this.col = col;
            this.grid = grid;
        }

        public void UpdatePosition(Directions dir, int move)
        {
            (int dRow, int dCol) = GetDirectionWeight(dir);
            if (grid[row + dRow, col + dCol] == '#') return;
            
            
            // Part 1
            if (grid[row + dRow, col + dCol] == 'O')
            {
                int extent = FindChainExtent(dRow, dCol);
                if (extent != 0) grid[row + extent * dRow, col + extent * dCol] = 'O';
                else return;
            }
            
            // Part 2
            bool interactedWithBoxes = false;
            bool moveable = true;
            List<BigBox> toMove = new List<BigBox>();
            if (grid[row + dRow, col + dCol] == '[')
            {
                moveable = bigBoxes.Find(box => (box.row == row + dRow && box.cols == (col + dCol, col + dCol + 1)))
                    .CheckNeighbors(dir, toMove);
                interactedWithBoxes = true;
            }
            if (grid[row + dRow, col + dCol] == ']')
            {
                moveable = bigBoxes.Find(box => (box.row == row + dRow && box.cols == (col + dCol - 1, col + dCol)))
                    .CheckNeighbors(dir, toMove);
                interactedWithBoxes = true;
            }

            
            if (!moveable) return;
            else if (toMove.Count > 0)
            {
                var uniqueToMove = toMove.Distinct().ToList();
                
                foreach (BigBox box in uniqueToMove)
                {
                    box.EraseOnGrid();
                    
                    // Move the box
                    box.row += dRow;
                    box.cols.col1 += dCol;
                    box.cols.col2 += dCol;
                }

                // This needs to be in a separate loop or else draws will get overwritten with erasures
                foreach (BigBox box in uniqueToMove) box.DrawOnGrid();
            }
            
            // Move the robot
            grid[row, col] = '.';
            grid[row + dRow, col + dCol] = '@';
            row += dRow;
            col += dCol;

            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (grid[i, j] == '[' && grid[i, j + 1] != ']')
                        throw new ApplicationException($"A box broke in half on move {move}!!! AHHHH!");
                }
            }
        }

        public int FindChainExtent(int dRow, int dCol)
        {
            int i = 2;
            while (grid[row + i * dRow, col + i * dCol] == 'O') i++;
            if (grid[row + i * dRow, col + i * dCol] == '#') return 0;
            return i;
        }
    }

    public class BigBox
    {
        public int row;
        public (int col1, int col2) cols;
        public char[,]? grid;
        
        public BigBox(int row, (int, int) cols, char[,] grid) { this.row = row; this.cols = cols; this.grid = grid; }

        public bool CheckNeighbors(Directions dir, List<BigBox> toMove)
        {
            (int dRow, int dCol) = GetDirectionWeight(dir);
            toMove.Add(this);
            
            // If true, there's not enough space
            switch (dir)
            {
                case Directions.Left:
                    if (grid[row + dRow, cols.col1 - 1] == '#') return false;
                    break;
                case Directions.Right:
                    if (grid[row + dRow, cols.col2 + 1] == '#') return false;
                    break;
                default:
                    if (grid[row + dRow, cols.col1] == '#' || grid[row + dRow, cols.col2] == '#') return false;
                    break;
            }
            
            // Now check if we have free space or boxes
            if (dir == Directions.Up || dir == Directions.Down)
            {
                // If it's up or down, we need to check two columns instead of 1
                if (grid[row + dRow, cols.col1] == '.' && grid[row + dRow, cols.col2] == '.') return true;
            }
            else
            {
                // If it's left or right, we only need to check if there's free space in the direction we want to move
                // The first condition will trigger if we're moving left and the second if we're moving right
                if (dir == Directions.Left && grid[row + dRow, cols.col1 - 1] == '.') return true;
                if (dir == Directions.Right && grid[row + dRow, cols.col2 + 1] == '.') return true;
            }

            bool box1Movable = false;
            // By default, assume we aren't at a junction
            bool box2Movable = true;
            
            // Okay, now to check for other boxes. Oh no
            switch (dir)
            {
                case Directions.Up:
                    // If true, not a junction
                    if (grid[row - 1, cols.col1] == '[')
                    {
                        return bigBoxes.Find(box => box.row == row - 1 && box.cols.col1 == cols.col1).CheckNeighbors(dir, toMove);
                    }
                    
                    // Okay, junction time
                    if (grid[row - 1, cols.col1] == ']')
                    {
                        box1Movable = bigBoxes.Find(box => box.row == row - 1 && box.cols.col2 == cols.col1).CheckNeighbors(dir, toMove);
                    } else box1Movable = true; // There isn't a box here

                    if (grid[row - 1, cols.col2] == '[')
                    {
                        box2Movable = bigBoxes.Find(box => box.row == row - 1 && box.cols.col1 == cols.col2).CheckNeighbors(dir, toMove);
                    }
                    else box2Movable = true; // There isn't a box here
                    
                    

                    break;
                case Directions.Down:
                    // If true, not a junction
                    if (grid[row + 1, cols.col1] == '[')
                    {
                        return bigBoxes.Find(box => box.row == row + 1 && box.cols.col1 == cols.col1).CheckNeighbors(dir, toMove);
                    }
                    
                    // Okay, junction time
                    if (grid[row + 1, cols.col1] == ']')
                    {
                        box1Movable = bigBoxes.Find(box => box.row == row + 1 && box.cols.col2 == cols.col1).CheckNeighbors(dir, toMove);
                    } else box1Movable = true; // There isn't a box here

                    if (grid[row + 1, cols.col2] == '[')
                    {
                        box2Movable = bigBoxes.Find(box => box.row == row + 1 && box.cols.col1 == cols.col2).CheckNeighbors(dir, toMove);
                    }
                    else box2Movable = true; // There isn't a box here

                    break;
                case Directions.Left:
                    if (grid[row, cols.col1 - 1] == ']')
                    {
                        box1Movable = bigBoxes.Find(box => box.row == row && box.cols.col2 == cols.col1 - 1).CheckNeighbors(dir, toMove);
                    }

                    break;
                case Directions.Right:
                    if (grid[row, cols.col2 + 1] == '[')
                    {
                        box1Movable = bigBoxes.Find(box => box.row == row && box.cols.col1 == cols.col2 + 1).CheckNeighbors(dir, toMove);
                    }

                    break;
            }
            
            return (box1Movable && box2Movable);
            
        }

        public (int, int) GetDirectionWeight(Directions dir)
        {
            if (dir == Directions.Right) return (0, 2);
            if (dir == Directions.Left) return (0, -2);
            if (dir == Directions.Up) return (-1, 0);
            if (dir == Directions.Down) return (1, 0);

            return (0, 0);
        }

        public void EraseOnGrid()
        {
            grid[row, cols.col1] = '.';
            grid[row, cols.col2] = '.';
        }
        
        public void DrawOnGrid()
        {
            grid[row, cols.col1] = '[';
            grid[row, cols.col2] = ']';
        }
    }
    
    public static string Name = "-- Day 15: Day 15 -- ";
    public static char[,] grid;
    public static char[,] gridWide;
    public static List<Directions> moves = new List<Directions>();
    public static List<BigBox> bigBoxes = new List<BigBox>();

    public static void Day15Main()
    {
        string[] input = File.ReadAllLines(Client.filePrefix + "Day 15 - WarehouseWoes.txt");
        grid = ParseInput(input);
        var (row, col) = FindStart(grid);
        
        
        // Part 1
        Robot robot = new Robot(row, col, grid);
        foreach (Directions dir in moves)
        {
            robot.UpdatePosition(dir, 0);
        }

        int sum = CalcGPS(grid, 'O');
        Console.WriteLine($"Calculated a box sum of {sum}.");
        
        // Part 2
        gridWide = ParseInputWide(input);
        (row, col) = FindStart(gridWide);
        Robot scraggleBot = new Robot(row, col, gridWide);
        
        for (int i = 0; i < moves.Count; i++)
        {
            scraggleBot.UpdatePosition(moves[i], i);
            if (i > 8165) PrintGrid(gridWide);
        }
        
        int sumWide = CalcGPS(gridWide, '[');
        Console.WriteLine($"Calculated a widened sum of {sumWide}.");
    }

    public static char[,] ParseInput(string[] input)
    {
        moves = new();
        int gridWidth = input[0].Length;
        int gridHeight = 0;
        for (int i = 0; i < input.Length; i++)
        {
            if (input[i] == "")
            {
                gridHeight = i;
                break;
            }
        }

        char[,] grid = new char[gridHeight, gridWidth];

        for (int i = 0; i < input.Length; i++)
        {
            if (i < gridHeight)
            {
                for (int j = 0; j < input[i].Length; j++)
                {
                    grid[i, j] = input[i][j];
                }
            }

            if (i > gridHeight)
            {
                foreach (char c in input[i])
                {
                    if (c == '<') moves.Add(Directions.Left);
                    else if (c == '>') moves.Add(Directions.Right);
                    else if (c == '^') moves.Add(Directions.Up);
                    else if (c == 'v') moves.Add(Directions.Down);
                }
            }
        }

        return grid;
    }

    public static char[,] ParseInputWide(string[] input)
    {
        char[,] gridTemp = ParseInput(input);
        char[,] gridWide = new char[gridTemp.GetLength(0), gridTemp.GetLength(1) * 2];
        for (int i = 0; i < gridTemp.GetLength(0); i++)
        {
            for (int j = 0; j < gridTemp.GetLength(1) * 2; j += 2)
            {
                if (gridTemp[i, j / 2] == '#')
                {
                    gridWide[i, j] = '#';
                    gridWide[i, j + 1] = '#';
                } else if (gridTemp[i, j / 2] == '.')
                {
                    gridWide[i, j] = '.';
                    gridWide[i, j + 1] = '.';
                } else if (gridTemp[i, j / 2] == 'O')
                {
                    gridWide[i, j] = '[';
                    gridWide[i, j + 1] = ']';
                    bigBoxes.Add(new BigBox(i, (j, j + 1), null));
                } else if (gridTemp[i, j / 2] == '@')
                {
                    gridWide[i, j] = '@';
                    gridWide[i, j + 1] = '.';
                }
            }
        }

        foreach (BigBox b in bigBoxes) b.grid = gridWide;
        return gridWide;
    }

    public static (int, int) FindStart(char[,] grid)
    {
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                if (grid[i, j] == '@') return (i, j);
            }
        }

        // Uh oh
        return (-1, -1);
    }
    
    public static int CalcGPS(char[,] grid, char boxIdentifier)
    {
        int sum = 0;
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                if (grid[i, j] == boxIdentifier) sum += (100 * i) + j;
            }
        }

        return sum;
    }
    
    public static (int, int) GetDirectionWeight(Directions direction)
    {
        if (direction == Directions.Up) return (-1, 0);
        if (direction == Directions.Down) return (1, 0);
        if (direction == Directions.Left) return (0, -1);
        if (direction == Directions.Right) return (0, 1);

        return (0, 0);
    }

    public static void PrintGrid(char[,] grid)
    {
        string[] gridStr = new string[grid.GetLength(0)];
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            string line = "";
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                line += grid[i, j];
            }

            gridStr[i] = line;
        }
        
        File.WriteAllLines(Client.filePrefix + "output.txt", gridStr);
    }

    public static void PrintGridToConsole(char[,] grid, Directions dir, int moveCount)
    {
        string[] gridStr = new string[grid.GetLength(0)];
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            string line = "";
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                line += grid[i, j];
            }

            gridStr[i] = line;
        }
        
        foreach (string line in gridStr) Console.WriteLine(line);
        Console.WriteLine($"Latest move: {dir.ToString()}");
        Console.WriteLine($"On move {moveCount}/{moves.Count}");
    }
}