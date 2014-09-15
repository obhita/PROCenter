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
    using System.Linq;

    using Pillar.Common.Utility;
    using Pillar.Domain.Event;
    using Pillar.Domain.FluentRuleEngine.Event;
    using Pillar.FluentRuleEngine;

    using ProCenter.Common;
    using ProCenter.Domain.AssessmentModule.Event;
    using ProCenter.Domain.AssessmentModule.Lookups;
    using ProCenter.Domain.AssessmentModule.Rules;
    using ProCenter.Domain.CommonModule;

    #endregion

    /// <summary>Instance of an assessment.</summary>
    public class AssessmentInstance : AggregateRootBase
    {
        #region Fields

        private readonly List<ItemInstance> _itemInstances = new List<ItemInstance> ();

        private RuleEngineExecutor<AssessmentInstance> _cachedRuleEngineExecutor;

        private int _requiredQuestionTotal;

        private List<ItemDefinition> _skippedItemDefinitions = new List<ItemDefinition> ();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AssessmentInstance" /> class.
        /// </summary>
        /// <param name="assessmentDefinition">The assessment definition.</param>
        /// <param name="patientKey">The patient key.</param>
        /// <param name="assessmentName">Name of the assessment.</param>
        /// <param name="canSelfAdminister">If set to <c>true</c> [can self administer].</param>
        internal AssessmentInstance ( AssessmentDefinition assessmentDefinition, Guid patientKey, string assessmentName, bool canSelfAdminister = false)
        {
            Key = CombGuid.NewCombGuid ();
            var itemDefinitions = assessmentDefinition.GetAllItemDefinitionsOfType ( ItemType.Question ).ToList ();
            var total = itemDefinitions.Count ( item => item.GetIsRequired () );
            Guid? staffKey = null;
            if ( UserContext.Current.StaffKey != null )
            {
                staffKey = UserContext.Current.StaffKey.Value;
            }
            RaiseEvent (
                new AssessmentCreatedEvent (
                    Key,
                    Version,
                    patientKey,
                    staffKey,
                    assessmentDefinition.Key,
                    assessmentName,
                    total,
                    DateTime.Now,
                    canSelfAdminister));
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AssessmentInstance" /> class.
        /// </summary>
        protected AssessmentInstance ()
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
        public virtual Guid AssessmentDefinitionKey { get; private set; }

        /// <summary>
        ///     Gets the name of the assessment.
        /// </summary>
        /// <value>
        ///     The name of the assessment.
        /// </value>
        public virtual string AssessmentName { get; private set; }

        /// <summary>
        ///     Gets a value indicating whether this instance can self administer.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance can self administer; otherwise, <c>false</c>.
        /// </value>
        public virtual bool CanSelfAdminister { get; private set; }

        /// <summary>
        ///     Gets the created by staff key.
        /// </summary>
        /// <value>
        ///     The created by staff key.
        /// </value>
        public virtual Guid? CreatedByStaffKey { get; private set; }

        /// <summary>
        ///     Gets the created date.
        /// </summary>
        /// <value>
        ///     The created date.
        /// </value>
        public virtual DateTime CreatedDate { get; private set; }

        /// <summary>
        ///     Gets the notified patient date.
        /// </summary>
        /// <value>
        ///     The notified patient date.
        /// </value>
        public virtual DateTime? EmailSentDate { get; private set; }

        /// <summary>
        /// Gets the email failed date.
        /// </summary>
        /// <value>
        /// The email failed date.
        /// </value>
        public virtual DateTime? EmailFailedDate { get; private set; }

        /// <summary>
        ///     Gets a value indicating whether this instance is submitted.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is submitted; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsSubmitted { get; private set; }

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
        ///     Gets the patient key.
        /// </summary>
        /// <value>
        ///     The patient key.
        /// </value>
        public virtual Guid PatientKey { get; private set; }

        /// <summary>
        ///     Gets the score.
        /// </summary>
        /// <value>
        ///     The score.
        /// </value>
        public virtual Score Score { get; private set; }

        /// <summary>Gets the skipped item definitions.</summary>
        /// <value>The skipped item definitions.</value>
        public virtual IEnumerable<ItemDefinition> SkippedItemDefinitions
        {
            get { return _skippedItemDefinitions; }
        }

        /// <summary>
        ///     Gets the submitted date.
        /// </summary>
        /// <value>
        ///     The submitted date.
        /// </value>
        public virtual DateTime SubmittedDate { get; private set; }

        /// <summary>
        ///     Gets the workflow key.
        /// </summary>
        /// <value>
        ///     The workflow key.
        /// </value>
        public virtual Guid? WorkflowKey { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>Adds to workflow.</summary>
        /// <param name="workflowKey">The workflow key.</param>
        public virtual void AddToWorkflow ( Guid workflowKey )
        {
            RaiseEvent ( new AssessmentAddedToWorkflowEvent ( Key, Version, workflowKey ) );
        }

        /// <summary>Allows the self administration.</summary>
        public virtual void AllowSelfAdministration ()
        {
            RaiseEvent ( new AssessmentCanBeSelfAdministeredEvent ( Key, Version ) );
        }

        /// <summary>Calculates the completeness.</summary>
        /// <returns>
        ///     A <see cref="CompletenessResults" />.
        /// </returns>
        public CompletenessResults CalculateCompleteness ()
        {
            return new CompletenessResults ( "Report", _requiredQuestionTotal - _skippedItemDefinitions.Count (), _itemInstances.Count ( i => i.IsRequired ) );
        }

        /// <summary>
        ///     Scores the complete.
        /// </summary>
        /// <param name="scoreCode">The score code.</param>
        /// <param name="value">The value.</param>
        /// <param name="hasReport">
        ///     If set to <c>True</c> has report.
        /// </param>
        /// <param name="guidance">The guidance.</param>
        public virtual void ScoreComplete ( CodedConcept scoreCode, object value, bool hasReport = false, CodedConcept guidance = null )
        {
            RaiseEvent ( new AssessmentScoredEvent ( Key, Version, scoreCode, value, hasReport, guidance ) );
        }

        /// <summary>
        /// Updates the email sent date.
        /// </summary>
        /// <param name="emailSentDate">The email sent date.</param>
        /// <param name="emailFailedDate">The email failed date.</param>
        public virtual void UpdateEmailSentDate ( DateTime? emailSentDate, DateTime? emailFailedDate )
        {
            RaiseEvent ( new UpdateEmailSentDateEvent ( Key, Version, emailSentDate, emailFailedDate ) );
        }

        /// <summary>Submits this instance.</summary>
        public virtual void Submit ()
        {
            if ( CalculateCompleteness ().IsComplete )
            {
                RaiseEvent ( new AssessmentSubmittedEvent ( Key, Version, AssessmentDefinitionKey, true ) );
            }
            else
            {
                DomainEvent.Raise (
                    new RuleViolationEvent
                        {
                            RuleViolations = new List<RuleViolation>
                                                 {
                                                     new RuleViolation ( null, this, "You cannot submit an even that is not complete." )
                                                 }
                        } );
            }
        }

        /// <summary>Unsubmits this instance.</summary>
        public virtual void Unsubmit ()
        {
            RaiseEvent ( new AssessmentSubmittedEvent ( Key, Version, AssessmentDefinitionKey, false ) );
        }

        /// <summary>Updates the item.</summary>
        /// <param name="itemDefinition">The item definition.</param>
        /// <param name="value">The value.</param>
        public virtual void UpdateItem ( ItemDefinition itemDefinition, object value )
        {
            //TODO: Figure out how to make sure item definition is valid for this assessment.

            var itemUpdatedEvent = new ItemUpdatedEvent ( Key, Version, itemDefinition.CodedConcept.Code, value, itemDefinition.GetIsRequired () );
            AssessmentRuleEngineExecutor.CreateRuleEngineExecutor ( this )
                                        .ForItemInstance ( new ItemInstance ( itemDefinition.CodedConcept.Code, value, itemDefinition.GetIsRequired () ) )
                                        .Execute ( () => RaiseEvent ( itemUpdatedEvent ) );
        }

        /// <summary>Updates the score item.</summary>
        /// <param name="itemDefinitionCode">The item definition code.</param>
        /// <param name="value">The value.</param>
        public virtual void UpdateScoreItem ( string itemDefinitionCode, object value )
        {
            RaiseEvent ( new ItemScoredEvent ( Key, Version, itemDefinitionCode, value ) );
        }

        #endregion

        #region Methods

        private void Apply ( UpdateEmailSentDateEvent updateEmailSentDateEvent )
        {
            EmailSentDate = updateEmailSentDateEvent.EmailSentDate;
            EmailFailedDate = updateEmailSentDateEvent.EmailFailedDate;
        }

        private void Apply ( AssessmentCanBeSelfAdministeredEvent assessmentCanBeSelfAdministeredEvent )
        {
            CanSelfAdminister = true;
        }

        private void Apply ( AssessmentCreatedEvent assessmentCreatedEvent )
        {
            PatientKey = assessmentCreatedEvent.PatientKey;
            AssessmentDefinitionKey = assessmentCreatedEvent.AssessmentDefinitionKey;
            AssessmentName = assessmentCreatedEvent.AssessmentName;
            _requiredQuestionTotal = assessmentCreatedEvent.RequiredQuestionTotal;
            CreatedByStaffKey = assessmentCreatedEvent.StaffKey;
            CreatedDate = assessmentCreatedEvent.CreatedDate;
            CanSelfAdminister = assessmentCreatedEvent.CanSelfAdminister;
            InitializeSkipping ();
        }

        private void Apply ( ItemUpdatedEvent itemUpdatedEvent )
        {
            var itemInstance = ItemInstances.FirstOrDefault ( ii => ii.ItemDefinitionCode == itemUpdatedEvent.ItemDefinitionCode );
            var isNull = itemUpdatedEvent.Value == null || string.IsNullOrEmpty ( itemUpdatedEvent.Value.ToString () );
            if ( itemInstance != null && isNull )
            {
                _itemInstances.Remove ( itemInstance );
            }

            if ( itemInstance == null )
            {
                itemInstance = new ItemInstance ( itemUpdatedEvent.ItemDefinitionCode, itemUpdatedEvent.Value, itemUpdatedEvent.IsRequired );
                if ( !isNull )
                {
                    _itemInstances.Add ( itemInstance );
                }
            }
            else
            {
                itemInstance.Apply ( itemUpdatedEvent );
            }

            var skippingContext = new SkippingContext ();
            if ( _cachedRuleEngineExecutor == null )
            {
                var assessmentRuleCollectionFactory = new AssessmentRuleCollectionFactory ();
                _cachedRuleEngineExecutor = new RuleEngineExecutor<AssessmentInstance> (
                    this,
                    assessmentRuleCollectionFactory.CreateRuleCollection ( this.AssessmentName ) );
            }
            _cachedRuleEngineExecutor
                .ForRuleSet ( "ItemUpdatedRuleSet" + itemInstance.ItemDefinitionCode )
                .WithContext ( itemInstance, itemInstance.ItemDefinitionCode )
                .WithContext ( skippingContext )
                .Execute ();

            skippingContext.SkippedItemDefinitions.ForEach (
                item =>
                    {
                        if ( item.GetIsRequired () && !_skippedItemDefinitions.Contains ( item ) )
                        {
                            _skippedItemDefinitions.Add ( item );
                        }
                        if ( ItemInstances.Any ( i => i.ItemDefinitionCode == item.CodedConcept.Code ) )
                        {
                            UpdateItem ( item, null );
                        }
                    } );
            skippingContext.UnSkippedItemDefinitions.ForEach ( item => _skippedItemDefinitions.Remove ( item ) );
        }

        private void Apply ( AssessmentSubmittedEvent assessmentSubmittedEvent )
        {
            IsSubmitted = assessmentSubmittedEvent.Submit;
            if ( !IsSubmitted )
            {
                Score = null;
            }
            SubmittedDate = DateTime.Now;
        }

        private void Apply ( AssessmentScoredEvent assessmentScoredEvent )
        {
            Score = new Score ( assessmentScoredEvent.ScoreCode )
                        {
                            Value = assessmentScoredEvent.Value,
                            Guidance = assessmentScoredEvent.Guidance,
                            HasReport = assessmentScoredEvent.HasReport
                        };
        }

        private void Apply ( ItemScoredEvent itemScoredEvent )
        {
            var scoreItem = Score.ScoreItems.FirstOrDefault ( ii => ii.ItemDefinitionCode == itemScoredEvent.ItemDefinitionCode );
            if ( scoreItem == null )
            {
                Score.AddScoreItem ( new ScoreItem ( itemScoredEvent.ItemDefinitionCode, itemScoredEvent.Value ) );
            }
            else
            {
                scoreItem.UpdateValue ( itemScoredEvent.Value );
            }
        }

        private void Apply ( AssessmentAddedToWorkflowEvent assessmentAddedToWorkflowEvent )
        {
            WorkflowKey = assessmentAddedToWorkflowEvent.WorkflowKey;
        }

        private void InitializeSkipping ()
        {
            var ruleCollection = new AssessmentRuleCollectionFactory ().CreateRuleCollection ( AssessmentName );
            _skippedItemDefinitions = ruleCollection.ItemSkippingRules.SelectMany ( r => r.SkippedItemDefinitions ).Where ( i => i.GetIsRequired () ).ToList ();
        }

        #endregion
    }
}