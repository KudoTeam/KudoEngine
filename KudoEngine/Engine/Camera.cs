﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KudoEngine.Engine
{
    /// <summary>
    /// <see langword="Object"/>
    /// This object allows the player to see the scene
    /// </summary>
    public class Camera
    {
        public Vector2 Position;
        public float Rotation;
        public float Zoom;

        /// <summary>
        /// Initialize a new Camera
        /// </summary>
        public Camera(Vector2 position, float rotation = 0f, float zoom = 1f)
        {
            Position = position;
            Rotation = rotation;
            Zoom = zoom;
        }
    }
}
