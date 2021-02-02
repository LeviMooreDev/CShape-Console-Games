using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;

namespace Engine
{
    public static class Game
    {
        public static Vector2I Size { get; private set; }

        public static event Action OnStart;
        public static event Action OnUpdate;

        /// <summary>
        /// Start the game update and render loops
        /// </summary>
        public static void Begin(string title, Vector2I size)
        {
            Console.Title = title;
            Size = size;

            Render.Start();
            Input.Start();
            OnStart?.Invoke();

            while (true)
            {
                Input.Update();
                OnUpdate?.Invoke();
                Render.Update();
            }
        }
    }
}
