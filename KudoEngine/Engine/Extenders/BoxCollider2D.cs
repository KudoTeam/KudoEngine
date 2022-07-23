using KudoEngine.Engine.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KudoEngine.Engine.Extenders
{
    /// <summary>
    /// <see langword="Extender"/>
    /// Extends multiple objects and adds collisions
    /// </summary>
    public class BoxCollider2D
    {
        public dynamic Subject { get; private set; }
        /// <summary>
        /// A Name to differentiate between Colliders
        /// </summary>
        public string Tag = "";
        /// <summary>
        /// Size of collider relative to Subject
        /// </summary>
        public float Offset = 0f;

        /// <summary>
        /// Initialize a new BoxCollider2D for a Shape2D
        /// </summary>
        /// <param name="tag">A Name to differentiate between Colliders</param>
        /// <param name="offset">Size of collider relative to Subject</param>
        public BoxCollider2D(Shape2D shape, string tag = "", float offset = 0f)
        {
            Subject = shape;
            Tag = tag;
            Offset = offset;

            Kudo.AddBoxCollider2D(this);
        }

        /// <summary>
        /// Initialize a new BoxCollider2D for a Sprite2D
        /// </summary>
        /// <param name="tag">A Name to differentiate between Colliders</param>
        /// <param name="offset">Size of collider relative to Subject</param>
        public BoxCollider2D(Sprite2D sprite, string tag = "", float offset = 0f)
        {
            Subject = sprite;
            Tag = tag;
            Offset = offset;

            Kudo.AddBoxCollider2D(this);
        }

        // Every compatible type needs to be added

        /// <summary>
        /// Check if colliding with another BoxCollider2D with a certain tag/tags
        /// </summary>
        public bool IsColliding(List<string> tags)
        {
            foreach (BoxCollider2D collider in Kudo.BoxColliders2D)
            {
                if (tags.Any(collider.Tag.Contains))
                {
                    if (Subject.Position.X - Offset < collider.Subject.Position.X + collider.Subject.Scale.X + collider.Offset &&
                        Subject.Position.X + Subject.Scale.X + Offset > collider.Subject.Position.X - collider.Offset &&
                        Subject.Position.Y - Offset < collider.Subject.Position.Y + collider.Subject.Scale.Y + collider.Offset &&
                        Subject.Position.Y + Subject.Scale.Y + Offset > collider.Subject.Position.Y - collider.Offset)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
