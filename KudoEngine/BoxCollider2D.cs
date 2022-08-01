namespace KudoEngine
{
    /// <summary>
    /// <see langword="Kudo"/>
    /// Extends multiple objects and adds collisions
    /// </summary>
    public class BoxCollider2D
    {
        public RenderedObject2D Rendered { get; private set; }
        /// <summary>
        /// A tag to differentiate between colliders
        /// </summary>
        public string Tag { get; set; }
        /// <summary>
        /// Size of collider relative to subject
        /// </summary>
        public Vector2 ScaleModifier { get; set; }
        /// <summary>
        /// Position of collider relative to subject
        /// </summary>
        public Vector2 PositionModifier { get; set; }
        

        private readonly Vector2 _lastPosition = new();

        /// <summary>
        /// Initialize a new BoxCollider2D for a RenderedObject2D
        /// </summary>
        /// <param name="tag">A Name to differentiate between Colliders</param>
        public BoxCollider2D(RenderedObject2D rendered, string tag = "", Vector2? scaleModifier = null, Vector2? positionModifier = null)
        {
            Rendered = rendered;
            Tag = tag;
            ScaleModifier = scaleModifier ?? new();
            PositionModifier = positionModifier ?? new();

            Kudo.AddBoxCollider2D(this);
        }

        // Every compatible type needs to be added

        /// <summary>
        /// Check if colliding with another instance of BoxCollider2D
        /// </summary>
        public bool IsColliding(BoxCollider2D collider)
        {
            return GetModifiedPosition(this).X - ScaleModifier.X < GetModifiedPosition(collider).X + collider.Rendered.Scale.X + collider.ScaleModifier.X &&
                   GetModifiedPosition(this).X + Rendered.Scale.X + ScaleModifier.X > GetModifiedPosition(collider).X - collider.ScaleModifier.X &&
                   GetModifiedPosition(this).Y - ScaleModifier.Y < GetModifiedPosition(collider).Y + collider.Rendered.Scale.Y + collider.ScaleModifier.Y &&
                   GetModifiedPosition(this).Y + Rendered.Scale.Y + ScaleModifier.Y > GetModifiedPosition(collider).Y - collider.ScaleModifier.Y;
        }

        /// <summary>
        /// Check if colliding with another BoxCollider2D with a certain tag/tags
        /// </summary>
        public bool IsColliding(List<string>? tags = null)
        {
            foreach (BoxCollider2D collider in Kudo.BoxColliders2D)
            {
                if (tags == null || tags.Contains(collider.Tag) && IsColliding(collider))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Get all BoxColliders2D with a certain tag this collider is colliding with
        /// </summary>
        public List<BoxCollider2D> GetCollisions(List<string>? tags = null)
        {
            List<BoxCollider2D> collisions = new();
            foreach (BoxCollider2D collider in Kudo.BoxColliders2D)
            {
                if (tags == null || tags.Contains(collider.Tag) && IsColliding(collider))
                {
                    collisions.Add(collider);
                }
            }
            return collisions;
        }

        /// <summary>
        /// Stop all collisions
        /// </summary>
        /// <returns>If collision was stopped</returns>
        public bool StopCollision(List<string>? tags = null)
        {
            // TODO: Not tested - may cause bugs
            if (StopVerticalCollision(tags)) { if (StopHorizontalCollision(tags))
            {
                return true;
            }}
            return false;
        }

        /// <summary>
        /// Stop all vertical collisions
        /// </summary>
        /// <returns>If collision was stopped</returns>
        public bool StopVerticalCollision(List<string>? tags = null)
        {
            if (tags == null || IsColliding(tags))
            {
                Rendered.Position.Y = _lastPosition.Y;
                return true;
            }
            else
            {
                _lastPosition.Y = Rendered.Position.Y;
                return false;
            }
        }

        /// <summary>
        /// Stop all horizontal collisions
        /// </summary>
        /// <returns>If collision was stopped</returns>
        public bool StopHorizontalCollision(List<string>? tags = null)
        {
            if (tags == null || IsColliding(tags))
            {
                Rendered.Position.X = _lastPosition.X;
                return true;
            }
            else
            {
                _lastPosition.X = Rendered.Position.X;
                return false;
            }
        }

        private static Vector2 GetModifiedPosition(BoxCollider2D collider)
        {
            return new(collider.Rendered.Position.X + collider.PositionModifier.X, collider.Rendered.Position.Y + collider.PositionModifier.Y);
        }
    }
}
