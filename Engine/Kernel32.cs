using System;
using System.Runtime.InteropServices;

namespace Engine
{
    internal static class Kernel32
    {
        /// <summary>
        /// Retrieves a handle to the specified standard device (standard input, standard output, or standard error).
        /// https://docs.microsoft.com/en-us/windows/console/getstdhandle
        /// </summary>
        /// <returns>If the function succeeds, the return value is a handle to the specified device, or a redirected handle set by a previous call to SetStdHandle.</returns>
        [DllImport("kernel32")]
        internal static extern IntPtr GetStdHandle(StdHandle index);
        internal enum StdHandle
        {
            OutputHandle = -11
        }

        #region Buffer
        /// <summary>
        /// Creates a new console screen buffer.
        /// https://docs.microsoft.com/en-us/windows/console/createconsolescreenbuffer
        /// </summary>
        /// <param name="dwDesiredAccess">The access to the console screen buffer.</param>
        /// <param name="dwShareMode">A pointer to a SECURITY_ATTRIBUTES structure that determines whether the returned handle can be inherited by child processes</param>
        /// <param name="secutiryAttributes"></param>
        /// <param name="flags">The type of console screen buffer to create. The only supported screen buffer type is CONSOLE_TEXTMODE_BUFFER.</param>
        /// <param name="screenBufferData">Reserved; should be Null.</param>
        /// <returns>If the function succeeds, the return value is a handle to the new console screen buffer.</returns>
        [DllImport("Kernel32.dll")]
        internal static extern IntPtr CreateConsoleScreenBuffer(
            uint dwDesiredAccess,
            uint dwShareMode,
            IntPtr secutiryAttributes,
            uint flags,
            IntPtr screenBufferData
        );

        /// <summary>
        /// Sets the specified screen buffer to be the currently displayed console screen buffer.
        /// https://docs.microsoft.com/en-us/windows/console/createconsolescreenbuffer
        /// </summary>
        /// <param name="hConsoleOutput">A handle to the console screen buffer.</param>
        /// <returns>If the function succeeds, the return value is nonzero.</returns>
        [DllImport("Kernel32.dll")]
        internal static extern bool SetConsoleActiveScreenBuffer(
            IntPtr hConsoleOutput
        );

