namespace ProCenter.Domain.OrganizationModule
{
    #region Using Statements

    using System;
    using CommonModule;
    using Pillar.Common.Utility;

    #endregion

    /// <summary>
    ///     Organization Address
    /// </summary>
    public class OrganizationAddress : IEquatable<OrganizationAddress>
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="OrganizationAddress" /> class.
        /// </summary>
        /// <param name="organizationAddressType">Type of the organization address.</param>
        /// <param name="address">The address.</param>
        /// <param name="isPrimary">
        ///     if set to <c>true</c> [is primary].
        /// </param>
        public OrganizationAddress ( OrganizationAddressType organizationAddressType, Address address, bool isPrimary = false )
        {
            Check.IsNotNull ( organizationAddressType, () => OrganizationAddressType );
            Check.IsNotNull ( address, () => Address );

            OrganizationAddressType = organizationAddressType;
            Address = address;
            IsPrimary = isPrimary;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the address.
        /// </summary>
        /// <value>
        ///     The address.
        /// </value>
        public Address Address { get; private set; }

        /// <summary>
        ///     Gets a value indicating whether this instance is primary.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is primary; otherwise, <c>false</c>.
        /// </value>
        public bool IsPrimary { get; internal set; }

        /// <summary>
        ///     Gets the type of the organization address.
        /// </summary>
        /// <value>
        ///     The type of the organization address.
        /// </value>
        public OrganizationAddressType OrganizationAddressType { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     ==s the specified left.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns></returns>
        public static bool operator == ( OrganizationAddress left, OrganizationAddress right )
        {
            return Equals ( left, right );
        }

        /// <summary>
        ///     !=s the specified left.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns></returns>
        public static bool operator != ( OrganizationAddress left, OrganizationAddress right )
        {
            return !Equals ( left, right );
        }

        /// <summary>
        ///     Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public bool Equals ( OrganizationAddress other )
        {
            if ( ReferenceEquals ( null, other ) )
                return false;
            if ( ReferenceEquals ( this, other ) )
                return true;
            return Equals ( Address, other.Address ) && Equals ( OrganizationAddressType, other.OrganizationAddressType );
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
            return Equals ( (OrganizationAddress) obj );
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
                return ( ( Address != null ? Address.GetHashCode () : 0 ) * 397 ) ^ ( OrganizationAddressType != null ? OrganizationAddressType.GetHashCode () : 0 );
            }
        }

        #endregion
    }
}