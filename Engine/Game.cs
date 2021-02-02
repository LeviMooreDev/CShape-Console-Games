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

        private static void SetWindowSize()
        {
            short fontSize = 16;

            //get console font
            Kernel32.Font.CONSOLE_FONT_INFOEX font = new Kernel32.Font.CONSOLE_FONT_INFOEX();
            font.cbSize = (uint)Marshal.SizeOf(font);
            Kernel32.Font.GetCurrentConsoleFontEx(Kernel32.Generic.GetStdHandle(Kernel32.Generic.StdHandle.Output), false, ref font);

            //try to set window and buffer size, if it fails we decrease the font size and try again
            while (true)
            {
                if (fontSize < 1)
                {
                    throw new Exception("Unable to find a font size that allows for the given game size");
                }

                try
                {
                    Console.SetWindowSize(Size.x, Size.y);
                    Console.SetBufferSize(Size.x, Size.y);
                    break;
                }
                catch (ArgumentOutOfRangeException)
                {
                    fontSize--;
                    font.dwFontSize.Y = fontSize;
                    font.dwFontSize.X = fontSize;
                    Kernel32.Font.SetCurrentConsoleFontEx(Kernel32.Generic.GetStdHandle(Kernel32.Generic.StdHandle.Output), false, ref font);
                }
            }
        }
        private static void CenterWindow()
        {
            //get console size
            IntPtr consoleId = Kernel32.Window.GetForegroundWindow();
            Kernel32.Generic.RECT consoleRect = new Kernel32.Generic.RECT();
            Kernel32.Window.GetWindowRect(consoleId, ref consoleRect);
            int consoleWidth = consoleRect.right - consoleRect.left;
            int consoleHeight = consoleRect.bottom - consoleRect.top;

            //get screen size
            IntPtr desktopId = Kernel32.Window.GetDesktopWindow();
            Kernel32.Generic.RECT screenRect = new Kernel32.Generic.RECT();
            Kernel32.Window.GetWindowRect(desktopId, ref screenRect);
            int screenWidth = screenRect.right - screenRect.left;
            int screenHeight = screenRect.bottom - screenRect.top;

            //move console to center of screen
            Kernel32.Window.MoveWindow(consoleId, screenWidth / 2 - consoleWidth / 2,
                                           screenHeight / 2 - consoleHeight / 2,
                                           consoleWidth, consoleHeight, true);
        }

        /// <summary>
        /// Start the game update and render loops
        /// </summary>
        public static void Begin(string title, Vector2I size)
        {
            Console.Title = title;
            Size = size;

            SetWindowSize();
            CenterWindow();

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
