namespace ProCenter.Infrastructure
{
    #region Using Statements

    using System.Text;

    using NLog;
    using NLog.LayoutRenderers;

    using ProCenter.Common;

    #endregion

    /// <summary>NLog Renderer for User Context.</summary>
    [LayoutRenderer ( "user-context" )]
    public class UserContextLayoutRenderer : LayoutRenderer
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UserContextLayoutRenderer"/> class.
        /// </summary>
        public UserContextLayoutRenderer ()
        {
            Separator = ":";
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the separator.
        /// </summary>
        /// <value>
        /// The separator.
        /// </value>
        public string Separator { get; set; }

        #endregion

        #region Methods

        /// <summary>Renders the specified environmental information and appends it to the specified <see cref="T:System.Text.StringBuilder" />.</summary>
        /// <param name="builder">The <see cref="T:System.Text.StringBuilder" /> to append the rendered data to.</param>
        /// <param name="logEvent">Logging event.</param>
        protected override void Append ( StringBuilder builder, LogEventInfo logEvent )
        {
            builder.Append (
                            string.Format (
                                           "User: {0}",
                                string.Join (
                                             Separator,
                                    UserContext.Current.SystemAccountKey,
                                    UserContext.Current.DisplayName,
                                    UserContext.Current.StaffKey ?? UserContext.Current.PatientKey ) ) );
        }

        #endregion
    }
}