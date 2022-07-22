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
        string[,] Map =
        {
            {".",".",".",".",".",".",".",".","."},
            {".","g",".",".",".",".",".",".","."},
            {".","g",".",".",".",".","g",".","."},
            {".","g",".",".",".","g","g","g","."},
            {".","g","g",".","g","g","g","g","g"},
            {"g","g","g","g","g","g","g","g","g"},
            {"g","g","g","g","g","g","g","g","g"},

        };

        public DemoGame() : base(new Vector2(615, 515),"Terraria") { }

        Sprite2D player;

        Camera cam1;
        Camera cam2;

        public override void Load()
        {
            Skybox = Color.Aqua;

            cam1 = new(new());
            cam2 = new(new(100, 100));

            ActiveCamera = cam1;

            player = new(new(150,195), new(50,100), "guide", "Player");

            for(int i = 0; i < Map.GetLength(1); i++)
            {
                for(int j = 0; j < Map.GetLength(0); j++)
                {
                    if (Map[j,i] == "g")
                    {
                        new Shape2D(new(i * ScreenSize.X / Map.GetLength(1), j * ScreenSize.Y / Map.GetLength(0)), new(ScreenSize.X / Map.GetLength(1), ScreenSize.Y / Map.GetLength(0)), Color.Yellow, "Ground");
                    }
                }
            }
        }

        public override void Draw()
        {

        }

        int timer = 0;
        public override void Update()
        {
            if (timer % 50 == 0)
            {
                if (ActiveCamera == cam1)
                {
                    ActiveCamera = cam2;
                } else
                {
                    ActiveCamera = cam1;
                }
            }

            cam1.Position.X = player.Position.X - ScreenSize.X / 2 + player.Scale.X / 2;
            cam1.Rotation += 1f;

            cam2.Zoom += 0.01f;
            timer++;
        }
    }
}
