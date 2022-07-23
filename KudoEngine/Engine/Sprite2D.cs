using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace KudoEngine.Engine
{
    public class Sprite2D
    {
        public Vector2 Position;
        public Vector2 Scale;
        public string Directory;
        public string Tag;

        public Bitmap Sprite;

        public bool isAlive { get; private set; }

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
