using KudoEngine;

// This file's code is effortless and is
// just for testing purposes
namespace KudoGame
{
    internal class DemoGame : Kudo
    {

        bool left;
        bool right;
        bool up;
        bool down;

        Vector2 gridSize = new(16, 9);

        public float DefaultMovementSpeed = 8f;

        string[,] Map = new string[10,60];

        public DemoGame() : base(new Vector2(1080, 607),"Kudo Test Demo") { }

        Sprite2D player;
        BoxCollider2D playerCollider;
        BoxCollider2D playerGroundCollider;
        Physics2D playerPhysics;
        Sprite2D eoc;
        BoxCollider2D eocCollider;

        float playerScaleY;

        public Vector2 lastPos;

        Camera cam1;

        List<Sprite2D> coins = new();

        SpriteSheet s = new("2T");
        SpriteSheet ad = new("ad");


        int eocVelocity = -1;
        int eocSpeed = 1;

        public bool isGrounded(BoxCollider2D playerGroundCollider)
        {
            if (playerGroundCollider.IsColliding(new(new string[] {"tiles"})))
            {
                return true;
            }
            return false;
        }

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
                if (rnd.Next(0, 9) == 0 && Map.GetLength(0) - 2 - height >= 0)
                {
                    Map[Map.GetLength(0) - 2 - height, i] = "c";
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
                    else if (Map[j, i] == "c")
                    {
                        new BoxCollider2D(new Sprite2D(new(i * ScreenSize.X / gridSize.X, j * ScreenSize.Y / gridSize.Y), new(ScreenSize.X / gridSize.X, ScreenSize.Y / gridSize.Y), s.GetSprite("coin"), "Coin"), "collectible");
                    }
                }
            }
        }

        public override void Load()
        {
            Skybox = Color.Aqua;

            cam1 = new(new());

            ActiveCamera = cam1;

            s.AddSprite("coin", new(new(225, 385), new(35, 30)));

            ad.AddSprite("idle", new(new(5, 11), new(18, 20)));
            for(int i = 0;i<8;i++)
            {
                ad.AddSprite("walk"+i, new(new(5+32*i, 41), new(18, 20)));
            }

            MapGen();
            MapRender();

            eoc = new(new(550, 200), new(400, 200), "eoc", "Boss");
            eocCollider = new(eoc, "bosses", new(-30f,-30f));
            player = new(new(150, 150), new(70, 100), ad.GetSprite("idle"), "Player");
            playerCollider = new(player, "player", new(-21.5f,-3f));
            playerPhysics = new Physics2D(playerCollider, new(new string[] {"tiles"}));
            playerPhysics.Weight = 10f;
            playerGroundCollider = new(player, "Ground Check", new(5f, 0f), new(0f, 1f));

            playerScaleY = player.Scale.Y;

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
            playerPhysics.Update();
        }

        int animation = 0;

        public override void Update()
        {      
            float MovementSpeed = DefaultMovementSpeed;

            cam1.Position = player.Center();

            if (playerCollider.IsColliding(new(new string[] { "bushes" })))
            {
                MovementSpeed /= 5f;
            }

            if (up && isGrounded(playerGroundCollider))
            {
                playerPhysics.Velocity.Y = -MovementSpeed*2;
            }
            if (down)
            {
                player.Position.Y += MovementSpeed;
            }
            if (left)
            {
                // TODO: Implement this as an Extender
                if (Timer % 5 == 0)
                {
                    animation++;
                    if (animation > 7)
                    {
                        animation = 0;
                    }
                    player.Sprite = ad.GetSprite("walk" + animation);
                }
                playerPhysics.Velocity.X = -MovementSpeed;
            }
            if (right)
            {
                if (Timer % 5 == 0)
                {
                    animation++;
                    if (animation > 7)
                    {
                        animation = 0;
                    }
                    player.Sprite = ad.GetSprite("walk" + animation);
                }
                playerPhysics.Velocity.X = MovementSpeed;
            }

            if (eoc.Position.X <= 300)
            {
                eocVelocity = 1;
            } else if (eoc.Position.X >= 600)
            {
                eocVelocity = -1;
            }

            var a = playerCollider.GetCollisions();
            foreach (var o in a)
            {
                if (o.Tag == "collectible" && o.Subject.IsAlive)
                {
                    o.Subject.Kill();
                }
            }

            eoc.Position.X += eocSpeed * eocVelocity;

            if (playerCollider.IsColliding(new(new string[] { "bosses" })))
            {
                playerCollider.Subject.Kill();
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
            if (e.KeyCode == Keys.V)
            {
                ActiveCamera.Rotation += 1;
            }
            if (e.KeyCode == Keys.C)
            {
                ActiveCamera.Rotation -= 1;
            }
            if (e.KeyCode == Keys.G)
            {
                ActiveCamera.Zoom += 0.01f;
            }
            if (e.KeyCode == Keys.F)
            {
                ActiveCamera.Zoom -= 0.01f;
            }
            if (e.KeyCode == Keys.LControlKey)
            {
                playerCollider.ScaleModifier.Y = player.Scale.Y / 2f;
                playerCollider.PositionModifier.Y -= player.Scale.Y / 2f;

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
                animation = 0;
                player.Sprite = ad.GetSprite("idle");
                left = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                animation = 0;
                player.Sprite = ad.GetSprite("idle");
                right = false;
            }
            if (e.KeyCode == Keys.LControlKey)
            {
                playerCollider.PositionModifier.Y += player.Scale.Y / 2f;
                playerCollider.ScaleModifier.Y = playerScaleY;

            }
        }
    }
}
