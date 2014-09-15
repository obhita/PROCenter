namespace ProCenter.Service.Message.Security
{
    /// <summary>Response codes for resetting password.</summary>
    public enum ResetPasswordResponseCode
    {
        /// <summary>
        ///     The error.
        /// </summary>
        Error,

        /// <summary>
        ///     The unknown account.
        /// </summary>
        UnknownAccount,

        /// <summary>
        ///     The success.
        /// </summary>
        Success
    }
}