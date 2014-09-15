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

namespace ProCenter.Domain.Gpra
{
    #region Using Statements

    using System.Collections.Generic;

    using ProCenter.Domain.AssessmentModule;
    using ProCenter.Domain.AssessmentModule.Lookups;
    using ProCenter.Domain.CommonModule;

    #endregion

    /// <summary>The gpra interview class.</summary>
    public class GpraInterview : AssessmentDefinition
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GpraInterview"/> class.
        /// </summary>
        public GpraInterview ()
            : base ( AssessmentCodedConcept, ScoreTypeEnum.ScoreTypeInt )
        {
            var itemDefinitions = GpraInterviewInformationSection.GpraInterviewInformationGroup;

            var interviewInformationGroupItemDefinition =
                new ItemDefinition (
                    new CodedConcept ( CodeSystems.Obhita, "0001000", "Interview Information" ),
                    ItemType.Section,
                    null,
                    null,
                    itemDefinitions );
            AddItemDefinition ( interviewInformationGroupItemDefinition );

            itemDefinitions = new List<ItemDefinition>
                              {
                                  new ItemDefinition (
                                      new CodedConcept ( CodeSystems.Obhita, "0002001", "Modality" ),
                                      ItemType.Group,
                                      null,
                                      null,
                                      GpraPlannedServicesSection.GpraModalityGroup ),
                                  new ItemDefinition (
                                      new CodedConcept ( CodeSystems.Obhita, "0002002", "Treatment Services" ),
                                      ItemType.Group,
                                      null,
                                      null,
                                      GpraPlannedServicesSection.TreatmentServicesGroup ),
                                  new ItemDefinition (
                                      new CodedConcept ( CodeSystems.Obhita, "0002003", "Case Management Services" ),
                                      ItemType.Group,
                                      null,
                                      null,
                                      GpraPlannedServicesSection.CaseManagementServicesGroup ),
                                  new ItemDefinition (
                                      new CodedConcept ( CodeSystems.Obhita, "0002004", "Medical Services" ),
                                      ItemType.Group,
                                      null,
                                      null,
                                      GpraPlannedServicesSection.MedicalServicesGroup ),
                                  new ItemDefinition (
                                      new CodedConcept ( CodeSystems.Obhita, "0002005", "After Care Services" ),
                                      ItemType.Group,
                                      null,
                                      null,
                                      GpraPlannedServicesSection.AfterCareServicesGroup ),
                                  new ItemDefinition (
                                      new CodedConcept ( CodeSystems.Obhita, "0002006", "Education Services" ),
                                      ItemType.Group,
                                      null,
                                      null,
                                      GpraPlannedServicesSection.EducationServicesGroup ),
                                  new ItemDefinition (
                                      new CodedConcept ( CodeSystems.Obhita, "0002007", "Peer To Peer Recovery Support Services" ),
                                      ItemType.Group,
                                      null,
                                      null,
                                      GpraPlannedServicesSection.PeerToPeerRecoverySupportServicesGroup ),
                              };

            var gpraPlannedServicesSection =
                new ItemDefinition (
                    new CodedConcept ( CodeSystems.Obhita, "0002000", "Planned Services" ),
                    ItemType.Section,
                    null,
                    null,
                    itemDefinitions );
            AddItemDefinition ( gpraPlannedServicesSection );

            itemDefinitions = GpraDemographicsSection.GpraDemographicsGroup;

            var gpraDemographicsSection =
                new ItemDefinition (
                    new CodedConcept ( CodeSystems.Obhita, "0003000", "Demographics" ),
                    ItemType.Section,
                    null,
                    null,
                    itemDefinitions );
            AddItemDefinition ( gpraDemographicsSection );

            itemDefinitions = GpraDrugAlcoholUseSection.GpraDrugAlcoholUseGroup;

            var gpraDrugAlcoholUseGroup =
                new ItemDefinition (
                    new CodedConcept ( CodeSystems.Obhita, "0004000", "Drug and Alcohol Use" ),
                    ItemType.Section,
                    null,
                    null,
                    itemDefinitions );
            AddItemDefinition ( gpraDrugAlcoholUseGroup );

            itemDefinitions = GpraFamilyLivingConditionsSection.GpraFamilyLivingConditionsGroup;

            var gpraFamilyLivingConditionsGroup =
                new ItemDefinition (
                    new CodedConcept ( CodeSystems.Obhita, "0005000", " Family and Living Conditions" ),
                    ItemType.Section,
                    null,
                    null,
                    itemDefinitions );
            AddItemDefinition ( gpraFamilyLivingConditionsGroup );

            itemDefinitions = GpraProfessionalInformationSection.GpraProfessionalInformationGroup;

            var gpraProfessionalInformationGroup =
                new ItemDefinition (
                    new CodedConcept ( CodeSystems.Obhita, "0006000", "Professional Information" ),
                    ItemType.Section,
                    null,
                    null,
                    itemDefinitions );
            AddItemDefinition ( gpraProfessionalInformationGroup );

            itemDefinitions = GpraCriminalJusticeSection.GpraCrimeCriminalJusticeGroup;

            var gpraCrimeCriminalJusticeGroup =
                new ItemDefinition (
                    new CodedConcept ( CodeSystems.Obhita, "0007000", "Crime and Criminal Justice" ),
                    ItemType.Section,
                    null,
                    null,
                    itemDefinitions );
            AddItemDefinition ( gpraCrimeCriminalJusticeGroup );

            itemDefinitions = GpraProblemsTreatmentRecoverySection.GpraProblemsTreatmentRecoveryGroup;

            var gpraProblemsTreatmentRecoveryGroup =
                new ItemDefinition (
                    new CodedConcept ( CodeSystems.Obhita, "0008000", "Problems and Treatment/Recovery" ),
                    ItemType.Section,
                    null,
                    null,
                    itemDefinitions );
            AddItemDefinition ( gpraProblemsTreatmentRecoveryGroup );

            itemDefinitions = GpraSocialConnectednessSection.GpraSocialConnectednessGroup;

            var gpraSocialConnectednessGroup =
                new ItemDefinition (
                    new CodedConcept ( CodeSystems.Obhita, "0009000", "Social Connectedness" ),
                    ItemType.Section,
                    null,
                    null,
                    itemDefinitions );
            AddItemDefinition ( gpraSocialConnectednessGroup );

            itemDefinitions = GpraFollowUpSection.GpraFollowUpGroup;

            var gpraFollowUpGroup =
                new ItemDefinition (
                    new CodedConcept ( CodeSystems.Obhita, "0010000", "Follow-Up" ),
                    ItemType.Section,
                    null,
                    null,
                    itemDefinitions );
            AddItemDefinition ( gpraFollowUpGroup );

            itemDefinitions = GpraDischargeSection.GpraDischargeGroup;

            var gpraDischargeGroup =
                new ItemDefinition (
                    new CodedConcept ( CodeSystems.Obhita, "0020000", "Discharge" ),
                    ItemType.Section,
                    null,
                    null,
                    itemDefinitions );
            AddItemDefinition ( gpraDischargeGroup );
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the assessment coded concept.
        /// </summary>
        /// <value>
        /// The assessment coded concept.
        /// </value>
        public static CodedConcept AssessmentCodedConcept
        {
            get { return new CodedConcept ( CodeSystems.Obhita, "1000000", "GpraInterview" ); }
        }

        #endregion
    }
}