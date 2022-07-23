using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using KudoEngine.Engine.Objects;
using KudoEngine.Engine.Extenders;

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
            // Make Window Unresizable
            Window.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            // Render Screen
            Window.Paint += Renderer;
            // Check Key Events
            Window.KeyDown += KeyDown;
            Window.KeyUp += KeyUp;
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

        #region HardwareInteractions
        private void KeyDown(object? sender, KeyEventArgs e)
        {
            KeyDown(e);
        }

        private void KeyUp(object? sender, KeyEventArgs e)
        {
            KeyUp(e);
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
                    Thread.Sleep(1);
                }
                catch
                {
                    Log.warn("Waiting for Window...");
                }
            }
        }

        #region Shapes2D
        public static List<Shape2D> Shapes2D { get; private set; } = new();

        public static void AddShape2D(Shape2D shape)
        {
            Shapes2D.Add(shape);
        }

        public static void RemoveShape2D(Shape2D shape)
        {
            Shapes2D.Remove(shape);
        }
        #endregion

        #region Sprites2D
        public static List<Sprite2D> Sprites2D { get; private set; } = new();

        public static void AddSprite2D(Sprite2D sprite)
        {
            Sprites2D.Add(sprite);
        }

        public static void RemoveSprite2D(Sprite2D sprite)
        {
            Sprites2D.Remove(sprite);
        }
        #endregion

        #region BoxColliders2D
        public static List<BoxCollider2D> BoxColliders2D { get; private set; } = new();

        public static void AddBoxCollider2D(BoxCollider2D collider)
        {
            BoxColliders2D.Add(collider);
        }

        public static void RemoveBoxCollider2D(BoxCollider2D collider)
        {
            BoxColliders2D.Remove(collider);
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
            foreach (Shape2D shape in Shapes2D.ToList())
            {
                g.FillRectangle(new SolidBrush(shape.Color), shape.Position.X, shape.Position.Y, shape.Scale.X, shape.Scale.Y);
            }
            // Draw sprites
            foreach (Sprite2D sprite in Sprites2D.ToList())
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

        /// <summary>
        /// This runs when a key is pressed
        /// </summary>
        public abstract void KeyDown(KeyEventArgs e);

        /// <summary>
        /// This runs when a key is released
        /// </summary>
        public abstract void KeyUp(KeyEventArgs e);
    }
}
