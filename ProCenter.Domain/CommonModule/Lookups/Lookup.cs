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

namespace ProCenter.Domain.CommonModule.Lookups
{
    #region Using Statements

    using System;
    using System.Resources;

    #endregion

    /// <summary>Base Class for lookups.</summary>
    public class Lookup : IEquatable<Lookup>, IComparable
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Lookup" /> class.
        /// </summary>
        public Lookup ()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Lookup" /> class.
        /// </summary>
        /// <param name="codedConcept">The coded concept.</param>
        public Lookup ( CodedConcept codedConcept )
            : this ( codedConcept, 0.0 )
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Lookup" /> class.
        /// </summary>
        /// <param name="codedConcept">The coded concept.</param>
        /// <param name="value">The value.</param>
        /// <param name="sortOrder">The sort order.</param>
        /// <param name="isDefault">If set to <c>true</c> [is default].</param>
        public Lookup ( CodedConcept codedConcept, double value, int sortOrder = 0, bool isDefault = false )
        {
            CodedConcept = codedConcept;
            IsDefault = isDefault;
            SortOrder = sortOrder;
            Value = value;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the coded concept.
        /// </summary>
        /// <value>
        ///     The coded concept.
        /// </value>
        public CodedConcept CodedConcept { get; protected set; }

        /// <summary>
        ///     Gets the display name.
        /// </summary>
        /// <value>
        ///     The display name.
        /// </value>
        public string DisplayName
        {
            get
            {
                const string Pre = "_";
                var type = GetType ();
                if ( type == typeof(Lookup) || CodedConcept == null || CodedConcept.Code == null )
                {
                    return string.Empty;
                }
                var resourceManger = new ResourceManager ( type );
                var returnString = resourceManger.GetString(CodedConcept.Code) ?? string.Empty;
                if ( string.IsNullOrWhiteSpace ( returnString ) )
                {
                    returnString = resourceManger.GetString(Pre + CodedConcept.Code) ?? string.Empty;
                }
                return returnString;
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
        ///     Checks equals.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>Whether they are equal.</returns>
        public static bool operator == ( Lookup left, Lookup right )
        {
            return Equals ( left, right );
        }

        /// <summary>
        ///     Checks not equals.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>Whether they are not equal.</returns>
        public static bool operator != ( Lookup left, Lookup right )
        {
            return !Equals ( left, right );
        }

        /// <summary>
        ///     Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>Whether they are equal.</returns>
        public bool Equals ( Lookup other )
        {
            if ( ReferenceEquals ( null, other ) )
            {
                return false;
            }
            if ( ReferenceEquals ( this, other ) )
            {
                return true;
            }
            return Equals ( CodedConcept, other.CodedConcept ) && Value.Equals ( other.Value );
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
            {
                return false;
            }
            if ( ReferenceEquals ( this, obj ) )
            {
                return true;
            }
            if ( obj.GetType () != GetType () )
            {
                return false;
            }
            return Equals ( (Lookup)obj );
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
                return ( ( CodedConcept != null ? CodedConcept.GetHashCode () : 0 ) * 397 ) ^ Value.GetHashCode ();
            }
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates 
        /// whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has these meanings: 
        /// Value Meaning Less than zero This instance precedes <paramref name="obj"/> in the sort order. 
        /// Zero This instance occurs in the same position in the sort order as 
        /// <paramref name="obj"/>. Greater than zero This instance follows <paramref name="obj"/> in the sort order. 
        /// </returns>
        /// <param name="obj">An object to compare with this instance. </param><exception cref="T:System.ArgumentException"><paramref name="obj"/> 
        /// is not the same type as this instance. </exception>
        public int CompareTo ( object obj )
        {
            if ( Equals ( (Lookup)obj ) )
            {
                return 0;
            }
            return -1;
        }

        /// <summary>Implicitly convert lookup to string.</summary>
        /// <param name="lookup">The lookup.</param>
        /// <returns>The coded concept code of the lookup.</returns>
        public static implicit operator string ( Lookup lookup )
        {
            if ( lookup == null )
            {
                return null;
            }
            return lookup.CodedConcept == null ? null : lookup.CodedConcept.Code;
        }

        #endregion
    }
}