// /*******************************************************************************
//  * Open Behavioral Health Information Technology Architecture (OBHITA.org)
//  * 
//  * Redistribution and use in source and binary forms, with or without
//  * modification, are permitted provided that the following conditions are met:
//  *     * Redistributions of source code must retain the above copyright
//  *       notice, this list of conditions and the following disclaimer.
//  *     * Redistributions in binary form must reproduce the above copyright
//  *       notice, this list of conditions and the following disclaimer in the
//  *       documentation and/or other materials provided with the distribution.
//  *     * Neither the name of the <organization> nor the
//  *       names of its contributors may be used to endorse or promote products
//  *       derived from this software without specific prior written permission.
//  * 
//  * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
//  * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
//  * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
//  * DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
//  * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
//  * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
//  * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
//  * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
//  * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
//  * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//  ******************************************************************************/

namespace ProCenter.Mvc.Infrastructure.BrowserDetection
{
    #region Using Statements

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Xml.Linq;

    #endregion

    /// <summary>
    ///     Use this class when you want to limit the browsers that are used for application.
    /// </summary>
    public class SupportedBrowser : ISupportedBrowser
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SupportedBrowser" /> class.
        /// </summary>
        public SupportedBrowser()
        {
            if (HttpContextFactory.Current.Request.Browser == null)
            {
                return;
            }

            BrowserName = GetBrowserName();
            if ( !SetMachineType () )
            {
                return;
            }
            if (!SetVersion())
            {
                return;
            }
            IsValid = SetSupportStatus ();
        }

        #endregion

        #region Enums

        /// <summary>
        ///     Enum for support status.
        /// </summary>
        public enum SupportStatusEnum
        {
            /// <summary>
            ///     The unknown.
            /// </summary>
            Unknown = 0,

            /// <summary>
            ///     The supported.
            /// </summary>
            Supported = 1,

            /// <summary>
            ///     The warning.
            /// </summary>
            Warning = 2,

