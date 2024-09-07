using Core.Logs;
using Core.Maths.Vectors;
using Silk.NET.Input;

namespace Engine.Utilities
{
    public static class Input
    {
        public static IKeyboard? Keyboard { get; set; }
        public static IMouse? Mouse { get; set; }

        static bool LeftMouseDown = false;
        static bool RightMouseDown = false;
        static bool MiddleMouseDown = false;

        static Dictionary<Key, bool> KeyStatePressed = new Dictionary<Key, bool>();
        static Dictionary<Key, bool> KeyStateReleased = new Dictionary<Key, bool>();

        public static bool KeyPressed(Key key)
        {
            if(Keyboard is null)
            {
                Logger.Warning("Input Keyboard is absent.");
                return false;
            }

            bool state = Keyboard.IsKeyPressed(key);
            if (KeyStatePressed.TryGetValue(key, out bool value))
            {
                KeyStatePressed[key] = state;

                if (!value && state)
                    return true;
                else
                    return false;
            }
            else
            {
                KeyStatePressed[key] = state;
                return false;
            }

        }

        public static bool KeyDown(Key key)
        {
            if (Keyboard is null)
            {
                Logger.Warning("Input Keyboard is absent.");
                return false;
            }

            bool state = Keyboard.IsKeyPressed(key);

            return state;

        }

        public static bool KeyReleased(Key key)
        {
            if (Keyboard is null)
            {
                Logger.Warning("Input Keyboard is absent.");
                return false;
            }

            bool state = Keyboard.IsKeyPressed(key);
            if (KeyStateReleased.TryGetValue(key, out bool value))
            {
                KeyStateReleased[key] = state;

                if (value && !state)
                    return true;
                else
                    return false;
            }
            else
            {
                KeyStateReleased[key] = state;
                return false;
            }
        }

        public static bool MousePressed(int i)
        {
            if (Mouse is null)
            {
                Logger.Warning("Input Mouse is absent.");
                return false;
            }

            bool state;
            bool result;
            switch (i)
            {
                case 0:
                    state = Mouse.IsButtonPressed(MouseButton.Left);
                    result = !LeftMouseDown && state;

                    if(result)
                        LeftMouseDown = state;
                    return result;

                case 1:
                    state = Mouse.IsButtonPressed(MouseButton.Right);
                    result = !RightMouseDown && state;

                    if (result)
                        RightMouseDown = state;
                    return result;

                case 2:
                    state = Mouse.IsButtonPressed(MouseButton.Middle);
                    result = !MiddleMouseDown && state;

                    if (result)
                        MiddleMouseDown = state;
                    return result;
            }

            return false;
        }

        public static bool MouseDown(int i)
        {
            if (Mouse is null)
            {
                Logger.Warning("Input Mouse is absent.");
                return false;
            }

            switch (i)
            {
                case 0:
                    return Mouse.IsButtonPressed(MouseButton.Left);
                case 1:
                    return Mouse.IsButtonPressed(MouseButton.Right);
                case 2:
                    return Mouse.IsButtonPressed(MouseButton.Middle);
            }

            return false;
        }

        public static bool MouseReleased(int i)
        {
            if (Mouse is null)
            {
                Logger.Warning("Input Mouse is absent.");
                return false;
            }

            bool state;
            bool result;
            switch (i)
            {
                case 0:
                    state = Mouse.IsButtonPressed(MouseButton.Left);
                    result = LeftMouseDown && !state;

                    if (result)
                        LeftMouseDown = state;
                    return result;

                case 1:
                    state = Mouse.IsButtonPressed(MouseButton.Right);
                    result = RightMouseDown && !state;

                    if (result)
                        RightMouseDown = state;
                    return result;

                case 2:
                    state = Mouse.IsButtonPressed(MouseButton.Middle);
                    result = MiddleMouseDown && !state;

                    if (result)
                        MiddleMouseDown = state;
                    return result;
            }

            return false;
        }

        public static float MouseScroll()
        {
            if (Mouse is null)
            {
                Logger.Warning("Input Mouse is absent.");
                return 0;
            }

            return Mouse.ScrollWheels[0].Y;
        }

        public static Vector2Float MousePosition()
        {
            if (Mouse is null)
            {
                Logger.Warning("Input Mouse is absent.");
                return Vector2Float.Zero;
            }

            return new Vector2Float(Mouse.Position.X, Mouse.Position.Y);
        }
    }
}
