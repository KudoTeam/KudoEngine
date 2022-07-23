using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace KudoEngine.Engine.Objects
{
    /// <summary>
    /// <see langword="Object"/>
    /// A textured rectangle
    /// </summary>
    public class Sprite2D
    {
        public Vector2 Position;
        public Vector2 Scale;
        public string Directory;
        /// <summary>
        /// A cosmetic tag for debugging
        /// </summary>
        public string Tag;

        public Bitmap Sprite;

        public bool isAlive { get; private set; }

        /// <summary>
        /// Initialize a new Sprite2D
        /// </summary>
        /// <param name="directory">A .png file for the texture (name only)</param>
        /// <param name="tag">A cosmetic tag for debugging</param>
        public Sprite2D(Vector2 position, Vector2 scale, string directory, string tag = "Sprite2D")
        {
            Position = position;
            Scale = scale;
            Directory = directory;
            Tag = tag;

            Image tmp = Image.FromFile($"Assets/Sprites/{Directory}.png");
            Sprite = new(tmp, (int)Scale.X, (int)Scale.Y);

            isAlive = true;

            Kudo.AddSprite2D(this);
        }

        /// <summary>
        /// Get center position of Sprite2D instance
        /// </summary>
        public Vector2 Center()
        {
            return new(Position.X + Scale.X / 2, Position.Y + Scale.Y / 2);
        }

        /// <summary>
        /// Remove Sprite2D instance from memory
        /// </summary>
        public void Kill()
        {
            if (isAlive)
            {
                isAlive = false;

                Kudo.RemoveSprite2D(this);
                // TODO: Remove class instance from memory
            }
            else
            {
                Log.error("This sprite does not exist");
            }
        }
    }
}
