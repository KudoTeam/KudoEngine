using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;

namespace KudoEngine
{
    public abstract class RenderedObject2D
    {
        public Vector2 Position { get; set; } = new();
        public Vector2 Scale { get; set; } = new();
        /// <summary>
        /// A cosmetic tag for debugging
        /// </summary>
        public string Tag { get; set; } = "RenderedObject2D";
        /// <summary>
        /// Rendering layer (0 is the lowest)
        /// </summary>
        public int Layer { get; set; } = 0;

        public bool IsAlive { get; private protected set; } = true;

        /// <summary>
        /// Get center position of RenderedObject2D instance
        /// </summary>
        public Vector2 Center()
        {
            return new(Position.X + Scale.X / 2, Position.Y + Scale.Y / 2);
        }

        public bool Kill()
        {
            if (Kudo.RenderedObjects2D.Contains(this))
            {
                Kudo.RemoveRender2D(this);
            }
            IsAlive = false;

            return true;
        }
    }
}
