namespace ProCenter.Domain.AssessmentModule
{
    #region Using Statements

    using System;
    using Event;

    #endregion

    public class ItemInstance
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ItemInstance" /> class.
        /// </summary>
        /// <param name="itemDefinitionCode">The item definition code.</param>
        /// <param name="value">The value.</param>
        public ItemInstance ( string itemDefinitionCode, object value )
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

        #region Public Methods and Operators

        /// <summary>
        ///     Applies the specified item updated event.
        /// </summary>
        /// <param name="itemUpdatedEvent">The item updated event.</param>
        /// <exception cref="System.InvalidOperationException">Cannot update an item based on a different Item Definition.</exception>
        public void Apply ( ItemUpdatedEvent itemUpdatedEvent )
        {
            if ( itemUpdatedEvent.ItemDefinitionCode != ItemDefinitionCode )
            {
                throw new InvalidOperationException ( "Cannot update an item based on a different Item Definition." );
            }

            Value = itemUpdatedEvent.Value;
        }

        #endregion
    }
}