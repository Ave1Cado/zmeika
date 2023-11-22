using System;
using System.Collections.Generic;
using System.Threading;

enum Border
{
    MaxRight = 39,
    MaxBottom = 19
}

class SnakeGame
{
    private List<Position> snake;
    private Position food;
    private Direction direction;
    private bool isGameOver;

    public SnakeGame()
    {
        snake = new List<Position>();
        snake.Add(new Position(10, 10));
        direction = Direction.Right;
        isGameOver = false;
        GenerateFood();
    }

    public void StartGame()
    {
        Console.CursorVisible = false;

        while (!isGameOver)
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                ChangeDirection(key.Key);
            }

            UpdateSnake();
            DrawGame();
            Thread.Sleep(100);
        }

        Console.Clear();
        Console.WriteLine("Game Over!");
    }

    private void UpdateSnake()
    {
        Position head = snake[0].Move(direction);

        if (head.X < 1 || head.X >= (int)Border.MaxRight || head.Y < 1 || head.Y >= (int)Border.MaxBottom)
        {
            isGameOver = true;
            return;
        }

        if (snake.Contains(head))
        {
            isGameOver = true;
            return;
        }

        snake.Insert(0, head);

        if (head.Equals(food))
        {
            GenerateFood();
        }
        else
        {
            snake.RemoveAt(snake.Count - 1);
        }
    }

    private void DrawGame()
    {
        Console.Clear();

        DrawBorders();

        foreach (Position segment in snake)
        {
            Console.SetCursorPosition(segment.X, segment.Y);
            Console.Write("■");
        }

        Console.SetCursorPosition(food.X, food.Y);
        Console.Write("O");
    }

    private void DrawBorders()
    {
        Console.SetCursorPosition(0, 0);
        Console.Write("+");

        for (int i = 1; i <= (int)Border.MaxRight; i++)
        {
            Console.Write("-");
        }

        Console.Write("+");

        for (int i = 1; i <= (int)Border.MaxBottom; i++)
        {
            Console.SetCursorPosition(0, i);
            Console.Write("|");

            Console.SetCursorPosition((int)Border.MaxRight + 1, i);
            Console.Write("|");
        }

        Console.SetCursorPosition(0, (int)Border.MaxBottom + 1);
        Console.Write("+");

        for (int i = 1; i <= (int)Border.MaxRight; i++)
        {
            Console.Write("-");
        }

        Console.Write("+");
    }

    private void ChangeDirection(ConsoleKey key)
    {
        switch (key)
        {
            case ConsoleKey.UpArrow:
                direction = Direction.Up;
                break;
            case ConsoleKey.DownArrow:
                direction = Direction.Down;
                break;
            case ConsoleKey.LeftArrow:
                direction = Direction.Left;
                break;
            case ConsoleKey.RightArrow:
                direction = Direction.Right;
                break;
        }
    }

    private void GenerateFood()
    {
        Random random = new Random();
        int x = random.Next(1, (int)Border.MaxRight);
        int y = random.Next(1, (int)Border.MaxBottom);
        food = new Position(x, y);

        while (snake.Contains(food))
        {
            x = random.Next(1, (int)Border.MaxRight);
            y = random.Next(1, (int)Border.MaxBottom);
            food = new Position(x, y);
        }
    }
}

class Position
{
    public int X { get; set; }
    public int Y { get; set; }

    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }

    public Position Move(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                return new Position(X, Y - 1);
            case Direction.Down:
                return new Position(X, Y + 1);
            case Direction.Left:
                return new Position(X - 1, Y);
            case Direction.Right:
                return new Position(X + 1, Y);
            default:
                return this;
        }
    }

    public override bool Equals(object obj)
    {
        if (obj is Position)
        {
            Position other = (Position)obj;
            return X == other.X && Y == other.Y;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return X.GetHashCode() ^ Y.GetHashCode();
    }
}

enum Direction
{
    Up,
    Down,
    Left,
    Right
}

class Program
{
    static void Main(string[] args)
    {
        SnakeGame game = new SnakeGame();
        game.StartGame();
    }
}
