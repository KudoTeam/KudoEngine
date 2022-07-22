using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KudoEngine.Engine
{
    public class Camera
    {
        public Vector2 Position = new();
        public float Rotation;
        public float Zoom;

        public Camera(Vector2 position, float rotation = 1f, float zoom = 1f)
        {
            Position = position;
            Rotation = rotation;
            Zoom = zoom;
        }
    }
}
