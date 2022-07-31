using System.Drawing.Drawing2D;

namespace KudoEngine
{
    class Canvas : Form
    {
        public Canvas()
        {
            DoubleBuffered = true;
        }
    }

    public abstract class Kudo
    {
        public static Vector2 ScreenSize { get; private set; } = new(512, 512);
        public static string Title { get; private set; } = "Kudo Game";
        private readonly Canvas Window;
        private readonly Thread GameLoopThread;

        // Accessible by the user
        /// <summary>
        /// Window Background Color
        /// </summary>
        public Color Skybox = Color.White;
        /// <summary>
        /// Current Camera
        /// </summary>
        public Camera ActiveCamera = new(new(),1);
        /// <summary>
        /// Increments after each frame
        /// </summary>
        public int Timer = 0;

        public Kudo(Vector2? screenSize = null, string? title = null)
        {
            ScreenSize = screenSize ?? ScreenSize;
            Title = title ?? Title;

            Window = new Canvas
            {
                Size = new Size((int)ScreenSize.X, (int)ScreenSize.Y),
                Text = Title,
                // Make Window Unresizable
                FormBorderStyle = FormBorderStyle.FixedToolWindow
            };
            // Render Screen
            Window.Paint += Renderer;
            #region Input
            // Check Key Events
            Window.KeyDown += KeyDown;
            Window.KeyUp += KeyUp;
            // Input Class Communication
            Window.KeyDown += InputKeyDown;
            Window.KeyUp += InputKeyUp;
            #endregion
            // On Close Events
            Window.FormClosing += Quit;

            // Make a thread to refresh window asynchronously
            GameLoopThread = new Thread(GameLoop/*here*/);
            GameLoopThread.Start();

            // Start window
            Application.Run(Window);
        }

        // Abort thread when quitting to avoid memory leaks
        private void Quit(object? sender, FormClosingEventArgs e)
        {
            // TODO: Abort thread
        }

        #region Input
        // Events
        private void KeyDown(object? sender, KeyEventArgs e)
        {
            KeyDown(e);
        }

        private void KeyUp(object? sender, KeyEventArgs e)
        {
            KeyUp(e);
        }

        // Input Class
        private static void InputKeyDown(object? sender, KeyEventArgs e)
        {
            Input.PressedKeys[e.KeyCode] = true;
        }

        private static void InputKeyUp(object? sender, KeyEventArgs e)
        {
            Input.PressedKeys.Remove(e.KeyCode);
        }
        #endregion

        void GameLoop()
        {
            Load();
            while (GameLoopThread.IsAlive)
            {
                try
                {
                    // Constantly refresh window
                    Draw();
                    Window.BeginInvoke((MethodInvoker)delegate { Window.Refresh(); });
                    Update();
                    Timer++;
                    Thread.Sleep(1);
                }
                catch
                {
                    Log.warn("Waiting for Window...");
                }
            }
        }

        #region RenderedObjects2D
        internal static List<RenderedObject2D> RenderedObjects2D { get; private set; } = new();

        internal static void AddRender2D(RenderedObject2D rendered)
        {
            RenderedObjects2D.Add(rendered);
        }

        internal static void RemoveRender2D(RenderedObject2D rendered)
        {
            RenderedObjects2D.Remove(rendered);
        }
        #endregion

        #region BoxColliders2D
        internal static List<BoxCollider2D> BoxColliders2D { get; private set; } = new();

        internal static void AddBoxCollider2D(BoxCollider2D collider)
        {
            BoxColliders2D.Add(collider);
        }

        internal static void RemoveBoxCollider2D(BoxCollider2D collider)
        {
            BoxColliders2D.Remove(collider);
        }
        #endregion

        // Initialize renderer
        // It draws stuff
        private void Renderer(object? sender, PaintEventArgs e)
        {
            // Camera origin points (to get center)
            Vector2 Move = ActiveCamera.Position;

            Graphics g = e.Graphics;
            // Unblur small resolution sprites
            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            // Clear screen
            g.Clear(Skybox);
            #region Camera
            // Adjust Camera Position
            g.TranslateTransform(-ActiveCamera.Position.X + ScreenSize.X / 2f, -ActiveCamera.Position.Y + ScreenSize.Y / 2f);
            // Adjust Camera Rotation
            g.TranslateTransform(Move.X, Move.Y);
            g.RotateTransform(ActiveCamera.Rotation);
            g.TranslateTransform(-Move.X, -Move.Y);
            // Adjust Camera Zoom
            g.TranslateTransform(Move.X, Move.Y);
            g.ScaleTransform(ActiveCamera.Zoom, ActiveCamera.Zoom);
            g.TranslateTransform(-Move.X, -Move.Y);
            #endregion
            #region Drawing
            // Draw RenderedObjects2D
            foreach (RenderedObject2D rendered in RenderedObjects2D.ToList())
            {
                switch(rendered) {
                    case Shape2D r:
                        g.FillRectangle(new SolidBrush(r.Color), r.Position.X, r.Position.Y, r.Scale.X, r.Scale.Y);
                        break;
                    case Sprite2D r:
                        g.DrawImage(r.Sprite, r.Position.X, r.Position.Y, r.Scale.X, r.Scale.Y);
                        break;
                }
            }
            //g.DrawImage(sprite.Sprite, sprite.Position.X, sprite.Position.Y, sprite.Scale.X, sprite.Scale.Y);
            #endregion
            // Reset Transform
            g.ResetTransform();
        }

        /// <summary>
        /// This runs after the game is loaded
        /// </summary>
        public virtual void Load() { }

        /// <summary>
        /// This runs before the frame is rendered
        /// </summary>
        public virtual void Draw() { }

        /// <summary>
        /// This runs after the frame is rendered
        /// </summary>
        public virtual void Update() { }

        /// <summary>
        /// This runs when a key is pressed
        /// </summary>
        public virtual void KeyDown(KeyEventArgs e) { }

        /// <summary>
        /// This runs when a key is released
        /// </summary>
        public virtual void KeyUp(KeyEventArgs e) { }

        #region Additional Methods
        public static Bitmap BitmapFromFile(string filename)
        {
            Image tmp = Image.FromFile($"Assets/{filename}.png");
            return new Bitmap(tmp);
        }
        #endregion
    }
}
