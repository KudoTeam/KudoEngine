using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;

namespace KudoEngine
{
    public abstract class RenderedObject2D : IDisposable
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

        #region Dispose
        // To detect redundant calls
        private bool _disposedValue;

        // Instantiate a SafeHandle instance.
        private readonly SafeHandle _safeHandle = new SafeFileHandle(IntPtr.Zero, true);

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            if (Kudo.RenderedObjects2D.Contains(this))
            {
                Kudo.RemoveRender2D(this);
            }
            IsAlive = false;

            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _safeHandle.Dispose();
                }

                _disposedValue = true;
            }
        }
        #endregion
    }
}
