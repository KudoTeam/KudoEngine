using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KudoEngine
{
    /// <summary>
    /// <see langword="Kudo"/>
    /// A colored text
    /// </summary>
    public class Text2D : RenderedObject2D
    {
        public string Text { get; set; }
        public Color Color { get; set; }
        public Font Font { get; set; }

        /// <summary>
        /// Initialize a new Text2D
        /// </summary>
        /// <param name="tag">A cosmetic tag for debugging</param>
        public Text2D(Vector2 position, Vector2 scale, string text, Color color, float rotation = 0f, Font? font = null, string tag = "Text2D", int layer = 0)
        {
            Position = position;
            Scale = scale;
            Text = text;
            Color = color;
            Rotation = rotation;
            Font = font ?? new("Arial", 10);
            Tag = tag;
            Layer = Math.Clamp(layer, -999, 1000);

            IsAlive = true;

            Kudo.AddRender2D(this);
        }
    }
}
