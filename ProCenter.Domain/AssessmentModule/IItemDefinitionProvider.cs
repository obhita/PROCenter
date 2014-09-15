namespace ProCenter.Domain.AssessmentModule
{
    using ProCenter.Domain.CommonModule;

    /// <summary>Interface for item definition provider.</summary>
    public interface IItemDefinitionProvider
    {
        /// <summary>Creates the item definition.</summary>
        /// <param name="codedConcept">The coded concept.</param>
        /// <returns>A <see cref="ItemDefinition" />.</returns>
        ItemDefinition CreateItemDefinition (CodedConcept codedConcept);
    }
}