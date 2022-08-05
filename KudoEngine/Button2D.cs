namespace KudoEngine
{
    public class Button2D
    {
        public RenderedObject2D BaseObject { get; private set; }
        public Text2D Text { get; private set; }

        public Button2D (RenderedObject2D baseObject, Text2D? text = null)
        {
            BaseObject = baseObject;
            Text = text ?? new(BaseObject.Position, BaseObject.Scale, "", Color.Black);
        }

        public bool IsHovered()
        {
            return Input.MouseOver(BaseObject);
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
