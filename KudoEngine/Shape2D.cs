namespace KudoEngine
{
    // TODO: Add other shapes
    /// <summary>
    /// <see langword="Kudo"/>
    /// A single-color rectangle
    /// </summary>
    public class Shape2D : RenderedObject2D
    {
        public Color Color { get; set; }

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

            Kudo.AddRender2D(this);
        }
    }
}
