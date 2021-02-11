using System;
using System.Runtime.InteropServices;

namespace Engine
{
    public class Render
    {
        private static IntPtr consoleHandle;
        private static Kernel32.Generic.COORD dwWriteCoord = new Kernel32.Generic.COORD(0, 0);

        private static int bufferLength;
        private static char[] textBuffer;
        private static ushort[] colorBuffer;

        internal static void Start()
        {
            bufferLength = Game.Size.x * Game.Size.y;
            textBuffer = new char[bufferLength];
            colorBuffer = new ushort[bufferLength];
            Clear();

            SetFont();
            CenterWindow();

            consoleHandle = Kernel32.Buffer.CreateConsoleScreenBuffer(0x80000000 | 0x40000000, 0, IntPtr.Zero, 1, IntPtr.Zero);
            Kernel32.Buffer.SetConsoleActiveScreenBuffer(consoleHandle);
        }
        internal static void Update()
        {
            Kernel32.Buffer.WriteConsoleOutputCharacter(consoleHandle, new string(textBuffer), (uint)bufferLength, dwWriteCoord, out _);
            Kernel32.Buffer.WriteConsoleOutputAttribute(consoleHandle, colorBuffer, (uint)bufferLength, dwWriteCoord, out _);
        }

        private static void SetFont()
        {
            //font
            short fontSize = 8;
            Kernel32.Font.CONSOLE_FONT_INFOEX font = new Kernel32.Font.CONSOLE_FONT_INFOEX();
            Kernel32.Font.GetCurrentConsoleFontEx(Kernel32.Generic.GetStdHandle(Kernel32.Generic.StdHandle.Output), false, ref font);
            font.FaceName = "Terminal";
            font.dwFontSize.Y = fontSize;
            font.dwFontSize.X = fontSize;
            font.cbSize = (uint)Marshal.SizeOf(font);
            Kernel32.Font.SetCurrentConsoleFontEx(Kernel32.Generic.GetStdHandle(Kernel32.Generic.StdHandle.Output), false, ref font);

            //try to set window and buffer size, if it fails we decrease the font size and try again
            while (true)
            {
                if (fontSize < 1)
                {
                    throw new Exception("Unable to find a font size that allows for the given game size");
                }

                try
                {
                    Console.SetWindowSize(Game.Size.x, Game.Size.y);
                    Console.SetBufferSize(Game.Size.x, Game.Size.y);
                    break;
                }
                catch (ArgumentOutOfRangeException)
                {
                    fontSize--;
                    font.dwFontSize.Y = fontSize;
                    font.dwFontSize.X = fontSize;
                    font.cbSize = (uint)Marshal.SizeOf(font);
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

        public static void Clear()
        {
            for (int i = 0; i < bufferLength; i++)
            {
                textBuffer[i] = ' ';
                colorBuffer[i] = (ushort)Color.White | (ushort)Color.Black << 4;
            }
        }

        //all draw methods needs a better way of handling Vector2I, Vector2F, and float

        public static void DrawText(Vector2I position, char character)
        {
            if (GetIndex(position, out int index))
            {
                textBuffer[index] = character;
            }
        }
        public static void DrawText(Vector2I position, string text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                if (GetIndex(position + Vector2I.Right * i, out int index))
                {
                    textBuffer[index] = text[i];
                }
            }
        }
        public static void DrawText(Vector2F position, char character)
        {
            if (GetIndex(new Vector2I(position.x, position.y), out int index))
            {
                textBuffer[index] = character;
            }
        }
        public static void DrawText(Vector2F position, string text)
        {
            Vector2I positionI = new Vector2I(position.x, position.y);
            for (int i = 0; i < text.Length; i++)
            {
                if (GetIndex(positionI + Vector2I.Right * i, out int index))
                {
                    textBuffer[index] = text[i];
                }
            }
        }
        public static void DrawText(int x, int y, char character)
        {
            if (GetIndex(new Vector2I(x, y), out int index))
            {
                textBuffer[index] = character;
            }
        }
        public static void DrawText(int x, int y, string text)
        {
            Vector2I position = new Vector2I(x, y);
            for (int i = 0; i < text.Length; i++)
            {
                if (GetIndex(position + Vector2I.Right * i, out int index))
                {
                    textBuffer[index] = text[i];
                }
            }
        }
        public static void DrawText(float x, float y, char character)
        {
            if (GetIndex(new Vector2I(x, y), out int index))
            {
                textBuffer[index] = character;
            }
        }
        public static void DrawText(float x, float y, string text)
        {
            Vector2I position = new Vector2I(x, y);
            for (int i = 0; i < text.Length; i++)
            {
                if (GetIndex(position + Vector2I.Right * i, out int index))
                {
                    textBuffer[index] = text[i];
                }
            }
        }

        public static void DrawBackgroundColor(Vector2I position, Color color)
        {
            //how to get only text color from colorBuffer (right 4)
            //filter    = 1111 0000
            //buffer    = 0101 1001
            // | (or)   = 1111 1001
            // ^ (excl) = 0000 1001

            if (GetIndex(position, out int index))
            {
                int textColor = (colorBuffer[index] | 240) ^ 240;
                colorBuffer[index] = (ushort)((ushort)color << 4 | textColor);
            }
        }
        public static void DrawBackgroundColor(Vector2F position, Color color)
        {
            if (GetIndex(new Vector2I(position.x, position.y), out int index))
            {
                int textColor = (colorBuffer[index] | 240) ^ 240;
                colorBuffer[index] = (ushort)((ushort)color << 4 | textColor);
            }
        }
        public static void DrawBackgroundColor(int x, int y, Color color)
        {
            if (GetIndex(new Vector2I(x, y), out int index))
            {
                int textColor = (colorBuffer[index] | 240) ^ 240;
                colorBuffer[index] = (ushort)((ushort)color << 4 | textColor);
            }
        }
        public static void DrawBackgroundColor(float x, float y, Color color)
        {
            if (GetIndex(new Vector2I(x, y), out int index))
            {
                int textColor = (colorBuffer[index] | 240) ^ 240;
                colorBuffer[index] = (ushort)((ushort)color << 4 | textColor);
            }
        }

        public static void DrawTextColor(Vector2I position, Color color)
        {
            //how to get only background color from colorBuffer (left 4)
            //buffer              = 0101 1001
            // >> 4 (shift right) = 0000 0101
            // << 4 (shift left)  = 0101 0000

            //if you want to use or and excl like in DrawBackgroundColor use this line
            //int backgroundColor = (colorBuffer[index] | 15) ^ 15;

            if (GetIndex(position, out int index))
            {
                int backgroundColor = colorBuffer[index] >> 4 << 4;
                colorBuffer[index] = (ushort)(backgroundColor | (ushort)color);
            }
        }
        public static void DrawTextColor(Vector2F position, Color color)
        {
            if (GetIndex(new Vector2I(position.x, position.y), out int index))
            {
                int backgroundColor = colorBuffer[index] >> 4 << 4;
                colorBuffer[index] = (ushort)(backgroundColor | (ushort)color);
            }
        }
        public static void DrawTextColor(int x, int y, Color color)
        {
            if (GetIndex(new Vector2I(x, y), out int index))
            {
                int backgroundColor = colorBuffer[index] >> 4 << 4;
                colorBuffer[index] = (ushort)(backgroundColor | (ushort)color);
            }
        }
        public static void DrawTextColor(float x, float y, Color color)
        {
            if (GetIndex(new Vector2I(x, y), out int index))
            {
                int backgroundColor = colorBuffer[index] >> 4 << 4;
                colorBuffer[index] = (ushort)(backgroundColor | (ushort)color);
            }
        }

        private static bool GetIndex(Vector2I position, out int index)
        {
            index = position.y * Game.Size.x + position.x;
            return true;
        }
    }
}
