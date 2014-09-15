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

#region Using Statements

#endregion

namespace ProCenter.Primitive
{
    #region Using Statements

    using System;
    using System.ComponentModel.DataAnnotations;

    using Pillar.Common.Utility;
    using Pillar.Domain.Attributes;

    #endregion

    /// <summary>Person name primitive.</summary>
    [Component]
    public class PersonName : IPrimitive, IEquatable<PersonName>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonName"/> class.
        /// </summary>
        public PersonName ()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonName"/> class.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        public PersonName ( string firstName, string lastName )
            : this ( null, firstName, null, lastName, null )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonName"/> class.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <param name="middleName">Name of the middle.</param>
        /// <param name="lastName">The last name.</param>
        public PersonName ( string firstName, string middleName, string lastName )
            : this ( null, firstName, middleName, lastName, null )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonName"/> class.
        /// </summary>
        /// <param name="prefix">The prefix.</param>
        /// <param name="firstName">The first name.</param>
        /// <param name="middleName">Name of the middle.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="suffix">The suffix.</param>
        public PersonName ( string prefix, string firstName, string middleName, string lastName, string suffix )
        {
            Check.IsNotNullOrWhitespace ( firstName, () => FirstName );
            Check.IsNotNullOrWhitespace ( lastName, () => LastName );

            Prefix = prefix;
            FirstName = firstName == null ? null : firstName.Trim();
            MiddleName = middleName == null ? null : middleName.Trim();
            LastName = lastName == null ? null : lastName.Trim();
            Suffix = suffix;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        [Required ( ErrorMessage = "The First Name field is required." )]
        [NotNull]
        public virtual string FirstName { get; private set; }

        /// <summary>
        /// Gets the full name.
        /// </summary>
        /// <value>
        /// The full name.
        /// </value>
        public virtual string FullName
        {
            get { return FirstName + " " + LastName; }
        }

        /// <summary>
        /// Gets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        [Required ( ErrorMessage = "The Last Name field is required." )]
        [NotNull]
        public virtual string LastName { get; private set; }

        /// <summary>
        /// Gets the name of the middle.
        /// </summary>
        /// <value>
        /// The name of the middle.
        /// </value>
        public virtual string MiddleName { get; private set; }

        /// <summary>
        /// Gets the prefix.
        /// </summary>
        /// <value>
        /// The prefix.
        /// </value>
        public virtual string Prefix { get; private set; }

        /// <summary>
        /// Gets the suffix.
        /// </summary>
        /// <value>
        /// The suffix.
        /// </value>
        public virtual string Suffix { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>Checks if equal.</summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>Whether they are equal.</returns>
        public static bool operator == ( PersonName left, PersonName right )
        {
            return Equals ( left, right );
        }

        /// <summary>Checks if not equal.</summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>Whether they are not equal.</returns>
        public static bool operator != ( PersonName left, PersonName right )
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
        public bool Equals ( PersonName other )
        {
            if ( ReferenceEquals ( null, other ) )
            {
                return false;
            }
            if ( ReferenceEquals ( this, other ) )
            {
                return true;
            }
            return Equals ( other.Suffix, Suffix ) && Equals ( other.LastName, LastName ) &&
                   Equals ( other.MiddleName, MiddleName ) && Equals ( other.FirstName, FirstName ) &&
                   Equals ( other.Prefix, Prefix );
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
            if ( obj.GetType () != typeof(PersonName) )
            {
                return false;
            }
            return Equals ( (PersonName)obj );
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
                var result = ( Suffix != null ? Suffix.GetHashCode () : 0 );
                result = ( result * 397 ) ^ ( LastName != null ? LastName.GetHashCode () : 0 );
                result = ( result * 397 ) ^ ( MiddleName != null ? MiddleName.GetHashCode () : 0 );
                result = ( result * 397 ) ^ ( FirstName != null ? FirstName.GetHashCode () : 0 );
                result = ( result * 397 ) ^ ( Prefix != null ? Prefix.GetHashCode () : 0 );
                return result;
            }
        }

        #endregion
    }
}