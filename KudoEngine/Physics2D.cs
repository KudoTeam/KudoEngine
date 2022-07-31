namespace KudoEngine
{
    /// <summary>
    /// <see langword="Kudo"/>
    /// Extends BoxCollider2D and adds physics
    /// </summary>
    public class Physics2D
    {
        public BoxCollider2D Collider { get; private set; }
        /// <summary>
        /// Hitbox colliders this instance interacts with
        /// </summary>
        public List<string> Tags { get; private set; }

        /// <summary>
        /// Current velocity
        /// </summary>
        public Vector2 Velocity = new();
        /// <summary>
        /// Terminal velocity (ignored when setting Velocity manually)
        /// </summary>
        public Vector2 MaxVelocity = new(15f, 15f);
        /// <summary>
        /// Vertical falling velocity increase
        /// </summary>
        public float Gravity = 0.5f;
        /// <summary>
        /// Horizontal velocity decrease
        /// </summary>
        public float Weight = 0.5f;

        /// <summary>
        /// Initialize new Physics2D
        /// </summary>
        /// <param name="tags">Hitbox Tags this instance interacts with</param>
        public Physics2D(BoxCollider2D collider, List<string> tags)
        {
            Collider = collider;
            Tags = tags;
        }

        /// <summary>
        /// This updates physics (they don't work without it)
        /// </summary>
        public void Update()
        {
            // Update velocity based on gravity
            if (Velocity.Y <= MaxVelocity.Y)
            {
                Velocity.Y += Gravity;
            }
            // Change position based on velocity
            Collider.Rendered.Position.X += Velocity.X;
            if (Collider.StopHorizontalCollision(Tags))
            {
                Velocity.X = 0;
            }
            Collider.Rendered.Position.Y += Velocity.Y;
            if (Collider.StopVerticalCollision(Tags))
            {
                Velocity.Y = 0;
            }
            // Wear Out Horizontal Velocity
            if (Velocity.X > 0)
            {
                Velocity.X = Math.Max(Velocity.X - Weight, 0);
            }
            else
            if (Velocity.X < 0)
            {
                Velocity.X = Math.Min(Velocity.X + Weight, 0);
            }
        }
    }
}
