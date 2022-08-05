namespace KudoEngine
{
    /// <summary>
    /// <see langword="Kudo"/>
    /// Simple implementation of a button
    /// </summary>
    public class Button2D
    {
        public RenderedObject2D Rendered { get; private set; }

        public Button2D (RenderedObject2D baseObject)
        {
            Rendered = baseObject;
        }

        public bool IsHovered()
        {
            return Input.MouseOver(Rendered);
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
