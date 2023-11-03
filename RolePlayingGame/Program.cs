using RolePlayingGame.Data;
using RolePlayingGame.Entities;
using RolePlayingGame.Models;
using System.Runtime.CompilerServices;

namespace RolePlayingGame
{
    public class Program
    {
        public static Screen screen = Screen.MainMenu;
        public static Entity player;
        static void Main(string[] args)
        {
            while (true)
            {
                switch (screen)
                {
                    case Screen.MainMenu:
                        screen = MainMenuScreen();
                        break;
                    case Screen.CharacterSelect:
                        screen = CharacterSelect();
                        using (var context = new RolePlayingGameDbContext())
                        {
                            var character = new Character()
                            {
                                Name = player.GetType().Name,
                                Agility = player.Agility,
                                Strenght = player.Strenght,
                                Intelligence = player.Intelligence,
                                Mana = player.Mana,
                                Health = player.Health,
                                Damage = player.Damage,
                                Range = player.Range,
                                Symbol = player.Symbol,
                            };

                            context.Characters.Add(character);
                            context.SaveChanges();  
                        }
                        break;
                    case Screen.InGame:
                        screen = InGame();
                        break;
                    case Screen.Exit:
                        screen = Exit();
                        break;
                }
            }
        }

        public static Screen MainMenuScreen()
        {
            Console.WriteLine("Welcome!");
            Console.WriteLine("Press any key to play.");

            Console.ReadKey();

            return Screen.CharacterSelect;
        }

        public static Screen CharacterSelect()
        {
            Console.WriteLine("Choose character type:");
            Console.WriteLine("Options:");
            Console.WriteLine("1) Warrior");
            Console.WriteLine("2) Archer");
            Console.WriteLine("3) Mage");

            int choice = 0;

            if (int.TryParse(Console.ReadLine(), out choice))
            {
                switch (choice)
                {
                    case 1:
                        player = new Warrior();
                        break; 
                    case 2:
                        player = new Archer();
                        break;
                    case 3:
                        player = new Mage();
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please choose between given options!");
                        return Screen.CharacterSelect;
                }
            }
            else
            {
                Console.WriteLine("Invalid input.Please enter valid number!");
                return Screen.CharacterSelect;
            }

            Console.WriteLine("Would you like to buff up your stats before starting?");
            Console.WriteLine("Response (Y\\N):");

            string response = Console.ReadLine()!;

            if (response.ToUpper() == "Y")
            {
                int points = 3;
                while (points > 0)
                {
                    Console.WriteLine($"Remaining Points: {points}");
                    Console.WriteLine("Add to Strenght:");
                    if (int.TryParse(Console.ReadLine(), out int strengthPoints)
                        && strengthPoints >= 0
                        && strengthPoints <= points)
                    {
                        player.Strenght += strengthPoints;
                        points -= strengthPoints;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter valid number!");
                    }

                    Console.WriteLine($"Remaining Points: {points}");
                    Console.WriteLine("Add to Agility:");

                    if (int.TryParse(Console.ReadLine(), out int agilityPoints)
                        && agilityPoints >= 0
                        && agilityPoints <= points)
                    {
                        player.Agility += agilityPoints;
                        points -= agilityPoints;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter valid number!");
                    }

                    Console.WriteLine($"Remaining Points: {points}");
                    Console.WriteLine("Add to Intelligence:");

                    if (int.TryParse(Console.ReadLine(), out int intelligencePoints)
                        && intelligencePoints >= 0
                        && intelligencePoints <= points)
                    {
                        player.Intelligence += intelligencePoints;
                        points -= intelligencePoints;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter valid number!");
                    }
                }
            }

            return Screen.InGame;

        }


        public static Screen InGame()
        {
            TaskManager taskManager = new TaskManager(player);

            while (player.Health > 0)
            {
                Point pointMonster = taskManager.StartMonsterPosition();
                Console.WriteLine();
                Console.WriteLine($"Health: {player.Health}  Mana:{player.Mana}");
                taskManager.DrawGameBoard();
                

                Console.WriteLine("Choose action:");
                Console.WriteLine("1) Attack");
                Console.WriteLine("2) Move");

                Dictionary<Monster, Point> monstersInRange = taskManager.MonstersInHeroRange(taskManager.Monsters, player);
                int command = int.Parse(Console.ReadLine()!);
                if (command == 1)
                {
                    if (monstersInRange.Count == 0)
                    {
                        Console.WriteLine("No available targets in your range");
                    }
                    else
                    {
                        for (int i = 0; i < monstersInRange.Count; i++)
                        {
                            Console.WriteLine($"{i} target with remaining blood {monstersInRange.Keys.ElementAt(i).Health}");
                        }

                        Console.WriteLine("Which one to attack:");
                        int selectedMonsterIndex = int.Parse(Console.ReadLine()!);
                        Monster selectedMonster = monstersInRange.Keys.ElementAt(selectedMonsterIndex);
                        Point monsterPoint = monstersInRange[selectedMonster];
                        int monsterX = monsterPoint.X;  
                        int monsterY = monsterPoint.Y;

                        //Check if monster is next to the hero
                        if (Math.Abs(monsterX - taskManager.CoordinatesHero.X) == 1 && 
                            Math.Abs(monsterY - taskManager.CoordinatesHero.Y) == 1)
                        {
                            int damage = selectedMonster.Damage;
                            player.Health -= damage;
                            Console.WriteLine($"You were attacked by the monster for {damage} damage.");
                        }
                        //Check selected monster's health
                        if (selectedMonster.Health == 0)
                        {
                            taskManager.RemoveMonsterFromMap(selectedMonster);
                            break;
                        }

                    }
                }
                else if (command == 2)
                {
                    Console.WriteLine("Choose your move!");
                    char move = char.ToUpper(Console.ReadKey().KeyChar);

                    taskManager.MoveHero(move);
                }

                //logic move monster and etc
                foreach (var monster in monstersInRange)
                {
                    if (taskManager.MonstersInHeroRange(monstersInRange, player).Count != 0)
                    {
                        player.Health -= monster.Key.Health;
                        if (player.Health == 0)
                        {
                            break;
                        }
                    }
                    else
                    {
                        var newPoint = taskManager.MoveMonster(monster.Key, player);
                        monster.Value.X = newPoint.X;
                        monster.Value.Y = newPoint.Y;
                    }
                }
            }
            return Screen.Exit;

        }

        public static Screen Exit()
        {
            Console.WriteLine("Game over! Do you want to play again? Y/N");

            string response = Console.ReadLine()!;
            if (response.ToLower() == "N")
            {
                Environment.Exit(0);
            }
            else if (response.ToLower() == "Y")
            {
                return Screen.MainMenu;
            }

            return Screen.MainMenu;

        }

    }
}