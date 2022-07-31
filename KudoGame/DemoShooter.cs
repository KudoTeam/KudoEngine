using KudoEngine;

namespace KudoGame
{
    // This is good engine code
    // You can copy this to your own game
    internal partial class DemoShooter : Kudo
    {
        public DemoShooter() : base(new Vector2(600, 600), "Shooter Game") { }

        // === VARIABLES === //

        // Player
        Sprite2D player;
        BoxCollider2D playerCollider;
        readonly float MovementSpeed = 5f;

        // Enemies
        Sprite2D enemy;
        BoxCollider2D enemyCollider;
        readonly List<Sprite2D> enemies = new();

        // === FUNCTIONS === //

        public override void Load()
        {
            Skybox = Color.FromArgb(41, 41, 41);

            // Player
            player = new Sprite2D(new(), new(85, 85), BitmapFromFile("Sprites/Bro"));
            playerCollider = new BoxCollider2D(player, "player");
        }

        public override void Update()
        {
            // Movement
            Vector2 axis = new();
            if (Input.IsKeyDown(Keys.Up))
            {
                axis.Y = -1;
            } else
            if (Input.IsKeyDown(Keys.Down))
            {
                axis.Y = 1;
            }
            if (Input.IsKeyDown(Keys.Left))
            {
                axis.X = -1;
            } else
            if (Input.IsKeyDown(Keys.Right))
            {
                axis.X = 1;
            }
            player.Position.Y += MovementSpeed * axis.Y;
            player.Position.X += MovementSpeed * axis.X;

            // Enemy Spawn System
            if (Timer % 100 == 0)
            {
                Random rnd = new Random();

                // Enemy
                enemy = new Sprite2D(new(rnd.Next(-((int)ScreenSize.X / 2), (int)ScreenSize.X / 2), rnd.Next(-((int)ScreenSize.Y / 2), (int)ScreenSize.Y / 2)), new(85, 85), BitmapFromFile("Sprites/Zombie"));
                enemyCollider = new BoxCollider2D(enemy, "enemy");

                enemies.Add(enemy);
            }

            // Enemy Behaviour
            foreach (Sprite2D enemy in enemies)
            {
                // Move Towards Player
                enemy.Position = enemy.Position.MoveTowards(player.Position, 1f);
            }
        }
    }
}