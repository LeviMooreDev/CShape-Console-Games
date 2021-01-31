using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace Engine
{
    public class Game
    {
        public readonly Vector2I size;

        private readonly int bufferLength;
        private readonly char[] textBuffer;
        /// <summary>
        /// The colors applied to textBuffer. See Color enum for values.
        /// </summary>
        private readonly ushort[] colorBuffer;
        /// <summary>
        /// console window handle used when calling Kernel32 methods
        /// </summary>
        private readonly IntPtr consoleHandle;

        private int frameCount;

        /// <summary>
        /// triggered every frame
        /// </summary>
        public event Action OnUpdate;

        public Game(string title, Vector2I size)
        {
            Console.Title = title;
            this.size = size;

            SetWindowSize();
            CenterWindow();

            //create buffers and set ours as the active console buffer
            bufferLength = size.x * size.y;
            textBuffer = new char[bufferLength];
            colorBuffer = new ushort[bufferLength];
            consoleHandle = Kernel32.CreateConsoleScreenBuffer(0x80000000 | 0x40000000, 0, IntPtr.Zero, 1, IntPtr.Zero);
            Kernel32.SetConsoleActiveScreenBuffer(consoleHandle);

            //start thread that counts the frame rate. this needs to be replaced.
            new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(1000);
                    Console.Title = $"{title} - FPS: {frameCount}";
                    frameCount = 0;
                }
            }).Start();
        }
    
        private void SetWindowSize()
        {
            short fontSize = 16;

            //get console font
            Kernel32.CONSOLE_FONT_INFOEX font = new Kernel32.CONSOLE_FONT_INFOEX();
            font.cbSize = (uint)Marshal.SizeOf(font);
            Kernel32.GetCurrentConsoleFontEx(Kernel32.GetStdHandle(Kernel32.StdHandle.OutputHandle), false, ref font);

            //try to set window and buffer size, if it fails we decrease the font size and try again
            while (true)
            {
                if (fontSize < 1)
                {
                    throw new Exception("Unable to find a font size that allows for the given game size");
                }

                try
                {
                    Console.SetWindowSize(size.x, size.y);
                    Console.SetBufferSize(size.x, size.y);
                    break;
                }
                catch (ArgumentOutOfRangeException)
                {
                    fontSize--;
                    font.dwFontSize.Y = fontSize;
                    font.dwFontSize.X = fontSize;
                    Kernel32.SetCurrentConsoleFontEx(Kernel32.GetStdHandle(Kernel32.StdHandle.OutputHandle), false, ref font);
                }
            }
        }
        private void CenterWindow()
        {
            //get console size
            IntPtr consoleId = Kernel32.GetForegroundWindow();
            Kernel32.RECT consoleRect = new Kernel32.RECT();
            Kernel32.GetWindowRect(consoleId, ref consoleRect);
            int consoleWidth = consoleRect.right - consoleRect.left;
            int consoleHeight = consoleRect.bottom - consoleRect.top;

            //get screen size
            IntPtr desktopId = Kernel32.GetDesktopWindow();
            Kernel32.RECT screenRect = new Kernel32.RECT();
            Kernel32.GetWindowRect(desktopId, ref screenRect);
            int screenWidth = screenRect.right - screenRect.left;
            int screenHeight = screenRect.bottom - screenRect.top;

            //move console to center of screen
            Kernel32.MoveWindow(consoleId, screenWidth / 2 - consoleWidth / 2,
                                           screenHeight / 2 - consoleHeight / 2,
                                           consoleWidth, consoleHeight, true);
        }

        /// <summary>
        /// Start the game update and render loops
        /// </summary>
        public void Start()
        {
            Kernel32.COORD dwWriteCoord = new Kernel32.COORD(0, 0);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character">Needs to be Unicode</param>
        public void Draw(char character, Vector2I position, Color foregroundColor = Color.White, Color backgroundColor = Color.Black)
        {
            int index = position.y * size.x + position.x;
                        textBuffer[index] = character;
            colorBuffer[index] = (ushort)((ushort)foregroundColor | (ushort)backgroundColor << 4);
        }
    }
}
