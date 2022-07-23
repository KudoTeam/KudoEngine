using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KudoEngine.Engine.Objects
{
    // TODO: Add other shapes
    public class Shape2D
    {
        public Vector2 Position;
        public Vector2 Scale;
        public Color Color;
        public string Tag;

        public bool isAlive { get; private set; }

        public Shape2D(Vector2 position, Vector2 scale, Color color, string tag = "Shape2D")
        {
            Position = position;
            Scale = scale;
            Color = color;
            Tag = tag;

            isAlive = true;

            Kudo.AddShape2D(this);
        }

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
