namespace ProCenter.Mvc.Infrastructure.Service.Completeness
{
    #region

    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    #endregion

    public class CompletenessModelValidator : ModelValidator
    {
        private readonly string _completenessCategory;

        public CompletenessModelValidator(ModelMetadata metadata, ControllerContext controllerContext, string completenessCategory) : base(metadata, controllerContext)
        {
            _completenessCategory = completenessCategory;
        }

        public override IEnumerable<ModelValidationResult> Validate(object container)
        {
            return Enumerable.Empty<ModelValidationResult>();
        }

        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
        {
            var rule = new ModelClientValidationRule
                {
                    ValidationType = ValidationType.Completeness,
                };
            rule.ValidationParameters.Add(new KeyValuePair<string, object>(_completenessCategory.ToLower(), null));
            return new List<ModelClientValidationRule> {rule};
        }
    }
}