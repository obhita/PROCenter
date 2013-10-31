namespace ProCenter.Infrastructure.Service.ReadSideService
{
    #region Using Statements

    using ProCenter.Domain.CommonModule;

    #endregion

    public interface IHandleMessages<in T> : IHandleMessage
        where T : ICommitEvent
    {
        /// <summary>
        ///     Handles the message provided.
        /// </summary>
        /// <param name="message">The message to be handled.</param>
        void Handle(T message);
    }

    public interface IHandleMessage
    {
    }
}