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
        public Camera ActiveCamera = new(new());
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
            // Send Key Input
            Window.KeyDown += InputKeyDown;
            Window.KeyUp += InputKeyUp;
            // Check Mouse Events
            Window.MouseDown += MouseDown;
            Window.MouseUp += MouseUp;
            Window.MouseMove += MouseMove;
            // Check Mouse Input
            Window.MouseDown += InputMouseDown;
            Window.MouseUp += InputMouseUp;
            Window.MouseMove += InputMouseMove;
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
        // Key Events
        private void KeyDown(object? sender, KeyEventArgs e)
        {
            KeyDown(e);
        }

        private void KeyUp(object? sender, KeyEventArgs e)
        {
            KeyUp(e);
        }

        // Key Input
        private static void InputKeyDown(object? sender, KeyEventArgs e)
        {
            Input.PressedKeys[e.KeyCode] = true;
        }

        private static void InputKeyUp(object? sender, KeyEventArgs e)
        {
            Input.PressedKeys.Remove(e.KeyCode);
        }

        // Mouse Events
        private void MouseDown(object? sender, MouseEventArgs e)
        {
            MouseDown(e);
        }

        private void MouseUp(object? sender, MouseEventArgs e)
        {
            MouseUp(e);
        }

        private void MouseMove(object? sender, MouseEventArgs e)
        {
            MouseMove(e);
        }

        // Mouse Input
        private void InputMouseDown(object? sender, MouseEventArgs e)
        {
            Input.PressedMouse[e.Button] = true;
        }

        private void InputMouseUp(object? sender, MouseEventArgs e)
        {
            Input.PressedMouse.Remove(e.Button);
        }

        private void InputMouseMove(object? sender, MouseEventArgs e)
        {
            // TODO: Compatibility with Rotation and Zoom
            Input.MousePosition = new(e.Location.X - ScreenSize.X / 2 + ActiveCamera.Position.X,
                e.Location.Y - ScreenSize.Y / 2 + ActiveCamera.Position.Y);

            Input.ScreenMousePosition = new(e.Location.X, e.Location.Y);
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
                    Window.BeginInvoke((MethodInvoker) delegate { Window.Refresh(); });
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
            void ApplyCamera()
            {
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
            }
            #endregion
            #region Layer Binary Sort
            static List<RenderedObject2D> BiSort(List<RenderedObject2D> list)
            {
                bool run = true;
                while (run)
                {
                    int curr = -1000;
                    bool finished = true;
                    for (int i = 0; i < list.Count; i++)
                    {
                        int prev = curr;
                        curr = list[i].Layer;
                        if (prev > curr)
                        {
                            finished = false;
                            RenderedObject2D tmp = list[i];
                            list[i] = list[i - 1];
                            list[i - 1] = tmp;
                            break;
                        }
                    }
                    if (finished) { run = false; }
                }
                return list;
            }
            #endregion
            #region Drawing
            List<RenderedObject2D> queue = BiSort(RenderedObjects2D.ToList());
            foreach (RenderedObject2D rendered in queue)
            {
                // Apply Camera Effects (unless disabled with layer 1000)
                if (rendered.Layer != 1000) { ApplyCamera(); }

                // Draw RenderedObjects2D
                switch (rendered) {
                    case Shape2D r:
                        g.FillRectangle(new SolidBrush(r.Color), r.Position.X, r.Position.Y, r.Scale.X, r.Scale.Y);
                        break;
                    case Sprite2D r:
                        g.DrawImage(r.Sprite, r.Position.X, r.Position.Y, r.Scale.X, r.Scale.Y);
                        break;
                    case Text2D r:
                        g.DrawString(r.Text, r.Font, new SolidBrush(r.Color), new Rectangle((int)r.Position.X, (int)r.Position.Y, (int)r.Scale.X, (int)r.Scale.Y));
                        break;
                }

                // Reset Transform
                g.ResetTransform();
            }
            #endregion
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

        /// <summary>
        /// This runs when the mouse is clicked in the window
        /// </summary>
        /// <param name="e"></param>
        public virtual void MouseDown(MouseEventArgs e) { }

        /// <summary>
        /// This runs when the mouse is released in the window
        /// </summary>
        /// <param name="e"></param>
        public virtual void MouseUp(MouseEventArgs e) { }

        /// <summary>
        /// This runs when the mouse is moved in the window
        /// </summary>
        /// <param name="e"></param>
        public virtual void MouseMove(MouseEventArgs e) { }

        #region Additional Methods
        public static Bitmap BitmapFromFile(string filename)
        {
            Image tmp = Image.FromFile($"Assets/{filename}.png");
            return new Bitmap(tmp);
        }
        #endregion
    }
}
