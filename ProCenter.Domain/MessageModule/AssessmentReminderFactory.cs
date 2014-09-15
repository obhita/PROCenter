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

namespace ProCenter.Domain.MessageModule
{
    #region Using Statements

    using System;

    #endregion

    /// <summary>The assessment reminder factory class.</summary>
    public class AssessmentReminderFactory : IAssessmentReminderFactory
    {
        #region Public Methods and Operators

        /// <summary>
        /// Creates the specified organization key.
        /// </summary>
        /// <param name="organizationKey">The organization key.</param>
        /// <param name="patientKey">The patient key.</param>
        /// <param name="createdByStaffKey">The created by staff key.</param>
        /// <param name="assessmentDefinitionKey">The assessment definition key.</param>
        /// <param name="title">The title.</param>
        /// <param name="start">The start.</param>
        /// <param name="description">The description.</param>
        /// <param name="reminderRecurrence">The reminder recurrence.</param>
        /// <param name="end">The end.</param>
        /// <returns>
        /// A <see cref="AssessmentReminder" />.
        /// </returns>
        public AssessmentReminder Create (
            Guid organizationKey,
            Guid patientKey,
            Guid createdByStaffKey,
            Guid assessmentDefinitionKey,
            string title,
            DateTime start,
            string description,
            AssessmentReminderRecurrence reminderRecurrence,
            DateTime? end)
        {
            return new AssessmentReminder ( organizationKey, patientKey, createdByStaffKey, assessmentDefinitionKey, title, start, description, reminderRecurrence, end);
        }

        #endregion
    }
}