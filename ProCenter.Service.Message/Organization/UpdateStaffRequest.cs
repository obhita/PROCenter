﻿#region License Header

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

namespace ProCenter.Service.Message.Organization
{
    #region Using Statements

    using System;

    using Agatha.Common;

    #endregion

    /// <summary>The update staff request class.</summary>
    public class UpdateStaffRequest : Request
    {
        #region Enums

        /// <summary>
        /// Staff update type.
        /// </summary>
        public enum StaffUpdateType
        {
            /// <summary>
            /// The name.
            /// </summary>
            Name,

            /// <summary>
            /// The email.
            /// </summary>
            Email,

            /// <summary>
            /// The location.
            /// </summary>
            Location,

            /// <summary>
            /// The npi.
            /// </summary>
            NPI
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the staff key.
        /// </summary>
        /// <value>
        /// The staff key.
        /// </value>
        public Guid StaffKey { get; set; }

        /// <summary>
        /// Gets or sets the type of the update.
        /// </summary>
        /// <value>
        /// The type of the update.
        /// </value>
        public StaffUpdateType UpdateType { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public object Value { get; set; }

        #endregion
    }
}