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
            Game.Begin("Test Game", new Vector2(120, 60));
        }

        private static void Start()
        {

        }

        private static void Update()
        {
            Render.Clear();
            Render.DrawLine(Vector2.One * 5, Input.MousePosition);
        }
    }
}
