namespace ProCenter.Service.Message.Assessment
{
    #region Using Statements

    using System.Collections.Generic;

    #endregion

    /// <summary>
    ///     Interfance for classes that contain item dtos.
    /// </summary>
    public interface IContainItems
    {
        #region Public Properties

        /// <summary>
        ///     Gets the items.
        /// </summary>
        /// <value>
        ///     The items.
        /// </value>
        IList<ItemDto> Items { get; }

        #endregion
    }
}