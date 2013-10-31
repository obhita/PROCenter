namespace ProCenter.Service.Message.Common
{
    #region Using Statements

    using Agatha.Common;

    #endregion

    /// <summary>
    ///     Generic Data transfer object response
    /// </summary>
    /// <typeparam name="TDto">The type of the dto.</typeparam>
    public class DtoResponse<TDto> : Response, IDtoResponse
        where TDto : KeyedDataTransferObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the data transfer object.
        /// </summary>
        /// <value> The dto that has to be sent as response. </value>
        public TDto DataTransferObject { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Gets the dto.
        /// </summary>
        /// <returns>
        ///     The dto that has to be sent as response.
        /// </returns>
        public KeyedDataTransferObject GetDto ()
        {
            return DataTransferObject;
        }

        #endregion
    }
}