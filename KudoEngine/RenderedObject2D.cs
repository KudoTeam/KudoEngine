namespace KudoEngine
{
    public abstract class RenderedObject2D
    {
        public Vector2 Position { get; set; } = new();
        public Vector2 Scale { get; set; } = new();
        /// <summary>
        /// A cosmetic tag for debugging
        /// </summary>
        public string Tag { get; set; } = "RenderedObject2D";

        public bool IsAlive { get; private protected set; }

        /// <summary>
        /// Get center position of RenderedObject2D instance
        /// </summary>
        public Vector2 Center()
        {
            return new(Position.X + Scale.X / 2, Position.Y + Scale.Y / 2);
        }

        /// <summary>
        /// Remove RenderedObject2D instance from memory
        /// </summary>
        public void Kill()
        {
            if (IsAlive)
            {
                IsAlive = false;

                Kudo.RemoveRender2D(this);
                // TODO: Remove class instance from memory
            }
            else
            {
                Log.error("This sprite does not exist");
            }
        }
    }
}
