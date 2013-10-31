namespace ProCenter.Domain.AssessmentModule.Event
{
    #region Using Statements

    using System;
    using CommonModule;

    #endregion

    /// <summary>
    ///     Event when an item definition is added.
    /// </summary>
    public class ItemDefinitionAddedEvent : CommitEventBase
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemDefinitionAddedEvent" /> class.
        /// </summary>
        /// <param name="assessmentDefinitionKey">The assessment definition key.</param>
        /// <param name="version">The version.</param>
        /// <param name="itemDefinition">The item definition.</param>
        public ItemDefinitionAddedEvent(Guid assessmentDefinitionKey, int version, ItemDefinition itemDefinition)
            : base ( assessmentDefinitionKey, version )
        {
            ItemDefinition = itemDefinition;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the item definition.
        /// </summary>
        /// <value>
        ///     The item definition.
        /// </value>
        public ItemDefinition ItemDefinition { get; private set; }

        #endregion
    }
}