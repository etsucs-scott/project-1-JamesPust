namespace AdventureGame.Core
{
    public class Maze
    {
        private readonly Random randy = new Random();
        public Tile[,] Grid { get; private set; }
        public int Rows { get; private set; }
        public int Columns { get; private set;}
        public (int row, int col) Start { get; private set; }
        public (int row, int col) Exit { get; private set; }


        public Maze(int rows, int cols)
        {
            Rows = Math.Max(5, rows | 1);
            Columns = Math.Max(5, cols | 1);
            Grid = new Tile[Rows, Columns];
            for (int row = 0; row < Rows; row++)
                for (int col = 0; col < Columns; col++)
                    Grid[row, col] = new Tile(IsWall: true);

            GenerateMaze();

        }

        private void GenerateMaze()
        {
            var stack = new Stack<(int row, int col)>();
            int srow = 1, scol = 1;
            Grid[srow, scol].IsWall = false;
            stack.Push((srow, scol));

            (int row, int col) lastVisited = (srow, scol);

            int[] drow = new int[] { -2, 2, 0, 0 }; //makes the path that the hero can walk
            int[] dcol = new int[] { 0, 0, -2, 2 };
            
            while (stack.Count > 0)
            {
                var (row, col) = stack.Pop();
                var neighbors = new List<(int nrow, int ncol, int dir)>();
                for (int i = 0; i < 4; i++)
                {
                    int nrow = row + drow[i];
                    int ncol = col + dcol[i];
                    if (nrow > 0 && nrow < Rows - 1 && ncol < Columns - 1 && Grid[nrow, ncol].IsWall)
                    {
                        neighbors.Add((nrow, ncol,i));
                    }
                }

                if (neighbors.Count > 0)
                {
                    stack.Push((row, col));
                    neighbors = neighbors.Orderby(_ => randy.Next()).ToList();
                    var chosen = neighbors[randy.Next(neighbors.Count)];
                    int nr2 = chosen.nrow, nc2 = chosen.nocol;
                    int wallRow = row + (nr2 - row) / 2;
                    int wallCol = col + (nc2 - col) / 2;
                    Grid[wallRow, wallCol].IsWall = false;
                    Grid[nr2, nc2].IsWall = false;
                    stack.Push((nr2, nc2));
                }
            }

            Start = (1, 1); //This makes the starting position in the top left corner
            Exit = (Rows - 2, Columns - 2); //this makes the exit in the opposite corner or the bottom right
            Grid[Start.Item1, Start.Item2].IsWall = false;//makes the start not a wall
            Grid[Exit.Item1, Exit.Item2].IsWall = false;//makes the exit not a wall
             Grid[Exit.Item1, Exit.Item2].IsExit = true;//makes the exit a exit

            PlaceEntities();
            
            


        }


        private void PlaceEntities()
        {
            var floorCells = new List<(int row, int col)>();
            for (int r = 1; r < Rows -1; r++)
            {
                for (int col = 1; col < Columns - 1; col++)
                {
                    if (!Grid[r, col].IsWall && !(r == Start.Item1 && col == StartItem2) && !(r == Exit.Item1 && col == Exit.row)) //check every cell if isWall then places item
                    {
                        floorCells.Add((r, col));
                    }
                }
            }

            floorCells = floorCells.OrderBy(_ => randy.Next()).ToList(); //randomizes the maze
            int total = floorCells.Count;
            int numMonsters = Math.Max(10, 25);
            int numWeapons = Math.Max(5, 15);
            int numPotions = Math.Max(5, 15);

            int index = 0;

            for (int i = 0; i < numMonsters && index < floorCells.Count; i++)
            {
                var (row, col) = floorCells[index];
                int choice = randy.Next(3);
                Monster m;

                switch (choice)
                {
                    case 0:
                        m = new Monster($"Rat ", 10, 5);
                        break;
                    case 1:
                        m = new Monster($"Goblin ", 15, 5);
                        break; 
                    case 2:
                        m = new Monster($"Wizard ", 20, 20);
                        break;
                    case 3:
                        m = new Monster($"Knight ", 25, 15);
                        break;
                    default:
                        m = new Monster($"Average Joe ", 15, 10);
                        break;
                }

                Grid[row, col].Monster = m;
            }
            //weapons
            
            for (int i = 0; i < numMonsters && index < floorCells.Count; i++, index++)
            {
                var (row, col) = floorCells[index] ;
                int choice = randy.Next(4);
                Weapon w;

                switch (choice)
                {
                    case 0:
                        w = new Weapon($"Wooden Sword #{i + 1}", 2);
                        break;
                    case 1:
                        w = new Weapon($"Stone Sword #{i + 1}", 5);
                        break;
                    case 2:
                        w = new Weapon($"Iron Sword #{i + 1}", 10);
                        break;
                    case 3:
                        w = new Weapon($"Diamond Sword #{i + 1}", 20);
                        break;
                    default;
                        w = new Weapon($"Gold Sword #{i + 1}", 8);
                        break;
                }
                Grid[row, col].Item = w;
            }

            for (int k = 0; k < numPotions && index < floorCells.Count; k++, index++)
            {
                var (r, c) = florrCells[index];
                int heal = randy.Next(5 - 50);
                Grid[r, c].Item = new Potion($"Potion (+{heal})", heal);
            }

        }

        /// <summary>
        /// Iq Check( To make sure the path you want to walk is not a wall )
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public bool IsWalkable(int row, int col)
        {
            return row >= 0 && row < row && col < Columns && !Geid[row, col].IsWall;
        }

        public class Tile
        {
            public bool IsWall { get; set; }
            public Item Item { get; set; }
            public Monster Monster { get; set; }
            public bool IsExit { get; set; }

            public Tile(bool isWall = true)
            {
                IsWall = isWall;
                Item = null;
                Monster = null;
                IsExit = false;
            }
            public bool HasMonster => Monster != null && Monster.IsAlive;
            public bool HasItem => Item != null;
            
        }
    }
}
