using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KudoEngine.Engine
{
    /// <summary>
    /// <see langword="Extender"/>
    /// Extends multiple objects and adds collisions
    /// </summary>
    public class BoxCollider2D
    {
        public dynamic Subject { get; private set; }
        /// <summary>
        /// A tag to differentiate between colliders
        /// </summary>
        public string Tag = "";
        /// <summary>
        /// Size of collider relative to subject
        /// </summary>
        public Vector2 ScaleModifier = new();
        /// <summary>
        /// Position of collider relative to subject
        /// </summary>
        public Vector2 PositionModifier = new();

        /// <summary>
        /// Initialize a new BoxCollider2D for a Shape2D
        /// </summary>
        /// <param name="tag">A Name to differentiate between Colliders</param>
        /// <param name="offset">Size of collider relative to Subject</param>
        public BoxCollider2D(Shape2D shape, string tag = "", Vector2? scaleModifier = null, Vector2? positionModifier = null)
        {
            Subject = shape;
            Tag = tag;
            ScaleModifier = scaleModifier ?? new();
            PositionModifier = positionModifier ?? new();

            Kudo.AddBoxCollider2D(this);
        }

        /// <summary>
        /// Initialize a new BoxCollider2D for a Sprite2D
        /// </summary>
        /// <param name="tag">A Name to differentiate between Colliders</param>
        /// <param name="offset">Size of collider relative to Subject</param>
        public BoxCollider2D(Sprite2D sprite, string tag = "", Vector2? scaleModifier = null, Vector2? positionModifier = null)
        {
            Subject = sprite;
            Tag = tag;
            ScaleModifier = scaleModifier ?? new();
            PositionModifier = positionModifier ?? new();

            Kudo.AddBoxCollider2D(this);
        }

        // Every compatible type needs to be added

        /// <summary>
        /// Check if colliding with another BoxCollider2D with a certain tag/tags
        /// </summary>
        public bool IsColliding(List<string>? tags = null)
        {
            foreach (BoxCollider2D collider in Kudo.BoxColliders2D)
            {
                if (tags != null && tags.Any(collider.Tag.Contains) || tags == null)
                {
                    if (GetModifiedPosition(this).X - ScaleModifier.X < GetModifiedPosition(collider).X + collider.Subject.Scale.X + collider.ScaleModifier.X &&
                        GetModifiedPosition(this).X + Subject.Scale.X + ScaleModifier.X > GetModifiedPosition(collider).X - collider.ScaleModifier.X &&
                        GetModifiedPosition(this).Y - ScaleModifier.Y < GetModifiedPosition(collider).Y + collider.Subject.Scale.Y + collider.ScaleModifier.Y &&
                        GetModifiedPosition(this).Y + Subject.Scale.Y + ScaleModifier.Y > GetModifiedPosition(collider).Y - collider.ScaleModifier.Y)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Get all BoxColliders2D this collider is colliding with
        /// </summary>
        public List<BoxCollider2D> GetCollisions(List<string>? tags = null)
        {
            List<BoxCollider2D> collisions = new();
            foreach (BoxCollider2D collider in Kudo.BoxColliders2D)
            {
                if (tags != null && tags.Any(collider.Tag.Contains) || tags == null)
                {
                    if (GetModifiedPosition(this).X - ScaleModifier.X < GetModifiedPosition(collider).X + collider.Subject.Scale.X + collider.ScaleModifier.X &&
                        GetModifiedPosition(this).X + Subject.Scale.X + ScaleModifier.X > GetModifiedPosition(collider).X - collider.ScaleModifier.X &&
                        GetModifiedPosition(this).Y - ScaleModifier.Y < GetModifiedPosition(collider).Y + collider.Subject.Scale.Y + collider.ScaleModifier.Y &&
                        GetModifiedPosition(this).Y + Subject.Scale.Y + ScaleModifier.Y > GetModifiedPosition(collider).Y - collider.ScaleModifier.Y)
                    {
                        collisions.Add(collider);
                    }
                }
            }
            return collisions;
        }

        private Vector2 GetModifiedPosition(BoxCollider2D collider)
        {
            return new(collider.Subject.Position.X + collider.PositionModifier.X, collider.Subject.Position.Y + collider.PositionModifier.Y);
        }
    }
}
