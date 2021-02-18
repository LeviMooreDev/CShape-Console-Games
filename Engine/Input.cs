using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Engine
{
    public static class Input
    {
        private static Vector2 mousePosition = new Vector2(0, 0);
        public static Vector2 MousePosition { get; private set; } = new Vector2(0, 0);
        public static Vector2 LastMousePosition { get; private set; } = new Vector2(0, 0);

        private static bool mouseLeft;
        public static bool MouseLeftDown { get; private set; }
        public static bool MouseLeftHold { get; private set; }
        public static bool MouseLeftUp { get; private set; }

        private static bool mouseRight;
        public static bool MouseRightDown { get; private set; }
        public static bool MouseRightHold { get; private set; }
        public static bool MouseRightUp { get; private set; }

        private static bool mouseMiddle;
        public static bool MouseMiddleDown { get; private set; }
        public static bool MouseMiddleHold { get; private set; }
        public static bool MouseMiddleUp { get; private set; }

        private static bool mouseScrollUp;
        public static bool MouseScrollUp { get; private set; }
        private static bool mouseScrollDown;
        public static bool MouseScrollDown { get; private set; }

        public static bool AnyKeyDown { get; private set; }
        public static bool AnyKeyHold { get; private set; }
        public static bool AnyKeyUp { get; private set; }

        private static ConsoleKey[] consoleKeys;
        private static Dictionary<ConsoleKey, bool> key;
        private static Dictionary<ConsoleKey, bool> keyDown;
        private static Dictionary<ConsoleKey, bool> keyHold;
        private static Dictionary<ConsoleKey, bool> keyUp;

        private static string text = string.Empty;
        public static string Text { get; private set; } = string.Empty;

        public static bool KeyDown(ConsoleKey key)
        {
            return keyDown[key];
        }
        public static bool KeyHold(ConsoleKey key)
        {
            return keyHold[key];
        }
        public static bool KeyUp(ConsoleKey key)
        {
            return keyUp[key];
        }

        internal static void Start()
        {
            IntPtr handle = Kernel32.Generic.GetStdHandle(Kernel32.Generic.StdHandle.Input);

            //set console mode, remove quick edit (mouse dragging)
            int mode = 0;
            if (!Kernel32.Input.GetConsoleMode(handle, ref mode))
            {
                throw new Exception("Unable to get input handle");
            }
            mode |= Kernel32.Input.ENABLE_MOUSE_INPUT;
            mode &= ~Kernel32.Input.ENABLE_QUICK_EDIT_MODE;
            if (!Kernel32.Input.SetConsoleMode(handle, mode))
            {
                throw new Exception("Unable to set console mode");
            }

            consoleKeys = (ConsoleKey[])Enum.GetValues(typeof(ConsoleKey));
            key = new Dictionary<ConsoleKey, bool>();
            keyDown = new Dictionary<ConsoleKey, bool>();
            keyHold = new Dictionary<ConsoleKey, bool>();
            keyUp = new Dictionary<ConsoleKey, bool>();
            foreach (ConsoleKey consoleKey in consoleKeys)
            {
                key.Add(consoleKey, false);
                keyDown.Add(consoleKey, false);
                keyHold.Add(consoleKey, false);
                keyUp.Add(consoleKey, false);
            }

            new Thread(() =>
            {
                Kernel32.Input.INPUT_RECORD record = new Kernel32.Input.INPUT_RECORD();
                while (true)
                {
                    uint recordLen = 0;
                    if (!Kernel32.Input.ReadConsoleInput(handle, ref record, 1, ref recordLen))
                    {
                        throw new Exception("Unable to read console input");
                    }

                    switch (record.EventType)
                    {
                        case Kernel32.Input.MOUSE_EVENT:
                            {
                                mousePosition.Set(record.MouseEvent.dwMousePosition.X, record.MouseEvent.dwMousePosition.Y);
                                mouseLeft = record.MouseEvent.dwButtonState == 1;
                                mouseRight = record.MouseEvent.dwButtonState == 2;
                                mouseMiddle = record.MouseEvent.dwButtonState == 4;
                                mouseScrollUp = record.MouseEvent.dwButtonState == 7864320;
                                mouseScrollDown = record.MouseEvent.dwButtonState == -7864320;
                            }
                            break;

                        case Kernel32.Input.KEY_EVENT:
                            {
                                ConsoleKey consoleKey = (ConsoleKey)record.KeyEvent.wVirtualKeyCode;
                                if (record.KeyEvent.bKeyDown)
                                {
                                    key[consoleKey] = true;
                                }
                                else
                                {
                                    key[consoleKey] = false;
                                }

                                text += record.KeyEvent.UnicodeChar;

                                bool leftControl = record.KeyEvent.dwControlKeyState == 8;
                                bool rightControl = record.KeyEvent.dwControlKeyState == 260;
                                bool control = leftControl || rightControl;
                                bool shift = record.KeyEvent.dwControlKeyState == 16;
                                bool alt = record.KeyEvent.dwControlKeyState == 2;
                                bool controlAlt = record.KeyEvent.dwControlKeyState == 265;
                            }
                            break;
                    }
                }
            }).Start();
        }
        internal static void Update()
        {
            LastMousePosition = MousePosition;
            MousePosition = mousePosition;

            MouseLeftUp = !mouseLeft && MouseLeftHold;
            MouseLeftDown = mouseLeft && !MouseLeftHold;
            MouseLeftHold = mouseLeft;

            MouseRightUp = !mouseRight && MouseRightHold;
            MouseRightDown = mouseRight && !MouseRightHold;
            MouseRightHold = mouseRight;

            MouseMiddleUp = !mouseMiddle && MouseMiddleHold;
            MouseMiddleDown = mouseMiddle && !MouseMiddleHold;
            MouseMiddleHold = mouseMiddle;

            MouseScrollUp = mouseScrollUp;
            mouseScrollUp = false;

            MouseScrollDown = mouseScrollDown;
            mouseScrollDown = false;

            AnyKeyUp = false;
            AnyKeyDown = false;
            AnyKeyHold = false;
            foreach (var consoleKey in consoleKeys)
            {
                keyUp[consoleKey] = !key[consoleKey] && keyHold[consoleKey];
                keyDown[consoleKey] = key[consoleKey] && !keyHold[consoleKey];
                keyHold[consoleKey] = key[consoleKey];

                AnyKeyDown = keyDown[consoleKey] || AnyKeyDown;
                AnyKeyHold = keyHold[consoleKey] || AnyKeyHold;
                AnyKeyUp = keyUp[consoleKey] || AnyKeyUp;
            }

            Text = text;
            text = string.Empty;
        }
    }
}
