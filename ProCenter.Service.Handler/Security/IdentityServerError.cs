namespace ProCenter.Service.Handler.Security
{
    /// <summary>
    /// The IdentityServerError error class.
    /// </summary>
    public class IdentityServerError
    {
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the error sub code.
        /// </summary>
        /// <value>
        /// The error sub code.
        /// </value>
        public int ErrorSubCode { get; set; }
    }
}
