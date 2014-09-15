namespace ProCenter.Domain.Tests.AssessmentModule.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using NSubstitute;

    using Pillar.Common.Metadata;
    using Pillar.FluentRuleEngine;
    using Pillar.FluentRuleEngine.Constraints;

    using ProCenter.Domain.AssessmentModule;
    using ProCenter.Domain.AssessmentModule.Lookups;
    using ProCenter.Domain.AssessmentModule.Metadata;
    using ProCenter.Domain.AssessmentModule.Rules;
    using ProCenter.Domain.CommonModule;

    [TestClass]
    public class ItemSkippingRuleTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateItemSkippingRule_NullItemDefinition_ThrowsException ()
        {
            new ItemSkippingRule<object> ( null, "Rule" );
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateItemSkippingRule_EmptyItemDefinition_ThrowsException()
        {
            new ItemSkippingRule<object>("", "Rule");
        }

        [TestMethod]
        public void CreateItemSkippingRule_ValidItemDefinition_CreatesInstanceOfRule()
        {
            var rule = new ItemSkippingRule<object>("1", "Rule");

            Assert.IsNotNull ( rule );
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddConstraint_NullConstraint_ThrowsException()
        {
            var rule = new ItemSkippingRule<object>("1", "Rule");

            rule.AddConstraint ( null );
        }

        [TestMethod]
        public void AddConstraint_ValidConstraint_RunsWhenRuleExecuted()
        {
            var rule = new ItemSkippingRule<object>("1", "Rule");
            var constraint = Substitute.For<IConstraint> ();

            rule.AddConstraint(constraint);

            var itemInstance = new ItemInstance ( "1", "Test", true );
            var context = new RuleEngineContext<object> ( itemInstance );
            context.WorkingMemory.AddContextObject ( itemInstance, itemInstance.ItemDefinitionCode );

            rule.WhenClause.Invoke(context);

            constraint.Received ().IsCompliant ( Arg.Any<object> (), Arg.Any<IRuleEngineContext> () );
        }

        [TestMethod]
        public void WhenClause_ConstraintFails_ReturnsFalse()
        {
            var rule = new ItemSkippingRule<object>("1", "Rule");
            var constraint = Substitute.For<IConstraint>();
            constraint.IsCompliant ( Arg.Any<object> (), Arg.Any<IRuleEngineContext> () ).Returns ( false );

            rule.AddConstraint(constraint);

            var itemInstance = new ItemInstance("1", "Test", true);
            var context = new RuleEngineContext<object>(itemInstance);
            context.WorkingMemory.AddContextObject(itemInstance, itemInstance.ItemDefinitionCode);

            Assert.IsFalse(rule.WhenClause.Invoke(context));
        }

        [TestMethod]
        public void WhenClause_ConstraintFails_WorkingMemoryContainsFailedConstraint()
        {
            var rule = new ItemSkippingRule<object>("1", "Rule");
            var constraint = Substitute.For<IConstraint>();
            constraint.IsCompliant(Arg.Any<object>(), Arg.Any<IRuleEngineContext>()).Returns(false);

            rule.AddConstraint(constraint);

            var itemInstance = new ItemInstance("1", "Test", true);
            var context = new RuleEngineContext<object>(itemInstance);
            context.WorkingMemory.AddContextObject(itemInstance, itemInstance.ItemDefinitionCode);

            rule.WhenClause.Invoke(context);

            var failedConstraints = context.WorkingMemory.GetContextObject<List<IConstraint>> ( rule.Name );

            CollectionAssert.Contains ( failedConstraints, constraint );
        }

        [TestMethod]
        public void WhenClause_ConstraintPasses_ReturnsTrue()
        {
            var rule = new ItemSkippingRule<object>("1", "Rule");
            var constraint = Substitute.For<IConstraint>();
            constraint.IsCompliant(Arg.Any<object>(), Arg.Any<IRuleEngineContext>()).Returns(true);

            rule.AddConstraint(constraint);

            var itemInstance = new ItemInstance("1", "Test", true);
            var context = new RuleEngineContext<object>(itemInstance);
            context.WorkingMemory.AddContextObject(itemInstance, itemInstance.ItemDefinitionCode);

            Assert.IsTrue(rule.WhenClause.Invoke(context));
        }

        [TestMethod]
        public void WhenClause_ConstraintPasses_WorkingMemoryDoesNotContainPassedConstraint()
        {
            var rule = new ItemSkippingRule<object>("1", "Rule");
            var constraint = Substitute.For<IConstraint>();
            constraint.IsCompliant(Arg.Any<object>(), Arg.Any<IRuleEngineContext>()).Returns(true);

            rule.AddConstraint(constraint);

            var itemInstance = new ItemInstance("1", "Test", true);
            var context = new RuleEngineContext<object>(itemInstance);
            context.WorkingMemory.AddContextObject(itemInstance, itemInstance.ItemDefinitionCode);

            rule.WhenClause.Invoke(context);

            var failedConstraints = context.WorkingMemory.GetContextObject<List<IConstraint>>(rule.Name);

            CollectionAssert.DoesNotContain(failedConstraints, constraint);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddItemDefinition_Null_ThrowsException()
        {
            var rule = new ItemSkippingRule<object>("1", "Rule");

            rule.AddItemDefinitionToSkip(null);
        }

        [TestMethod]
        public void AddItemDefinition_Valid_ThenClauseSkipsItem ()
        {
            var rule = new ItemSkippingRule<object> ( "1", "Rule" );

            var itemDefinition = new ItemDefinition ( new CodedConcept ( new CodeSystem ( "1", "1", "Test" ), "1", "Test" ), ItemType.Question, null )
                                 {
                                     ItemMetadata = new ItemMetadata
                                                    {
                                                        MetadataItems = new List<IMetadataItem>
                                                                        {
                                                                            new RequiredForCompletenessMetadataItem ( "Report" )
                                                                        }
                                                    }
                                 };
            rule.AddItemDefinitionToSkip ( itemDefinition );

            var itemInstance = new ItemInstance ( "1", "Test", true );
            
            var assessmentInstance = Substitute.For<AssessmentInstance> ();
            assessmentInstance.AssessmentName.Returns ( "Test" );

            var skippingContext = new SkippingContext ();

            var context = new RuleEngineContext<AssessmentInstance> ( assessmentInstance );
            context.WorkingMemory.AddContextObject(itemInstance, itemInstance.ItemDefinitionCode);
            context.WorkingMemory.AddContextObject(skippingContext);

            rule.ThenClauses.First ().Invoke ( context );

            CollectionAssert.Contains( skippingContext.SkippedItemDefinitions, itemDefinition);
        }

        [TestMethod]
        public void AddItemDefinition_Valid_ElseThenClauseUnSkipsItem()
        {
            var rule = new ItemSkippingRule<object>("1", "Rule");

            var itemDefinition = new ItemDefinition(new CodedConcept(new CodeSystem("1", "1", "Test"), "1", "Test"), ItemType.Question, null)
            {
                ItemMetadata = new ItemMetadata
                {
                    MetadataItems = new List<IMetadataItem>
                                                                        {
                                                                            new RequiredForCompletenessMetadataItem ( "Report" )
                                                                        }
                }
            };
            rule.AddItemDefinitionToSkip(itemDefinition);

            var itemInstance = new ItemInstance("1", "Test", true);

            var assessmentInstance = Substitute.For<AssessmentInstance>();
            assessmentInstance.AssessmentName.Returns("Test");

            var skippingContext = new SkippingContext();

            var context = new RuleEngineContext<AssessmentInstance>(assessmentInstance);
            context.WorkingMemory.AddContextObject(itemInstance, itemInstance.ItemDefinitionCode);
            context.WorkingMemory.AddContextObject(skippingContext);

            rule.ElseThenClauses.First().Invoke(context);

            CollectionAssert.Contains(skippingContext.UnSkippedItemDefinitions, itemDefinition);
        }

        [TestMethod]
        public void ValidClauses ()
        {
            var rule = new ItemSkippingRule<object>("1", "Rule");
            
            Assert.IsNotNull ( rule.WhenClause );
            Assert.IsTrue(rule.ElseThenClauses.Count() == 1);
            Assert.IsTrue ( rule.ThenClauses.Count() == 1 );
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CheckNonResponse_NullNonResponseLookups_ThrowsException()
        {
            var rule = new ItemSkippingRule<object>("1", "Rule");

            rule.CheckNonResponse( null );
        }

        [TestMethod]
        public void CheckNonResponse_NonResponseValue_WhenClauseReturnsTrue()
        {
            var nonResponseValue = "TestNonResponse";
            var rule = new ItemSkippingRule<object>("1", "Rule");

            rule.CheckNonResponse(new List<string> { nonResponseValue });

            var itemInstance = new ItemInstance("1", nonResponseValue, true);
            var context = new RuleEngineContext<object>(itemInstance);
            context.WorkingMemory.AddContextObject(itemInstance, itemInstance.ItemDefinitionCode);

            Assert.IsTrue(rule.WhenClause.Invoke(context));
        }

        [TestMethod]
        public void CheckNonResponse_NotANonResponseValue_WhenClauseReturnsFalse()
        {
            var nonResponseValue = "TestNonResponse";
            var rule = new ItemSkippingRule<object>("1", "Rule");

            rule.CheckNonResponse(new List<string> { nonResponseValue });

            var itemInstance = new ItemInstance("1", "Test", true);
            var context = new RuleEngineContext<object>(itemInstance);
            context.WorkingMemory.AddContextObject(itemInstance, itemInstance.ItemDefinitionCode);

            Assert.IsFalse(rule.WhenClause.Invoke(context));
        }

        [TestMethod]
        public void CheckNonResponse_NonResponseValue_DoesNotExecuteConstraints()
        {
            var nonResponseValue = "TestNonResponse";
            var rule = new ItemSkippingRule<object>("1", "Rule");

            rule.CheckNonResponse(new List<string> { nonResponseValue });

            var constraint = Substitute.For<IConstraint>();

            rule.AddConstraint(constraint);

            var itemInstance = new ItemInstance("1", nonResponseValue, true);
            var context = new RuleEngineContext<object>(itemInstance);
            context.WorkingMemory.AddContextObject(itemInstance, itemInstance.ItemDefinitionCode);

            rule.WhenClause.Invoke(context);

            constraint.DidNotReceive ().IsCompliant ( Arg.Any<object> (), Arg.Any<IRuleEngineContext> () );
        }

        [TestMethod]
        public void CheckNonResponse_NotANonResponseValue_ExecutesConstraints()
        {
            var nonResponseValue = "TestNonResponse";
            var rule = new ItemSkippingRule<object>("1", "Rule");

            rule.CheckNonResponse(new List<string> { nonResponseValue });

            var constraint = Substitute.For<IConstraint>();

            rule.AddConstraint(constraint);

            var itemInstance = new ItemInstance("1", "Test", true);
            var context = new RuleEngineContext<object>(itemInstance);
            context.WorkingMemory.AddContextObject(itemInstance, itemInstance.ItemDefinitionCode);

            rule.WhenClause.Invoke(context);

            constraint.Received().IsCompliant(Arg.Any<object>(), Arg.Any<IRuleEngineContext>());
        }
    }
}
