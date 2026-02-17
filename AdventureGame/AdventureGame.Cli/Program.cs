using AdventureGame.Core;
namespace AdventureGame.Cli
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false; //not having the cursor visible makes for a cleaner look
            var randy = new Random();

            int rows = 21;
            int cols = 31;

            Maze maze = new Maze(rows, cols);
            var start = maze.Start;
            Player player = new Player("Hero", start.Item1, start.Item2, health: 100, attackpower: 15);
            string introGreeting = "Welcome to the maze game. Reach the exit(E) to win. use WASD or arrow keys to move. Press Q to quit.\n";
            bool gameRunning = true;

            while (gameRunning)
            {
                Console.Clear();
                MakeMaze(maze, player);
                Console.WriteLine();
                Console.WriteLine($"HP: {player,Health}    Base Attack: {player.AttackPower}       Best Weapon: {player.TrueAttack - player.AttackPower}            Total Attack Power: {player.TrueAttack}\n");
                Console.WriteLine($"Inventory: {player.InvSummary()}\n")
                Console.WriteLine();
                if(!string.IsNullOrEmpty(introGreeting))
                {
                    Console.WriteLine(introGreeting);
                }
                else
                {
                    Console.WriteLine();
                }

                if (!player.IsAlive)
                {
                    Console.WriteLine("You died!\n")
                        break;
                }
                
                if (maze.Grid[player.Row, player.Col].isExit)
                {
                    Console.WriteLine("You Won! Great Job!\n")
                        break;
                }

                var key = Console.ReadKey(true).Key;
                int newRow = player.Row, newCol = player.Col;
                bool moved = false;
                if (key == ConsoleKey.W || key == ConsoleKey.UpArrow)
                {
                    newRow--;
                    moved = true;
                }
                else if (key == ConsoleKey.S || key == ConsoleKey.DownArrow)
                {
                    newRow++;
                    moved = true;
                }
                else if (key == ConsoleKey.A || key == ConsoleKey.LeftArrow)
                {
                    newCol--;
                    moved = true;
                }
                else if (key == ConsoleKey.D || key == ConsoleKey.RightArrow)
                {
                    newCol++;
                    moved = true;
                }
                else if (key == ConsoleKey.Q)
                {
                    Console.WriteLine("Quitting The Maze\n")
                    break;
                }
                else  
                {
                    introGreeting = "WASD or Arrow keys to move and Q to quit.\n"
                        continue;
                }

                if (moved)
                {
                    if (maze.IsWalkable(newRow, newCol))
                    {
                        player.Row = newRow;
                        player.Col = newCol;

                        var tile = maze.Grid[player.Row, player.Col];
                        introGreeting = "";

                        if (tile.HasMonster)
                        {
                            var monster = tile.Monster;
                            introGreeting += $"Encountered {monster.Name} (HP:{monster.Health}, Attack: {monster.AttackPower}). \n";

                            while (player.IsAlive && monster.IsAlive)
                            {
                                int playerAttack = player.TrueAttack;//player attacks
                                monster.TakeDamage(playerAttack);
                                introGreeting += $"You attacked {monster.Name} for {playerAttack}dmg. {Math.Max(0, monster.Health)} hp left.\n"
                                if (monster.IsAlive) break;

                                int monsterAttack = monster.AttackPower;//monster attacks
                                player.TakeDamage(monsterAttack);
                                introGreeting += $"{monster.Name} hit you for {monsterAttack}dmg. You have {player.Health}hp left.\n"

                            }

                            if(!player.IsAlive)
                            {
                                introGreeting += $"You have perished in battle.\n
                            }
                            else
                            {
                                introGreeting += "You have won this battle. Now continue."
                                tile.Monster = null;
                            }
                        }

                        //if the player walks on an item
                        if (tile.HasItem)
                        {
                            var item = tile.Item;
                            item.ApplyTo(player);
                            introGreeting += item.Message + "\n"
                            tile.Item = null;
                        }

                    }
                    else
                    {
                        introGreeting += "you cannot go there!";
                    }
                }

            }

            Console.WriteLine();
            Console.WriteLine("Press any key to quit.");
            Console.ReadKey(true);
        }

        static void MakeMaze(Maze maze, Player player)
        {
            for( int j = 0; j < maze.Rows; j++)
            {
                for (int k = 0; k < maze.Columns; k++)
                {
                    var tile = maze.Grid[j, k];

                    if (j == player.Row && k == player.Col)
                    {
                        Console.Write("H"); //Hero
                    }
                    else if (tile.IsWall)
                    {
                        Console.Write("#"); //Wall
                    }
                     else if (tile.IsExit)
                    {
                        Console.Write("E"); //Exit
                    }
                     else if (tile.HasMonster)
                    {
                        Console.Write("M"); //Monster
                    }
                     else if (tile.HasItem)
                    {
                        if (tile.Item is Weapon) Console.Write("W"); //Weapon
                        else if (tile.Item is Potion) Console.Write("P"); //Potion
                        else Console.Write("I"); //anything else
                        
                    }
                     else
                    {
                        Console.Write(" "); //walkable tile
                    }
                    Console.WriteLine();
                }
            }
        }
       
    }
}
