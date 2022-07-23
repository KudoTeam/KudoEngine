using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KudoEngine.Engine
{
    public class Camera
    {
        // TODO: Sync camera position with zoom
        // TODO: Zoom and rotate around center
        public Vector2 Position = new();
        public float Rotation;
        public float Zoom;

        public Camera(Vector2 position, float rotation = 0f, float zoom = 1f)
        {
            Position = position;
            Rotation = rotation;
            Zoom = zoom;
        }
    }
}
