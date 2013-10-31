namespace ProCenter.Mvc.Infrastructure.Service.Completeness
{
    #region

    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    #endregion

    public class RequiredForCompletenessAttribute : ValidationAttribute, IClientValidatable
    {
        public string CompletenessCategory { get; private set; }

        public RequiredForCompletenessAttribute(string completenessCategory)
        {
            CompletenessCategory = completenessCategory;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule {ValidationType = ValidationType.Completeness};
            rule.ValidationParameters.Add(new KeyValuePair<string, object>(CompletenessCategory, null));
            return new List<ModelClientValidationRule> {rule};
        }
    }
}