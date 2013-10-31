namespace ProCenter.Service.Message.Common
{
    /// <summary>
    ///     The <see cref="ErrorLevel" /> has the possible criticality levels of an error.
    /// </summary>
    public enum ErrorLevel
    {
        /// <summary>
        ///     Error indicate that the system cannot proceed without resolving.
        /// </summary>
        Error,

        /// <summary>
        ///     Warning indicates that it is better to resolve but the system can still proceed.
        /// </summary>
        Warning
    }
}