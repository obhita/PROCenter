namespace ProCenter.Domain.AssessmentModule
{
    using System.Collections.Generic;

    using ProCenter.Domain.AssessmentModule.Lookups;
    using ProCenter.Domain.CommonModule;

    /// <summary>The group class.</summary>
    public class Group : AssessmentPart, IItemDefinitionProvider
    {
        /// <summary>Initializes a new instance of the <see cref="Group"/> class.</summary>
        /// <param name="assessmentInstance">The assessment instance.</param>
        public Group ( AssessmentInstance assessmentInstance )
            : base ( assessmentInstance )
        {
        }

        /// <summary>Creates the item definition.</summary>
        /// <param name="codedConcept">The coded concept.</param>
        /// <returns>A <see cref="ItemDefinition" />.</returns>
        public ItemDefinition CreateItemDefinition (CodedConcept codedConcept)
        {
            var itemDefinitions = new List<ItemDefinition> ();
            foreach (var propertyInfo in GetOrderedProperties ())
            {
                if (typeof(IItemDefinitionProvider).IsAssignableFrom(propertyInfo.PropertyType))
                {
                    var itemDefinitionProvider = propertyInfo.GetValue(this) as IItemDefinitionProvider;
                    itemDefinitions.Add(itemDefinitionProvider.CreateItemDefinition(GetCodedConcept(propertyInfo, codedConcept.CodeSystem)));
                }
                else
                {
                    itemDefinitions.Add(CreateItemDefinitionForQuestionProperty(propertyInfo, codedConcept.CodeSystem));
                }
            }
            return new ItemDefinition ( codedConcept, ItemType.Group, null, null, itemDefinitions );
        }
    }
}
