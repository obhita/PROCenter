#region Licence Header
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
namespace ProCenter.Domain.Nida
{
    #region Using Statements

    using System.Collections.Generic;
    using System.Linq;
    using System.Resources;
    using AssessmentModule;
    using Common;
    using CommonModule;
    using CommonModule.Lookups;
    using PatientModule;
    using Pillar.Common.InversionOfControl;

    #endregion

    public class NidaAssessFurtherScoringEngine : IScoringEngine
    {
        private readonly IResourcesManager _resourcesManager;

        public string AssessmentName
        {
            get { return NidaAssessFurther.AssessmentCodedConcept.Name; }
        }

        public NidaAssessFurtherScoringEngine()
        {
            
        }

        public NidaAssessFurtherScoringEngine(IResourcesManager resourcesManager )
        {
            _resourcesManager = resourcesManager;
        }

        public ResourceManager ResourceManager
        {
            get { return _resourcesManager.GetResourceManagerByName(AssessmentName); }
        }

        public void CalculateScore(AssessmentInstance assessment)
        {
            //•	No daily use of any substance 
            //•	No weekly use of opioids, cocaine, or methamphetamine 
            //•	No injection drug use in the past three months
            //•	Not currently in drug abuse treatment

            var value = false;
            var dailyUseSubstances = new List<string>
                {
                    "3269979",
                    "3269980",
                    "3269981",
                    "3269982",
                    "3269983",
                    "3269985",
                    "3269984"
                };
            var weeklyUseOpioidsCocaineMethamphetamine = new List<string> { "3269981", "3269980", "3269982" };

            value =
                assessment.ItemInstances.Any(
                    i =>
                    dailyUseSubstances.Contains(i.ItemDefinitionCode) && i.Value != null &&
                    Equals(double.Parse((i.Value as Lookup).Value.ToString()), Frequency.DailyOrAlmostDaily.Value))
                ||
                assessment.ItemInstances.Any(
                    i =>
                    weeklyUseOpioidsCocaineMethamphetamine.Contains(i.ItemDefinitionCode) && i.Value != null &&
                    Equals(double.Parse((i.Value as Lookup).Value.ToString()), Frequency.Weekly.Value))
                ||
                (
                    assessment.ItemInstances.Any(
                        i => i.ItemDefinitionCode == "3269978" && bool.Parse(i.Value.ToString()))
                    &&
                    assessment.ItemInstances.Any(
                        i =>
                        i.ItemDefinitionCode == "3269986" &&
                        Equals(double.Parse((i.Value as Lookup).Value.ToString()), Frequency.InThePast90Days.Value))
                )
                ||
                assessment.ItemInstances.Any(
                    i => i.ItemDefinitionCode == "3269976" && bool.Parse(i.Value.ToString()));

            assessment.ScoreComplete(new CodedConcept(CodeSystems.Obhita, "", ""), value);
        }
    }
}