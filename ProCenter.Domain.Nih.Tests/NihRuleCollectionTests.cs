#region License Header
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
#endregion
namespace ProCenter.Domain.Nih.Tests
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AssessmentModule;
    using CommonModule;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using ProCenter.Domain.AssessmentModule.Lookups;

    #endregion

    [TestClass]
    public class NihRuleCollectionTests
    {
        private static List<NihHealthBehaviorsAssessmentRuleCollection> _ruleCollections;
        private static readonly Guid _assessmentDefKey = Guid.NewGuid();

        public NihRuleCollectionTests()
        {
            var codes = new List<string> { "7125038", "7125041", "7125010" };
            var mockAssessmentDefRepository = BuildIAssessmentDefinitionRepositoryMock(_assessmentDefKey, codes);
            var ruleCollection1 = new NihHealthBehaviorsAssessmentRuleCollection(mockAssessmentDefRepository.Object);
            var ruleCollection2 = new NihHealthBehaviorsAssessmentRuleCollection(mockAssessmentDefRepository.Object);
            _ruleCollections = new List<NihHealthBehaviorsAssessmentRuleCollection> { ruleCollection1, ruleCollection2 };
        }

        [TestMethod]
        public void AllRulesAndRuleSetsDefined()
        {
            foreach (var propertyInfo in typeof(NihHealthBehaviorsAssessmentRuleCollection).GetProperties())
            {
                foreach ( var rules in _ruleCollections )
                {
                    Assert.IsNotNull(propertyInfo.GetMethod.Invoke(rules, null), string.Format("Property {0} is null.", propertyInfo.Name));
                }
            }
        }

        [TestMethod]
        public void NihRuleSetContainsCorrectRules()
        {
            Assert.AreEqual(1, _ruleCollections.ElementAt ( 0 ).ItemUpdatedRuleSet7125031.Count());
            Assert.AreEqual(1, _ruleCollections.ElementAt ( 1 ).ItemUpdatedRuleSet7125031.Count());
        }

        private static Mock<IAssessmentDefinitionRepository> BuildIAssessmentDefinitionRepositoryMock(Guid key, List<string> codes)
        {
            var assessmentDefintionRepositoryMock = new Mock<IAssessmentDefinitionRepository>();
            assessmentDefintionRepositoryMock.Setup(r => r.GetKeyByCode(It.IsAny<string>())).Returns(key);

            var assessmentDefinitionMock = new Mock<AssessmentDefinition>();
            assessmentDefintionRepositoryMock.Setup(r => r.GetByKey(key)).Returns(assessmentDefinitionMock.Object);
            foreach ( var code in codes )
            {
                var itemDefinition = new ItemDefinition(new CodedConcept(CodeSystems.Obhita, code, code), ItemType.Question, null);
                var code1 = code;
                assessmentDefinitionMock.Setup(r => r.GetItemDefinitionByCode(code1)).Returns(itemDefinition);
            }

            return assessmentDefintionRepositoryMock;
        }
    }
}