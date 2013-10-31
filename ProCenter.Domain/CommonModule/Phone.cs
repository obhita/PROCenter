namespace ProCenter.Domain.CommonModule
{
    #region Using Statements

    using System;
    using Pillar.Common.Utility;

    #endregion

    /// <summary>
    ///     Phone value object.
    /// </summary>
    public class Phone : IValueObject, IEquatable<Phone>
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Phone" /> class.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <param name="extension">The extension.</param>
        public Phone ( string number, string extension = null )
        {
            Check.IsNotNullOrWhitespace ( number, () => Number );

            Number = number;
            Extension = string.IsNullOrWhiteSpace ( extension ) ? null : extension;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the extension.
        /// </summary>
        /// <value>
        ///     The extension.
        /// </value>
        public string Extension { get; private set; }

        /// <summary>
        ///     Gets the number.
        /// </summary>
        /// <value>
        ///     The number.
        /// </value>
        public string Number { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     ==s the specified left.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns></returns>
        public static bool operator == ( Phone left, Phone right )
        {
            return Equals ( left, right );
        }

        /// <summary>
        ///     !=s the specified left.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns></returns>
        public static bool operator != ( Phone left, Phone right )
        {
            return !Equals ( left, right );
        }

        /// <summary>
        ///     Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public bool Equals ( Phone other )
        {
            if ( ReferenceEquals ( null, other ) )
                return false;
            if ( ReferenceEquals ( this, other ) )
                return true;
            return string.Equals ( Number, other.Number ) && string.Equals ( Extension, other.Extension );
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
            return Equals ( (Phone) obj );
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
                return ( ( Number != null ? Number.GetHashCode () : 0 ) * 397 ) ^ ( Extension != null ? Extension.GetHashCode () : 0 );
            }
        }

        #endregion
    }
}