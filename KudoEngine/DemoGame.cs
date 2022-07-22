using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KudoEngine.Engine;
using System.Drawing;

namespace KudoEngine
{
    internal class DemoGame : Kudo
    {

        bool left;
        bool right;
        bool up;
        bool down;

        Vector2 gridSize = new(9, 7);

        public float MovementSpeed = 3f;

        string[,] Map =
        {
            {".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","."},
            {".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","."},
            {".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","."},
            {".",".",".",".",".",".",".",".",".",".",".",".",".",".","g",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","."},
            {".",".",".",".","g","g","g","g","g",".",".",".",".",".","g",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","."},
            {".",".",".",".",".","g","g","g",".",".",".",".","g","g","g","g","g",".",".",".",".",".",".",".",".",".",".",".",".",".","."},
            {".",".",".",".",".",".","g",".",".",".",".",".",".",".","g",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","."},
            {".",".",".",".",".",".",".",".",".",".",".",".",".",".","g",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","."},
            {".",".",".",".",".",".",".","g",".",".",".",".",".",".",".",".",".","g",".",".",".",".",".",".",".",".","b",".",".",".","."},
            {".",".",".",".",".",".","g","g","g",".",".",".",".",".",".",".","g","g","g",".",".",".","g",".",".","g","g","g","g",".","."},
            {".",".",".",".",".",".",".","p",".",".",".",".",".",".",".",".",".","p",".",".",".","g","g","g",".","g",".",".","g",".","."},
            {".",".",".","b",".",".",".","p",".","b",".",".","p","p","p",".",".","p",".",".",".",".","p",".",".","g",".",".","g",".","."},
            {".","b","g","g","g",".",".","g","g","g","g","g","g","g","g","g","g","g","g","g",".",".","p",".",".","g",".",".","g",".","."},
            {"g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g"},
            {"g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g"},
            {"g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g"},
            {"g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g","g"},

        };

        public DemoGame() : base(new Vector2(615, 515),"Kudo Test Demo") { }

        Sprite2D player;
        Sprite2D eoc;

        Camera cam1;
        Camera cam2;

        int eocVelocity = -1;
        int eocSpeed = 1;

        public override void Load()
        {
            Skybox = Color.Aqua;

            cam1 = new(new());
            cam2 = new(new(100, 100));

            ActiveCamera = cam1;

            for(int i = 0; i < Map.GetLength(1); i++)
            {
                for(int j = 0; j < Map.GetLength(0); j++)
                {
                    if (Map[j, i] == "g")
                    {
                        new Shape2D(new(i * ScreenSize.X / gridSize.X, j * ScreenSize.Y / gridSize.Y), new(ScreenSize.X / gridSize.X, ScreenSize.Y / gridSize.Y), Color.DarkGreen, "Ground");
                    }
                    else if (Map[j, i] == "b")
                    {
                        new Sprite2D(new(i * ScreenSize.X / gridSize.X, j * ScreenSize.Y / gridSize.Y), new(ScreenSize.X / gridSize.X, ScreenSize.Y / gridSize.Y), "bush", "Bush");
                    } 
                    else if (Map[j, i] == "p")
                    {
                        new Sprite2D(new(i * ScreenSize.X / gridSize.X, j * ScreenSize.Y / gridSize.Y), new(ScreenSize.X / gridSize.X, ScreenSize.Y / gridSize.Y), "plank", "Wood");
                    }
                }
            }

            eoc = new(new(550, 100), new(400, 200), "eoc", "Boss");
            player = new(new(150, 195), new(50, 100), "guide", "Player");
        }

        public override void Draw()
        {

        }

        int timer = 0;
        public override void Update()
        {
            if (up)
            {
                player.Position.Y -= MovementSpeed;
            }
            if (down)
            {
                player.Position.Y += MovementSpeed;
            }
            if (left)
            {
                player.Position.X -= MovementSpeed;
            }
            if (right)
            {
                player.Position.X += MovementSpeed;
            }

            cam1.Position.X = player.Position.X - ScreenSize.X / 2 + player.Scale.X / 2;
            cam1.Position.Y = player.Position.Y - ScreenSize.Y / 2 + player.Scale.Y / 2;

            if (eoc.Position.X <= 300)
            {
                eocVelocity = 1;
            } else if (eoc.Position.X >= 600)
            {
                eocVelocity = -1;
            }

            eoc.Position.X += eocSpeed * eocVelocity;
        }

        public override void KeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
{
                up = true;
            }
            if (e.KeyCode == Keys.Down)
            {
                down = true;
            }
            if (e.KeyCode == Keys.Left)
            {
                left = true;
            }
            if (e.KeyCode == Keys.Right)
            {
                right = true;
            }
        }

        public override void KeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                up = false;
            }
            if (e.KeyCode == Keys.Down)
            {
                down = false;
            }
            if (e.KeyCode == Keys.Left)
            {
                left = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                right = false;
            }
        }
    }
}
