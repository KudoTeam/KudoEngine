using System.Diagnostics;

namespace KudoEngine
{
    /// <summary>
    /// <see langword="Kudo"/>
    /// Useful functions for time measurment
    /// </summary>
    public class Time
    {
        private float _lastFrameTime = 0f;
        private readonly Stopwatch _stopwatch = new();

        /// <summary>
        /// Milliseconds elapsed since the start
        /// </summary>
        public float Elapsed
        {
            get
            {
                return _stopwatch.ElapsedMilliseconds;
            }
        }
        /// <summary>
        /// Milliseconds from the last frame to the current one
        /// </summary>
        public float DeltaTime { get; private set; }
        /// <summary>
        /// Seconds from the last frame to the current one
        /// </summary>
        public float DeltaTimeSeconds
        {
            get
            {
                return DeltaTime / 1000f;
            }
        }

        public Time()
        {
            _stopwatch.Start();
        }

        /// <summary>
        /// Update each frame
        /// </summary>
        internal void Update()
        {
            DeltaTime = Elapsed - _lastFrameTime;
            _lastFrameTime = _stopwatch.ElapsedMilliseconds;
        }
    }
}
