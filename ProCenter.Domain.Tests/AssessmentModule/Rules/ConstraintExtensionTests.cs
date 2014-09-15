namespace ProCenter.Domain.Tests.AssessmentModule.Rules
{
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using NSubstitute;

    using Pillar.FluentRuleEngine;

    using ProCenter.Domain.AssessmentModule;
    using ProCenter.Domain.AssessmentModule.Rules;

    [TestClass]
    public class ConstraintExtensionTests
    {
        private bool CheckSkip ( TestAssessmentRuleCollection assessmentRuleCollection, IItemSkippingRule rule, ItemInstance itemInstance )
        {
            var ruleEngine = new RuleEngine<AssessmentInstance>(assessmentRuleCollection, new RuleProcessor());

            var assessmentInstance = Substitute.For<AssessmentInstance>();

            var ruleEngineContext = new RuleEngineContext<AssessmentInstance>(
                assessmentInstance,
                new SingleRuleSelector(rule));

            var skippingContext = new SkippingContext ();

            ruleEngineContext
                .WorkingMemory
                .AddContextObject(itemInstance, itemInstance.ItemDefinitionCode);
            ruleEngineContext.WorkingMemory.AddContextObject(skippingContext);

            ruleEngine.ExecuteRules(ruleEngineContext);


            return skippingContext.SkippedItemDefinitions.Any( );
        }

        [TestMethod]
        public void EqualTo_IsEqualTo_CallSkip ()
        {
            var assessmentRuleCollection = new TestAssessmentRuleCollection();
            var itemInstance = new ItemInstance ( "1", 1, true );

            Assert.IsTrue(CheckSkip(assessmentRuleCollection, assessmentRuleCollection.EqualToTest, itemInstance));
        }

        [TestMethod]
        public void EqualTo_IsNotEqualTo_DoesNotCallSkip()
        {
            var assessmentRuleCollection = new TestAssessmentRuleCollection();
            var itemInstance = new ItemInstance("1", 2, true);

            Assert.IsFalse(CheckSkip(assessmentRuleCollection, assessmentRuleCollection.EqualToTest, itemInstance));
        }

        [TestMethod]
        public void ExclusiveBetween_IsBetween_CallSkip()
        {
            var assessmentRuleCollection = new TestAssessmentRuleCollection();
            var itemInstance = new ItemInstance("1", 2, true);

            Assert.IsTrue(CheckSkip(assessmentRuleCollection, assessmentRuleCollection.ExclusiveBetweenTest, itemInstance));
        }

        [TestMethod]
        public void ExclusiveBetween_IsNotBetween_DoesNotCallSkip()
        {
            var assessmentRuleCollection = new TestAssessmentRuleCollection();
            var itemInstance = new ItemInstance("1", 1, true);

            Assert.IsFalse(CheckSkip(assessmentRuleCollection, assessmentRuleCollection.ExclusiveBetweenTest, itemInstance));
        }

        [TestMethod]
        public void GreaterThen_IsGreaterThen_CallSkip()
        {
            var assessmentRuleCollection = new TestAssessmentRuleCollection();
            var itemInstance = new ItemInstance("1", 2, true);

            Assert.IsTrue(CheckSkip(assessmentRuleCollection, assessmentRuleCollection.GreaterThenTest, itemInstance));
        }

        [TestMethod]
        public void GreaterThen_IsNotGreaterThen_DoesNotCallSkip()
        {
            var assessmentRuleCollection = new TestAssessmentRuleCollection();
            var itemInstance = new ItemInstance("1", 1, true);

            Assert.IsFalse(CheckSkip(assessmentRuleCollection, assessmentRuleCollection.GreaterThenTest, itemInstance));
        }

        [TestMethod]
        public void GreaterThenOrEqualTo_IsGreaterThen_CallSkip()
        {
            var assessmentRuleCollection = new TestAssessmentRuleCollection();
            var itemInstance = new ItemInstance("1", 2, true);

            Assert.IsTrue(CheckSkip(assessmentRuleCollection, assessmentRuleCollection.GreaterThenOrEqualToTest, itemInstance));
        }

        [TestMethod]
        public void GreaterThenOrEqualTo_IsNotGreaterThen_DoesNotCallSkip()
        {
            var assessmentRuleCollection = new TestAssessmentRuleCollection();
            var itemInstance = new ItemInstance("1", 0, true);

            Assert.IsFalse(CheckSkip(assessmentRuleCollection, assessmentRuleCollection.GreaterThenOrEqualToTest, itemInstance));
        }

        [TestMethod]
        public void GreaterThenOrEqualTo_IsEqualTo_DoesNotCallSkip()
        {
            var assessmentRuleCollection = new TestAssessmentRuleCollection();
            var itemInstance = new ItemInstance("1", 1, true);

            Assert.IsTrue(CheckSkip(assessmentRuleCollection, assessmentRuleCollection.GreaterThenOrEqualToTest, itemInstance));
        }

        [TestMethod]
        public void InList_IsInList_CallSkip()
        {
            var assessmentRuleCollection = new TestAssessmentRuleCollection();
            var itemInstance = new ItemInstance("1", 2, true);

            Assert.IsTrue(CheckSkip(assessmentRuleCollection, assessmentRuleCollection.InListTest, itemInstance));
        }

        [TestMethod]
        public void InList_IsNotInList_DoesNotCallSkip()
        {
            var assessmentRuleCollection = new TestAssessmentRuleCollection();
            var itemInstance = new ItemInstance("1", 0, true);

            Assert.IsFalse(CheckSkip(assessmentRuleCollection, assessmentRuleCollection.InListTest, itemInstance));
        }

        [TestMethod]
        public void InclusiveBetween_IsInclusiveBetween_CallSkip()
        {
            var assessmentRuleCollection = new TestAssessmentRuleCollection();
            var itemInstance = new ItemInstance("1", 1, true);

            Assert.IsTrue(CheckSkip(assessmentRuleCollection, assessmentRuleCollection.InclusiveBetweenTest, itemInstance));
        }

        [TestMethod]
        public void InclusiveBetween_IsNotInclusiveBetween_DoesNotCallSkip()
        {
            var assessmentRuleCollection = new TestAssessmentRuleCollection();
            var itemInstance = new ItemInstance("1", 0, true);

            Assert.IsFalse(CheckSkip(assessmentRuleCollection, assessmentRuleCollection.InclusiveBetweenTest, itemInstance));
        }

        [TestMethod]
        public void LessThen_IsLessThen_CallSkip()
        {
            var assessmentRuleCollection = new TestAssessmentRuleCollection();
            var itemInstance = new ItemInstance("1", 0, true);

            Assert.IsTrue(CheckSkip(assessmentRuleCollection, assessmentRuleCollection.LessThenTest, itemInstance));
        }

        [TestMethod]
        public void LessThen_IsNotLessThen_DoesNotCallSkip()
        {
            var assessmentRuleCollection = new TestAssessmentRuleCollection();
            var itemInstance = new ItemInstance("1", 1, true);

            Assert.IsFalse(CheckSkip(assessmentRuleCollection, assessmentRuleCollection.LessThenTest, itemInstance));
        }

        [TestMethod]
        public void LessThenOrEqualTo_IsLessThen_CallSkip()
        {
            var assessmentRuleCollection = new TestAssessmentRuleCollection();
            var itemInstance = new ItemInstance("1", 0, true);

            Assert.IsTrue(CheckSkip(assessmentRuleCollection, assessmentRuleCollection.LessThenOrEqualToTest, itemInstance));
        }

        [TestMethod]
        public void LessThenOrEqualTo_IsNotLessThen_DoesNotCallSkip()
        {
            var assessmentRuleCollection = new TestAssessmentRuleCollection();
            var itemInstance = new ItemInstance("1", 2, true);

            Assert.IsFalse(CheckSkip(assessmentRuleCollection, assessmentRuleCollection.LessThenOrEqualToTest, itemInstance));
        }

        [TestMethod]
        public void LessThenOrEqualTo_IsEqualTo_DoesNotCallSkip()
        {
            var assessmentRuleCollection = new TestAssessmentRuleCollection();
            var itemInstance = new ItemInstance("1", 1, true);

            Assert.IsTrue(CheckSkip(assessmentRuleCollection, assessmentRuleCollection.LessThenOrEqualToTest, itemInstance));
        }

        [TestMethod]
        public void MatchesRegex_DoesMatchesRegex_CallSkip()
        {
            var assessmentRuleCollection = new TestAssessmentRuleCollection();
            var itemInstance = new ItemInstance("1", 1, true);

            Assert.IsTrue(CheckSkip(assessmentRuleCollection, assessmentRuleCollection.MatchesRegexTest, itemInstance));
        }

        [TestMethod]
        public void MatchesRegex_DoesNotMatchesRegex_DoesNotCallSkip()
        {
            var assessmentRuleCollection = new TestAssessmentRuleCollection();
            var itemInstance = new ItemInstance("1", 2, true);

            Assert.IsFalse(CheckSkip(assessmentRuleCollection, assessmentRuleCollection.MatchesRegexTest, itemInstance));
        }

        [TestMethod]
        public void MeetsSpecification_DoesMeetSpecification_CallSkip()
        {
            var assessmentRuleCollection = new TestAssessmentRuleCollection();
            var itemInstance = new ItemInstance("1", 1, true);

            Assert.IsTrue(CheckSkip(assessmentRuleCollection, assessmentRuleCollection.MeetsSpecificationTest, itemInstance));
        }

        [TestMethod]
        public void MeetsSpecification_DoesNotMeetSpecification_DoesNotCallSkip()
        {
            var assessmentRuleCollection = new TestAssessmentRuleCollection();
            var itemInstance = new ItemInstance("1", 2, true);

            Assert.IsFalse(CheckSkip(assessmentRuleCollection, assessmentRuleCollection.MeetsSpecificationTest, itemInstance));
        }

        [TestMethod]
        public void NotEmpty_IsNotEmpty_CallSkip()
        {
            var assessmentRuleCollection = new TestAssessmentRuleCollection();
            var itemInstance = new ItemInstance("1", "1", true);

            Assert.IsTrue(CheckSkip(assessmentRuleCollection, assessmentRuleCollection.NotEmptyTest, itemInstance));
        }

        [TestMethod]
        public void NotEmpty_IsNotNotEmpty_DoesNotCallSkip()
        {
            var assessmentRuleCollection = new TestAssessmentRuleCollection();
            var itemInstance = new ItemInstance("1", string.Empty, true);

            Assert.IsFalse(CheckSkip(assessmentRuleCollection, assessmentRuleCollection.NotEmptyTest, itemInstance));
        }

        [TestMethod]
        public void NotEqualTo_IsNotEqualTo_CallSkip()
        {
            var assessmentRuleCollection = new TestAssessmentRuleCollection();
            var itemInstance = new ItemInstance("1", 0, true);

            Assert.IsTrue(CheckSkip(assessmentRuleCollection, assessmentRuleCollection.NotEqualToTest, itemInstance));
        }

        [TestMethod]
        public void NotEqualTo_IsNotNotEqualTo_DoesNotCallSkip()
        {
            var assessmentRuleCollection = new TestAssessmentRuleCollection();
            var itemInstance = new ItemInstance("1", 1, true);

            Assert.IsFalse(CheckSkip(assessmentRuleCollection, assessmentRuleCollection.NotEqualToTest, itemInstance));
        }

        [TestMethod]
        public void NotNull_IsNotNull_CallSkip()
        {
            var assessmentRuleCollection = new TestAssessmentRuleCollection();
            var itemInstance = new ItemInstance("1", "1", true);

            Assert.IsTrue(CheckSkip(assessmentRuleCollection, assessmentRuleCollection.NotNullTest, itemInstance));
        }

        [TestMethod]
        public void NotNull_IsNotNotNull_DoesNotCallSkip()
        {
            var assessmentRuleCollection = new TestAssessmentRuleCollection();
            var itemInstance = new ItemInstance("1", null, true);

            Assert.IsFalse(CheckSkip(assessmentRuleCollection, assessmentRuleCollection.NotNullTest, itemInstance));
        }

        [TestMethod]
        public void Null_IsNull_CallSkip()
        {
            var assessmentRuleCollection = new TestAssessmentRuleCollection();
            var itemInstance = new ItemInstance("1", null, true);

            Assert.IsTrue(CheckSkip(assessmentRuleCollection, assessmentRuleCollection.NullTest, itemInstance));
        }

        [TestMethod]
        public void Null_IsNotNull_DoesNotCallSkip()
        {
            var assessmentRuleCollection = new TestAssessmentRuleCollection();
            var itemInstance = new ItemInstance("1", "1", true);

            Assert.IsFalse(CheckSkip(assessmentRuleCollection, assessmentRuleCollection.NullTest, itemInstance));
        }
    }
}
