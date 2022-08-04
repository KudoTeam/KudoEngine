namespace KudoEngine
{
    public class Button2D : RenderedObject2D
    {
        public RenderedObject2D BaseObject { get; private set; }
        public Text2D Text { get; private set; }

        public Button2D (RenderedObject2D baseObject, Text2D? text = null)
        {
            BaseObject = baseObject;
            Text = text ?? new(BaseObject.Position, BaseObject.Scale, "", Color.Black);

            Position = BaseObject.Position;
            Scale = BaseObject.Scale;
            Tag = "Button2D";
        }

        public bool IsHovered()
        {
            return Input.MouseOver(this);
        }

        public bool IsPressed()
        {
            return IsHovered() && Input.IsMouseDown();
        }

        public bool IsPressed(MouseButtons button)
        {
            return IsHovered() && Input.IsMouseDown(button);
        }
    }
}
