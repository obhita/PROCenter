namespace ProCenter.Service.Message.Common
{
    /// <summary>
    ///     Interface for generic Data transfer object response
    /// </summary>
    public interface IDtoResponse
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Gets the dto.
        /// </summary>
        /// <returns>
        ///     The dto, that was requested.
        /// </returns>
        KeyedDataTransferObject GetDto ();

        #endregion
    }
}