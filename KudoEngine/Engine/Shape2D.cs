using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KudoEngine.Engine
{
    // TODO: Add other shapes
    public class Shape2D
    {
        public Vector2 Position;
        public Vector2 Scale;
        public Color Color;
        public string Name;

        public bool isAlive { get; private set; }

        public Shape2D(Vector2 position, Vector2 scale, Color color, string name = "Shape2D")
        {
            Position = position;
            Scale = scale;
            Color = color;
            Name = name;

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
            } else
            {
                Log.error("This shape does not exist");
            }
        }
    }
}
