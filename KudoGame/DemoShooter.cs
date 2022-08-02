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
        Vector2 shootAxis = new();
        List<ExpandableObject> bullets = new();

        // Enemies
        int maxEnemies = 3;

        readonly List<ExpandableObject> enemies = new();

        // UI
        string healthTextString = "Health: ";
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
            if (Input.IsKeyDown(Keys.Up))
            {
                axis.Y = -1;
                shootAxis = axis.Copy();
            } else
            if (Input.IsKeyDown(Keys.Down))
            {
                axis.Y = 1;
                shootAxis = axis.Copy();
            }
            if (Input.IsKeyDown(Keys.Left))
            {
                axis.X = -1;
                shootAxis = axis.Copy();
            } else
            if (Input.IsKeyDown(Keys.Right))
            {
                axis.X = 1;
                shootAxis = axis.Copy();
            }
            player.Position.Y += MovementSpeed * axis.Y;
            player.Position.X += MovementSpeed * axis.X;

            // Enemy Spawn System
            if (Timer % 100 == 0 && enemies.Count < maxEnemies)
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
                    enemies.Remove(enemy);
                    enemy.Get("sprite").Kill();
                    bulletCollider.Rendered.Kill();
                }
            }

            // Drag Player
            if (Input.IsMouseDown())
            {
                player.Position = Input.MousePosition;
            }

            // Bullet Behaviour
            foreach (ExpandableObject bullet in bullets)
            {
                bullet.Get("sprite").Position.X += bullet.Get("direction").X * 20f;
                bullet.Get("sprite").Position.Y += bullet.Get("direction").Y * 20f;
            }
        }

        public override void KeyDown(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.X:
                    if (!canShoot) { break; }

                    ExpandableObject bullet = new();
                    bullet.Set("sprite", new Sprite2D(new(player.Center().X - 10, player.Center().Y - 10), new(20, 20), BitmapFromFile("Sprites/Bullet")));
                    bullet.Set("collider", new BoxCollider2D(bullet.Get("sprite"), "bullet"));
                    bullet.Set("direction", shootAxis.Copy());

                    bullets.Add(bullet);

                    canShoot = false;

                    break;
            }
        }

        public override void KeyUp(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.X:
                    canShoot = true;
                    break;
            }
        }
    }
}