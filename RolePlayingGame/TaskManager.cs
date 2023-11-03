using Microsoft.Identity.Client;
using RolePlayingGame.Entities;
using RolePlayingGame.Models;
using System.ComponentModel;
using System.Drawing;
using System.Linq.Expressions;

namespace RolePlayingGame
{
    public class TaskManager
    {
        public Entity Entity { get; set; }
        public char[,] Map { get; set; }
        public Point CoordinatesHero { get; set; }
        public Dictionary<Monster, Point> Monsters { get; set; }

        public const char MatrixSymbol = '▒';

        public TaskManager(Entity entity)
        {
            this.Entity = entity;
            this.Monsters = new Dictionary<Monster, Point>();
            this.Map = new char[10, 10];
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Map[i, j] = '▒';
                }
            }
            this.Map[1, 1] = this.Entity.Symbol;
            this.CoordinatesHero = new Point(1, 1);

        }

        public void DrawGameBoard()
        {
            for (int i = 0; i < Map.GetLength(0); i++)
            {
                for (int j = 0; j < Map.GetLength(1); j++)
                {
                    Console.Write(Map[i, j]);
                }
                Console.WriteLine();
            }
        }

        public Point StartMonsterPosition()
        {
            Point monsterPoint;
            while (true)
            {
                Monster monster = new Monster();
                int monsterX = new Random().Next(0, this.Map.GetLength(0) - 1);
                int monsterY = new Random().Next(0, this.Map.GetLength(1) - 1);

                if (!isMonsterPosition(monsterX, monsterY) && !isHeroPosition(monsterX, monsterY))
                {
                    monsterPoint = new Point(monsterX, monsterY);
                    this.Monsters.Add(monster, monsterPoint);
                    this.Map[monsterX, monsterY] = monster.Symbol;
                    break;
                }
            }
            return monsterPoint;
        }

        public void MoveHero(char direction)
        {
            
            switch (direction)
            {
                case 'W': //Move Up
                    if(this.ValidateCordinates(CoordinatesHero.X - 1, CoordinatesHero.Y))
                    {
                        this.Map[CoordinatesHero.X, CoordinatesHero.Y] = MatrixSymbol;
                        this.CoordinatesHero.X--;
                        this.Map[CoordinatesHero.X, CoordinatesHero.Y] = Entity.Symbol;
                    }
                    else
                    {
                        Console.WriteLine("Invalid move!");
                    }
                    break;
                case 'S': //Move down 
                    if (this.ValidateCordinates(CoordinatesHero.X + 1, CoordinatesHero.Y))
                    {
                        this.Map[CoordinatesHero.X, CoordinatesHero.Y] = MatrixSymbol;
                        this.CoordinatesHero.X++;
                        this.Map[CoordinatesHero.X, CoordinatesHero.Y] = Entity.Symbol;
                    }
                    else
                    {
                        Console.WriteLine("Invalid move!");
                    }
                    break;
                case 'D': //Move right
                    if (this.ValidateCordinates(CoordinatesHero.X, CoordinatesHero.Y + 1))
                    {
                        this.Map[CoordinatesHero.X, CoordinatesHero.Y] = '▒';
                        this.CoordinatesHero.Y++;
                        this.Map[CoordinatesHero.X, CoordinatesHero.Y] = Entity.Symbol;
                    }
                    else
                    {
                        Console.WriteLine("Invalid move!");
                    }
                    break;
                case 'A': //Move left
                    if (this.ValidateCordinates(CoordinatesHero.X, CoordinatesHero.Y - 1))
                    {
                        this.Map[CoordinatesHero.X, CoordinatesHero.Y] = '▒';
                        this.CoordinatesHero.Y--;
                        this.Map[CoordinatesHero.X, CoordinatesHero.Y] = Entity.Symbol;
                    }
                    else
                    {
                        Console.WriteLine("Invalid move!");
                    }
                    break;
                case 'E': //Move diagonally up & right
                    if (this.ValidateCordinates(CoordinatesHero.X - 1, CoordinatesHero.Y + 1))
                    {
                        this.Map[CoordinatesHero.X, CoordinatesHero.Y] = '▒';
                        this.CoordinatesHero.Y++;
                        this.CoordinatesHero.X--;
                        this.Map[CoordinatesHero.X, CoordinatesHero.Y] = Entity.Symbol;
                    }
                    else
                    {
                        Console.WriteLine("Invalid move!");
                    }
                    break;
                case 'X': //Move diagonally down & right
                    if (this.ValidateCordinates(CoordinatesHero.X + 1, CoordinatesHero.Y + 1))
                    {
                        this.Map[CoordinatesHero.X, CoordinatesHero.Y] = '▒';
                        this.CoordinatesHero.Y++;
                        this.CoordinatesHero.X++;
                        this.Map[CoordinatesHero.X, CoordinatesHero.Y] = Entity.Symbol;
                    }
                    else
                    {
                        Console.WriteLine("Invalid move!");
                    }
                    break;
                case 'Q': //Move diagonally up & left
                    if (this.ValidateCordinates(CoordinatesHero.X - 1, CoordinatesHero.Y - 1))
                    {
                        this.Map[CoordinatesHero.X, CoordinatesHero.Y] = '▒';
                        this.CoordinatesHero.Y--;
                        this.CoordinatesHero.X--;
                        this.Map[CoordinatesHero.X, CoordinatesHero.Y] = Entity.Symbol;
                    }
                    else
                    {
                        Console.WriteLine("Invalid move!");
                    }
                    break;
                case 'Z': //Move diagonally down & left
                    if (this.ValidateCordinates(CoordinatesHero.X + 1, CoordinatesHero.Y - 1))
                    {
                        this.Map[CoordinatesHero.X, CoordinatesHero.Y] = '▒';
                        this.CoordinatesHero.Y--;
                        this.CoordinatesHero.X++;
                        this.Map[CoordinatesHero.X, CoordinatesHero.Y] = Entity.Symbol;
                    }
                    else
                    {
                        Console.WriteLine("Invalid move!");
                    }
                    break;
                default:
                    Console.WriteLine("Invalid symbol!");
                    break;
            }

        }

        public void RemoveMonsterFromMap(Monster monster)
        {
            var killedMonster = this.Monsters.First(m => m.Key == monster);
            this.Monsters.Remove(monster);
            this.Map[killedMonster.Value.X, killedMonster.Value.Y] = MatrixSymbol;
        }

        public Point MoveMonster(Monster monster, Entity hero)
        {
            var searchedMonster = this.Monsters.First(m => m.Key == monster);
            int currPositionX = searchedMonster.Value.X;
            int currPositionY = searchedMonster.Value.Y;

            int newPositionX;
            int newPositionY;   

            int diffX = Math.Abs(this.CoordinatesHero.X - currPositionX);
            int diffY = Math.Abs(this.CoordinatesHero.Y - currPositionY);

            if (diffY == Math.Max(diffX, diffY))
            {
                int newY = currPositionY;
                if (this.CoordinatesHero.Y > diffY)
                {
                    newY++;
                }
                else
                {
                    newY--;
                }

                newPositionX = currPositionX;
                newPositionY = newY;   
            }
            else
            {
                int newX = currPositionX;
                if (this.CoordinatesHero.X > diffX)
                {
                    newX++;
                }
                else
                {
                    newX--;
                }

                newPositionX = newX;
                newPositionY = currPositionY;
            }

            if (!ValidateCordinates(newPositionX, newPositionY)
                && !isMonsterPosition(newPositionX, newPositionY))
            {
                this.Map[currPositionX, currPositionY] = MatrixSymbol;
                this.Map[newPositionX, newPositionY] = '!';

                return new Point(newPositionX, newPositionY);
            }

            return new Point(currPositionX, currPositionY);
            
        }

        public static void BuffUpStats(Entity player)
        {
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

        }

        private bool ValidateCordinates(int x, int y)
        {
            return x >= 0 && y >= 0 && x < this.Map.GetLength(0) && y < this.Map.GetLength(1);
        }

        private bool isMonsterPosition(int x, int y)
        {
            return this.Map[x, y] == '!';
        }

        private bool isHeroPosition(int x, int y) 
        {
            return this.Map[x, y] == this.Entity.Symbol;
        }

        public Dictionary<Monster, Point> MonstersInHeroRange(Dictionary<Monster, Point> monsters, Entity hero)
        {
            Dictionary<Monster, Point> monstersInRange = new Dictionary<Monster, Point>();
            foreach (var monster in monsters)
            {
                int rangeX = Math.Abs(monster.Value.X - this.CoordinatesHero.X);
                int rangeY = Math.Abs(monster.Value.Y - this.CoordinatesHero.Y);

                if (rangeX <= hero.Range && rangeY <= hero.Range)
                {
                    monstersInRange.Add(monster.Key, monster.Value);
                }
            }

            return monstersInRange;
        }

    }
}
