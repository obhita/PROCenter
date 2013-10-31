namespace ProCenter.Service.Message.Common
{
    /// <summary>
    ///     Type of the data error.
    /// </summary>
    public enum DataErrorInfoType
    {
        /// <summary>
        ///     Object level error.
        /// </summary>
        ObjectLevel,

        /// <summary>
        ///     Property level error.
        /// </summary>
        PropertyLevel,

        /// <summary>
        ///     Multiple property level error.
        /// </summary>
        CrossPropertyLevel
    }
}