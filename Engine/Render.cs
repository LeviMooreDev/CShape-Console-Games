using System;
using System.Runtime.InteropServices;

namespace Engine
{
    public static class Render
    {
        private static IntPtr consoleHandle;
        private static Kernel32.Generic.COORD dwWriteCoord = new Kernel32.Generic.COORD(0, 0);

        private static int bufferLength;
        private static char[] textBuffer;
        private static ushort[] colorBuffer;

        internal static void Start()
        {
            bufferLength = Game.Size.XInt * Game.Size.YInt;
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
                    Console.SetWindowSize(Game.Size.XInt, Game.Size.YInt);
                    Console.SetBufferSize(Game.Size.XInt, Game.Size.YInt);
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

        public static void DrawText(Vector2 position, char character)
        {
            DrawText(position.XInt, position.YInt, character);
        }
        public static void DrawText(float x, float y, char character)
        {
            DrawText((int)x, (int)y, character);
        }
        public static void DrawText(int x, int y, char character)
        {
            if (GetIndex(new Vector2(x, y), out int index))
            {
                DrawText(index, character);
            }
        }
        public static void DrawText(Vector2 position, string text)
        {
            DrawText(position.XInt, position.YInt, text);
        }
        public static void DrawText(float x, float y, string text)
        {
            DrawText((int)x, (int)y, text);
        }
        public static void DrawText(int x, int y, string text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                if (GetIndex(x + i, y, out int index))
                {
                    DrawText(index, text[i]);
                }
            }
        }
        public static void DrawText(int index, char character)
        {
            textBuffer[index] = character;
        }

        public static void DrawBackgroundColor(Vector2 position, Color color)
        {
            DrawBackgroundColor(position.XInt, position.YInt, color);
        }
        public static void DrawBackgroundColor(float x, float y, Color color)
        {
            DrawBackgroundColor((int)x, (int)y, color);
        }
        public static void DrawBackgroundColor(int x, int y, Color color)
        {
            if (GetIndex(x, y, out int index))
            {
                int textColor = (colorBuffer[index] | 240) ^ 240;
                colorBuffer[index] = (ushort)((ushort)color << 4 | textColor);
            }
        }

        public static void DrawTextColor(Vector2 position, Color color)
        {
            DrawTextColor(position.XInt, position.YInt, color);
        }
        public static void DrawTextColor(float x, float y, Color color)
        {
            DrawTextColor((int)x, (int)y, color);
        }
        public static void DrawTextColor(int x, int y, Color color)
        {
            if (GetIndex(x, y, out int index))
            {
                int backgroundColor = colorBuffer[index] >> 4 << 4;
                colorBuffer[index] = (ushort)(backgroundColor | (ushort)color);
            }
        }

        public static void DrawLine(Vector2 start, Vector2 end, Color color = Color.White)
        {
            Vector2 rayDirection = (end - start).Normalized;
            Vector2 rayLength = Vector2.Zero;

            Vector2 stepSize = new Vector2(MathF.Sqrt(1 + (rayDirection.y / rayDirection.x) * (rayDirection.y / rayDirection.x)), MathF.Sqrt(1 + (rayDirection.x / rayDirection.y) * (rayDirection.x / rayDirection.y)));
            Vector2 stepDirection = Vector2.Zero;
            
            Vector2 position = start;

            if (rayDirection.x < 0)
            {
                stepDirection.x = -1;
                rayLength.x = (start.x - position.x) * stepSize.x;
            }
            else
            {
                stepDirection.x = 1;
                rayLength.x = (position.x + 1 - start.x) * stepSize.x;
            }

            if (rayDirection.y < 0)
            {
                stepDirection.y = -1;
                rayLength.y = (start.y - position.y) * stepSize.y;
            }
            else
            {
                stepDirection.y = 1;
                rayLength.y = (position.y + 1 - start.y) * stepSize.y;
            }

            float maxDistance = Vector2.Distance(start, end);
            float distance = 0f;
            while (distance < maxDistance)
            {
                if (rayLength.x < rayLength.y)
                {
                    position.x += stepDirection.x;
                    distance = rayLength.x;
                    rayLength.x += stepSize.x;
                }
                else
                {
                    position.y += stepDirection.y;
                    distance = rayLength.y;
                    rayLength.y += stepSize.y;
                }

                if (position.x >= 0 && position.x < Game.Size.x && position.y >= 0 && position.y < Game.Size.y)
                {
                    DrawBackgroundColor(position, color);
                }
                else
                {
                    break;
                }
            }
            DrawBackgroundColor(start, color);
            DrawBackgroundColor(end, color);
        }

        public static bool GetIndex(Vector2 position, out int index)
        {
            GetIndex(position.XInt, position.YInt, out index);
            return true;
        }
        public static bool GetIndex(float x, float y, out int index)
        {
            GetIndex((int)x, (int)y, out index);
            return true;
        }
        public static bool GetIndex(int x, int y, out int index)
        {
            index = y * Game.Size.XInt + x;
            return true;
        }
    }
}
