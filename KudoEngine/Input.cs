namespace KudoEngine
{
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
    }
}
