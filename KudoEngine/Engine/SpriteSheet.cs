using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KudoEngine.Engine
{
    /// <summary>
    /// <see langword="Type"/>
    /// Allows the usage of spritesheets
    /// </summary>
    public class SpriteSheet
    {
        public string Directory { get; private set; }
        public Bitmap Sprite { get; private set; }
        public Dictionary<string, Rectangle> Map { get; private set; } = new();

        /// <summary>
        /// Initialize a SpriteSheet
        /// </summary>
        /// <param name="directory">A .png file for the texture (name only)</param>
        public SpriteSheet(string directory)
        {
            Directory = directory;

            Image tmp = Image.FromFile($"Assets/Sprites/{Directory}.png");
            Sprite = new(tmp);
        }

        /// <summary>
        /// Add a sprite (section of sprite sheet)
        /// </summary>
        public void AddSprite(string name, Rectangle section)
        {
            Map.Add(name, section);
        }

        /// <summary>
        /// Get a sprite by name
        /// </summary>
        public Bitmap GetSprite(string name)
        {
            try
            {
                // Empty bitmap with target size
                Bitmap bitmap = new(Map[name].Width, Map[name].Height);
                using Graphics crop = Graphics.FromImage(bitmap);
                // Crop out from SpriteSheet into empty bitmap
                crop.DrawImage(Sprite, 0, 0, Map[name], GraphicsUnit.Pixel);
                return bitmap;
            } catch
            {
                Log.error("Invalid sprite");
                #pragma warning disable CS8603
                return null;
                #pragma warning restore CS8603
            }
        }
    }
}
