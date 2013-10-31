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
    using CommonModule;
    using CommonModule.Lookups;

    #endregion

    /// <summary>
    ///     Class for defining an Item definition.
    /// </summary>
    public class ItemDefinition : IEquatable<ItemDefinition>, IContainItemDefinitions
    {
        public ItemDefinition (CodedConcept codedConcept, Lookup itemType, Lookup valueType, IEnumerable<Lookup> options = null, IEnumerable<ItemDefinition> itemDefinitions = null  )
        {
            CodedConcept = codedConcept;
            ItemType = itemType;
            ValueType = valueType;
            Options = options;
            ItemDefinitions = itemDefinitions;
        }

        #region Public Properties

        /// <summary>
        ///     Gets the coded concept.
        /// </summary>
        /// <value>
        ///     The coded concept.
        /// </value>
        public CodedConcept CodedConcept { get; private set; }

        /// <summary>
        ///     Gets the item definitions.
        /// </summary>
        /// <value>
        ///     The item definitions.
        /// </value>
        public IEnumerable<ItemDefinition> ItemDefinitions { get; private set; }

        /// <summary>
        ///     Gets the type of the item.
        /// </summary>
        /// <value>
        ///     The type of the item.
        /// </value>
        public Lookup ItemType { get; private set; }

        /// <summary>
        ///     Gets or sets the options.
        /// </summary>
        /// <value>
        ///     The options.
        /// </value>
        public IEnumerable<Lookup> Options { get; set; }

        /// <summary>
        ///     Gets the type of the value.
        /// </summary>
        /// <value>
        ///     The type of the value.
        /// </value>
        public Lookup ValueType { get; private set; }

        /// <summary>
        ///     Gets the version.
        /// </summary>
        /// <value>
        ///     The version.
        /// </value>
        public string Version { get; private set; }

        /// <summary>
        /// Gets or sets the item metadata.
        /// </summary>
        /// <value>
        /// The item metadata.
        /// </value>
        public ItemMetadata ItemMetadata { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     ==s the specified left.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns></returns>
        public static bool operator == ( ItemDefinition left, ItemDefinition right )
        {
            return Equals ( left, right );
        }

        /// <summary>
        ///     !=s the specified left.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns></returns>
        public static bool operator != ( ItemDefinition left, ItemDefinition right )
        {
            return !Equals ( left, right );
        }

        /// <summary>
        ///     Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public bool Equals ( ItemDefinition other )
        {
            if ( ReferenceEquals ( null, other ) )
                return false;
            if ( ReferenceEquals ( this, other ) )
                return true;
            return Equals ( CodedConcept, other.CodedConcept ) && string.Equals ( Version, other.Version );
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
            return Equals ( (ItemDefinition) obj );
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
                return ( ( CodedConcept != null ? CodedConcept.GetHashCode () : 0 ) * 397 ) ^ ( Version != null ? Version.GetHashCode () : 0 );
            }
        }

        #endregion
    }
}