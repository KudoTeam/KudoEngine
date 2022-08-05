namespace KudoEngine
{
    /// <summary>
    /// <see langword="Kudo"/>
    /// A textured rectangle
    /// </summary>
    public class Sprite2D : RenderedObject2D
    {
        public Bitmap Sprite { get; set; }

        /// <summary>
        /// Initialize a new Sprite2D
        /// </summary>
        /// <param name="sprite">A bitmap for the texture</param>
        /// <param name="tag">A cosmetic tag for debugging</param>
        /// <param name="layer">Min Value: -999 | Max Value: 999 (1000 for UI) </param>
        public Sprite2D(Vector2 position, Vector2 scale, Bitmap sprite, float rotation = 0f, string tag = "Sprite2D", int layer = 0)
        {
            Position = position;
            Scale = scale;
            Sprite = sprite;
            Rotation = rotation;
            Tag = tag;
            Layer = Math.Clamp(layer, -999, 1000);

            IsAlive = true;

            Kudo.AddRender2D(this);
        }
    }
}
