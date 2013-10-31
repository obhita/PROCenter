namespace ProCenter.Infrastructure.EventStore
{
    #region Using Statements

    using Pillar.Common.InversionOfControl;

    #endregion

    /// <summary>
    ///     Class that provides the current unit of work.
    /// </summary>
    public class UnitOfWorkProvider : IUnitOfWorkProvider
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Gets the current unit of work.
        /// </summary>
        /// <returns>
        ///     A <see cref="IUnitOfWork" />.
        /// </returns>
        public IUnitOfWork GetCurrentUnitOfWork ()
        {
            return IoC.CurrentContainer.Resolve<IUnitOfWork> ();
        }

        #endregion
    }
}