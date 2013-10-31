namespace ProCenter.Domain.CommonModule.Lookups
{
    #region Using Statements

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Resources;

    #endregion

    /// <summary>
    ///     Base Class for lookups.
    /// </summary>
    public class Lookup : IEquatable<Lookup>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Lookup" /> class.
        /// </summary>
        public Lookup() {}

        /// <summary>
        /// Initializes a new instance of the <see cref="Lookup" /> class.
        /// </summary>
        /// <param name="codedConcept">The coded concept.</param>
        public Lookup (CodedConcept codedConcept) : this(codedConcept, 0.0)
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Lookup" /> class.
        /// </summary>
        /// <param name="codedConcept">The coded concept.</param>
        /// <param name="value">The value.</param>
        /// <param name="sortOrder">The sort order.</param>
        /// <param name="isDefault">if set to <c>true</c> [is default].</param>
        public Lookup(CodedConcept codedConcept, double value, int sortOrder = 0, bool isDefault = false)
        {
            CodedConcept = codedConcept;
            IsDefault = isDefault;
            SortOrder = sortOrder;
            Value = value;
        }

        #region Public Properties

        /// <summary>
        ///     Gets or sets the coded concept.
        /// </summary>
        /// <value>
        ///     The coded concept.
        /// </value>
        public CodedConcept CodedConcept { get; protected set; }

        /// <summary>
        ///     Gets or sets the display name.
        /// </summary>
        /// <value>
        ///     The display name.
        /// </value>
        public string DisplayName
        {
            get
            {
                var type = GetType ();
                if ( type == typeof(Lookup) || CodedConcept == null || CodedConcept.Code == null)
                {
                    return string.Empty;
                }
                var resourceManger = new ResourceManager(type);
                return resourceManger.GetString(CodedConcept.Code) ?? string.Empty;
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is default.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is default; otherwise, <c>false</c>.
        /// </value>
        public bool IsDefault { get; protected set; }

        /// <summary>
        ///     Gets or sets the sort order.
        /// </summary>
        /// <value>
        ///     The sort order.
        /// </value>
        public int SortOrder { get; protected set; }

        /// <summary>
        ///     Gets or sets the value.
        /// </summary>
        /// <value>
        ///     The value.
        /// </value>
        public double Value { get; protected set; }

        #endregion

        #region Public Methods and Operators


        /// <summary>
        ///     ==s the specified left.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns></returns>
        public static bool operator == ( Lookup left, Lookup right )
        {
            return Equals ( left, right );
        }

        /// <summary>
        ///     !=s the specified left.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns></returns>
        public static bool operator != ( Lookup left, Lookup right )
        {
            return !Equals ( left, right );
        }

        /// <summary>
        ///     Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public bool Equals ( Lookup other )
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(CodedConcept, other.CodedConcept) && Value.Equals(other.Value);
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
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Lookup) obj);
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
                return ((CodedConcept != null ? CodedConcept.GetHashCode() : 0)*397) ^ Value.GetHashCode();
            }
        }

        #endregion


    }
}