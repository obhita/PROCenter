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
    using System.Linq;
    using CommonModule;
    using Event;
    using Pillar.Common.Utility;

    #endregion

    /// <summary>
    ///     Organization aggregate root.
    /// </summary>
    public class Organization : AggregateRootBase
    {
        #region Fields

        private readonly List<OrganizationAddress> _organizationAddresses = new List<OrganizationAddress> ();
        private readonly List<OrganizationPhone> _organizationPhones = new List<OrganizationPhone> ();
        private readonly List<Guid> _assessmentDefinitions = new List<Guid>();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Organization" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public Organization ( string name )
        {
            Key = CombGuid.NewCombGuid ();
            RaiseEvent ( new OrganizationCreatedEvent ( Key, Version, name ) );
        }

        public Organization ()
        {
        }

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
        ///     Gets the organization addresses.
        /// </summary>
        /// <value>
        ///     The organization addresses.
        /// </value>
        public IEnumerable<OrganizationAddress> OrganizationAddresses
        {
            get { return _organizationAddresses; }
        }

        /// <summary>
        ///     Gets the organization phones.
        /// </summary>
        /// <value>
        ///     The organization phones.
        /// </value>
        public IEnumerable<OrganizationPhone> OrganizationPhones
        {
            get { return _organizationPhones; }
        }

        public IEnumerable<Guid> AssessmentDefinitions {
            get { return _assessmentDefinitions; }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Adds the address.
        /// </summary>
        /// <param name="organizationAddress">The organization address.</param>
        public void AddAddress ( OrganizationAddress organizationAddress )
        {
            RaiseEvent(new OrganizationAddressAddedEvent(Key, Version, organizationAddress));
            if (organizationAddress.IsPrimary)
            {
                MakePrimary(organizationAddress);
            }
        }

        /// <summary>
        ///     Adds the phone.
        /// </summary>
        /// <param name="organizationPhone">The organization phone.</param>
        public void AddPhone ( OrganizationPhone organizationPhone )
        {
            RaiseEvent(new OrganizationPhoneAddedEvent(Key, Version, organizationPhone));
            if (organizationPhone.IsPrimary)
            {
                MakePrimary ( organizationPhone );
            }
        }

        public void MakePrimary ( OrganizationAddress organizationAddress )
        {
            Check.IsNotNull ( organizationAddress, "organizationAddress is required." );
            var currentPrimary = OrganizationAddresses.FirstOrDefault ( oa => oa.IsPrimary );
            if ( currentPrimary != organizationAddress )
            {
                RaiseEvent ( new OrganizaionPrimaryAddressChangedEvent ( Key, Version, organizationAddress.GetHashCode () ) );
            }
        }

        public void MakePrimary(OrganizationPhone organizationPhone)
        {
            Check.IsNotNull(organizationPhone, "organizationPhone is required.");
            var currentPrimary = OrganizationPhones.FirstOrDefault(oa => oa.IsPrimary);
            if (currentPrimary != organizationPhone)
            {
                RaiseEvent(new OrganizaionPrimaryPhoneChangedEvent(Key, Version, organizationPhone.GetHashCode()));
            }
        }

        /// <summary>
        ///     Removes the address.
        /// </summary>
        /// <param name="organizationAddress">The organization address.</param>
        public void RemoveAddress ( OrganizationAddress organizationAddress )
        {
            if ( _organizationAddresses.Contains ( organizationAddress ) )
            {
                RaiseEvent ( new OrganizationAddressRemovedEvent ( Key, Version, organizationAddress ) );
            }
        }

        /// <summary>
        ///     Removes the phone.
        /// </summary>
        /// <param name="organizationPhone">The organization phone.</param>
        public void RemovePhone ( OrganizationPhone organizationPhone )
        {
            if ( _organizationPhones.Contains ( organizationPhone ) )
            {
                RaiseEvent ( new OrganizationPhoneRemovedEvent ( Key, Version, organizationPhone ) );
            }
        }


        public void AddAssessmentDefinition( Guid assessmnetDefinitionKey)
        {
            RaiseEvent(new AssessmentDefinitionAddedEvent(Key, Version, assessmnetDefinitionKey));
        }

        public void RemoveAssessmentDefinition(Guid assessmentDefinitionKey)
        {
            if (_assessmentDefinitions.Contains(assessmentDefinitionKey))
            {
                RaiseEvent(new AssessmentDefinitionRemovedEvent(Key, Version, assessmentDefinitionKey));
            }
        }


        public void ReviseName ( string name )
        {
            Check.IsNotNullOrWhitespace ( name, () => Name );

            RaiseEvent ( new OrganizationNameRevisedEvent ( Key, Version, name ) );
        }

        #endregion

        #region Methods

        private void Apply ( OrganizaionPrimaryAddressChangedEvent organizaionPrimaryAddressChangedEvent )
        {
            foreach ( var organizationAddress in OrganizationAddresses )
            {
                organizationAddress.IsPrimary = organizationAddress.GetHashCode () == organizaionPrimaryAddressChangedEvent.AddressHashCode;
            }
        }

        private void Apply(OrganizaionPrimaryPhoneChangedEvent organizaionPrimaryPhoneChangedEvent)
        {
            foreach (var organizationPhone in OrganizationPhones)
            {
                organizationPhone.IsPrimary = organizationPhone.GetHashCode() == organizaionPrimaryPhoneChangedEvent.PhoneHashCode;
            }
        }

        private void Apply ( OrganizationNameRevisedEvent organizationNameRevisedEvent )
        {
            Name = organizationNameRevisedEvent.Name;
        }

        private void Apply ( OrganizationCreatedEvent organizationCreatedEvent )
        {
            Name = organizationCreatedEvent.Name;
        }

        private void Apply ( OrganizationAddressAddedEvent organizationAddressAddedEvent )
        {
            _organizationAddresses.Add ( organizationAddressAddedEvent.OrganizationAddress );
        }

        private void Apply ( OrganizationPhoneAddedEvent organizationPhoneAddedEvent )
        {
            _organizationPhones.Add ( organizationPhoneAddedEvent.OrganizationPhone );
        }

        private void Apply ( OrganizationAddressRemovedEvent organizationAddressRemovedEvent )
        {
            _organizationAddresses.Remove ( organizationAddressRemovedEvent.OrganizationAddress );
        }

        private void Apply ( OrganizationPhoneRemovedEvent organizationPhoneRemovedEvent )
        {
            _organizationPhones.Remove ( organizationPhoneRemovedEvent.OrganizationPhone );
        }

        private void Apply(AssessmentDefinitionAddedEvent assessmentDefinitionAddedEvent)
        {
            _assessmentDefinitions.Add(assessmentDefinitionAddedEvent.AssessmentDefinitionKey);
        }

        private void Apply(AssessmentDefinitionRemovedEvent assessmentDefinitionRemovedEvent)
        {
            _assessmentDefinitions.Remove(assessmentDefinitionRemovedEvent.AssessmentDefinitionKey);
        }

        #endregion
    }
}