using KudoEngine.Engine.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KudoEngine.Engine.Extenders
{
    public class BoxCollider2D
    {
        public dynamic Subject { get; private set; }
        public string Tag = "";
        public float Offset = 0f;

        public BoxCollider2D(Shape2D shape, string tag = "", float offset = 0f)
        {
            Subject = shape;
            Tag = tag;
            Offset = offset;

            Kudo.AddBoxCollider2D(this);
        }

        public BoxCollider2D(Sprite2D sprite, string tag = "", float offset = 0f)
        {
            Subject = sprite;
            Tag = tag;
            Offset = offset;

            Kudo.AddBoxCollider2D(this);
        }

        // Every compatible type needs to be added

        /// <summary>
        /// Check if sprite is colliding with another sprite
        /// </summary>
        public bool IsColliding(string tag)
        {
            foreach (BoxCollider2D collider in Kudo.BoxColliders2D)
            {
                if (collider.Tag == tag)
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
