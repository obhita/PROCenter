namespace ProCenter.Service.Handler.Assessment
{
    #region Using Statements

    using System.Linq;
    using Common;
    using Domain.AssessmentModule;
    using Domain.CommonModule.Lookups;
    using Infrastructure.Service.Completeness;
    using Service.Message.Assessment;
    using Service.Message.Common.Lookups;

    #endregion

    internal class SaveAssessmentItemRequestHandler :
        ServiceRequestHandler<SaveAssessmentItemRequest, SaveAssessmentItemResponse>
    {
        private readonly IAssessmentDefinitionRepository _assessmentDefinitionRepository;
        private readonly IAssessmentInstanceRepository _assessmentInstanceRepository;
        private readonly IAssessmentCompletenessManager _assessmentCompletenessManager;

        #region Fields


        #endregion

        #region Constructors and Destructors

        public SaveAssessmentItemRequestHandler(IAssessmentDefinitionRepository assessmentDefinitionRepository,
            IAssessmentInstanceRepository assessmentInstanceRepository,
            IAssessmentCompletenessManager assessmentCompletenessManager)
        {
            _assessmentDefinitionRepository = assessmentDefinitionRepository;
            _assessmentInstanceRepository = assessmentInstanceRepository;
            _assessmentCompletenessManager = assessmentCompletenessManager;
        }

        #endregion

        #region Methods

        protected override void Handle(SaveAssessmentItemRequest request, SaveAssessmentItemResponse response)
        {
            var assessmentInstance = _assessmentInstanceRepository.GetByKey( request.Key );
            var assessmentDefinition = _assessmentDefinitionRepository.GetByKey ( assessmentInstance.AssessmentDefinitionKey );

            var item = assessmentInstance.ItemInstances.FirstOrDefault ( i => i.ItemDefinitionCode == request.Item.ItemDefinitionCode );
            var value = request.Item.Value;
            if ( value is LookupDto )
            {
                var lookupDto = value as LookupDto;
                if ( string.IsNullOrWhiteSpace ( lookupDto.Code ) )
                {
                    value = null;
                }
                else
                {
                    var itemDefinition = assessmentDefinition.GetItemDefinitionByCode ( request.Item.ItemDefinitionCode );
                    value = itemDefinition.Options.Single ( l => l.CodedConcept.Code == lookupDto.Code );
                }
            }
            var hasItemUpdated = HasItemUpdated ( item, value );
            if ( hasItemUpdated )
            {
                if ( value != null )
                {
                    value = string.IsNullOrEmpty ( value.ToString () ) ? null : value;
                }
                assessmentInstance.UpdateItem ( request.Item.ItemDefinitionCode, value );
            }

            var completenessResults = _assessmentCompletenessManager.CalculateCompleteness(CompletenessCategory.Report, assessmentInstance, assessmentDefinition);
            assessmentInstance.UpdatePercentComplete(completenessResults.PercentComplete);
            response.CanSubmit = !assessmentInstance.IsSubmitted && completenessResults.NumberIncomplete == 0;
        }

        private static bool HasItemUpdated ( ItemInstance item, object value )
        {
            if ( item == null || item.Value == null )
            {
                if ( value is Lookup )
                {
                    return true;
                }
                if ( value == null )
                {
                    return false;
                }
                return !string.IsNullOrEmpty ( value.ToString () );
            }
            if ( value == null || string.IsNullOrEmpty ( value.ToString () ) )
            {
                return true;
            }
            if ( item.Value is Lookup )
            {
                return !( item.Value as Lookup ).Equals ( value as Lookup );
            }
            return !Equals ( item.Value, value );
        }

        #endregion
    }
}