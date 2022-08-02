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
        /// Return a Vector2 value added to this Vector2
        /// </summary>
        public Vector2 Add(Vector2 vector)
        {
            return new(X + vector.X, Y + vector.Y);
        }

        /// <summary>
        /// Return a Vector2 value subtracted from this Vector2
        /// </summary>
        public Vector2 Subtract(Vector2 vector)
        {
            return new(X - vector.X, Y - vector.Y);
        }

        /// <summary>
        /// Return a Vector2 value multiplied by this Vector2
        /// </summary>
        public Vector2 Multiply(Vector2 vector)
        {
            return new(X * vector.X, Y * vector.Y);
        }

        /// <summary>
        /// Return this Vector2 divided by a Vector2 value
        /// </summary>
        public Vector2 Divide(Vector2 vector)
        {
            return new(X / vector.X, Y / vector.Y);
        }

        /// <summary>
        /// Return this Vector2 raised to the specified power
        /// </summary>
        public Vector2 Pow(double power)
        {
            return new((float)Math.Pow(X, power), (float)Math.Pow(Y, power));
        }

        /// <summary>
        /// Return a Vector2 with absolute values based on this Vector2
        /// </summary>
        public Vector2 Abs()
        {
            return new((float)Math.Abs(X), (float)Math.Abs(Y));
        }

        /// <summary>
        /// Initialize a new Vector2 based on this Vector2
        /// </summary>
        public Vector2 Copy()
        {
            return new(X, Y);
        }

        /// <summary>
        /// Calculate a position to move towards another <see langword="Vector2"/>
        /// </summary>
        public Vector2 MoveTowards(Vector2 target, float step = 1f)
        {
            return new(X + Math.Sign(target.X - X) * step, Y + Math.Sign(target.Y - Y) * step);
        }

        /// <summary>
        /// Returns the length between this Vector2 and another Vector2
        /// </summary>
        public double HowFarIs(Vector2 vector)
        {
            // Pythagorean theorem
            Vector2 tmp = Subtract(vector).Abs().Pow(2);
            return Math.Sqrt(tmp.X + tmp.Y);
        }
    }
}
