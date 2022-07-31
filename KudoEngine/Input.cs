namespace KudoEngine
{
    public class Input
    {
        internal static Dictionary<Keys, bool> PressedKeys { get; set; } = new();

        public static bool IsKeyDown(Keys key)
        {
            return PressedKeys.ContainsKey(key);
        }
    }
}
