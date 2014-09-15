namespace ProCenter.Domain.Tests.AssessmentModule.Rules
{
    using System;

    using Pillar.Common.Specification;

    using ProCenter.Domain.AssessmentModule;
    using ProCenter.Domain.AssessmentModule.Lookups;
    using ProCenter.Domain.AssessmentModule.Rules;
    using ProCenter.Domain.CommonModule;

    public class TestAssessmentRuleCollection : AbstractAssessmentRuleCollection
    {
        public TestAssessmentRuleCollection ()
        {
            var skipItem = new ItemDefinition ( new CodedConcept ( new CodeSystem ( "1", "1", "Test" ), "2", "Test" ), ItemType.Question, null );
            
            NewItemSkippingRule ( () => EqualToTest )
                .ForItemInstance<int> ( "1" )
                .EqualTo ( 1 )
                .SkipItem ( skipItem );

            NewItemSkippingRule ( () => ExclusiveBetweenTest )
                .ForItemInstance<int> ( "1" )
                .ExclusiveBetween ( 1, 3 )
                .SkipItem ( skipItem );

            NewItemSkippingRule ( () => GreaterThenTest )
                .ForItemInstance<int> ( "1" )
                .GreaterThen ( 1 )
                .SkipItem ( skipItem );

            NewItemSkippingRule ( () => GreaterThenOrEqualToTest )
                .ForItemInstance<int> ( "1" )
                .GreaterThenOrEqualTo ( 1 )
                .SkipItem(skipItem);

            NewItemSkippingRule(() => InListTest)
                .ForItemInstance<int>("1")
                .InList(1,2,3)
                .SkipItem(skipItem);

            NewItemSkippingRule(() => InclusiveBetweenTest)
                .ForItemInstance<int>("1")
                .InclusiveBetween(1,3)
                .SkipItem(skipItem);

            NewItemSkippingRule(() => LessThenTest)
                .ForItemInstance<int>("1")
                .LessThen(1)
                .SkipItem(skipItem);

            NewItemSkippingRule(() => LessThenOrEqualToTest)
                .ForItemInstance<int>("1")
                .LessThenOrEqualTo(1)
                .SkipItem(skipItem);

            NewItemSkippingRule(() => MatchesRegexTest)
                .ForItemInstance<int>("1")
                .MatchesRegex("[1]")
                .SkipItem(skipItem);

            NewItemSkippingRule(() => MeetsSpecificationTest)
                .ForItemInstance<int>("1")
                .MeetsSpecification ( new SpecificationTest () )
                .SkipItem(skipItem);

            NewItemSkippingRule(() => NotEmptyTest)
                .ForItemInstance<string>("1")
                .NotEmpty()
                .SkipItem(skipItem);

            NewItemSkippingRule(() => NotEqualToTest)
                .ForItemInstance<int>("1")
                .NotEqualTo(1)
                .SkipItem(skipItem);

            NewItemSkippingRule(() => NotNullTest)
                .ForItemInstance<string>("1")
                .NotNull()
                .SkipItem(skipItem);

            NewItemSkippingRule(() => NullTest)
                .ForItemInstance<string>("1")
                .Null()
                .SkipItem(skipItem);
        }

        private class SpecificationTest : ISpecification<int>
        {
            /// <summary>
            /// Gets whether <paramref name="entity">Entity</paramref> meets specification.
            /// </summary>
            /// <param name="entity">Entity to test.</param>
            /// <returns>
            /// A <see cref="T:System.Boolean">Boolean</see>.
            /// </returns>
            public bool IsSatisfiedBy ( int entity )
            {
                return entity == 1;
            }
        }

        public IItemSkippingRule EmptyTestItemSkippingRule { get; set; }

        public IItemSkippingRule EqualToTest { get; set; }
        public IItemSkippingRule ExclusiveBetweenTest { get; set; }
        public IItemSkippingRule GreaterThenTest { get; set; }
        public IItemSkippingRule GreaterThenOrEqualToTest { get; set; }
        public IItemSkippingRule InListTest { get; set; }
        public IItemSkippingRule InclusiveBetweenTest { get; set; }
        public IItemSkippingRule LessThenTest { get; set; }
        public IItemSkippingRule LessThenOrEqualToTest { get; set; }
        public IItemSkippingRule MatchesRegexTest { get; set; }
        public IItemSkippingRule MaxLengthTest { get; set; }
        public IItemSkippingRule MeetsSpecificationTest { get; set; }
        public IItemSkippingRule MinLengthTest { get; set; }
        public IItemSkippingRule NotEmptyTest { get; set; }
        public IItemSkippingRule NotEqualToTest { get; set; }
        public IItemSkippingRule NotNullTest { get; set; }
        public IItemSkippingRule NullTest { get; set; }
    }
}
