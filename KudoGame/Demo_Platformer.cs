using KudoEngine;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.Xml;
using System.Text;

namespace KudoGame
{
    internal class Demo_Platformer : Kudo
    {

        #pragma warning disable CS8618
        public Demo_Platformer() : base(new Vector2(1080, 600), "Platformer") { }
        #pragma warning restore CS8618

        public Player Player;

        public static List<Projectile> Projectiles = new List<Projectile>();

        public override void Load()
        {
            Skybox = Color.Aqua;

            Player = new Player
            {
                JumpForce = 7.5f,
                MovementSpeed = 10f,
            };

            World ground = new(new Vector2(-100, 300), new Vector2(2000, 100), Color.Beige);
        }

        public override void Draw()
        {
            Player.Draw();
        }

        public override void Update()
        {
            ActiveCamera.Position = Player.Shape.Position;
            Player.Update();
            foreach (Projectile projectile in Projectiles)
            {
                projectile.Update();
            }
        }
    }


    internal class Player
    {
        public Shape2D Shape;
        public BoxCollider2D Collider;
        public Physics2D Physics;

        public float JumpForce;
        public float MovementSpeed;

        private Time _time = new();
        private bool _canJump = true;
        private BoxCollider2D _jumpTrigger;
        private bool _canShoot = true;

        public Player()
        {
            Shape = new Shape2D(position: new Vector2(0, 0), scale: new Vector2(40, 60), Color.Purple, tag: "Player", layer: 500);
            Collider = new BoxCollider2D(rendered: Shape, tag: "PlayerCollider");
            Physics = new Physics2D(Collider, new List<string> { "WorldCollider" })
            {
                Weight = 100f,
            };

            _jumpTrigger = new(rendered: Shape, tag: "PlayerTrigger", positionModifier: new Vector2(0, 1f));
        }

        public void Draw()
        {
            Physics.Update();
        }

        public void Update()
        {
            _playerMovement();
            _playerShoot();
        }

        void _playerMovement()
        {
            if (Input.IsKeyDown(Keys.Space) && _canJump) { Physics.Velocity.Y -= JumpForce /* * _time.DeltaTime*/; _canJump = false; }
            if (Input.IsKeyDown(Keys.A)) { Physics.Velocity.X -= MovementSpeed /* * _time.DeltaTime*/; }
            if (Input.IsKeyDown(Keys.D)) { Physics.Velocity.X += MovementSpeed /* * _time.DeltaTime*/; }

            if (_jumpTrigger.IsColliding("WorldCollider") && !_canJump) { _canJump = true; }
        }

        void _playerShoot()
        {
            if (Input.IsMouseDown(MouseButtons.Left) && _canShoot) 
            {
                _canShoot = false;

                _ = new Projectile(owner: Shape, Color.Blue)
                {
                    Speed = 30f,
                };
            }

            if (!Input.IsMouseDown(MouseButtons.Left) && !_canShoot)
            {
                _canShoot = true;
            }
        }
    }

    internal class Projectile
    {
        public Shape2D Shape;
        public BoxCollider2D Collider;

        public dynamic Owner;
        public float Speed = 1f;
        public uint Timeout = 5000; // how many milliseconds it exists

        private Time _time = new();

        public Projectile(RenderedObject2D owner, Color color, float? direction = null)
        {
            Demo_Platformer.Projectiles.Add(this);

            Vector2 origin = owner.Position.Add(owner.Scale.Divide(new Vector2(2)));
            Shape = new Shape2D(origin, scale: new Vector2(5f), color, rotation: direction ?? origin.GetRelativeAngle(Input.MousePosition), tag: "Projectile");
            Collider = new BoxCollider2D(rendered: Shape, tag: "ProjectileCollider");

            Owner = owner;
        }

        public void Update()
        {
            _move();
            _timeout();
        }

        void _move()
        {
            Shape.Position = Shape.Position.MoveInDirection(Shape.Rotation, Speed);
        }

        void _timeout()
        {
            if (_time.Elapsed >= Timeout || Collider.IsColliding("WorldCollider")) { Shape.Kill(); }
        }
    }

    internal class World
    {
        Shape2D Shape;
        BoxCollider2D Collider;

        public World(Vector2 position, Vector2 scale, Color color)
        {
            Shape = new Shape2D(position, scale, color, 0, tag: "World");
            Collider = new BoxCollider2D(rendered: Shape, tag: "WorldCollider");
        }
    }
}
