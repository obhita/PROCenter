namespace ProCenter.Domain.MessageModule.Event
{
    #region Using Statements

    using System;

    #endregion

    /// <summary>The message not for self administration event class.</summary>
    public class MessageNotForSelfAdministrationEvent : MessageEventBase
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageNotForSelfAdministrationEvent"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="messageType">Type of the message.</param>
        public MessageNotForSelfAdministrationEvent ( Guid key, MessageType messageType )
            : base ( key, messageType )
        {
        }

        #endregion
    }
}