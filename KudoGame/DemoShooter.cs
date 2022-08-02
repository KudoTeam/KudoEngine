using KudoEngine;

namespace KudoGame
{
    // This is good engine code
    // You can copy this to your own game
    internal class DemoShooter : Kudo
    {
        public DemoShooter() : base(new Vector2(600, 600), "Shooter Game") { }

        // === VARIABLES === //

        // Player
        Sprite2D player;
        BoxCollider2D playerCollider;

        int playerHealth = 5;
        readonly int immunityFrame = 50;
        int lastDamage = 0;

        readonly float MovementSpeed = 5f;

        // shooting
        bool canShoot = true;
        readonly List<ExpandableObject> bullets = new();
        readonly float bulletSpeed = 15f;

        // Enemies
        readonly int maxEnemies = 5;
        readonly int spawnDelay = 75;

        readonly List<ExpandableObject> enemies = new();

        // UI
        readonly string healthTextString = "Health: ";
        Text2D healthText;

        // === FUNCTIONS === //

        public override void Load()
        {
            Skybox = Color.FromArgb(41, 41, 41);

            // Player
            player = new Sprite2D(new(), new(85, 85), BitmapFromFile("Sprites/Bro"));
            playerCollider = new BoxCollider2D(player, "player", new(-20f,-10f));

            // UI
            healthText = new Text2D(new(10, 10), new(100, 20), healthTextString + playerHealth, Color.Red, default, "HealthText", 1000);
        }

        public override void Update()
        {
            // Movement
            Vector2 axis = new();
            if (Input.IsKeyDown(Keys.W))
            {
                axis.Y = -1;
            } else
            if (Input.IsKeyDown(Keys.S))
            {
                axis.Y = 1;
            }
            if (Input.IsKeyDown(Keys.A))
            {
                axis.X = -1;
            } else
            if (Input.IsKeyDown(Keys.D))
            {
                axis.X = 1;
            }
            player.Position.Y += MovementSpeed * axis.Y;
            player.Position.X += MovementSpeed * axis.X;

            // Enemy Spawn System
            if (Timer % spawnDelay == 0 && enemies.Count < maxEnemies)
            {
                Random rnd = new Random();

                // Enemy
                ExpandableObject enemy = new();
                enemy.Set("sprite", new Sprite2D(new(rnd.Next(-((int)ScreenSize.X / 2), (int)ScreenSize.X / 2), rnd.Next(-((int)ScreenSize.Y / 2), (int)ScreenSize.Y / 2)), new(85, 85), BitmapFromFile("Sprites/Zombie")));
                enemy.Set("collider", new BoxCollider2D(enemy.Get("sprite"), "enemy"));
                

                enemies.Add(enemy);
            }

            // Enemy Behaviour
            foreach (ExpandableObject enemy in enemies)
            {
                // Move Towards Player
                enemy.Get("sprite").Position = enemy.Get("sprite").Position.MoveTowards(player.Position, 1f);

                // Damage Player
                if (enemy.Get("collider").IsColliding(playerCollider) && immunityFrame < Timer - lastDamage && playerHealth > 0)
                {
                    playerHealth--;
                    lastDamage = Timer;
                    healthText.Text = healthTextString + playerHealth;
                }

                // Get Pushed By Player
                if (enemy.Get("collider").IsColliding(playerCollider))
                {
                    enemy.Get("sprite").Position = enemy.Get("sprite").Position.MoveTowards(player.Position, -MovementSpeed * 2);
                }

                // Die By Bullet
                if (enemy.Get("collider").IsColliding(out BoxCollider2D bulletCollider, "bullet"))
                {
                    if (bulletCollider.Rendered.IsAlive)
                    {
                        enemies.Remove(enemy);
                        enemy.Get("sprite").Kill();
                        bulletCollider.Rendered.Kill();
                    }
                }
            }

            // Bullet Behaviour
            foreach (ExpandableObject bullet in bullets)
            {
                // Shoot in direction of cursor
                bullet.Get("sprite").Position = bullet.Get("sprite").Position.Add(new Vector2((float)Math.Cos(bullet.Get("direction")), (float)Math.Sin(bullet.Get("direction"))).Multiply(new(bulletSpeed)));
            }
        }

        public ExpandableObject? Shoot(double angle)
        {
            if (canShoot)
            {

                ExpandableObject bullet = new();
                bullet.Set("sprite", new Sprite2D(new(player.Center().X - 10, player.Center().Y - 10), new(20, 20), BitmapFromFile("Sprites/Bullet")));
                bullet.Set("collider", new BoxCollider2D(bullet.Get("sprite"), "bullet"));
                bullet.Set("direction", angle);
                bullet.Set("active", true);

                bullets.Add(bullet);

                canShoot = false;

                return bullet;
            }

            return null;
        }

        public override void MouseDown(MouseEventArgs e)
        {
            // Calculate cursor angle relative to player
            double cursorAngle = Math.Atan2(Input.MousePosition.Y - player.Position.Y, Input.MousePosition.X - player.Position.X);
            Shoot(cursorAngle);
            
        }

        public override void MouseUp(MouseEventArgs e)
        {
            canShoot = true;
        }
    }
}