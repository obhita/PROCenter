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