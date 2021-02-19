using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;

namespace Engine
{
    public static class Game
    {
        public static Vector2 Size { get; private set; }

        public static event Action OnStart;
        public static event Action OnUpdate;

        private static DateTime lastTime = DateTime.Now;
        private static DateTime currentTime = DateTime.Now;
        private static DateTime nextFrameRateUpdate = DateTime.Now;
        private static int frameRate;
        public static float DeltaTime { get; private set; }

        /// <summary>
        /// Start the game update and render loops
        /// </summary>
        public static void Begin(string title, Vector2 size)
        {
            Console.Title = title;
            Size = size;

            Render.Start();
            Input.Start();
            OnStart?.Invoke();


            while (true)
            {
                currentTime = DateTime.Now;
                DeltaTime = (float)currentTime.Subtract(lastTime).TotalSeconds;
                lastTime = currentTime;
                frameRate++;
                if (currentTime >= nextFrameRateUpdate)
                {
                    nextFrameRateUpdate = currentTime.AddSeconds(1);
                    Console.Title = $"{title} - {frameRate}";
                    frameRate = 0;
                }

                Input.Update();
                OnUpdate?.Invoke();
                Render.Update();
            }
        }
    }
}
