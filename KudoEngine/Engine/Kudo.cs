using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;


namespace KudoEngine.Engine
{
    class Canvas : Form
    {
        public Canvas()
        {
            this.DoubleBuffered = true;
        }
    }

    public abstract class Kudo
    {
        public Vector2 ScreenSize { get; private set; } = new(512,512);
        private string Title = "Kudo Game";
        private Canvas Window;
        private Thread GameLoopThread;

        // Accessible by the user
        /// <summary>
        /// Window Background Color
        /// </summary>
        public Color Skybox = Color.White;
        /// <summary>
        /// Current Camera
        /// </summary>
        public Camera ActiveCamera = new(new(),1);

        public Kudo(Vector2 screenSize, string title)
        {
            ScreenSize = screenSize;
            Title = title;

            Window = new Canvas();
            Window.Size = new Size((int)ScreenSize.X, (int)ScreenSize.Y);
            Window.Text = Title;
            Window.Paint += Renderer;

            // Make a thread to refresh window asynchronously
            GameLoopThread = new Thread(GameLoop/*here*/);
            GameLoopThread.Start();

            // Start window
            Application.Run(Window);
        }

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
                    Thread.Sleep(1);
                }
                catch
                {
                    Log.warn("Waiting for Window...");
                }
            }
        }

        #region Shape2D
        private static List<Shape2D> Shapes2D = new();

        public static void AddShape2D(Shape2D shape)
        {
            Shapes2D.Add(shape);
        }

        public static void RemoveShape2D(Shape2D shape)
        {
            Shapes2D.Remove(shape);
        }
        #endregion

        #region Sprite2D
        private static List<Sprite2D> Sprites2D = new();

        public static void AddSprite2D(Sprite2D sprite)
        {
            Sprites2D.Add(sprite);
        }

        public static void RemoveSprite2D(Sprite2D sprite)
        {
            Sprites2D.Remove(sprite);
        }
        #endregion

        // Initialize renderer
        // It draws stuff
        private void Renderer(object? sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            // Clear screen
            g.Clear(Skybox);
            // Adjust Camera Position
            g.TranslateTransform(-ActiveCamera.Position.X, -ActiveCamera.Position.Y);
            // Adjust Camera Rotation
            g.RotateTransform(ActiveCamera.Rotation);
            // Adjust Camera Zoom
            g.ScaleTransform(ActiveCamera.Zoom, ActiveCamera.Zoom);
            // Draw shapes
            foreach (Shape2D shape in Shapes2D)
            {
                g.FillRectangle(new SolidBrush(shape.Color), shape.Position.X, shape.Position.Y, shape.Scale.X, shape.Scale.Y);
            }
            // Draw sprites
            foreach (Sprite2D sprite in Sprites2D)
            {
                g.DrawImage(sprite.Sprite, sprite.Position.X, sprite.Position.Y, sprite.Scale.X, sprite.Scale.Y);
            }
        }

        /// <summary>
        /// This runs after the game is loaded
        /// </summary>
        public abstract void Load();

        /// <summary>
        /// This runs before the frame is rendered
        /// </summary>
        public abstract void Draw();

        /// <summary>
        /// This runs after the frame is rendered
        /// </summary>
        public abstract void Update();
    }
}
