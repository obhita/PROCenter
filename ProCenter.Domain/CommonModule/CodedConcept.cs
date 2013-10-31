namespace ProCenter.Domain.CommonModule
{
    #region Using Statements

    using System;

    #endregion

    /// <summary>
    ///     Class for holding standardization information.
    /// </summary>
    public class CodedConcept : IValueObject, IEquatable<CodedConcept>
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="CodedConcept" /> class.
        /// </summary>
        /// <param name="codeSystem">The code system.</param>
        /// <param name="code">The code.</param>
        /// <param name="name">The name.</param>
        public CodedConcept ( CodeSystem codeSystem, string code, string name )
        {
            CodeSystem = codeSystem;
            Code = code;
            Name = name;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the code.
        /// </summary>
        /// <value>
        ///     The code.
        /// </value>
        public string Code { get; protected set; }

        /// <summary>
        ///     Gets or sets the code system.
        /// </summary>
        /// <value>
        ///     The code system.
        /// </value>
        public CodeSystem CodeSystem { get; protected set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public string Name { get; protected set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     ==s the specified left.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns></returns>
        public static bool operator == ( CodedConcept left, CodedConcept right )
        {
            return Equals ( left, right );
        }

        /// <summary>
        ///     !=s the specified left.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns></returns>
        public static bool operator != ( CodedConcept left, CodedConcept right )
        {
            return !Equals ( left, right );
        }

        /// <summary>
        ///     Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public bool Equals ( CodedConcept other )
        {
            if ( ReferenceEquals ( null, other ) )
                return false;
            if ( ReferenceEquals ( this, other ) )
                return true;
            return Equals ( CodeSystem, other.CodeSystem ) && string.Equals ( Code, other.Code );
        }

        /// <summary>
        ///     Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">
        ///     The <see cref="System.Object" /> to compare with this instance.
        /// </param>
        /// <returns>
        ///     <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals ( object obj )
        {
            if ( ReferenceEquals ( null, obj ) )
                return false;
            if ( ReferenceEquals ( this, obj ) )
                return true;
            if ( obj.GetType () != this.GetType () )
                return false;
            return Equals ( (CodedConcept) obj );
        }

        /// <summary>
        ///     Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        ///     A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode ()
        {
            unchecked
            {
                return ( ( CodeSystem != null ? CodeSystem.GetHashCode () : 0 ) * 397 ) ^ ( Code != null ? Code.GetHashCode () : 0 );
            }
        }

        #endregion
    }
}