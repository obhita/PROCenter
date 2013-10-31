#region License Header
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
#endregion
namespace ProCenter.Domain.OrganizationModule
{
    #region Using Statements

    using System;
    using CommonModule;
    using Pillar.Common.Utility;

    #endregion

    /// <summary>
    ///     Organization phone.
    /// </summary>
    public class OrganizationPhone : IEquatable<OrganizationPhone>
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="OrganizationPhone" /> class.
        /// </summary>
        /// <param name="organizationPhoneType">Type of the organization phone.</param>
        /// <param name="phone">The phone.</param>
        /// <param name="isPrimary">
        ///     if set to <c>true</c> [is primary].
        /// </param>
        public OrganizationPhone ( OrganizationPhoneType organizationPhoneType, Phone phone, bool isPrimary = false )
        {
            Check.IsNotNull ( organizationPhoneType, () => OrganizationPhoneType );
            Check.IsNotNull ( phone, () => Phone );

            OrganizationPhoneType = organizationPhoneType;
            Phone = phone;
            IsPrimary = isPrimary;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets a value indicating whether this instance is primary.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is primary; otherwise, <c>false</c>.
        /// </value>
        public bool IsPrimary { get; internal set; }

        /// <summary>
        ///     Gets the type of the organization phone.
        /// </summary>
        /// <value>
        ///     The type of the organization phone.
        /// </value>
        public OrganizationPhoneType OrganizationPhoneType { get; private set; }

        /// <summary>
        ///     Gets the phone.
        /// </summary>
        /// <value>
        ///     The phone.
        /// </value>
        public Phone Phone { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     ==s the specified left.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        ///     <c>true</c> if the specified left <see cref="OrganizationPhone" /> is equal to the right
        ///     <see
        ///         cref="OrganizationPhone" />
        ///     ; otherwise, <c>false</c>.
        /// </returns>
        public static bool operator == ( OrganizationPhone left, OrganizationPhone right )
        {
            return Equals ( left, right );
        }

        /// <summary>
        ///     !=s the specified left.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        ///     <c>true</c> if the specified left <see cref="OrganizationPhone" /> is not equal to the right
        ///     <see
        ///         cref="OrganizationPhone" />
        ///     ; otherwise, <c>false</c>.
        /// </returns>
        public static bool operator != ( OrganizationPhone left, OrganizationPhone right )
        {
            return !Equals ( left, right );
        }

        /// <summary>
        ///     Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>
        ///     <c>true</c> if the specified <see cref="OrganizationPhone" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals ( OrganizationPhone other )
        {
            if ( ReferenceEquals ( null, other ) )
                return false;
            if ( ReferenceEquals ( this, other ) )
                return true;
            return Equals ( OrganizationPhoneType, other.OrganizationPhoneType ) && Equals ( Phone, other.Phone );
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
            return Equals ( (OrganizationPhone) obj );
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
                return ( ( OrganizationPhoneType != null ? OrganizationPhoneType.GetHashCode () : 0 ) * 397 ) ^ ( Phone != null ? Phone.GetHashCode () : 0 );
            }
        }

        #endregion
    }
}