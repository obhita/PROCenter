#region Licence Header
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
    using System.Linq;
    using CommonModule;
    using Event;
    using Pillar.Common.Utility;
    using Pillar.FluentRuleEngine;

    #endregion

    /// <summary>
    ///     Instance of an assessment.
    /// </summary>
    public class AssessmentInstance : AggregateRootBase
    {
        #region Fields

        private readonly List<ItemInstance> _itemInstances = new List<ItemInstance> ();

        #endregion

        #region Constructors and Destructors

        public AssessmentInstance ( Guid assessmentDefinitionKey, Guid patientKey, string assessmentName )
        {
            Key = CombGuid.NewCombGuid ();
            RaiseEvent ( new AssessmentCreatedEvent ( Key, Version, patientKey, assessmentDefinitionKey, assessmentName ) );
        }

        public AssessmentInstance ()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the assessment definition key.
        /// </summary>
        /// <value>
        ///     The assessment definition key.
        /// </value>
        public Guid AssessmentDefinitionKey { get; private set; }

        /// <summary>
        ///     Gets a value indicating whether this instance is submitted.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is submitted; otherwise, <c>false</c>.
        /// </value>
        public bool IsSubmitted { get; private set; }

        public Guid? WorkflowKey { get; private set; }

        /// <summary>
        ///     Gets the item instances.
        /// </summary>
        /// <value>
        ///     The item instances.
        /// </value>
        public virtual IEnumerable<ItemInstance> ItemInstances
        {
            get { return _itemInstances; }
        }

        /// <summary>
        /// Gets the name of the assessment.
        /// </summary>
        /// <value>
        /// The name of the assessment.
        /// </value>
        public string AssessmentName { get; private set; }

        /// <summary>
        ///     Gets the patient key.
        /// </summary>
        /// <value>
        ///     The patient key.
        /// </value>
        public Guid PatientKey { get; private set; }

        /// <summary>
        /// Gets the score.
        /// </summary>
        /// <value>
        /// The score.
        /// </value>
        public Score Score { get; private set; }

        /// <summary>
        /// Gets the percent complete.
        /// </summary>
        /// <value>
        /// The percent complete.
        /// </value>
        public double PercentComplete { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance can self administer.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance can self administer; otherwise, <c>false</c>.
        /// </value>
        public bool CanSelfAdminister { get; private set; }

        #endregion

        #region Public Methods and Operators

        public void UpdateItem ( string itemDefinitionCode, object value )
        {
            //TODO: Figure out how to make sure item definition is valid for this assessment.
            //if ( !AssessmentDefinition.ItemDefinitions.Contains ( itemDefinition ) )
            //{
            //    throw new ArgumentException("Assessment Definition does not contain the item definition.","itemDefinition");
            //}
            var itemUpdatedEvent = new ItemUpdatedEvent ( Key, Version, itemDefinitionCode, value );
            AssessmentRuleEngineExecutor.CreateRuleEngineExecutor ( this )
                .ForItemDefinitionCode ( itemDefinitionCode )
                .WithContext ( itemUpdatedEvent )
                .Execute( () => RaiseEvent ( itemUpdatedEvent ) );
        }

        public void Submit()
        {
            RaiseEvent(new AssessmentSubmittedEvent(Key, Version, true));
        }

        public void Unsubmit()
        {
            RaiseEvent(new AssessmentSubmittedEvent(Key, Version, false));
        }

        public void AddToWorkflow(Guid workflowKey)
        {
            RaiseEvent(new AssessmentAddedToWorkflowEvent(Key, Version, workflowKey));
        }

        public void UpdateScoreItem(string itemDefinitionCode, object value)
        {
            RaiseEvent(new ItemScoredEvent(Key, Version, itemDefinitionCode, value));
        }

        public void ScoreComplete(CodedConcept scoreCode, object value, CodedConcept guidance = null)
        {
            RaiseEvent(new AssessmentScoredEvent(Key, Version, scoreCode, value, guidance));
        }

        public void UpdatePercentComplete(double percentComplete)
        {
            RaiseEvent(new PercentCompleteUpdatedEvent(Key, Version, percentComplete));
        }

        public void AllowSelfAdministration ()
        {
            RaiseEvent ( new AssessmentCanBeSelfAdministeredEvent ( Key, Version ) );
        }

        #endregion

        #region Methods

        private void Apply ( AssessmentCanBeSelfAdministeredEvent assessmentCanBeSelfAdministeredEvent )
        {
            CanSelfAdminister = true;
        }

        private void Apply ( AssessmentCreatedEvent assessmentCreatedEvent )
        {
            PatientKey = assessmentCreatedEvent.PatientKey;
            AssessmentDefinitionKey = assessmentCreatedEvent.AssessmentDefinitionKey;
            AssessmentName = assessmentCreatedEvent.AssessmentName;
        }

        private void Apply ( ItemUpdatedEvent itemUpdatedEvent )
        {
            var itemInstance = ItemInstances.FirstOrDefault ( ii => ii.ItemDefinitionCode == itemUpdatedEvent.ItemDefinitionCode );
            if ( itemInstance == null )
            {
                _itemInstances.Add ( new ItemInstance ( itemUpdatedEvent.ItemDefinitionCode, itemUpdatedEvent.Value ) );
            }
            else
            {
                itemInstance.Apply ( itemUpdatedEvent );
            }
        }

        private void Apply(AssessmentSubmittedEvent assessmentSubmittedEvent)
        {
            IsSubmitted = assessmentSubmittedEvent.Submit;
            if (!IsSubmitted)
            {
                Score = null;
            }
        }

        private void Apply(AssessmentScoredEvent assessmentScoredEvent)
        {
            Score = new Score(assessmentScoredEvent.ScoreCode)
                {
                    Value = assessmentScoredEvent.Value,
                    Guidance = assessmentScoredEvent.Guidance
                };
        }

        private void Apply(ItemScoredEvent itemScoredEvent)
        {
            var scoreItem = Score.ScoreItems.FirstOrDefault(ii => ii.ItemDefinitionCode == itemScoredEvent.ItemDefinitionCode);
            if (scoreItem == null)
            {
                Score.AddScoreItem(new ScoreItem(itemScoredEvent.ItemDefinitionCode, itemScoredEvent.Value));
            }
            else
            {
                scoreItem.UpdateValue ( itemScoredEvent.Value );
            }
        }

        private void Apply(AssessmentAddedToWorkflowEvent assessmentAddedToWorkflowEvent)
        {
            WorkflowKey = assessmentAddedToWorkflowEvent.WorkflowKey;
        }

        private void Apply(PercentCompleteUpdatedEvent percentCompleteUpdatedEvent)
        {
            PercentComplete = percentCompleteUpdatedEvent.PercentComplete;
        }
        #endregion
    }
}