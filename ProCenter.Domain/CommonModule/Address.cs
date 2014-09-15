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

namespace ProCenter.Domain.CommonModule
{
    #region Using Statements

    using System;

    using Pillar.Common.Utility;
    using Pillar.Domain.Attributes;
    using Pillar.Domain.Primitives;

    #endregion

    /// <summary>Address value object.</summary>
    public class Address : IValueObject, IEquatable<Address>
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Address" /> class.
        /// </summary>
        /// <param name="firstStreetAddress">The first street address.</param>
        /// <param name="secondStreetAddress">The second street address.</param>
        /// <param name="cityName">Name of the city.</param>
        /// <param name="stateProvince">The state province.</param>
        /// <param name="postalCode">The postal code.</param>
        public Address (
            string firstStreetAddress,
            string secondStreetAddress,
            string cityName,
            ////CountyArea countyArea,
            StateProvince stateProvince,
            ////Country country,
            PostalCode postalCode )
        {
            Check.IsNotNullOrWhitespace ( firstStreetAddress, () => FirstStreetAddress );
            Check.IsNotNullOrWhitespace ( cityName, () => CityName );
            Check.IsNotNull ( stateProvince, () => StateProvince );

            FirstStreetAddress = firstStreetAddress;
            SecondStreetAddress = string.IsNullOrWhiteSpace ( secondStreetAddress ) ? null : secondStreetAddress;
            CityName = cityName;
            ////CountyArea = countyArea;
            StateProvince = stateProvince;
            ////Country = country;
            PostalCode = postalCode;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the name of the city.
        /// </summary>
        /// <value>
        ///     The name of the city.
        /// </value>
        [NotNull]
        public virtual string CityName { get; private set; }

        /// <summary>
        ///     Gets the first street address.
        /// </summary>
        [NotNull]
        public virtual string FirstStreetAddress { get; private set; }

        ///// <summary>
        ///// Gets the county area.
        ///// </summary>
        //public virtual CountyArea CountyArea { get; private set; }

        ///// <summary>
        ///// Gets the country.
        ///// </summary>
        //public virtual Country Country { get; private set; }

        /// <summary>
        ///     Gets the postal code.
        /// </summary>
        public virtual PostalCode PostalCode { get; private set; }

        /// <summary>
        ///     Gets the second street address.
        /// </summary>
        public virtual string SecondStreetAddress { get; private set; }

        /// <summary>
        ///     Gets the state province.
        /// </summary>
        [NotNull]
        public virtual StateProvince StateProvince { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        ///     The result of the operator.
        /// </returns>
        public static bool operator == ( Address left, Address right )
        {
            return Equals ( left, right );
        }

        /// <summary>
        ///     Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        ///     The result of the operator.
        /// </returns>
        public static bool operator != ( Address left, Address right )
        {
            return !Equals ( left, right );
        }

        /// <summary>
        ///     Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        ///     True if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals ( Address other )
        {
            if ( ReferenceEquals ( null, other ) )
            {
                return false;
            }
            if ( ReferenceEquals ( this, other ) )
            {
                return true;
            }
            return Equals ( other.FirstStreetAddress, FirstStreetAddress ) &&
                   Equals ( other.SecondStreetAddress, SecondStreetAddress ) &&
                   Equals ( other.CityName, CityName ) &&
                   ////Equals ( other.CountyArea, CountyArea ) &&
                   Equals ( other.StateProvince, StateProvince ) &&
                   ////Equals ( other.Country, Country ) &&
                   Equals ( other.PostalCode, PostalCode );
        }

        /// <summary>
        ///     Determines whether the specified <see cref="T:System.Object" /> is equal to the current
        ///     <see cref="T:System.Object" />.
        /// </summary>
        /// <returns>
        ///     True if the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />;
        ///     otherwise, false.
        /// </returns>
        /// <param name="obj">
        ///     The <see cref="T:System.Object" /> to compare with the current <see cref="T:System.Object" />.
        /// </param>
        /// <filterpriority>2</filterpriority>
        public override bool Equals ( object obj )
        {
            if ( ReferenceEquals ( null, obj ) )
            {
                return false;
            }
            if ( ReferenceEquals ( this, obj ) )
            {
                return true;
            }
            if ( obj.GetType () != typeof(Address) )
            {
                return false;
            }
            return Equals ( (Address)obj );
        }

        /// <summary>
        ///     Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        ///     A hash code for the current <see cref="T:System.Object" />.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode ()
        {
            unchecked
            {
                var result = FirstStreetAddress != null ? FirstStreetAddress.GetHashCode () : 0;
                result = ( result * 397 ) ^ ( SecondStreetAddress != null ? SecondStreetAddress.GetHashCode () : 0 );
                result = ( result * 397 ) ^ ( CityName != null ? CityName.GetHashCode () : 0 );
                ////result = (result * 397) ^ (CountyArea != null ? CountyArea.GetHashCode() : 0);
                result = ( result * 397 ) ^ ( StateProvince != null ? StateProvince.GetHashCode () : 0 );
                ////result = (result * 397) ^ (Country != null ? Country.GetHashCode() : 0);
                result = ( result * 397 ) ^ ( PostalCode != null ? PostalCode.GetHashCode () : 0 );
                return result;
            }
        }

        #endregion
    }
}