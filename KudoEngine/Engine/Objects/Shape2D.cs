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
        /// A Name for cosmetic purposes
        /// </summary>
        public string Tag;

        public bool isAlive { get; private set; }

        /// <summary>
        /// Initialize a new Shape2D
        /// </summary>
        /// <param name="tag">A Name for cosmetic purposes</param>
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
