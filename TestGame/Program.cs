using System;
using System.Threading;
using Engine;

namespace TestGame
{
    class Program
    {
        static void Main(string[] args)
        {
            int width = Console.LargestWindowWidth / 2;
            int height = Console.LargestWindowHeight / 2;

            Game.OnStart += Start;
            Game.OnUpdate += Update;
            Game.Begin("Test Game", new Vector2(120, 30));
        }

        private static void Start()
        {

        }

        private static void Update()
        {
            Random random = new Random();
            Array colorValues = Enum.GetValues(typeof(Color));

            Render.DrawText(Vector2.Zero, "hej med dig!");

            if (Input.KeyHold(ConsoleKey.A))
            {
                for (int x = 0; x < Game.Size.x; x++)
                {
                    for (int y = 0; y < Game.Size.y; y++)
                    {
                        char text = random.Next(0, 10).ToString()[0];
                        Render.DrawText(new Vector2(x, y), text);
                    }
                }
            }
            if (Input.KeyHold(ConsoleKey.S))
            {
                for (int x = 0; x < Game.Size.x; x++)
                {
                    for (int y = 0; y < Game.Size.y; y++)
                    {
                        Color textColor = (Color)colorValues.GetValue(random.Next(colorValues.Length));
                        Color backgroundColor = (Color)colorValues.GetValue(random.Next(colorValues.Length));
                        Render.DrawTextColor(new Vector2(x, y), textColor);
                        Render.DrawBackgroundColor(new Vector2(x, y), backgroundColor);
                    }
                }
            }
            if (Input.KeyHold(ConsoleKey.D))
            {
                Render.Clear();
            }
        }
    }
}
