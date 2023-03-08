namespace KudoEngine
{
    public abstract class RenderedObject2D
    {
        public Vector2 Position { get; set; } = new();
        public Vector2 Scale { get; set; } = new();
        public float Rotation { get; set; } = 0f;
        /// <summary>
        /// A cosmetic tag for debugging
        /// </summary>
        public string Tag { get; set; } = "RenderedObject2D";
        /// <summary>
        /// Rendering layer (0 is the lowest)
        /// </summary>
        public int Layer { get; set; } = 0;

        public Vector2 TopLeft
        {
            get { return Position; }
            set { Position = value; }
        }
        public Vector2 TopRight
        {
            get { return new(Position.X - Scale.X, Position.Y); }
            set { Position = new(value.X - Scale.X, value.Y); }
        }
        public Vector2 BottomLeft
        {
            get { return new(Position.X, Position.Y - Scale.Y); }
            set { Position = new(value.X, value.Y - Scale.Y); }
        }
        public Vector2 BottomRight
        {
            get { return new(Position.X - Scale.X, Position.Y - Scale.Y); }
            set { Position = new(value.X - Scale.X, value.Y - Scale.Y); }
        }

        public bool IsAlive { get; private protected set; } = true;

        /// <summary>
        /// Get center position of RenderedObject2D instance
        /// </summary>
        public Vector2 Center()
        {
            return new(Position.X + Scale.X / 2, Position.Y + Scale.Y / 2);
        }

        // TODO: Kill it properly
        public bool Kill()
        {
            if (Kudo.RenderedObjects2D.Contains(this))
            {
                Kudo.RemoveRender2D(this);
            }
            IsAlive = false;

            return true;
        }

        /// <summary>
        /// Get position relative to the camera's top left corner
        /// </summary>
        public Vector2 GetPositionOnCamera()
        {
            // TODO: Account for Rotation and Zoom
            return Position.Subtract(Kudo.ScreenSize.Divide(new(2f)));
        }
    }
}
