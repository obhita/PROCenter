namespace ProCenter.Domain.AssessmentModule.Event
{
    #region Using Statements

    using System;
    using CommonModule;

    #endregion

    /// <summary>
    ///     Item scored event.
    /// </summary>
    public class ItemScoredEvent : CommitEventBase
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ItemScoredEvent" /> class.
        /// </summary>
        /// <param name="scoreKey">The score key.</param>
        /// <param name="version">The version.</param>
        /// <param name="itemDefinitionCode">The item definition code.</param>
        /// <param name="value">The value.</param>
        public ItemScoredEvent ( Guid scoreKey, int version, string itemDefinitionCode, object value )
            : base ( scoreKey, version )
        {
            ItemDefinitionCode = itemDefinitionCode;
            Value = value;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the item definition code.
        /// </summary>
        /// <value>
        ///     The item definition code.
        /// </value>
        public string ItemDefinitionCode { get; private set; }

        /// <summary>
        ///     Gets the value.
        /// </summary>
        /// <value>
        ///     The value.
        /// </value>
        public object Value { get; private set; }

        #endregion
    }
}