#region Using Statements

#endregion

namespace ProCenter.Service.Message.Common
{
    #region Using Statements

    using System;
    using System.Linq;
    using System.Runtime.Serialization;

    #endregion

    /// <summary>
    ///     The <see cref="DataErrorInfo" /> class stores information about a rule violation.
    /// </summary>
    [DataContract]
    public class DataErrorInfo
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DataErrorInfo" /> class.
        /// </summary>
        /// <param name="message">
        ///     The message.
        /// </param>
        /// <param name="errorLevel">
        ///     The error level.
        /// </param>
        public DataErrorInfo(string message, ErrorLevel errorLevel)
            :
                this(message, errorLevel, null)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DataErrorInfo" /> class.
        /// </summary>
        /// <param name="message">
        ///     The message.
        /// </param>
        /// <param name="errorLevel">
        ///     The error level.
        /// </param>
        /// <param name="propertyNames">
        ///     The property names.
        /// </param>
        public DataErrorInfo(
            string message,
            ErrorLevel errorLevel,
            params string[] propertyNames)
        {
            if (propertyNames != null)
            {
                var emptyPropertyNames = propertyNames.Where(string.IsNullOrEmpty).ToList();
                if (emptyPropertyNames.Any())
                {
                    throw new ArgumentException("Null or empty property names are not allowed.");
                }
            }

            Message = message;
            ErrorLevel = errorLevel;
            Properties = propertyNames;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the type of error.
        /// </summary>
        /// <value> The type of the error. </value>
        public DataErrorInfoType DataErrorInfoType
        {
            get
            {
                var type = DataErrorInfoType.ObjectLevel;

                if (Properties != null)
                {
                    if (Properties.Length == 1)
                    {
                        type = DataErrorInfoType.PropertyLevel;
                    }
                    else if (Properties.Length > 1)
                    {
                        type = DataErrorInfoType.CrossPropertyLevel;
                    }
                }

                return type;
            }
        }

        /// <summary>
        ///     Gets the error level.
        /// </summary>
        [DataMember]
        public ErrorLevel ErrorLevel { get; internal set; }

        /// <summary>
        ///     Gets the error message.
        /// </summary>
        [DataMember]
        public string Message { get; set; }

        /// <summary>
        ///     Gets the property names.
        /// </summary>
        [DataMember]
        public string[] Properties { get; internal set; }

        #endregion

        #region Public Methods

        /// <summary>
        ///     Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        ///     A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Message;
        }

        #endregion
    }
}