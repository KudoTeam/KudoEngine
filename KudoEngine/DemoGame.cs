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
        public DemoGame() : base(new Vector2(615, 515),"Kudo Demo Game") { }

        Sprite2D player;

        public override void Load()
        {
            Skybox = Color.Black;
            player = new(new(ScreenSize.X/2,ScreenSize.Y-200),new(100,100),"ship");
            player.Position.X -= player.Scale.X / 2;
            Log.mark("Resources Loaded");
        }

        public override void Draw()
        {

        }

        public override void Update()
        {
            player.Position.Y -= 1f;
        }
    }
}
