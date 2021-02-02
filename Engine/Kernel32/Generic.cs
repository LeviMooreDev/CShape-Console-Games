using System;
using System.Runtime.InteropServices;

namespace Engine.Kernel32
{
    internal static class Generic
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
            Output = -11,
            Input = -10
        }

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
    }
}
