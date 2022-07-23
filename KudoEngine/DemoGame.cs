using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using KudoEngine.Engine;
using KudoEngine.Engine.Objects;
using KudoEngine.Engine.Extenders;

// This file's code is effortless and is
// just for testing purposes
namespace KudoEngine
{
    internal class DemoGame : Kudo
    {

        bool left;
        bool right;
        bool up;
        bool down;

        Vector2 gridSize = new(10, 8);

        public float DefaultMovementSpeed = 8f;

        string[,] Map = new string[10,60];

        public DemoGame() : base(new Vector2(615, 515),"Kudo Test Demo") { }

        Sprite2D player;
        BoxCollider2D playerCollider;
        Physics2D playerPhysics;
        Sprite2D eoc;
        BoxCollider2D eocCollider;

        public Vector2 lastPos;

        Camera cam1;
        Camera cam2;

        int eocVelocity = -1;
        int eocSpeed = 1;

        public void MapGen()
        {
            Random rnd = new Random();
            int height = rnd.Next(2, 6);
            int lastTree = 0;
            for (int i = 0; i < Map.GetLength(1); i++)
            {
                for (int j = 0; j <= height; j++)
                {
                    if (0 <= Map.GetLength(0) - 1 - j)
                    {
                        Map[Map.GetLength(0) - 1 - j, i] = "g";
                    }
                }
                if (rnd.Next(0,9) == 0 && Map.GetLength(0) - 2 - height >= 0)
                {
                    Map[Map.GetLength(0)-2-height, i] = "b";
                }
                if (rnd.Next(0,4) == 0
                    && lastTree - i < -3
                    && Map.GetLength(0) - 5 - height >= 0
                    && Map.GetLength(1) - 1 > i + 1
                    && 0 <= i - 1)
                {
                    Map[Map.GetLength(0) - 2 - height, i] = "p";
                    Map[Map.GetLength(0) - 3 - height, i] = "p";
                    Map[Map.GetLength(0) - 4 - height, i + 1] = "g";
                    Map[Map.GetLength(0) - 4 - height, i - 1] = "g";
                    Map[Map.GetLength(0) - 4 - height, i] = "g";
                    Map[Map.GetLength(0) - 5 - height, i] = "g";

                    lastTree = i;
                }
                if (height > 0)
                {
                    if (rnd.Next(0,2) == 0)
                    {
                        height += rnd.Next(-1, 2);
                    }
                } else
                {
                    height++;
                }
                
            }
        }

        public void MapRender()
        {
            for (int i = 0; i < Map.GetLength(1); i++)
            {
                for (int j = 0; j < Map.GetLength(0); j++)
                {
                    if (Map[j, i] == "g")
                    {
                        new BoxCollider2D(new Shape2D(new(i * ScreenSize.X / gridSize.X, j * ScreenSize.Y / gridSize.Y), new(ScreenSize.X / gridSize.X, ScreenSize.Y / gridSize.Y), Color.DarkGreen, "Ground"), "tiles");
                    }
                    else if (Map[j, i] == "p")
                    {
                        new BoxCollider2D(new Sprite2D(new(i * ScreenSize.X / gridSize.X, j * ScreenSize.Y / gridSize.Y), new(ScreenSize.X / gridSize.X, ScreenSize.Y / gridSize.Y), "plank", "Wood"), "tiles");
                    }
                }
            }
        }

        public override void Load()
        {
            Skybox = Color.Aqua;

            cam1 = new(new());
            cam2 = new(new(100, 100));

            ActiveCamera = cam1;

            lastPos = new();

            MapGen();
            MapRender();

            eoc = new(new(550, 100), new(400, 200), "eoc", "Boss");
            eocCollider = new(eoc, "bosses", -30f);
            player = new(new(150, 150), new(50, 100), "guide", "Player");
            playerCollider = new(player, "player", -5f);
            playerPhysics = new Physics2D(playerCollider, new(new string[] {"tiles"}));

            // This is duplicate code, but I want bushed to render before the player
            // TODO: Add rendering layers
            for (int i = 0; i < Map.GetLength(1); i++)
            {
                for (int j = 0; j < Map.GetLength(0); j++)
                {
                    if (Map[j, i] == "b")
                    {
                        new BoxCollider2D(new Sprite2D(new(i * ScreenSize.X / gridSize.X, j * ScreenSize.Y / gridSize.Y), new(ScreenSize.X / gridSize.X, ScreenSize.Y / gridSize.Y), "bush", "Bush"), "bushes");
                    }
                }
            }
                }

        public override void Draw()
        {
            
        }

        public void stopMovementOnCollision() {
            if (playerCollider.IsColliding(new(new string[] {"tiles"})))
            {
                player.Position.X = lastPos.X;
                player.Position.Y = lastPos.Y;
            }
            else
            {
                lastPos.X = player.Position.X;
                lastPos.Y = player.Position.Y;
            }
        }

        int timer = 0;
        public override void Update()
        {
            playerPhysics.Update();

            float MovementSpeed = DefaultMovementSpeed;

            if (playerCollider.IsColliding(new(new string[] { "bushes" })))
            {
                MovementSpeed /= 5f;
            }

            if (up)
            {
                playerPhysics.Velocity.Y = -MovementSpeed;
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

            if (playerCollider.IsColliding(new(new string[] { "bosses" })))
            {
                player.Kill();
                Skybox = Color.DarkRed;
            }
        }

        public override void KeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
{
                up = true;
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
