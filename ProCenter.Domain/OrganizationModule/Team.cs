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
namespace ProCenter.Domain.OrganizationModule
{
    #region Using Statements

    using System;
    using System.Collections.Generic;
    using CommonModule;
    using Event;
    using Pillar.Common.Utility;

    #endregion

    public class Team : AggregateRootBase
    {
        #region Fields

        private readonly List<Guid> _patientKeys = new List<Guid> ();
        private readonly List<Guid> _staffKeys = new List<Guid> ();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Team" /> class.
        /// </summary>
        /// <param name="organizationKey">The organization key.</param>
        /// <param name="name">The name.</param>
        internal Team ( Guid organizationKey, string name )
        {
            Check.IsNotNullOrWhitespace ( name, () => Name );

            Key = CombGuid.NewCombGuid ();
            RaiseEvent ( new TeamCreatedEvent ( Key, Version, organizationKey, name ) );
        }

        public Team(){}

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public string Name { get; private set; }

        /// <summary>
        ///     Gets the organization key.
        /// </summary>
        /// <value>
        ///     The organization key.
        /// </value>
        public Guid OrganizationKey { get; private set; }

        /// <summary>
        ///     Gets the patient keys.
        /// </summary>
        /// <value>
        ///     The patient keys.
        /// </value>
        public IEnumerable<Guid> PatientKeys
        {
            get { return _patientKeys; }
        }

        /// <summary>
        ///     Gets the staff keys.
        /// </summary>
        /// <value>
        ///     The staff keys.
        /// </value>
        public IEnumerable<Guid> StaffKeys
        {
            get { return _staffKeys; }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Adds the patient.
        /// </summary>
        /// <param name="patientKey">The patient key.</param>
        public void AddPatient ( Guid patientKey )
        {
            if ( !_patientKeys.Contains ( patientKey ) )
            {
                RaiseEvent ( new PatientAddedToTeamEvent ( Key, Version, patientKey ) );
            }
        }

        /// <summary>
        ///     Adds the staff.
        /// </summary>
        /// <param name="staffKey">The staff key.</param>
        public void AddStaff ( Guid staffKey )
        {
            if ( !_staffKeys.Contains ( staffKey ) )
            {
                RaiseEvent ( new StaffAddedToTeamEvent ( Key, Version, staffKey ) );
            }
        }

        /// <summary>
        /// Removes the staff.
        /// </summary>
        /// <param name="staffKey">The staff key.</param>
        public void RemoveStaff ( Guid staffKey )
        {
            if ( _staffKeys.Contains ( staffKey ) )
            {
                RaiseEvent ( new StaffRemovedFromTeamEvent ( Key, Version, staffKey ) );
            }
        }

        /// <summary>
        /// Removes the patient.
        /// </summary>
        /// <param name="patientKey">The patient key.</param>
        public void RemovePatient(Guid patientKey)
        {
            if (_patientKeys.Contains(patientKey))
            {
                RaiseEvent(new PatientRemovedFromTeamEvent(Key, Version, patientKey));
            }
        }

        /// <summary>
        /// Revises the name.
        /// </summary>
        /// <param name="name">The name.</param>
        public void ReviseName ( string name )
        {
            Check.IsNotNullOrWhitespace ( name, () => Name );

            RaiseEvent ( new TeamNameRevisedEvent ( Key, Version, name ) );
        }

        #endregion

        #region Methods

        private void Apply ( TeamCreatedEvent teamCreatedEvent )
        {
            Name = teamCreatedEvent.Name;
            OrganizationKey = teamCreatedEvent.OrganizationKey;
        }

        private void Apply ( StaffAddedToTeamEvent staffAddedToTeamEvent )
        {
            _staffKeys.Add ( staffAddedToTeamEvent.StaffKey );
        }

        private void Apply ( PatientAddedToTeamEvent patientAddedToTeamEvent )
        {
            _patientKeys.Add ( patientAddedToTeamEvent.PatientKey );
        }

        private void Apply(StaffRemovedFromTeamEvent staffRemovedFromTeamEvent)
        {
            _staffKeys.Remove(staffRemovedFromTeamEvent.StaffKey);
        }

        private void Apply(PatientRemovedFromTeamEvent patientRemovedFromTeamEvent)
        {
            _patientKeys.Remove(patientRemovedFromTeamEvent.PatientKey);
        }

        private void Apply ( TeamNameRevisedEvent teamNameRevisedEvent )
        {
            Name = teamNameRevisedEvent.Name;
        }

        #endregion
    }
}