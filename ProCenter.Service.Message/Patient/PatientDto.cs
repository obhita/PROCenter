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

namespace ProCenter.Service.Message.Patient
{
    #region Using Statements

    using System;
    using System.ComponentModel.DataAnnotations;

    using ProCenter.Primitive;
    using ProCenter.Service.Message.Common;
    using ProCenter.Service.Message.Common.Lookups;
    using ProCenter.Service.Message.Security;

    #endregion

    /// <summary>The patient dto class.</summary>
    public class PatientDto : KeyedDataTransferObject
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [Display ( Name = " " )]
        [Required]
        public PersonName Name { get; set; }

        /// <summary>
        /// Gets or sets the organization key.
        /// </summary>
        /// <value>
        /// The organization key.
        /// </value>
        [ScaffoldColumn ( false )]
        public Guid OrganizationKey { get; set; }

        /// <summary>
        /// Gets or sets the religion.
        /// </summary>
        /// <value>
        /// The religion.
        /// </value>
        public LookupDto Religion { get; set; }

        /// <summary>
        /// Gets or sets the system account.
        /// </summary>
        /// <value>
        /// The system account.
        /// </value>
        [ScaffoldColumn ( false )]
        public SystemAccountDto SystemAccount { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        /// <value>
        /// The unique identifier.
        /// </value>
        [Display ( Name = "Unique Identifier" )]
        [Editable ( false )]
        public string UniqueIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the date of birth.
        /// </summary>
        /// <value>
        /// The date of birth.
        /// </value>
        [Display ( Name = "Birth Date" )]
        [DisplayFormat ( DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true )]
        [Required]
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        [DataType ( DataType.EmailAddress )]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the ethnicity.
        /// </summary>
        /// <value>
        /// The ethnicity.
        /// </value>
        public LookupDto Ethnicity { get; set; }

        /// <summary>
        /// Gets or sets the gender.
        /// </summary>
        /// <value>
        /// The gender.
        /// </value>
        [Required(ErrorMessage = "The Gender field is required.")]
        public LookupDto Gender { get; set; }

        /// <summary>
        /// Gets a value indicating whether [has account].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [has account]; otherwise, <c>false</c>.
        /// </value>
        [ScaffoldColumn ( false )]
        public bool HasAccount
        {
            get { return SystemAccount != null; }
        }

        #endregion
    }
}