        /// <summary>
        /// Copies a number of characters to consecutive cells of a console screen buffer, beginning at a specified location.
        /// https://docs.microsoft.com/en-us/windows/console/writeconsoleoutputcharacter
        /// </summary>
        /// <param name="hConsoleOutput">A handle to the console screen buffer. The handle must have the GENERIC_WRITE access right.</param>
        /// <param name="lpCharacter">The characters to be written to the console screen buffer.</param>
        /// <param name="nLength">The number of characters to be written.</param>
        /// <param name="dwWriteCoord">A COORD structure that specifies the character coordinates of the first cell in the console screen buffer to which characters will be written.</param>
        /// <param name="lpNumberOfCharsWritten">A pointer to a variable that receives the number of characters actually written.</param>
        /// <returns>If the function succeeds, the return value is nonzero.</returns>
        [DllImport("Kernel32.DLL", EntryPoint = "WriteConsoleOutputCharacterW", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        internal static extern bool WriteConsoleOutputCharacter
        (
            IntPtr hConsoleOutput,
            string lpCharacter,
            uint nLength,
            COORD dwWriteCoord,
            out uint lpNumberOfCharsWritten
        );

        /// <summary>
        /// Copies a number of character attributes to consecutive cells of a console screen buffer, beginning at a specified location.
        /// https://docs.microsoft.com/en-us/windows/console/writeconsoleoutputattribute
        /// </summary>
        /// <param name="hConsoleOutput">A handle to the console screen buffer. The handle must have the GENERIC_WRITE access right.</param>
        /// <param name="lpAttribute">The attributes to be used when writing to the console screen buffer.</param>
        /// <param name="nLength">The number of screen buffer character cells to which the attributes will be copied.</param>
        /// <param name="dwWriteCoord">A COORD structure that specifies the character coordinates of the first cell in the console screen buffer to which the attributes will be written.</param>
        /// <param name="lpNumberOfAttrsWritten">A pointer to a variable that receives the number of attributes actually written to the console screen buffer.</param>
        /// <returns>If the function succeeds, the return value is nonzero.</returns>
        [DllImport("kernel32.dll")]
        internal static extern bool WriteConsoleOutputAttribute(
            IntPtr hConsoleOutput,
            ushort[] lpAttribute,
            uint nLength,
            COORD dwWriteCoord,
            out uint lpNumberOfAttrsWritten
        );
        #endregion

        #region Font
        /// <summary>
        /// Sets extended information about the current console font.
        /// https://docs.microsoft.com/en-us/windows/console/setcurrentconsolefontex
        /// </summary>
        /// <param name="ConsoleOutput">A handle to the console screen buffer. The handle must have the GENERIC_WRITE access right.</param>
        /// <param name="MaximumWindow">If this parameter is TRUE, font information is set for the maximum window size. If this parameter is FALSE, font information is set for the current window size.</param>
        /// <param name="ConsoleCurrentFontEx">A pointer to a CONSOLE_FONT_INFOEX structure that contains the font information.</param>
        /// <returns>If the function succeeds, the return value is nonzero.</returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern int SetCurrentConsoleFontEx(
            IntPtr ConsoleOutput,
            bool MaximumWindow,
            ref CONSOLE_FONT_INFOEX ConsoleCurrentFontEx
        );

        /// <summary>
        /// Retrieves extended information about the current console font.
        /// https://docs.microsoft.com/en-us/windows/console/getcurrentconsolefontex
        /// </summary>
        /// <param name="hConsoleOutput">A handle to the console screen buffer. The handle must have the GENERIC_READ access right.</param>
        /// <param name="bMaximumWindow">If this parameter is TRUE, font information is retrieved for the maximum window size. If this parameter is FALSE, font information is retrieved for the current window size.</param>
        /// <param name="lpConsoleCurrentFont">A pointer to a CONSOLE_FONT_INFOEX structure that receives the requested font information.</param>
        /// <returns>If the function succeeds, the return value is nonzero.</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal extern static bool GetCurrentConsoleFontEx(
            IntPtr hConsoleOutput,
            bool bMaximumWindow,
            ref CONSOLE_FONT_INFOEX lpConsoleCurrentFont
        );

