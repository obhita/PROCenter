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

namespace ProCenter.Domain.AssessmentModule
{
    #region Using Statements

    using System;
    using System.Collections.Generic;

    using ProCenter.Domain.CommonModule.Lookups;

    #endregion

    /// <summary>The score item class.</summary>
    public class ScoreItem : IEquatable<ScoreItem>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ScoreItem"/> class.
        /// </summary>
        /// <param name="itemDefinitionCode">The item definition code.</param>
        /// <param name="value">The value.</param>
        /// <param name="scoreItems">The score items.</param>
        public ScoreItem ( string itemDefinitionCode, object value, params ScoreItem[] scoreItems )
        {
            ItemDefinitionCode = itemDefinitionCode;
            Value = value;
            ScoreItems = scoreItems;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the item definition code.
        /// </summary>
        /// <value>
        /// The item definition code.
        /// </value>
        public string ItemDefinitionCode { get; private set; }

        /// <summary>
        /// Gets or sets the item metadata.
        /// </summary>
        /// <value>
        /// The item metadata.
        /// </value>
        public ItemMetadata ItemMetadata { get; set; }

        /// <summary>
        /// Gets the score items.
        /// </summary>
        /// <value>
        /// The score items.
        /// </value>
        public IEnumerable<ScoreItem> ScoreItems { get; private set; }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public object Value { get; private set; }

        /// <summary>
        /// Gets the type of the value.
        /// </summary>
        /// <value>
        /// The type of the value.
        /// </value>
        public Lookup ValueType { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>Checks if equal.</summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>Whether they are equal.</returns>
        public static bool operator == ( ScoreItem left, ScoreItem right )
        {
            return Equals ( left, right );
        }

        /// <summary>Checks if not equal.</summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>Whether they are not equal.</returns>
        public static bool operator != ( ScoreItem left, ScoreItem right )
        {
            return !Equals ( left, right );
        }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
        public bool Equals ( ScoreItem other )
        {
            if ( ReferenceEquals ( null, other ) )
            {
                return false;
            }
            if ( ReferenceEquals ( this, other ) )
            {
                return true;
            }
            return string.Equals ( ItemDefinitionCode, other.ItemDefinitionCode ) && Equals ( Value, other.Value );
        }

        /// <summary>Determines whether the specified <see cref="System.Object" />, is equal to this instance.</summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns><c>True</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>False</c>.</returns>
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
            if ( obj.GetType () != this.GetType () )
            {
                return false;
            }
            return Equals ( (ScoreItem)obj );
        }

        /// <summary>Returns a hash code for this instance.</summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. </returns>
        public override int GetHashCode ()
        {
            unchecked
            {
                return ( ( ItemDefinitionCode != null ? ItemDefinitionCode.GetHashCode () : 0 ) * 397 ) ^ ( Value != null ? Value.GetHashCode () : 0 );
            }
        }

        /// <summary>Updates the value.</summary>
        /// <param name="value">The value.</param>
        public void UpdateValue ( object value )
        {
            Value = value;
        }

        #endregion
    }
}