using KudoEngine;
using KudoGame;

namespace KudoGame
{

    static class Score
    {
        public static float value;

        public static string String()
        {
            switch (value)
            {
                case >= 1_000_000_000_000_000f: return $"{Math.Round(value / 1_000_000_000_000_000f, 1)}Q";
                case >= 1_000_000_000_000f: return $"{Math.Round(value / 1_000_000_000_000f, 1)}T";
                case >= 1_000_000_000f: return $"{Math.Round(value / 1_000_000_000f, 1)}B";
                case >= 1_000_000f: return $"{Math.Round(value / 1_000_000f, 1)}M";
                case >= 1_000f: return $"{Math.Round(value / 1_000f, 1)}K";
                default: return Math.Round(value, 2).ToString();
            }
        }
    }

    class GameButton : ScriptBehaviour
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
        public float Cost { get; set; }
        public float CostIncrease { get; set; }
        public string Name { get; set; }
        public float Amount { get; set; } = 0f;
        public float AmountIncrease { get; set; }

        private Text2D _text1;
        private Text2D _text2;

        public Upgrade(float yPos, Color color, float cost, string name, float costIncrease = 2f, float amount = 0, float amountIncrease = 1f) : base(new Button2D(new Shape2D(new(15f, yPos), new(150f, 50f), color, default, "Upgrade", 1000)))
        {
            Cost = cost;
            CostIncrease = costIncrease;
            Name = name;
            Amount = amount;
            AmountIncrease = amountIncrease;

            _text1 = GetText1();
            _text2 = GetText2();
        }

        public override void Update()
        {
            if (IsClicked())
            {
                if (Buy(out float cost)) { Score.value -= cost; }
            }
        }

        public bool Buy(out float cost)
        {
            cost = Cost;
            if (Score.value >= Cost)
            {
                Amount += AmountIncrease;
                Cost = (float)Math.Round(Cost * CostIncrease);
                _text2.Kill();
                _text2 = GetText2();
                return true;
            }
            return false;
        }

        public Text2D GetText1()
        {
            return new(Button.Rendered.Center().Subtract(new(60f, 17f)), new(200f, 50f), Name, Color.Black, default, new("Arial", 10f, FontStyle.Bold), "UpgradeName", 1000);
        }

        public Text2D GetText2()
        {
            return new(Button.Rendered.Center().Subtract(new(60f, 0f)), new(200f, 50f), $"Cost: {Cost}     x{Math.Round(Amount, 1)}", Color.Black, default, new("Arial", 10f, FontStyle.Bold), "UpgradeInfo", 1000);
        }
    }

    internal class OldDemo_Clicker : Kudo
    {
        public OldDemo_Clicker() : base(new Vector2(512, 512), "Cookie Clicker") { }

        // CODE START //

        // cookie
        GameButton cookie;
        // Score.value
        Text2D scoreText;

        // UPGRADES
        Upgrade bakingGloves;
        Upgrade factory;
        Upgrade reputation;
        Upgrade engine;

        // counter members
        float engineTime = 0f;
        float factoryTime = 0f;

        public override void Load()
        {
            Skybox = Color.Black;

            // cookie
            cookie = new GameButton(new Button2D(new Sprite2D(new(-70f, -175f), new(300f), BitmapFromFile("Sprites/cookie"))));

            // upgrades
            bakingGloves = new Upgrade(100f, Color.PowderBlue, 5, "Baking Gloves", 1.2f, 1f);
            engine = new Upgrade(175f, Color.IndianRed, 50, "Engine", 1.3f, 0f);
            factory = new Upgrade(250f, Color.Gray, 100, "Factory", 1.25f, 0f);
            reputation = new Upgrade(325f, Color.White, 100, "Reputation", 1.3f, 0f, amountIncrease: 0.1f);

            // score
            scoreText = new Text2D(new(10f), new(200f), $"{Score.value}", Color.Red, default, new("Arial", 20f, FontStyle.Bold), "scoreText", 1000);

            // debug
            //bakingGloves.Amount = 1000f;
            //engine.Amount = 500f;
            Score.value = 100f;
        }

        public override void Update()
        {
            // spin cookie
            cookie.Button.Rendered.Rotation += 50f * Time.DeltaTimeSeconds;

            // hover effect
            if (cookie.Button.IsHovered())
            {
                cookie.TransitionButton(300f, 4f);
            }
            else
            {
                cookie.TransitionButton(270f, 4f);
            }

            // click effect
            if (cookie.Button.IsPressed())
            {
                cookie.TransitionButton(260f, 10f);

                // check engine
                engineTime += Time.DeltaTime;
                while (engineTime >= 1000f / engine.Amount)
                {
                    engineTime -= 1000f / engine.Amount;
                    Score.value += bakingGloves.Amount + bakingGloves.Amount * reputation.Amount;
                }
            } else
            {
                engineTime = 0f;
            }

            // click function
            if (cookie.IsClicked())
            {
                Score.value += bakingGloves.Amount;
                Score.value += bakingGloves.Amount * reputation.Amount;
            }

            // check factory
            factoryTime += Time.DeltaTime;
            while (factoryTime >= 1000f / factory.Amount)
            {
                factoryTime -= 1000f / factory.Amount;
                Score.value += 1 + 1 * reputation.Amount;
            }

            // update score text
            scoreText.Text = Score.String();
        }

    }
}