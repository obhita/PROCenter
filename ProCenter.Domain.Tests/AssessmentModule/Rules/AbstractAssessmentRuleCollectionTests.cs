namespace ProCenter.Domain.Tests.AssessmentModule.Rules
{
    using System;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using ProCenter.Domain.AssessmentModule.Rules;

    [TestClass]
    public class AbstractAssessmentRuleCollectionTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NewItemSkippingRule_NullPropertyExpression_ThrowsException()
        {
            var testAssessmentRuleCollection = new TestAssessmentRuleCollection ();

            testAssessmentRuleCollection.NewItemSkippingRule<IItemSkippingRule> ( null );
        }

        [TestMethod]
        public void NewItemSkippingRule_ValidSkippingRule_AddedToItemSkippingRules()
        {
            var testAssessmentRuleCollection = new TestAssessmentRuleCollection();

            testAssessmentRuleCollection.NewItemSkippingRule(() => testAssessmentRuleCollection.EmptyTestItemSkippingRule )
                .ForItemInstance<string> ( "1" );

            Assert.AreEqual(1, testAssessmentRuleCollection.ItemSkippingRules.Count(r => r.Name == "EmptyTestItemSkippingRule"));
        }
    }
}
