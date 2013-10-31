namespace ProCenter.Domain.AssessmentModule
{
    #region Using Statements

    using System.Collections.Generic;

    #endregion

    /// <summary>
    ///     Interface to classes that contain item definitions.
    /// </summary>
    public interface IContainItemDefinitions
    {
        #region Public Properties

        /// <summary>
        ///     Gets the item definitions.
        /// </summary>
        /// <value>
        ///     The item definitions.
        /// </value>
        IEnumerable<ItemDefinition> ItemDefinitions { get; }

        #endregion
    }
}