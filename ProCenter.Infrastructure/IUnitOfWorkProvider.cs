namespace ProCenter.Infrastructure
{
    /// <summary>
    ///     Interface for providing unit of work.
    /// </summary>
    public interface IUnitOfWorkProvider
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Gets the current unit of work.
        /// </summary>
        /// <returns>
        ///     A <see cref="IUnitOfWork" />.
        /// </returns>
        IUnitOfWork GetCurrentUnitOfWork ();

        #endregion
    }
}