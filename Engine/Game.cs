using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;

namespace Engine
{
    public class Game
    {
        public readonly Vector2I size;
        private readonly int bufferLength;
        private readonly char[] textBuffer;
        private readonly ushort[] colorBuffer;
        private readonly IntPtr consoleHandle;
        private readonly Kernel32.COORD dwWriteCoord = new Kernel32.COORD(0, 0);

        private int frameCount;

        public event Action OnUpdate;

        public Game(string title, Vector2I size)
        {
            this.size = size;
            bufferLength = size.x * size.y;
            textBuffer = new char[bufferLength];
            colorBuffer = new ushort[bufferLength];

            Console.Title = title;
            Console.CursorVisible = false;

            short fontSize = 16;
            Kernel32.CONSOLE_FONT_INFO_EX font = new Kernel32.CONSOLE_FONT_INFO_EX();
            font.cbSize = (uint)Marshal.SizeOf(font);
            Kernel32.GetCurrentConsoleFontEx(Kernel32.GetStdHandle(Kernel32.StdHandle.OutputHandle), false, ref font);
            while (true)
            {
                try
                {
                    Console.SetWindowSize(size.x, size.y);
                    Console.SetBufferSize(size.x, size.y);
                    break;
                }
                catch (Exception)
                {
                    fontSize--;
                    font.dwFontSize.Y = fontSize;
                    font.dwFontSize.X = fontSize;
                    Kernel32.SetCurrentConsoleFontEx(Kernel32.GetStdHandle(Kernel32.StdHandle.OutputHandle), false, ref font);
                }
            }

            consoleHandle = Kernel32.CreateConsoleScreenBuffer(0x80000000 | 0x40000000, 0, IntPtr.Zero, 1, IntPtr.Zero);
            Kernel32.SetConsoleActiveScreenBuffer(consoleHandle);

            Center();

            new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(1000);
                    Console.Title = "FPS: " + frameCount;
                    frameCount = 0;
                }
            }).Start();
        }
    
        private void Center()
        {
            IntPtr consoleId = Kernel32.GetForegroundWindow();
            Kernel32.RECT consoleRect = new Kernel32.RECT();
            Kernel32.GetWindowRect(consoleId, ref consoleRect);
            int consoleWidth = consoleRect.right - consoleRect.left;
            int consoleHeight = consoleRect.bottom - consoleRect.top;

            IntPtr desktopId = Kernel32.GetDesktopWindow();
            Kernel32.RECT screenRect = new Kernel32.RECT();
            Kernel32.GetWindowRect(desktopId, ref screenRect);
            int screenWidth = screenRect.right - screenRect.left;
            int screenHeight = screenRect.bottom - screenRect.top;

            Kernel32.MoveWindow(consoleId, screenWidth / 2 - consoleWidth / 2,
                                           screenHeight / 2 - consoleHeight / 2,
                                           consoleWidth, consoleHeight, true);
        }

        public void Start()
        {
            while (true)
            {
                frameCount++;

                OnUpdate?.Invoke();

                Kernel32.WriteConsoleOutputCharacter(consoleHandle, new string(textBuffer), (uint)bufferLength, dwWriteCoord, out _);
                Kernel32.WriteConsoleOutputAttribute(consoleHandle, colorBuffer, (uint)bufferLength, dwWriteCoord, out _);

                Console.WindowTop = 0;
                Thread.Sleep(1000/60);
            }
        }

        public void Draw(char text, Color textColor, Color backgroundColor, Vector2I position)
        {
            int index = position.y * size.x + position.x;
            textBuffer[index] = text;
            colorBuffer[index] = (ushort)((ushort)textColor | (ushort)backgroundColor << 4);
        }
    }
}
