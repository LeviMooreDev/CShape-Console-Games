using System;
using System.Runtime.InteropServices;

namespace Engine.Kernel32
{
    internal static class Input
    {
        internal const int ENABLE_MOUSE_INPUT = 0x0010;
        internal const int ENABLE_QUICK_EDIT_MODE = 0x0040;
        internal const int ENABLE_EXTENDED_FLAGS = 0x0080;
        internal const int KEY_EVENT = 1;
        internal const int MOUSE_EVENT = 2;

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetConsoleMode(
            IntPtr hConsoleHandle, 
            ref int lpMode
        );

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool ReadConsoleInput(
            IntPtr hConsoleInput,
            ref INPUT_RECORD lpBuffer,
            uint nLength,
            ref uint lpNumberOfEventsRead
        );

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetConsoleMode(
            IntPtr hConsoleHandle,
            int dwMode
        );

        [StructLayout(LayoutKind.Explicit)]
        internal struct INPUT_RECORD
        {
            [FieldOffset(0)]
            public short EventType;
            [FieldOffset(4)]
            public KEY_EVENT_RECORD KeyEvent;
            [FieldOffset(4)]
            public MOUSE_EVENT_RECORD MouseEvent;
        }

        internal struct MOUSE_EVENT_RECORD
        {
            public Generic.COORD dwMousePosition;
            public int dwButtonState;
            public int dwControlKeyState;
            public int dwEventFlags;
        }

        [StructLayout(LayoutKind.Explicit)]
        internal struct KEY_EVENT_RECORD
        {
            [FieldOffset(0)]
            [MarshalAs(UnmanagedType.Bool)]
            public bool bKeyDown;
            [FieldOffset(4)]
            public short wRepeatCount;
            [FieldOffset(6)]
            public short wVirtualKeyCode;
            [FieldOffset(8)]
            public short wVirtualScanCode;
            [FieldOffset(10)]
            public char UnicodeChar;
            [FieldOffset(10)]
            public byte AsciiChar;
            [FieldOffset(12)]
            public int dwControlKeyState;
        };
    }
}
