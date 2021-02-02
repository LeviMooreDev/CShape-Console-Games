using System;
using System.Runtime.InteropServices;

namespace Engine.Kernel32
{
    internal static class Font
    {
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
            public Generic.COORD dwFontSize;
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
    }
}
