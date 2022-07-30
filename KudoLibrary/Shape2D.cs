namespace KudoEngine
{
    // TODO: Add other shapes
    /// <summary>
    /// <see langword="Kudo"/>
    /// A single-color rectangle
    /// </summary>
    public class Shape2D
    {
        public Vector2 Position;
        public Vector2 Scale;
        public Color Color;
        /// <summary>
        /// A cosmetic tag for debugging
        /// </summary>
        public string Tag;

        public bool IsAlive { get; private set; }

        /// <summary>
        /// Initialize a new Shape2D
        /// </summary>
        /// <param name="tag">A cosmetic tag for debugging</param>
        public Shape2D(Vector2 position, Vector2 scale, Color color, string tag = "Shape2D")
        {
            Position = position;
            Scale = scale;
            Color = color;
            Tag = tag;

            IsAlive = true;

            Kudo.AddShape2D(this);
        }

        /// <summary>
        /// Get center position of Shape2D instance
        /// </summary>
        public Vector2 Center()
        {
            return new(Position.X + Scale.X / 2, Position.Y + Scale.Y / 2);
        }

        /// <summary>
        /// Remove Shape2D instance from memory
        /// </summary>
        public void Kill()
        {
            if (IsAlive)
            {
                IsAlive = false;

                Kudo.RemoveShape2D(this);
                // TODO: Remove class instance from memory
            }
            else
            {
                Log.error("This shape does not exist");
            }
        }
    }
}
