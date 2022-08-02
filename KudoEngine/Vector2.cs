namespace KudoEngine
{
    /// <summary>
    /// <see langword="Kudo"/>
    /// Contains <see langword="X"/> and <see langword="Y"/> values
    /// </summary>
    public class Vector2
    {
        public float X { get; set; }
        public float Y { get; set; }

        /// <summary>
        /// Initialize a new Vector2 with values 0, 0
        /// </summary>
        public Vector2()
        {
            X = Zero().X;
            Y = Zero().Y;
        }

        /// <summary>
        /// Initialize a new Vector2 with the same values
        /// </summary>
        public Vector2(float xy)
        {
            X = xy;
            Y = xy;
        }

        /// <summary>
        /// Initialize a new Vector2 with values
        /// </summary>
        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Returns <see langword="X"/> and <see langword="Y"/> as 0
        /// </summary>
        public static Vector2 Zero()
        {
            return new(0, 0);
        }

        /// <summary>
        /// Calculate a position to move towards another <see langword="Vector2"/>
        /// </summary>
        public Vector2 MoveTowards(Vector2 target, float step = 1f)
        {
            return new(X + Math.Sign(target.X - X) * step, Y + Math.Sign(target.Y - Y) * step);
        }

        /// <summary>
        /// Initialize a new Vector2 based on this Vector2
        /// </summary>
        public Vector2 Copy()
        {
            return new(X, Y);
        }
    }
}
