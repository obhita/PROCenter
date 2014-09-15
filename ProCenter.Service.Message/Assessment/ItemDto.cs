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

namespace ProCenter.Service.Message.Assessment
{
    #region Using Statements

    using System;
    using System.Collections.Generic;

    using ProCenter.Domain.AssessmentModule;
    using ProCenter.Service.Message.Common;
    using ProCenter.Service.Message.Common.Lookups;

    #endregion

    /// <summary>The item dto class.</summary>
    public class ItemDto : KeyedDataTransferObject, IAssessmentDto, IContainItems
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the assessment definition key.
        /// </summary>
        /// <value>
        /// The assessment definition key.
        /// </value>
        public Guid AssessmentDefinitionKey { get; set; }

        /// <summary>
        /// Gets or sets the assessment code.
        /// </summary>
        /// <value>
        /// The assessment code.
        /// </value>
        public string AssessmentCode { get; set; }

        /// <summary>
        /// Gets or sets the item definition code.
        /// </summary>
        /// <value>
        /// The item definition code.
        /// </value>
        public string ItemDefinitionCode { get; set; }

        /// <summary>
        /// Gets or sets the name of the item definition.
        /// </summary>
        /// <value>
        /// The name of the item definition.
        /// </value>
        public string ItemDefinitionName { get; set; }

        /// <summary>
        /// Gets or sets the type of the item.
        /// </summary>
        /// <value>
        /// The type of the item.
        /// </value>
        public string ItemType { get; set; }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        public IList<ItemDto> Items { get; set; }

        /// <summary>
        /// Gets or sets the metadata.
        /// </summary>
        /// <value>
        /// The metadata.
        /// </value>
        public ItemMetadata Metadata { get; set; }

        /// <summary>
        /// Gets or sets the options.
        /// </summary>
        /// <value>
        /// The options.
        /// </value>
        public IEnumerable<LookupDto> Options { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public object Value { get; set; }

        /// <summary>
        /// Gets or sets the name of the parent.
        /// </summary>
        /// <value>
        /// The name of the parent.
        /// </value>
        public string ParentName { get; set; }

        /// <summary>
        /// Gets or sets the name of the assessment.
        /// </summary>
        /// <value>
        /// The name of the assessment.
        /// </value>
        public string AssessmentName { get; set; }

        #endregion
    }
}