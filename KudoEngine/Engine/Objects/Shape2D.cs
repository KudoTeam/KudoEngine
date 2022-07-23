using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KudoEngine.Engine.Objects
{
    // TODO: Add other shapes
    /// <summary>
    /// <see langword="Object"/>
    /// A single-color rectangle
    /// </summary>
    public class Shape2D
    {
        public Vector2 Position;
        public Vector2 Scale;
        public Color Color;
        /// <summary>
        /// A cosmetic tag for debugging
        /// </summary>
        public string Tag;

        public bool isAlive { get; private set; }

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

            isAlive = true;

            Kudo.AddShape2D(this);
        }

        /// <summary>
        /// Get center position of Shape2D instance
        /// </summary>
        public Vector2 Center()
        {
            return new(Position.X + Scale.X / 2, Position.Y + Scale.Y / 2);
        }

        /// <summary>
        /// Remove Shape2D instance from memory
        /// </summary>
        public void Kill()
        {
            if (isAlive)
            {
                isAlive = false;

                Kudo.RemoveShape2D(this);
                // TODO: Remove class instance from memory
            }
            else
            {
                Log.error("This shape does not exist");
            }
        }
    }
}
