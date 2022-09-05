namespace KudoEngine
{
    /// <summary>
    /// <see langword="Kudo"/>
    /// Simple implementation of a button
    /// </summary>
    public class Button2D
    {
        public RenderedObject2D Rendered { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public Button2D (RenderedObject2D baseObject)
        {
            Rendered = baseObject;
        }

        /// <summary>
        /// Check if the button is hovered by the cursor
        /// </summary>
        public bool IsHovered()
        {
            return Input.MouseOver(Rendered);
        }

        /// <summary>
        /// Check if the button is pressed
        /// </summary>
        public bool IsPressed()
        {
            return IsHovered() && Input.IsMouseDown();
        }

        /// <summary>
        /// Check if the button is pressed by a certain button
        /// </summary>
        public bool IsPressed(MouseButtons button)
        {
            return IsHovered() && Input.IsMouseDown(button);
        }
    }
}
