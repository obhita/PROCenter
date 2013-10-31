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
    using System.Linq;
    using System.Collections.Generic;
    using CommonModule;
    using Event;
    using Lookups;
    using Pillar.Common.Utility;

    #endregion

    /// <summary>
    ///     Class for defining assessment definition.
    /// </summary>
    public class AssessmentDefinition : AggregateRootBase, IMemento, IContainItemDefinitions
    {
        public AssessmentDefinition (CodedConcept codedConcept) : this()
        {
            Check.IsNotNull ( codedConcept, () => CodedConcept );

            Key = CombGuid.NewCombGuid ();

            RaiseEvent ( new AssessmentDefinitionCreatedEvent(Key, Version, codedConcept) );
        }

        public AssessmentDefinition ()
        {
            ItemDefinitions = new List<ItemDefinition> ();
        }

        #region Public Properties

        /// <summary>
        /// Gets the coded concept.
        /// </summary>
        /// <value>
        /// The coded concept.
        /// </value>
        public CodedConcept CodedConcept { get; private set; }

        /// <summary>
        /// Gets the item definitions.
        /// </summary>
        /// <value>
        /// The item definitions.
        /// </value>
        public IEnumerable<ItemDefinition> ItemDefinitions { get; private set; }

        public int QuestionCount { get; private set; }

        #endregion

        protected void Apply ( AssessmentDefinitionCreatedEvent assessmentDefinitionCreatedEvent )
        {
            CodedConcept = assessmentDefinitionCreatedEvent.CodedConcept;
        }

        public void AddItemDefinition ( ItemDefinition itemDefinition )
        {
            RaiseEvent(new ItemDefinitionAddedEvent(Key, Version, itemDefinition));
        }

        protected void Apply ( ItemDefinitionAddedEvent itemDefinitionAddedEvent )
        {
            if ( itemDefinitionAddedEvent.ItemDefinition.ItemType == ItemType.Question )
            {
                QuestionCount++;
            }
            else
            {
                RecurseItems ( itemDefinitionAddedEvent.ItemDefinition,
                               def =>
                                   {
                                       if ( def.ItemType == ItemType.Question )
                                       {
                                           QuestionCount++;
                                       }
                                       return false;
                                   } );
            }
            (ItemDefinitions as List<ItemDefinition> ).Add ( itemDefinitionAddedEvent.ItemDefinition );
        }

        protected override void RestoreSnapshot(IMemento memento)
        {
            base.RestoreSnapshot(memento);
            var assessmentDefinition = memento as AssessmentDefinition;
            if ( assessmentDefinition != null )
            {
                CodedConcept = assessmentDefinition.CodedConcept;
                ItemDefinitions = assessmentDefinition.ItemDefinitions;
                QuestionCount = assessmentDefinition.QuestionCount;
            }
        }

        public ItemDefinition GetItemDefinitionByCode ( string itemDefinitionCode )
        {
            ItemDefinition itemDefinition = null;
            RecurseItems(this, def =>
                {
                    if ( def.CodedConcept.Code == itemDefinitionCode )
                    {
                        itemDefinition = def;
                        return true;
                    }
                    return false;
                });
            return itemDefinition;
        }

        private static bool RecurseItems ( IContainItemDefinitions itemDefinitionContainer, Func<ItemDefinition,bool> checkStop )
        {
            var shouldContinue = true;
            foreach (var definition in itemDefinitionContainer.ItemDefinitions)
            {
                if ( !shouldContinue || checkStop ( definition ) )
                {
                    return false;
                }
                if ( definition.ItemType == ItemType.Section || definition.ItemType == ItemType.Group )
                {
                    shouldContinue = RecurseItems(definition, checkStop);
                }
            }
            return true;
        }

        Guid IMemento.Key
        {
            get { return base.Key; }
            set { base.Key = value; }
        }

        int IMemento.Version
        {
            get { return base.Version; }
            set { base.Version = value; }
        }
    }
}