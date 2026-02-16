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
            }

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
