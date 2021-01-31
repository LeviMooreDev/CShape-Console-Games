using System;
using Engine;

namespace TestGame
{
    class Program
    {
        private static Game game;

        static void Main(string[] args)
        {
            int width = Console.LargestWindowWidth / 2;
            int height = Console.LargestWindowHeight / 2;

            game = new Game("Test Game", new Vector2I(120, 30));
            game.OnUpdate += Update;
            game.Start();
        }

        private static void Update()
        {
            Random random = new Random();
            Array colorValues = Enum.GetValues(typeof(Color));

            for (int x = 0; x < game.size.x; x++)
            {
                for (int y = 0; y < game.size.y; y++)
                {
                    char text = random.Next(0, 10).ToString()[0];
                    Color textColor = (Color)colorValues.GetValue(random.Next(colorValues.Length));
                    Color backgroundColor = (Color)colorValues.GetValue(random.Next(colorValues.Length));
                    game.Draw(text, new Vector2I(x, y), textColor, backgroundColor);
                }
            }
        }
    }
}
