namespace KudoEngine
{
    public class Input
    {
        internal static Dictionary<Keys, bool> PressedKeys { get; set; } = new();

        /// <summary>
        /// Check if a certain key is pressed
        /// </summary>
        public static bool IsKeyDown(Keys key)
        {
            return PressedKeys.ContainsKey(key);
        }
    }
}
