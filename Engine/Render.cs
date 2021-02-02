using System;

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

            consoleHandle = Kernel32.Buffer.CreateConsoleScreenBuffer(0x80000000 | 0x40000000, 0, IntPtr.Zero, 1, IntPtr.Zero);
            Kernel32.Buffer.SetConsoleActiveScreenBuffer(consoleHandle);
        }
        internal static void Update()
        {
            Kernel32.Buffer.WriteConsoleOutputCharacter(consoleHandle, new string(textBuffer), (uint)bufferLength, dwWriteCoord, out _);
            Kernel32.Buffer.WriteConsoleOutputAttribute(consoleHandle, colorBuffer, (uint)bufferLength, dwWriteCoord, out _);
        }

        public static void Clear()
        {
            for (int i = 0; i < bufferLength; i++)
            {
                textBuffer[i] = ' ';
                colorBuffer[i] = (ushort)Color.White | (ushort)Color.Black << 4;
            }
        }
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

        public static void DrawPixel(Vector2I position, Color color)
        {
            DrawText(position, ' ');
            DrawBackgroundColor(position, color);
        }

        private static bool GetIndex(Vector2I position, out int index)
        {
            index = position.y * Game.Size.x + position.x;
            return true;
        }
    }
}
