namespace KudoEngine
{
    /// <summary>
    /// <see langword="Kudo"/>
    /// A big class with input functions and variables (not events)
    /// </summary>
    public class Input
    {
        /// <summary>
        /// Position of mouse in the world
        /// </summary>
        public static Vector2 MousePosition { get; internal set; } = new();
        /// <summary>
        /// Position of mouse on screen
        /// </summary>
        public static Vector2 ScreenMousePosition { get; internal set; } = new();

        internal static Dictionary<Keys, bool> PressedKeys { get; set; } = new();

        internal static Dictionary<MouseButtons, bool> PressedMouse { get; set; } = new();

        /// <summary>
        /// Check if any key is pressed
        /// </summary>
        public static bool IsKeyDown()
        {
            return PressedKeys.Count > 0;
        }

        /// <summary>
        /// Check if a certain key is pressed
        /// </summary>
        public static bool IsKeyDown(Keys key)
        {
            return PressedKeys.ContainsKey(key);
        }

        /// <summary>
        /// Get a List of all pressed keys
        /// </summary>
        public static List<Keys> GetKeysDown()
        {
            return new(PressedKeys.Keys);
        }

        /// <summary>
        /// Check if any mouse button is pressed
        /// </summary>
        public static bool IsMouseDown()
        {
            return PressedMouse.Count > 0;
        }

        /// <summary>
        /// Check if a certain mouse button is pressed
        /// </summary>
        public static bool IsMouseDown(MouseButtons button)
        {
            return PressedMouse.ContainsKey(button);
        }

        /// <summary>
        /// Get a List of all pressed mouse buttons
        /// </summary>
        public static List<MouseButtons> GetMouseDown()
        {
            return new(PressedMouse.Keys);
        }

        /// <summary>
        /// Check if mouse if over a <see langword="RenderedObject"/>
        /// </summary>
        public static bool MouseOver(RenderedObject2D rendered)
        {
            return MousePosition.X >= rendered.Position.X &&
                MousePosition.X <= rendered.Position.X + rendered.Scale.X
                && MousePosition.Y >= rendered.Position.Y &&
                MousePosition.Y <= rendered.Position.Y + rendered.Scale.Y;
        }
    }
}
