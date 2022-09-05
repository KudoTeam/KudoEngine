using KudoEngine;

namespace KudoGame
{
    class GameButton
    {
        public Button2D Button { get; set; }
        public bool canPress = true;

        public GameButton(Button2D button)
        {
            Button = button;
        }

        public bool IsClicked()
        {
            if (Button.IsPressed())
            {
                if (!canPress) { return false; }
                canPress = false;
                return true;
            }
            else
            {
                canPress = true;
            }

            return false;
        }

        public void TransitionButton(float tSize, float speed)
        {
            if (Button.Rendered.Scale.X > tSize)
            {
                Button.Rendered.Scale = Button.Rendered.Scale.Subtract(new(speed));
                Button.Rendered.Position = Button.Rendered.Position.Add(new(speed / 2f));
            }
            else if (Button.Rendered.Scale.X < tSize)
            {
                Button.Rendered.Scale = Button.Rendered.Scale.Add(new(speed));
                Button.Rendered.Position = Button.Rendered.Position.Subtract(new(speed / 2f));
            }
        }
    }

    class Upgrade : GameButton
    {
        public int Cost { get; set; }
        public float CostIncrease { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; } = 0;

        private Text2D _text1;
        private Text2D _text2;

        public Upgrade(float yPos, Color color,int cost, string name, float costIncrease = 2f, int amount = 0) : base(new Button2D(new Shape2D(new(15f, yPos), new(150f, 50f), color, default, "Upgrade", 1000)))
        {
            Cost = cost;
            CostIncrease = costIncrease;
            Name = name;
            Amount = amount;

            _text1 = new(Button.Rendered.Center().Subtract(new(60f, 17f)), new(200f, 50f), Name, Color.Black, default, new("Arial", 10f, FontStyle.Bold), "UpgradeName", 1000);
            _text2 = new(Button.Rendered.Center().Subtract(new(60f, 0f)), new(200f, 50f), $"Cost: {Cost}     x{Amount}", Color.Black, default, new("Arial", 10f, FontStyle.Bold), "UpgradeInfo", 1000);
        }

        public bool Buy(int score, out int cost)
        {
            cost = Cost;
            if (score >= Cost)
            {
                Amount++;
                Cost = (int)Math.Round(Cost * CostIncrease);
                _text2.Text = $"Cost: {Cost}     x{Amount}";
                return true;
            }
            return false;
        }
    }

    internal class DemoClicker : Kudo
    {
        public DemoClicker() : base(new Vector2(512, 512), "Cookie Clicker") { }

        // CODE START //

        // cookie
        GameButton cookie;
        // score
        Text2D scoreText;
        int score = 0;

        // UPGRADES
        Upgrade scorePerClick;
        Upgrade autoClicker;
        Upgrade luckPotion;

        public override void Load()
        {
            Skybox = Color.Black;

            // cookie
            cookie = new GameButton(new Button2D(new Sprite2D(new(-70f,-175f), new(300f), BitmapFromFile("Sprites/cookie"))));

            // upgrades
            scorePerClick = new Upgrade(100f, Color.PowderBlue, 5, "Score Per Click", 1.3f, 1);
            autoClicker = new Upgrade(175f, Color.White, 100, "Auto-Clicker", 1.5f, 0);
            luckPotion = new Upgrade(250f, Color.Purple, 150, "Luck Potion", 1.1f, 0);

            // score
            scoreText = new Text2D(new(10f), new(200f), $"{score}", Color.Red, default, new("Arial", 20f, FontStyle.Bold), "scoreText", 1000);
        }

        public override void Update()
        {
            // spin cookie
            cookie.Button.Rendered.Rotation += 50f * Time.DeltaTimeSeconds;

            // hover effect
            if (cookie.Button.IsHovered() )
            {
                cookie.TransitionButton(300f, 4f);
            } else
            {
                cookie.TransitionButton(270f, 4f);
            }

            // click effect
            if (cookie.Button.IsPressed())
            {
                cookie.TransitionButton(260f, 10f);
            }

            // click function
            if (cookie.IsClicked())
            {
                score += scorePerClick.Amount;
                if (new Random().Next(0,1000) < luckPotion.Amount) {
                    score *= 2;
                }
            }

            // upgrades
            if (scorePerClick.IsClicked())
            {
                if(scorePerClick.Buy(score, out int cost)) { score -= cost; }
            }
            // ================================================
            if (autoClicker.IsClicked())
            {
                if (autoClicker.Buy(score, out int cost)) { score -= cost; }
            }
            if (Time.Elapsed  == 0) { score += autoClicker.Amount; }
            // ================================================
            if (luckPotion.IsClicked())
            {
                if (luckPotion.Buy(score, out int cost)) { score -= cost; }
            }

            // update score text
            scoreText.Text = $"{score}";
        }

    }
}