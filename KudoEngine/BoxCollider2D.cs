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


        private Vector2 _lastPosition = new();

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
        
        // TODO: Account for Rotation and Zoom

        #region IsColliding
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
        /// <param name="collider">The collider that returned true</param>
        public bool IsColliding(out BoxCollider2D? collider, List<string>? tags = null)
        {
            foreach (BoxCollider2D collision in Kudo.BoxColliders2D)
            {
                if (tags == null || tags.Contains(collision.Tag) && IsColliding(collision))
                {
                    collider = collision;
                    return true;
                }
            }
            collider = null;
            return false;
        }

        /// <summary>
        /// Check if colliding with another BoxCollider2D with certain tags
        /// </summary>
        public bool IsColliding(List<string>? tags = null)
        {
            return IsColliding(out _, tags);
        }

        /// <summary>
        /// Check if colliding with another BoxCollider2D with a certain tag
        /// </summary>
        /// <param name="collider">The collider that returned true</param>
        public bool IsColliding(out BoxCollider2D? collider, string tag)
        {
            return IsColliding(out collider, new List<string>() { tag });
        }

        /// <summary>
        /// Check if colliding with another BoxCollider2D with a certain tag
        /// </summary>
        public bool IsColliding(string tag)
        {
            return IsColliding(out _, tag);
        }
        #endregion

        #region GetCollisions
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
        /// Get all BoxColliders2D with a certain tag this collider is colliding with
        /// </summary>
        public List<BoxCollider2D> GetCollisions(string tag)
        {
            return GetCollisions(new List<string>() { tag });
        }
        #endregion

        #region StopCollision
        /// <summary>
        /// Stop all collisions with a certain collider
        /// </summary>
        /// <returns>If collision was stopped</returns>
        public bool StopCollision(BoxCollider2D collider)
        {
            return StopVerticalCollision(collider) && StopHorizontalCollision(collider);
        }

        /// <summary>
        /// Stop all collisions with colliders with certain tags
        /// </summary>
        /// <returns>If collision was stopped</returns>
        /// <param name="collider">The collider that returned true</param>
        public bool StopCollision(out BoxCollider2D? collider, List<string>? tags = null)
        {
            return StopVerticalCollision(out collider, tags) && StopHorizontalCollision(out collider, tags);
        }

        /// <summary>
        /// Stop all collisions with colliders with certain tags
        /// </summary>
        /// <returns>If collision was stopped</returns>
        public bool StopCollision(List<string>? tags = null)
        {
            return StopCollision(out _, tags);
        }

        /// <summary>
        /// Stop all collisions with colliders with a certain tag
        /// </summary>
        /// <returns>If collision was stopped</returns>
        /// <param name="collider">The collider that returned true</param>
        public bool StopCollision(out BoxCollider2D? collider, string tag)
        {
            return StopCollision(out collider, new List<string>() { tag });
        }

        /// <summary>
        /// Stop all collisions with colliders with a certain tag
        /// </summary>
        /// <returns>If collision was stopped</returns>
        public bool StopCollision(string tag)
        {
            return StopCollision(out _, tag);
        }

        #region StopVerticalCollision
        /// <summary>
        /// Stop all vertical collisions with a certain collider
        /// </summary>
        /// <returns>If collision was stopped</returns>
        public bool StopVerticalCollision(BoxCollider2D collider)
        {
            if (IsColliding(collider))
            {
                Rendered.Position.Y = _lastPosition.Y;
                return true;
            }
            _lastPosition.Y = Rendered.Position.Y;
            return false;
        }

        /// <summary>
        /// Stop all vertical collisions with certain tags
        /// </summary>
        /// <returns>If collision was stopped</returns>
        /// <param name="collider">The collider that returned true</param>
        public bool StopVerticalCollision(out BoxCollider2D? collider, List<string>? tags = null)
        {
            if (IsColliding(out BoxCollider2D? collision, tags))
            {
                if (collision != null)
                {
                    collider = collision;
                    return StopVerticalCollision(collider);
                }
            }
            _lastPosition.Y = Rendered.Position.Y;
            collider = null;
            return false;
        }

        /// <summary>
        /// Stop all vertical collisions with certain tags
        /// </summary>
        /// <returns>If collision was stopped</returns>
        public bool StopVerticalCollision(List<string>? tags = null)
        {
            return StopVerticalCollision(out _, tags);
        }

        /// <summary>
        /// Stop all vertical collisions with a certain tag
        /// </summary>
        /// <returns>If collision was stopped</returns>
        /// <param name="collider">The collider that returned true</param>
        public bool StopVerticalCollision(out BoxCollider2D? collider, string tag)
        {
            return StopVerticalCollision(out collider, new List<string>() { tag });
        }

        /// <summary>
        /// Stop all vertical collisions with a certain tag
        /// </summary>
        /// <returns>If collision was stopped</returns>
        public bool StopVerticalCollision(string tag)
        {
            return StopVerticalCollision(out _, tag);
        }
        #endregion

        #region StopHorizontalCollision
        /// <summary>
        /// Stop all horizontal collisions with a certain collider
        /// </summary>
        /// <returns>If collision was stopped</returns>
        public bool StopHorizontalCollision(BoxCollider2D collider)
        {
            if (IsColliding(collider))
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

        /// <summary>
        /// Stop all horizontal collisions with certain tags
        /// </summary>
        /// <returns>If collision was stopped</returns>
        /// <param name="collider">The collider that returned true</param>
        public bool StopHorizontalCollision(out BoxCollider2D? collider, List<string>? tags = null)
        {
            if (IsColliding(out BoxCollider2D? collision, tags))
            {
                if (collision != null)
                {
                    collider = collision;
                    return StopHorizontalCollision(collider);
                }
            }
            _lastPosition.X = Rendered.Position.X;
            collider = null;
            return false;
        }

        /// <summary>
        /// Stop all horizontal collisions with certain tags
        /// </summary>
        /// <returns>If collision was stopped</returns>
        public bool StopHorizontalCollision(List<string>? tags = null)
        {
            return StopHorizontalCollision(out _, tags);
        }
        /// <summary>
        /// Stop all horizontal collisions with a certain tag
        /// </summary>
        /// <returns>If collision was stopped</returns>
        /// <param name="collider">The collider that returned true</param>
        public bool StopHorizontalCollision(out BoxCollider2D? collider, string tag)
        {
            return StopHorizontalCollision(out collider, new List<string>() { tag });
        }

        /// <summary>
        /// Stop all horizontal collisions with a certain tag
        /// </summary>
        /// <returns>If collision was stopped</returns>
        public bool StopHorizontalCollision(string tag)
        {
            return StopHorizontalCollision(out _, tag);
        }
        #endregion

        #endregion

        private static Vector2 GetModifiedPosition(BoxCollider2D collider)
        {
            return new(collider.Rendered.Position.X + collider.PositionModifier.X, collider.Rendered.Position.Y + collider.PositionModifier.Y);
        }
    }
}
