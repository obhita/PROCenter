namespace ProCenter.Service.Message.Common
{
    #region Using Statements

    using System.Collections.Generic;

    #endregion

    /// <summary>
    ///     The <see cref="IValidatedObject" /> interface that provides data error information management.
    /// </summary>
    public interface IValidatedObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets the data error information collection.
        /// </summary>
        IEnumerable<DataErrorInfo> DataErrorInfoCollection { get; }

        #endregion

        #region Public Methods

        /// <summary>
        ///     Adds the data error information.
        /// </summary>
        /// <param name="dataErrorInfo">
        ///     The data error information.
        /// </param>
        void AddDataErrorInfo(DataErrorInfo dataErrorInfo);

        /// <summary>
        ///     Clears all data error information.
        /// </summary>
        void ClearAllDataErrorInfo();

        /// <summary>
        ///     Removes the data error information.
        /// </summary>
        /// <param name="propertyName">
        ///     Name of the property which has erroneous data.
        /// </param>
        void RemoveDataErrorInfo(string propertyName);

        #endregion
    }
}