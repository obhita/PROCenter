namespace ProCenter.Domain.AssessmentModule.Event
{
    #region Using Statements

    using System;
    using CommonModule;

    #endregion

    /// <summary>
    ///     Event when an item is updated.
    /// </summary>
    public class ItemUpdatedEvent : CommitEventBase
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemUpdatedEvent" /> class.
        /// </summary>
        /// <param name="assessmentInstanceKey">The assessment instance key.</param>
        /// <param name="version">The version.</param>
        /// <param name="itemDefinitionCode">The item definition code.</param>
        /// <param name="value">The value.</param>
        public ItemUpdatedEvent(Guid assessmentInstanceKey, int version, string itemDefinitionCode, object value)
            : base ( assessmentInstanceKey, version )
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