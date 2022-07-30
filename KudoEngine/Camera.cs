namespace KudoEngine
{
    /// <summary>
    /// <see langword="Kudo"/>
    /// This object allows the player to see the scene
    /// </summary>
    public class Camera
    {
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public float Zoom { get; set; }

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