            /// <summary>
            ///     The blocked.
            /// </summary>
            Blocked = 3
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the name of the browser.
        /// </summary>
        /// <value>
        ///     The name of the browser.
        /// </value>
        [DefaultValue ( "" )]
        public string BrowserName { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is valid.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is valid; otherwise, <c>false</c>.
        /// </value>
        [DefaultValue ( false )]
        public bool IsValid { get; set; }

        /// <summary>
        ///     Gets or sets the type of the machine.
        /// </summary>
        /// <value>
        ///     The type of the machine.
        /// </value>
        [DefaultValue ( "" )]
        public string MachineType { get; set; }

        /// <summary>
        /// Gets or sets the support status.
        /// </summary>
        /// <value>
        /// The support status.
        /// </value>
        public SupportStatusEnum SupportStatus { get; set; }

        /// <summary>
        ///     Gets or sets the version.
        /// </summary>
        /// <value>
        ///     The version.
        /// </value>
        [DefaultValue ( 0 )]
        public double Version { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <param name="machineType">Type of the machine.</param>
        /// <returns>
        /// A list of formatted strings.
        /// </returns>
        /// <exception cref="System.Exception">Error in SupportedBrowsers.xml file.
        /// or
        /// Error in SupportedBrowsers.xml file.</exception>
        public List<string> GetList ( string machineType )
        {
            const string Li = "{0} Minimum Version Supported {1} <a class='btn btn-info' href='{2}' target='_blank'>Get {3}</a>";
            //// {0}: browserName
            //// {1}: minVersion
            //// {2}: link
            //// {3}: Broswer Name
            var list = new List<string> ();

            var supported = LoadXmlData().Descendants("supported").ToList();

            foreach ( var el in supported.Descendants () )
            {
                var elname = el.Name;
                var minVersionAtt = el.Attribute ( "MinVersion" );
                var maxVersionAtt = el.Attribute ( "MaxVersion" );
                var machineTypeAtt = el.Attribute ( "MachineType" );
                var displayNameAtt = el.Attribute ( "displayName" );
                var linkAtt = el.Attribute ( "link" );

                if ( minVersionAtt == null || maxVersionAtt == null || machineTypeAtt == null || displayNameAtt == null || linkAtt == null )
                {
                    throw new Exception ( "Error in SupportedBrowsers.xml file." );
                }

                try
                {
                    var version = minVersionAtt.Value;
                    if ( machineTypeAtt.Value.ToUpper ().Contains ( machineType.ToUpper () ) )
                    {
                        list.Add (
                            string.Format (
                                Li,
                                displayNameAtt.Value,
                                version,
                                linkAtt.Value,
                                elname
                                ) );
                    }
                }
                catch ( Exception )
                {
                    throw new Exception ( "Error in SupportedBrowsers.xml file." );
                }
            }

            return list;
        }

        /// <summary>
        /// Retrieve the message to display to the users based on the browser settings.
        /// </summary>
        /// <returns>
        /// A formated string to display to user.
        /// </returns>
        public string GetMessage ()
        {
            var msg = string.Empty;
            switch ( SupportStatus )
            {
                case SupportStatusEnum.Warning:
                    msg = string.Format (
                        "Your web browser, {0} v{1} for {2}, is not fully supported.<br/>" +
                        "Please consider downloading one of the supported browsers listed for full functionality.",
                        BrowserName,
                        Version,
                        MachineType );
                    break;
                case SupportStatusEnum.Blocked:
                    msg = string.Format (
                        "Your web browser, {0} v{1} for {2}, is not supported.<br/>Please download one of the supported browsers listed.",
                        BrowserName,
                        Version,
                        MachineType );
                    break;
            }
            return msg;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the name of the browser.
        /// </summary>
        /// <returns>
        /// The browser name string.
        /// </returns>
        private string GetBrowserName( )
        {
            if (HttpContextFactory.Current.Request.UserAgent != null && HttpContextFactory.Current.Request.UserAgent.Contains("Trident"))
            {
                return "IE";
            }
            return HttpContextFactory.Current.Request.Browser.Browser;
        }

        /// <summary>
        /// Parse the machine type out of the UserAgent string.
        /// </summary>
        /// <returns>
        /// A boolean to know if a machine type is present.
        /// </returns>
        private bool SetMachineType ()
        {
            var platform = HttpContextFactory.Current.Request.UserAgent;
            if ( platform == null )
            {
                return false;
            }
            if ( platform.ToUpper ().Contains ( "WINDOWS" ) )
            {
                MachineType = "Windows";
                return true;
            }
            if ( platform.ToUpper ().Contains ( "MACINTOSH" ) )
            {
                MachineType = "Mac";
                return true;
            }
            return false;
        }

        /// <summary>
        /// Determine the level of support we have for this browser.
        /// </summary>
        /// <returns>
        /// Returns true if browser and version is supported otherwise false.
        /// </returns>
        private bool SetSupportStatus ()
        {
            var doc = LoadXmlData();

            var supported = doc.Descendants ( "supported" ).Descendants ( BrowserName ).ToList ();

            if ( IsSupportedVersionInData ( supported ) )
            {
                SupportStatus = SupportStatusEnum.Supported;
            }
            else
            {
                SupportStatus = SupportStatusEnum.Blocked;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Get the browser version.
        /// For IE we have temporarily hardcoded the version based on the Trident version in the UserAgent.
        /// </summary>
        /// <returns>
        /// True if the version is supported otherwise false.
        /// </returns>
        private bool SetVersion()
        {
            double version;

            if ( BrowserName == "IE" )
            {
                var platform = HttpContextFactory.Current.Request.UserAgent;
                if ( platform != null && platform.Contains ( "Trident/5.0" ) )
                {
                    Version = 9.0;
                }
                else if ( platform != null && platform.Contains ( "Trident/4.0" ) )
                {
                    Version = 8.0;
                }
                else if ( platform != null && platform.Contains ( "Trident" ) )
                {
                    // get from the UserAgent string it looks like this
                    // Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko
                    var startPos = platform.IndexOf("rv:", StringComparison.OrdinalIgnoreCase) + 3;
                    var endPos = platform.IndexOf(")", StringComparison.OrdinalIgnoreCase);
                    var versionString = platform.Substring (
                        startPos,
                        endPos - startPos );
                    double.TryParse ( versionString, out version );
                    Version = version;
                }
                else if (double.TryParse(HttpContextFactory.Current.Request.Browser.Version, out version))
                {
                    Version = version;
                }
                if (Version <= 0)
                {
                    // try getting the version by major version
                    Version = HttpContextFactory.Current.Request.Browser.MajorVersion;
                }
            }
            else if (double.TryParse(HttpContextFactory.Current.Request.Browser.Version, out version))
            {
                Version = version;
            }
            else
            {
                return false;
            }
            return true;
        }

        /// <summary>
        ///     Parses the actual XML Elements from the file and determines if it is supported.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>True if the browser information is supported based on the data in the XML file otherwise false.</returns>
        /// <exception cref="System.Exception">
        ///     Error in SupportedBrowsers.xml file.
        /// </exception>
        private bool IsSupportedVersionInData ( IEnumerable<XElement> data )
        {
            foreach ( var el in data )
            {
                var minVersionAtt = el.Attribute ( "MinVersion" );
                var maxVersionAtt = el.Attribute ( "MaxVersion" );
                var machineTypeAtt = el.Attribute ( "MachineType" );

                if ( minVersionAtt == null || maxVersionAtt == null || machineTypeAtt == null )
                {
                    throw new Exception ( "Error in SupportedBrowsers.xml file." );
                }

                try
                {
                    if ( Version >= double.Parse ( minVersionAtt.Value )
                         && Version <= double.Parse ( maxVersionAtt.Value )
                         && machineTypeAtt.Value.ToUpper ().Contains ( MachineType.ToUpper () ) )
                    {
                        return true;
                    }
                }
                catch ( Exception )
                {
                    throw new Exception ( "Error in SupportedBrowsers.xml file." );
                }
            }
            return false;
        }

        /// <summary>
        /// Loads the XML data.
        /// </summary>
        /// <returns>An XDocument object if the file or string data loads the XML.</returns>
        private XDocument LoadXmlData ()
        {
            var xmlPathOrData = XmlsDataFactory.XmlData;
            if (XmlsDataFactory.IsXmlDataSet)
            {
                return XDocument.Parse(xmlPathOrData);
            }
            return XDocument.Load(xmlPathOrData);
        }

        #endregion
    }
}