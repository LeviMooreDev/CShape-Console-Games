using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Engine
{
    internal static class Kernel32
    {
        [DllImport("Kernel32.dll")]
        internal static extern IntPtr CreateConsoleScreenBuffer(
            uint dwDesiredAccess,
            uint dwShareMode,
            IntPtr secutiryAttributes,
            uint flags,
            IntPtr screenBufferData
        );

        [DllImport("Kernel32.dll")]
        internal static extern bool SetConsoleActiveScreenBuffer(
            IntPtr hConsoleOutput
        );

        [DllImport("Kernel32.DLL", EntryPoint = "WriteConsoleOutputCharacterW", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        internal static extern bool WriteConsoleOutputCharacter
        (
            IntPtr hConsoleOutput,
            string lpCharacter,
            uint nLength,
            COORD dwWriteCoord,
            out uint lpNumberOfCharsWritten
        );
        [DllImport("kernel32.dll")]
        internal static extern bool WriteConsoleOutputAttribute(
            IntPtr hConsoleOutput,
            ushort[] lpAttribute,
            uint nLength,
            COORD dwWriteCoord,
            out uint lpNumberOfAttrsWritten
        );

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern int SetCurrentConsoleFontEx(
            IntPtr ConsoleOutput,
            bool MaximumWindow,
            ref CONSOLE_FONT_INFO_EX ConsoleCurrentFontEx
        );

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal extern static bool GetCurrentConsoleFontEx(
            IntPtr hConsoleOutput,
            bool bMaximumWindow,
            ref CONSOLE_FONT_INFO_EX lpConsoleCurrentFont
        );

        [DllImport("kernel32")]
        internal static extern IntPtr GetStdHandle(StdHandle index);

        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        internal static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = false)]
        internal static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, ExactSpelling = true, SetLastError = true)]
        internal static extern bool GetWindowRect(
            IntPtr hWnd,
            ref RECT rect
        );

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, ExactSpelling = true, SetLastError = true)]
        internal static extern void MoveWindow(
            IntPtr hwnd, 
            int X, 
            int Y, 
            int nWidth, 
            int nHeight, 
            bool bRepaint
        );

        [StructLayout(LayoutKind.Sequential)]
        internal struct COORD
        {
            public short X;
            public short Y;

            public COORD(short X, short Y)
            {
                this.X = X;
                this.Y = Y;
            }
        };

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct CONSOLE_FONT_INFO_EX
        {
            public uint cbSize;
            public uint nFont;
            public COORD dwFontSize;
            public int FontFamily;
            public int FontWeight;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)] // Edit sizeconst if the font name is too big
            public string FaceName;
        }

        internal struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        internal enum StdHandle
        {
            OutputHandle = -11
        }
    }
}
