namespace KudoEngine
{
    /// <summary>
    /// <see langword="Kudo"/>
    /// A dedicated dictionary with objects
    /// </summary>
    public sealed class ExpandableObject
    {
        public Dictionary<string, object> DynamicProperties { get; private set; } = new Dictionary<string, object>();

        /// <summary>
        /// Add/Overwrite a value of this instance
        /// </summary>
        public bool Set(string key, object value)
        {
            DynamicProperties.Add(key, value);

            return true;
        }

        /// <summary>
        /// Read a value of this instance
        /// </summary>
        public dynamic Get(string key)
        {
            return DynamicProperties[key];
        }
    }
}
