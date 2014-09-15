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
    using System.Reflection;

    using ProCenter.Domain.AssessmentModule.Metadata;
    using ProCenter.Domain.CommonModule;
    using ProCenter.Domain.CommonModule.Lookups;

    #endregion

    /// <summary>Class for defining an Item definition.</summary>
    public class ItemDefinition : IEquatable<ItemDefinition>, IContainItemDefinitions
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemDefinition"/> class.
        /// </summary>
        /// <param name="codedConcept">The coded concept.</param>
        /// <param name="itemType">Type of the item.</param>
        /// <param name="valueType">Type of the value.</param>
        /// <param name="options">The options.</param>
        /// <param name="itemDefinitions">The item definitions.</param>
        public ItemDefinition (
            CodedConcept codedConcept,
            Lookup itemType,
            Lookup valueType,
            IEnumerable<Lookup> options = null,
            IEnumerable<ItemDefinition> itemDefinitions = null )
        {
            CodedConcept = codedConcept;
            ItemType = itemType;
            ValueType = valueType;
            Options = options;
            ItemDefinitions = itemDefinitions;
        }

        #endregion

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
        ///     Gets or sets the item metadata.
        /// </summary>
        /// <value>
        ///     The item metadata.
        /// </value>
        public ItemMetadata ItemMetadata { get; set; }

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

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets the name of the template.
        /// </summary>
        /// <param name="propertyInfo">The property information.</param>
        /// <returns>Returns the name of the template if found in ItemMetatdata otherwise the default based on propertyInfo.</returns>
        public string GetTemplateName(PropertyInfo propertyInfo)
        {
            string templateName = propertyInfo.PropertyType.Name;
            if (ItemMetadata != null)
            {
                var templateNameMetaData = ItemMetadata.FindMetadataItem<TemplateNameMetadataItem> ();
                if ( templateNameMetaData != null )
                {
                    templateName = templateNameMetaData.TemplateName;
                }
                else if (propertyInfo.PropertyType.IsSubclassOf(typeof(Lookup)))
                {
                    templateName = "LookupDto";
                }
            }
            else if (propertyInfo.PropertyType.IsSubclassOf(typeof(Lookup)))
            {
                templateName = "LookupDto";
            } 
            return templateName;
        }

        /// <summary>Gets whether the item definition is required.</summary>
        /// <returns><c>True</c> if item definition is required, otherwise <c>False</c>.</returns>
        public bool GetIsRequired ()
        {
            if ( ItemMetadata != null )
            {
                return ItemMetadata.FindMetadataItem<RequiredForCompletenessMetadataItem> () != null;
            }
            return false;
        }

        /// <summary>
        ///     Checks equals.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>Whether they are equal or not.</returns>
        public static bool operator == ( ItemDefinition left, ItemDefinition right )
        {
            return Equals ( left, right );
        }

        /// <summary>
        ///     Checks not equals.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>Whether they are not equal.</returns>
        public static bool operator != ( ItemDefinition left, ItemDefinition right )
        {
            return !Equals ( left, right );
        }

        /// <summary>
        ///     Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>Whether they are equal.</returns>
        public bool Equals ( ItemDefinition other )
        {
            if ( ReferenceEquals ( null, other ) )
            {
                return false;
            }
            if ( ReferenceEquals ( this, other ) )
            {
                return true;
            }
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
            return Equals ( (ItemDefinition)obj );
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