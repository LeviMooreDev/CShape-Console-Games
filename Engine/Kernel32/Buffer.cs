using System;
using System.Runtime.InteropServices;

namespace Engine.Kernel32
{
    internal static class Buffer
    {
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
            Generic.COORD dwWriteCoord,
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
            Generic.COORD dwWriteCoord,
            out uint lpNumberOfAttrsWritten
        );
    }
}
