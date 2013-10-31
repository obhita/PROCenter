namespace ProCenter.Domain.MessageModule.Event
{
    #region Using Statements

    using System;
    using Common;
    using CommonModule;

    #endregion

    public abstract class MessageEventBase : ICommitEvent
    {
        protected MessageEventBase(Guid key, MessageType messageType)
        {
            Key = key;
            MessageType = messageType;
            OrganizationKey = UserContext.Current.OrganizationKey;
        }

        /// <summary>
        /// Gets or sets the type of the message.
        /// </summary>
        /// <value>
        /// The type of the message.
        /// </value>
        public MessageType MessageType { get; set; }

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        public Guid Key { get; private set; }

        /// <summary>
        /// Gets the organization key.
        /// </summary>
        /// <value>
        /// The organization key.
        /// </value>
        public Guid? OrganizationKey { get; private set; }
    }
}