        /// <summary>
        /// Contains extended information for a console font.
        /// https://docs.microsoft.com/en-us/windows/console/console-font-infoex
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct CONSOLE_FONT_INFOEX
        {
            /// <summary>
            /// The size of this structure, in bytes. This member must be set to sizeof(CONSOLE_FONT_INFOEX) before calling GetCurrentConsoleFontEx or it will fail.
            /// </summary>
            public uint cbSize;
            /// <summary>
            /// The index of the font in the system's console font table.
            /// </summary>
            public uint nFont;
            /// <summary>
            ///  COORD structure that contains the width and height of each character in the font, in logical units. The X member contains the width, while the Y member contains the height.
            /// </summary>
            public COORD dwFontSize;
            /// <summary>
            /// The font pitch and family. For information about the possible values for this member, see the description of the tmPitchAndFamily member of the TEXTMETRIC structure.
            /// </summary>
            public int FontFamily;
            /// <summary>
            /// The font weight. The weight can range from 100 to 1000, in multiples of 100. For example, the normal weight is 400, while 700 is bold.
            /// </summary>
            public int FontWeight;

            /// <summary>
            /// The name of the typeface (such as Courier or Arial).
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)] // Edit sizeconst if the font name is too big
            public string FaceName;
        }
        #endregion

        #region Window
        /// <summary>
        /// Retrieves a handle to the foreground window (the window with which the user is currently working). The system assigns a slightly higher priority to the thread that creates the foreground window than it does to other threads.
        /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getforegroundwindow
        /// </summary>
        /// <returns>The return value is a handle to the foreground window. The foreground window can be NULL in certain circumstances, such as when a window is losing activation.</returns>
        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        internal static extern IntPtr GetForegroundWindow();

        /// <summary>
        /// Retrieves a handle to the desktop window. The desktop window covers the entire screen. The desktop window is the area on top of which other windows are painted.
        /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getdesktopwindow
        /// </summary>
        /// <returns>The return value is a handle to the desktop window.</returns>
        [DllImport("user32.dll", SetLastError = false)]
        internal static extern IntPtr GetDesktopWindow();

        /// <summary>
        /// Retrieves the dimensions of the bounding rectangle of the specified window. The dimensions are given in screen coordinates that are relative to the upper-left corner of the screen.
        /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getwindowrect
        /// </summary>
        /// <param name="hWnd">A handle to the window.</param>
        /// <param name="rect">A pointer to a RECT structure that receives the screen coordinates of the upper-left and lower-right corners of the window.</param>
        /// <returns>If the function succeeds, the return value is nonzero.</returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, ExactSpelling = true, SetLastError = true)]
        internal static extern bool GetWindowRect(
            IntPtr hWnd,
            ref RECT rect
        );

        /// <summary>
        /// Changes the position and dimensions of the specified window. For a top-level window, the position and dimensions are relative to the upper-left corner of the screen. For a child window, they are relative to the upper-left corner of the parent window's client area.
        /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-movewindow
        /// </summary>
        /// <param name="hwnd">A handle to the window.</param>
        /// <param name="X">The new position of the left side of the window.</param>
        /// <param name="Y">The new position of the top of the window.</param>
        /// <param name="nWidth">The new width of the window.</param>
        /// <param name="nHeight">The new height of the window.</param>
        /// <param name="bRepaint">Indicates whether the window is to be repainted. If this parameter is TRUE, the window receives a message. If the parameter is FALSE, no repainting of any kind occurs. This applies to the client area, the nonclient area (including the title bar and scroll bars), and any part of the parent window uncovered as a result of moving a child window.</param>
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, ExactSpelling = true, SetLastError = true)]
        internal static extern void MoveWindow(
            IntPtr hwnd, 
            int X, 
            int Y, 
            int nWidth, 
            int nHeight, 
            bool bRepaint
        );
        #endregion

        #region Structs
        /// <summary>
        /// Defines the coordinates of a character cell in a console screen buffer. The origin of the coordinate system (0,0) is at the top, left cell of the buffer.
        /// https://docs.microsoft.com/en-us/windows/console/coord-str
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        internal struct COORD
        {
            /// <summary>
            /// The horizontal coordinate or column value. The units depend on the function call.
            /// </summary>
            public short X;
            /// <summary>
            /// The vertical coordinate or row value. The units depend on the function call.
            /// </summary>
            public short Y;

            public COORD(short X, short Y)
            {
                this.X = X;
                this.Y = Y;
            }
        };

        /// <summary>
        /// The RECT structure defines a rectangle by the coordinates of its upper-left and lower-right corners.
        /// https://docs.microsoft.com/en-us/windows/win32/api/windef/ns-windef-rect
        /// </summary>
        internal struct RECT
        {
            /// <summary>
            /// Specifies the x-coordinate of the upper-left corner of the rectangle.
            /// </summary>
            public int left;
            /// <summary>
            /// Specifies the y-coordinate of the upper-left corner of the rectangle.
            /// </summary>
            public int top;
            /// <summary>
            /// Specifies the x-coordinate of the lower-right corner of the rectangle.
            /// </summary>
            public int right;
            /// <summary>
            /// Specifies the y-coordinate of the lower-right corner of the rectangle.
            /// </summary>
            public int bottom;
        }
        #endregion
    }
}
