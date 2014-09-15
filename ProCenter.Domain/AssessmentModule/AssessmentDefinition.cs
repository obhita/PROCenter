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

namespace ProCenter.Domain.AssessmentModule
{
    #region Using Statements

    using System;
    using System.Collections.Generic;

    using Pillar.Common.Utility;

    using ProCenter.Domain.AssessmentModule.Event;
    using ProCenter.Domain.AssessmentModule.Lookups;
    using ProCenter.Domain.CommonModule;

    #endregion

    /// <summary>Class for defining assessment definition.</summary>
    public class AssessmentDefinition : AggregateRootBase, IMemento, IContainItemDefinitions
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AssessmentDefinition" /> class.
        /// </summary>
        /// <param name="codedConcept">The coded concept.</param>
        /// <param name="scoreType">Type of the score.</param>
        public AssessmentDefinition ( CodedConcept codedConcept, ScoreTypeEnum scoreType )
            : this ()
        {
            Check.IsNotNull ( codedConcept, () => CodedConcept );
            Check.IsNotNull ( scoreType, () => ScoreType );

            Key = CombGuid.NewCombGuid ();

            RaiseEvent ( new AssessmentDefinitionCreatedEvent ( Key, Version, codedConcept, scoreType ) );
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AssessmentDefinition" /> class.
        /// </summary>
        public AssessmentDefinition ()
        {
            ItemDefinitions = new List<ItemDefinition> ();
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
        ///     Gets the question count.
        /// </summary>
        /// <value>
        ///     The question count.
        /// </value>
        public int QuestionCount { get; private set; }

        /// <summary>
        /// Gets the type of the score.
        /// </summary>
        /// <value>
        /// The type of the score.
        /// </value>
        public ScoreTypeEnum ScoreType { get; private set; }

        #endregion

        #region Explicit Interface Properties

        Guid IMemento.Key
        {
            get { return Key; }
            set { Key = value; }
        }

        int IMemento.Version
        {
            get { return Version; }
            set { Version = value; }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>Gets all item definitions of type in container.</summary>
        /// <param name="itemContainer">The item container.</param>
        /// <param name="itemType">Type of the item.</param>
        /// <returns>
        ///     A <see cref="IEnumerable{ItemDefinition}" />.
        /// </returns>
        public static IEnumerable<ItemDefinition> GetAllItemDefinitionsOfTypeInContainer ( IContainItemDefinitions itemContainer, ItemType itemType )
        {
            var itemDefinitions = new List<ItemDefinition> ();
            RecurseItems (
                itemContainer,
                itemDefinition =>
                    {
                        if ( itemDefinition.ItemType == itemType )
                        {
                            itemDefinitions.Add ( itemDefinition );
                        }
                        return false;
                    } );
            return itemDefinitions;
        }

        /// <summary>Adds the item definition.</summary>
        /// <param name="itemDefinition">The item definition.</param>
        public virtual void AddItemDefinition ( ItemDefinition itemDefinition )
        {
            RaiseEvent ( new ItemDefinitionAddedEvent ( Key, Version, itemDefinition ) );
        }

        /// <summary>Gets all item definitions of type.</summary>
        /// <param name="itemType">Type of the item.</param>
        /// <returns>I enumerable{ item definition}.</returns>
        public virtual IEnumerable<ItemDefinition> GetAllItemDefinitionsOfType ( ItemType itemType )
        {
            return GetAllItemDefinitionsOfTypeInContainer ( this, itemType );
        }

        /// <summary>Gets the containing section.</summary>
        /// <param name="itemDefinition">The item definition.</param>
        /// <returns>
        ///     A <see cref="IContainItemDefinitions" />.
        /// </returns>
        public virtual IContainItemDefinitions GetContainingSection ( ItemDefinition itemDefinition )
        {
            if ( itemDefinition.ItemType == ItemType.Section )
            {
                return itemDefinition;
            }
            IContainItemDefinitions currentContainer = this;
            RecurseItems (
                this,
                item =>
                    {
                        if ( item.ItemType == ItemType.Section )
                        {
                            currentContainer = item;
                        }
                        return item == itemDefinition;
                    } );
            return currentContainer;
        }

        /// <summary>Gets the item definition by code.</summary>
        /// <param name="itemDefinitionCode">The item definition code.</param>
        /// <returns>
        ///     A <see cref="ItemDefinition" />.
        /// </returns>
        public virtual ItemDefinition GetItemDefinitionByCode ( string itemDefinitionCode )
        {
            ItemDefinition itemDefinition = null;
            RecurseItems (
                this,
                def =>
                    {
                        if ( def.CodedConcept.Code == itemDefinitionCode )
                        {
                            itemDefinition = def;
                            return true;
                        }
                        return false;
                    } );
            return itemDefinition;
        }

        #endregion

        #region Methods

        /// <summary>Applies the specified assessment definition created event.</summary>
        /// <param name="assessmentDefinitionCreatedEvent">The assessment definition created event.</param>
        protected virtual void Apply ( AssessmentDefinitionCreatedEvent assessmentDefinitionCreatedEvent )
        {
            CodedConcept = assessmentDefinitionCreatedEvent.CodedConcept;
            ScoreType = assessmentDefinitionCreatedEvent.ScoreType;
        }

        /// <summary>Applies the specified item definition added event.</summary>
        /// <param name="itemDefinitionAddedEvent">The item definition added event.</param>
        protected virtual void Apply ( ItemDefinitionAddedEvent itemDefinitionAddedEvent )
        {
            if ( itemDefinitionAddedEvent.ItemDefinition.ItemType == ItemType.Question )
            {
                QuestionCount++;
            }
            else
            {
                RecurseItems (
                    itemDefinitionAddedEvent.ItemDefinition,
                    def =>
                        {
                            if ( def.ItemType == ItemType.Question )
                            {
                                QuestionCount++;
                            }
                            return false;
                        } );
            }
            ( ItemDefinitions as List<ItemDefinition> ).Add ( itemDefinitionAddedEvent.ItemDefinition );
        }

        /// <summary>Restores the snapshot.</summary>
        /// <param name="memento">The memento.</param>
        protected override void RestoreSnapshot ( IMemento memento )
        {
            base.RestoreSnapshot ( memento );
            var assessmentDefinition = memento as AssessmentDefinition;
            if ( assessmentDefinition != null )
            {
                CodedConcept = assessmentDefinition.CodedConcept;
                ScoreType = assessmentDefinition.ScoreType;
                ItemDefinitions = assessmentDefinition.ItemDefinitions;
                QuestionCount = assessmentDefinition.QuestionCount;
            }
        }

        private static bool RecurseItems ( IContainItemDefinitions itemDefinitionContainer, Func<ItemDefinition, bool> checkStop )
        {
            var shouldContinue = true;
            foreach ( var definition in itemDefinitionContainer.ItemDefinitions )
            {
                if ( !shouldContinue || checkStop ( definition ) )
                {
                    return false;
                }
                if ( definition.ItemType == ItemType.Section || definition.ItemType == ItemType.Group )
                {
                    shouldContinue = RecurseItems ( definition, checkStop );
                }
            }
            return shouldContinue;
        }

        #endregion
    }
}