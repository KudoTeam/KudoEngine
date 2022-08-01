namespace KudoEngine
{
    public sealed class ExpandableObject
    {
        private readonly Dictionary<string, object> _dynamicProperties = new Dictionary<string, object>();

        /// <summary>
        /// Add/Overwrite a value of this instance
        /// </summary>
        public bool Set(string key, object value)
        {
            _dynamicProperties.Add(key, value);

            return true;
        }

        /// <summary>
        /// Read a value of this instance
        /// </summary>
        public dynamic Get(string key)
        {
            return _dynamicProperties[key];
        }
    }
}
