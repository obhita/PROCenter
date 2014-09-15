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

namespace ProCenter.Domain.Gpra
{
    #region Using Statements

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Pillar.FluentRuleEngine;

    using ProCenter.Domain.AssessmentModule;
    using ProCenter.Domain.AssessmentModule.Rules;
    using ProCenter.Domain.CommonModule.Lookups;
    using ProCenter.Domain.Gpra.Lookups;

    #endregion

    /// <summary>The gpra interview rule collection class.</summary>
    public class GpraInterviewRuleCollection : AbstractAssessmentRuleCollection
    {
        #region Static Fields

        private static Guid? _gpraAssessmentDefinitionKey;

        #endregion

        #region Fields

        private readonly IAssessmentDefinitionRepository _assessmentDefinitionRepository;

        /// <summary>
        ///     The unskipped items from the demographics section used to skip the rest of the section.
        /// </summary>
        private readonly string[] _demographicsSection = { "0000067", "0000069", "0000078", "0000079", "0000080", "0000081", "0000082", "0000083", "0000084", "0000085" };

        /// <summary>The _non response list.</summary>
        private readonly List<string> _nonResponseList = NonResponseLookups.NonResponse.Select ( a => a.CodedConcept.Name ).ToList ();

        #endregion

        #region Constructors and Destructors

        /// <summary>Initializes a new instance of the <see cref="GpraInterviewRuleCollection" /> class.</summary>
        /// <param name="assessmentDefinitionRepository">The assessment definition repository.</param>
        public GpraInterviewRuleCollection ( IAssessmentDefinitionRepository assessmentDefinitionRepository )
        {
            _assessmentDefinitionRepository = assessmentDefinitionRepository;

            #region InterviewInformationSectionRules

            NewItemSkippingRule ( () => SkipItem0000004 )
                .ForItemInstance<Lookup> ( "0000003" )
                .EqualTo ( InterviewInformationLookups.Intake )
                .SkipItem ( GetItemDefinition ( "0000004" ) );

            NewItemSkippingRule ( () => SkipDemographicsSection )
                .ForItemInstance<Lookup> ( "0000003" )
                .NotEqualTo ( InterviewInformationLookups.Intake )
                .SkipItem ( GetItemDefinition ( _demographicsSection ) );

            NewRuleSet ( () => ItemUpdatedRuleSet0000003, SkipItem0000004, SkipDemographicsSection );

            #endregion

            #region PlannedServicesSectionRules

            NewItemSkippingRule ( () => SkipItem0000020 ).ForItemInstance<bool> ( "0000019" ).EqualTo ( false ).SkipItem ( GetItemDefinition ( "0000020" ) );

            NewRuleSet ( () => ItemUpdatedRuleSet0000019, SkipItem0000020 );

            NewItemSkippingRule ( () => SkipItem0000034 ).ForItemInstance<bool> ( "0000033" ).EqualTo ( false ).SkipItem ( GetItemDefinition ( "0000034" ) );

            NewRuleSet ( () => ItemUpdatedRuleSet0000033, SkipItem0000034 );

            NewItemSkippingRule ( () => SkipItem0000044 ).ForItemInstance<bool> ( "0000043" ).EqualTo ( false ).SkipItem ( GetItemDefinition ( "0000044" ) );

            NewRuleSet ( () => ItemUpdatedRuleSet0000043, SkipItem0000044 );

            NewItemSkippingRule ( () => SkipItem0000049 ).ForItemInstance<bool> ( "0000048" ).EqualTo ( false ).SkipItem ( GetItemDefinition ( "0000049" ) );

            NewRuleSet ( () => ItemUpdatedRuleSet0000048, SkipItem0000049 );

            NewItemSkippingRule ( () => SkipItem0000056 ).ForItemInstance<bool> ( "0000055" ).EqualTo ( false ).SkipItem ( GetItemDefinition ( "0000056" ) );

            NewRuleSet ( () => ItemUpdatedRuleSet0000055, SkipItem0000056 );

            NewItemSkippingRule ( () => SkipItem0000060 ).ForItemInstance<bool> ( "0000059" ).EqualTo ( false ).SkipItem ( GetItemDefinition ( "0000060" ) );

            NewRuleSet ( () => ItemUpdatedRuleSet0000059, SkipItem0000060 );

            NewItemSkippingRule ( () => SkipItem0000066 ).ForItemInstance<bool> ( "0000065" ).EqualTo ( false ).SkipItem ( GetItemDefinition ( "0000066" ) );

            NewRuleSet ( () => ItemUpdatedRuleSet0000065, SkipItem0000066 );

            #endregion

            #region DemographicsSectionRules

            NewItemSkippingRule ( () => SkipItem0000068 ).ForItemInstance<Lookup> ( "0000067" ).NotEqualTo ( Gender.Other ).SkipItem ( GetItemDefinition ( "0000068" ) );

            NewRuleSet ( () => ItemUpdatedRuleSet0000067, SkipItem0000068 );

            NewItemSkippingRule ( () => SkipItem0000070To0000077 )
                .ForItemInstance<bool> ( "0000069" )
                .EqualTo ( false )
                .SkipItem ( GetItemDefinition ( "0000070", "0000071", "0000072", "0000073", "0000074", "0000075", "0000076" ) );

            NewRuleSet ( () => ItemUpdatedRuleSet0000069, SkipItem0000070To0000077 );

            NewItemSkippingRule ( () => SkipItem0000077 )
                .ForItemInstance<bool> ( "0000076" )
                .EqualTo ( false )
                .OrNonResponse ( _nonResponseList )
                .SkipItem ( GetItemDefinition ( "0000077" ) );

            NewRuleSet ( () => ItemUpdatedRuleSet0000076, SkipItem0000077 );

            #endregion

            #region DrugAlcoholSectionRules

            NewItemSkippingRule ( () => SkipItem0000092 )
                .ForItemInstance<int> ( "0000091" )
                .LessThen ( 1 )
                .OrNonResponse ( _nonResponseList )
                .SkipItem ( GetItemDefinition ( "0000092" ) );

            NewRuleSet ( () => ItemUpdatedRuleSet0000091, SkipItem0000092 );

            NewItemSkippingRule ( () => SkipItem0000094 )
                .ForItemInstance<int> ( "0000093" )
                .LessThen ( 1 )
                .OrNonResponse ( _nonResponseList )
                .SkipItem ( GetItemDefinition ( "0000094" ) );

            NewRuleSet ( () => ItemUpdatedRuleSet0000093, SkipItem0000094 );

            NewItemSkippingRule ( () => SkipItem0000096 )
                .ForItemInstance<int> ( "0000095" )
                .LessThen ( 1 )
                .OrNonResponse ( _nonResponseList )
                .SkipItem ( GetItemDefinition ( "0000096" ) );

            NewRuleSet ( () => ItemUpdatedRuleSet0000095, SkipItem0000096 );

            NewItemSkippingRule ( () => SkipItem0000098 )
                .ForItemInstance<int> ( "0000097" )
                .LessThen ( 1 )
                .OrNonResponse ( _nonResponseList )
                .SkipItem ( GetItemDefinition ( "0000098" ) );

            NewRuleSet ( () => ItemUpdatedRuleSet0000097, SkipItem0000098 );

            NewItemSkippingRule ( () => SkipItem0000100 )
                .ForItemInstance<int> ( "0000099" )
                .LessThen ( 1 )
                .OrNonResponse ( _nonResponseList )
                .SkipItem ( GetItemDefinition ( "0000100" ) );

            NewRuleSet ( () => ItemUpdatedRuleSet0000099, SkipItem0000100 );

            NewItemSkippingRule ( () => SkipItem0000102 )
                .ForItemInstance<int> ( "0000101" )
                .LessThen ( 1 )
                .OrNonResponse ( _nonResponseList )
                .SkipItem ( GetItemDefinition ( "0000102" ) );

            NewRuleSet ( () => ItemUpdatedRuleSet0000101, SkipItem0000102 );

            NewItemSkippingRule ( () => SkipItem0000104 )
                .ForItemInstance<int> ( "0000103" )
                .LessThen ( 1 )
                .OrNonResponse ( _nonResponseList )
                .SkipItem ( GetItemDefinition ( "0000104" ) );

            NewRuleSet ( () => ItemUpdatedRuleSet0000103, SkipItem0000104 );

            NewItemSkippingRule ( () => SkipItem0000106 )
                .ForItemInstance<int> ( "0000105" )
                .LessThen ( 1 )
                .OrNonResponse ( _nonResponseList )
                .SkipItem ( GetItemDefinition ( "0000106" ) );

            NewRuleSet ( () => ItemUpdatedRuleSet0000105, SkipItem0000106 );

            NewItemSkippingRule ( () => SkipItem0000108 )
                .ForItemInstance<int> ( "0000107" )
                .LessThen ( 1 )
                .OrNonResponse ( _nonResponseList )
                .SkipItem ( GetItemDefinition ( "0000108" ) );

            NewRuleSet ( () => ItemUpdatedRuleSet0000107, SkipItem0000108 );

            NewItemSkippingRule ( () => SkipItem0000110 )
                .ForItemInstance<int> ( "0000109" )
                .LessThen ( 1 )
                .OrNonResponse ( _nonResponseList )
                .SkipItem ( GetItemDefinition ( "0000110" ) );

            NewRuleSet ( () => ItemUpdatedRuleSet0000109, SkipItem0000110 );

            NewItemSkippingRule ( () => SkipItem0000112 )
                .ForItemInstance<int> ( "0000111" )
                .LessThen ( 1 )
                .OrNonResponse ( _nonResponseList )
                .SkipItem ( GetItemDefinition ( "0000112" ) );

            NewRuleSet ( () => ItemUpdatedRuleSet0000111, SkipItem0000112 );

            NewItemSkippingRule ( () => SkipItem0000114 )
                .ForItemInstance<int> ( "0000113" )
                .LessThen ( 1 )
                .OrNonResponse ( _nonResponseList )
                .SkipItem ( GetItemDefinition ( "0000114" ) );

            NewRuleSet ( () => ItemUpdatedRuleSet0000113, SkipItem0000114 );

            NewItemSkippingRule ( () => SkipItem0000116 )
                .ForItemInstance<int> ( "0000115" )
                .LessThen ( 1 )
                .OrNonResponse ( _nonResponseList )
                .SkipItem ( GetItemDefinition ( "0000116" ) );

            NewRuleSet ( () => ItemUpdatedRuleSet0000115, SkipItem0000116 );

            NewItemSkippingRule ( () => SkipItem0000118 )
                .ForItemInstance<int> ( "0000117" )
                .LessThen ( 1 )
                .OrNonResponse ( _nonResponseList )
                .SkipItem ( GetItemDefinition ( "0000118" ) );

            NewRuleSet ( () => ItemUpdatedRuleSet0000117, SkipItem0000118 );

            NewItemSkippingRule ( () => SkipItem0000120 )
                .ForItemInstance<int> ( "0000119" )
                .LessThen ( 1 )
                .OrNonResponse ( _nonResponseList )
                .SkipItem ( GetItemDefinition ( "0000120" ) );

            NewRuleSet ( () => ItemUpdatedRuleSet0000119, SkipItem0000120 );

            NewItemSkippingRule ( () => SkipItem0000122 )
                .ForItemInstance<int> ( "0000121" )
                .LessThen ( 1 )
                .OrNonResponse ( _nonResponseList )
                .SkipItem ( GetItemDefinition ( "0000122" ) );

            NewRuleSet ( () => ItemUpdatedRuleSet0000121, SkipItem0000122 );

            NewItemSkippingRule ( () => SkipItem0000124 )
                .ForItemInstance<int> ( "0000123" )
                .LessThen ( 1 )
                .OrNonResponse ( _nonResponseList )
                .SkipItem ( GetItemDefinition ( "0000124" ) );

            NewRuleSet ( () => ItemUpdatedRuleSet0000123, SkipItem0000124 );

            NewItemSkippingRule ( () => SkipItem0000126 )
                .ForItemInstance<int> ( "0000125" )
                .LessThen ( 1 )
                .OrNonResponse ( _nonResponseList )
                .SkipItem ( GetItemDefinition ( "0000126" ) );

            NewRuleSet ( () => ItemUpdatedRuleSet0000125, SkipItem0000126 );

            NewItemSkippingRule ( () => SkipItem0000128 )
                .ForItemInstance<int> ( "0000127" )
                .LessThen ( 1 )
                .OrNonResponse ( _nonResponseList )
                .SkipItem ( GetItemDefinition ( "0000128" ) );

            NewRuleSet ( () => ItemUpdatedRuleSet0000127, SkipItem0000128 );

            NewItemSkippingRule ( () => SkipItem0000130 )
                .ForItemInstance<int> ( "0000129" )
                .LessThen ( 1 )
                .OrNonResponse ( _nonResponseList )
                .SkipItem ( GetItemDefinition ( "0000130" ) );

            NewRuleSet ( () => ItemUpdatedRuleSet0000129, SkipItem0000130 );

            NewItemSkippingRule ( () => SkipItem0000132And0000133 )
                .ForItemInstance<int> ( "0000131" )
                .LessThen ( 1 )
                .OrNonResponse ( _nonResponseList )
                .SkipItem ( GetItemDefinition ( "0000132", "0000133" ) );

            NewRuleSet ( () => ItemUpdatedRuleSet0000131, SkipItem0000132And0000133 );

            #endregion

            #region FamilyLivingSituationSectionRules

            NewItemSkippingRule ( () => SkipItem0000137 )
                .ForItemInstance<Lookup> ( "0000136" )
                .NotEqualTo ( FamilyLivingSitationLookups.Housed )
                .SkipItem ( GetItemDefinition ( "0000137" ) );

            NewRuleSet ( () => ItemUpdatedRuleSet0000136, SkipItem0000137 );

            NewItemSkippingRule ( () => SkipItem0000138 )
                .ForItemInstance<Lookup> ( "0000137" )
                .NotEqualTo ( FamilyLivingSitationLookups.Other )
                .SkipItem ( GetItemDefinition ( "0000138" ) );

            NewRuleSet ( () => ItemUpdatedRuleSet0000137, SkipItem0000138 );

            #endregion

            #region ProfessionalInformationSectionRules

            NewItemSkippingRule ( () => SkipItem0000149 )
                .ForItemInstance<Lookup> ( "0000148" )
                .NotEqualTo ( ProfessionalInformationLookups.Other )
                .SkipItem ( GetItemDefinition ( "0000149" ) );

            NewRuleSet ( () => ItemUpdatedRuleSet0000148, SkipItem0000149 );

            NewItemSkippingRule ( () => SkipItem0000152 )
                .ForItemInstance<Lookup> ( "0000151" )
                .NotEqualTo ( ProfessionalInformationLookups.EmploymentOther )
                .SkipItem ( GetItemDefinition ( "0000152" ) );

            NewRuleSet ( () => ItemUpdatedRuleSet0000151, SkipItem0000152 );

            NewItemSkippingRule ( () => SkipItem0000160 ).ForItemInstance<int> ( "0000159" ).LessThen ( 1 ).SkipItem ( GetItemDefinition ( "0000160" ) );

            NewRuleSet ( () => ItemUpdatedRuleSet0000159, SkipItem0000160 );

            #endregion

            #region CriminalJusticeSectionRules

            NewItemSkippingRule ( () => SkipItem0000162 ).ForItemInstance<int> ( "0000161" ).LessThen ( 1 ).SkipItem ( GetItemDefinition ( "0000162" ) );

            NewRuleSet ( () => ItemUpdatedRuleSet0000161, SkipItem0000162 );

            #endregion

            #region TreatmentRecoverySection

            NewItemSkippingRule ( () => SkipItem0000169 ).ForItemInstance<bool> ( "0000168" ).EqualTo ( false ).SkipItem ( GetItemDefinition ( "0000169" ) );

            NewRuleSet ( () => ItemUpdatedRuleSet0000168, SkipItem0000169 );

            NewItemSkippingRule ( () => SkipItem0000171 ).ForItemInstance<bool> ( "0000170" ).EqualTo ( false ).SkipItem ( GetItemDefinition ( "0000171" ) );

            NewRuleSet ( () => ItemUpdatedRuleSet0000170, SkipItem0000171 );

            NewItemSkippingRule ( () => SkipItem0000173 ).ForItemInstance<bool> ( "0000172" ).EqualTo ( false ).SkipItem ( GetItemDefinition ( "0000173" ) );

            NewRuleSet ( () => ItemUpdatedRuleSet0000172, SkipItem0000173 );

            NewItemSkippingRule ( () => SkipItem0000175 ).ForItemInstance<bool> ( "0000174" ).EqualTo ( false ).SkipItem ( GetItemDefinition ( "0000175" ) );

            NewRuleSet ( () => ItemUpdatedRuleSet0000174, SkipItem0000175 );

            NewItemSkippingRule ( () => SkipItem0000177 ).ForItemInstance<bool> ( "0000176" ).EqualTo ( false ).SkipItem ( GetItemDefinition ( "0000177" ) );

            NewRuleSet ( () => ItemUpdatedRuleSet0000176, SkipItem0000177 );

            NewItemSkippingRule ( () => SkipItem0000179 ).ForItemInstance<bool> ( "0000178" ).EqualTo ( false ).SkipItem ( GetItemDefinition ( "0000179" ) );

            NewRuleSet ( () => ItemUpdatedRuleSet0000178, SkipItem0000179 );

            NewItemSkippingRule ( () => SkipItem0000181 ).ForItemInstance<bool> ( "0000180" ).EqualTo ( false ).SkipItem ( GetItemDefinition ( "0000181" ) );

            NewRuleSet ( () => ItemUpdatedRuleSet0000180, SkipItem0000181 );

            NewItemSkippingRule ( () => SkipItem0000183 ).ForItemInstance<bool> ( "0000182" ).EqualTo ( false ).SkipItem ( GetItemDefinition ( "0000183" ) );

            NewRuleSet ( () => ItemUpdatedRuleSet0000182, SkipItem0000183 );

            NewItemSkippingRule ( () => SkipItem0000185 ).ForItemInstance<bool> ( "0000184" ).EqualTo ( false ).SkipItem ( GetItemDefinition ( "0000185" ) );

            NewRuleSet ( () => ItemUpdatedRuleSet0000184, SkipItem0000185 );

            NewItemSkippingRule ( () => SkipItem0000187To0000191 )
                .ForItemInstance<bool> ( "0000186" )
                .EqualTo ( false )
                .OrNonResponse ( _nonResponseList )
                .SkipItem ( GetItemDefinition ( "0000187", "0000188", "0000189", "0000190", "0000191" ) );

            NewRuleSet ( () => ItemUpdatedRuleSet0000186, SkipItem0000187To0000191 );

            NewItemSkippingRule ( () => SkipItem0000193 )
                .ForItemInstance<bool> ( "0000192" )
                .EqualTo ( false )
                .OrNonResponse ( _nonResponseList )
                .SkipItem ( GetItemDefinition ( "0000193" ) );

            NewRuleSet ( () => ItemUpdatedRuleSet0000192, SkipItem0000193 );

            #endregion

            #region SocialConnectednessSection

            NewItemSkippingRule ( () => SkipItem0000203 ).ForItemInstance<bool> ( "0000202" ).EqualTo ( false ).SkipItem ( GetItemDefinition ( "0000203" ) );

            NewRuleSet ( () => ItemUpdatedRuleSet0000202, SkipItem0000203 );

            NewItemSkippingRule ( () => SkipItem0000205 ).ForItemInstance<bool> ( "0000204" ).EqualTo ( false ).SkipItem ( GetItemDefinition ( "0000205" ) );

            NewRuleSet ( () => ItemUpdatedRuleSet0000204, SkipItem0000205 );

            NewItemSkippingRule ( () => SkipItem0000207 ).ForItemInstance<bool> ( "0000206" ).EqualTo ( false ).SkipItem ( GetItemDefinition ( "0000207" ) );

            NewRuleSet ( () => ItemUpdatedRuleSet0000206, SkipItem0000207 );

            NewItemSkippingRule ( () => SkipItem0000210 ).ForItemInstance<Lookup> ( "0000209" ).NotEqualTo ( Gender.Other ).SkipItem ( GetItemDefinition ( "0000210" ) );

            NewRuleSet ( () => ItemUpdatedRuleSet0000209, SkipItem0000210 );

            #endregion

            #region FollowUpSectionRules

            NewItemSkippingRule ( () => SkipItem0000212 )
                .ForItemInstance<Lookup> ( "0000211" )
                .NotEqualTo ( FollowUpLookups.UnableToLocateOther )
                .SkipItem ( GetItemDefinition ( "0000212" ) );

            NewRuleSet ( () => ItemUpdatedRuleSet0000211, SkipItem0000212 );

            #endregion

            #region DischargeSectionRules

            NewItemSkippingRule ( () => SkipItem0000216 )
                .ForItemInstance<Lookup> ( "0000215" )
                .NotEqualTo ( DischargeLookups.Termination )
                .SkipItem ( GetItemDefinition ( "0000216" ) );

            NewRuleSet ( () => ItemUpdatedRuleSet0000215, SkipItem0000216 );

            NewItemSkippingRule ( () => SkipItem0000217 ).ForItemInstance<Lookup> ( "0000216" ).NotEqualTo ( DischargeLookups.Other ).SkipItem ( GetItemDefinition ( "0000217" ) );

            NewRuleSet ( () => ItemUpdatedRuleSet0000216, SkipItem0000217 );

            #endregion
        }

        #endregion

        #region Public Properties

        /// <summary>Gets or sets the answer0000162 shouldnt be greater than0000161.</summary>
        /// <value>The answer0000162 shouldnt be greater than0000161.</value>
        public IItemSkippingRule Answer0000162ShouldntBeGreaterThan0000161 { get; protected set; }

        /// <summary>Gets or sets the rule set0000003.</summary>
        /// <value>The rule set0000003.</value>
        public IRuleSet ItemUpdatedRuleSet0000003 { get; protected set; }

        /// <summary>Gets or sets the rule set0000019.</summary>
        /// <value>The rule set0000019.</value>
        public IRuleSet ItemUpdatedRuleSet0000019 { get; protected set; }

        /// <summary>Gets or sets the rule set0000033.</summary>
        /// <value>The rule set0000033.</value>
        public IRuleSet ItemUpdatedRuleSet0000033 { get; protected set; }

        /// <summary>Gets or sets the rule set0000043.</summary>
        /// <value>The rule set0000043.</value>
        public IRuleSet ItemUpdatedRuleSet0000043 { get; protected set; }

        /// <summary>Gets or sets the rule set0000048.</summary>
        /// <value>The rule set0000048.</value>
        public IRuleSet ItemUpdatedRuleSet0000048 { get; protected set; }

        /// <summary>Gets or sets the rule set0000055.</summary>
        /// <value>The rule set0000055.</value>
        public IRuleSet ItemUpdatedRuleSet0000055 { get; protected set; }

        /// <summary>Gets or sets the rule set0000059.</summary>
        /// <value>The rule set0000059.</value>
        public IRuleSet ItemUpdatedRuleSet0000059 { get; protected set; }

        /// <summary>Gets or sets the rule set0000065.</summary>
        /// <value>The rule set0000065.</value>
        public IRuleSet ItemUpdatedRuleSet0000065 { get; protected set; }

        /// <summary>Gets or sets the rule set0000067.</summary>
        /// <value>The rule set0000067.</value>
        public IRuleSet ItemUpdatedRuleSet0000067 { get; protected set; }

        /// <summary>Gets or sets the rule set0000069.</summary>
        /// <value>The rule set0000069.</value>
        public IRuleSet ItemUpdatedRuleSet0000069 { get; protected set; }

        /// <summary>Gets or sets the rule set0000076.</summary>
        /// <value>The rule set0000076.</value>
        public IRuleSet ItemUpdatedRuleSet0000076 { get; protected set; }

        /// <summary>Gets or sets the rule set0000091.</summary>
        /// <value>The rule set0000091.</value>
        public IRuleSet ItemUpdatedRuleSet0000091 { get; protected set; }

        /// <summary>Gets or sets the rule set0000093.</summary>
        /// <value>The rule set0000093.</value>
        public IRuleSet ItemUpdatedRuleSet0000093 { get; protected set; }

        /// <summary>Gets or sets the rule set0000095.</summary>
        /// <value>The rule set0000095.</value>
        public IRuleSet ItemUpdatedRuleSet0000095 { get; protected set; }

        /// <summary>Gets or sets the rule set0000097.</summary>
        /// <value>The rule set0000097.</value>
        public IRuleSet ItemUpdatedRuleSet0000097 { get; protected set; }

        /// <summary>Gets or sets the rule set0000099.</summary>
        /// <value>The rule set0000099.</value>
        public IRuleSet ItemUpdatedRuleSet0000099 { get; protected set; }

        /// <summary>Gets or sets the rule set0000101.</summary>
        /// <value>The rule set0000101.</value>
        public IRuleSet ItemUpdatedRuleSet0000101 { get; protected set; }

        /// <summary>Gets or sets the rule set0000103.</summary>
        /// <value>The rule set0000103.</value>
        public IRuleSet ItemUpdatedRuleSet0000103 { get; protected set; }

        /// <summary>Gets or sets the rule set0000105.</summary>
        /// <value>The rule set0000105.</value>
        public IRuleSet ItemUpdatedRuleSet0000105 { get; protected set; }

        /// <summary>Gets or sets the rule set0000107.</summary>
        /// <value>The rule set0000107.</value>
        public IRuleSet ItemUpdatedRuleSet0000107 { get; protected set; }

        /// <summary>Gets or sets the rule set0000109.</summary>
        /// <value>The rule set0000109.</value>
        public IRuleSet ItemUpdatedRuleSet0000109 { get; protected set; }

        /// <summary>Gets or sets the rule set0000111.</summary>
        /// <value>The rule set0000111.</value>
        public IRuleSet ItemUpdatedRuleSet0000111 { get; protected set; }

        /// <summary>Gets or sets the rule set0000113.</summary>
        /// <value>The rule set0000113.</value>
        public IRuleSet ItemUpdatedRuleSet0000113 { get; protected set; }

        /// <summary>Gets or sets the rule set0000115.</summary>
        /// <value>The rule set0000115.</value>
        public IRuleSet ItemUpdatedRuleSet0000115 { get; protected set; }

        /// <summary>Gets or sets the rule set0000117.</summary>
        /// <value>The rule set0000117.</value>
        public IRuleSet ItemUpdatedRuleSet0000117 { get; protected set; }

        /// <summary>Gets or sets the rule set0000119.</summary>
        /// <value>The rule set0000119.</value>
        public IRuleSet ItemUpdatedRuleSet0000119 { get; protected set; }

        /// <summary>Gets or sets the rule set0000121.</summary>
        /// <value>The rule set0000121.</value>
        public IRuleSet ItemUpdatedRuleSet0000121 { get; protected set; }

        /// <summary>Gets or sets the rule set0000123.</summary>
        /// <value>The rule set0000123.</value>
        public IRuleSet ItemUpdatedRuleSet0000123 { get; protected set; }

        /// <summary>Gets or sets the rule set0000125.</summary>
        /// <value>The rule set0000125.</value>
        public IRuleSet ItemUpdatedRuleSet0000125 { get; protected set; }

        /// <summary>Gets or sets the rule set0000127.</summary>
        /// <value>The rule set0000127.</value>
        public IRuleSet ItemUpdatedRuleSet0000127 { get; protected set; }

        /// <summary>Gets or sets the rule set0000129.</summary>
        /// <value>The rule set0000129.</value>
        public IRuleSet ItemUpdatedRuleSet0000129 { get; protected set; }

        /// <summary>Gets or sets the rule set0000131.</summary>
        /// <value>The rule set0000131.</value>
        public IRuleSet ItemUpdatedRuleSet0000131 { get; protected set; }

        /// <summary>Gets or sets the rule set0000136.</summary>
        /// <value>The rule set0000136.</value>
        public IRuleSet ItemUpdatedRuleSet0000136 { get; protected set; }

        /// <summary>Gets or sets the rule set0000137.</summary>
        /// <value>The rule set0000137.</value>
        public IRuleSet ItemUpdatedRuleSet0000137 { get; protected set; }

        /// <summary>Gets or sets the rule set0000148.</summary>
        /// <value>The rule set0000148.</value>
        public IRuleSet ItemUpdatedRuleSet0000148 { get; protected set; }

        /// <summary>Gets or sets the rule set0000151.</summary>
        /// <value>The rule set0000151.</value>
        public IRuleSet ItemUpdatedRuleSet0000151 { get; protected set; }

        /// <summary>Gets or sets the rule set0000159.</summary>
        /// <value>The rule set0000159.</value>
        public IRuleSet ItemUpdatedRuleSet0000159 { get; protected set; }

        /// <summary>Gets or sets the rule set0000161.</summary>
        /// <value>The rule set0000161.</value>
        public IRuleSet ItemUpdatedRuleSet0000161 { get; protected set; }

        /// <summary>Gets or sets the rule set0000162.</summary>
        /// <value>The rule set0000162.</value>
        public IRuleSet ItemUpdatedRuleSet0000162 { get; protected set; }

        /// <summary>Gets or sets the rule set0000168.</summary>
        /// <value>The rule set0000168.</value>
        public IRuleSet ItemUpdatedRuleSet0000168 { get; protected set; }

        /// <summary>Gets or sets the rule set0000170.</summary>
        /// <value>The rule set0000170.</value>
        public IRuleSet ItemUpdatedRuleSet0000170 { get; protected set; }

        /// <summary>Gets or sets the rule set0000172.</summary>
        /// <value>The rule set0000172.</value>
        public IRuleSet ItemUpdatedRuleSet0000172 { get; protected set; }

        /// <summary>Gets or sets the rule set0000174.</summary>
        /// <value>The rule set0000174.</value>
        public IRuleSet ItemUpdatedRuleSet0000174 { get; protected set; }

        /// <summary>Gets or sets the rule set0000176.</summary>
        /// <value>The rule set0000176.</value>
        public IRuleSet ItemUpdatedRuleSet0000176 { get; protected set; }

        /// <summary>Gets or sets the rule set0000178.</summary>
        /// <value>The rule set0000178.</value>
        public IRuleSet ItemUpdatedRuleSet0000178 { get; protected set; }

        /// <summary>Gets or sets the rule set0000180.</summary>
        /// <value>The rule set0000180.</value>
        public IRuleSet ItemUpdatedRuleSet0000180 { get; protected set; }

        /// <summary>Gets or sets the rule set0000182.</summary>
        /// <value>The rule set0000182.</value>
        public IRuleSet ItemUpdatedRuleSet0000182 { get; protected set; }

        /// <summary>Gets or sets the rule set0000184.</summary>
        /// <value>The rule set0000184.</value>
        public IRuleSet ItemUpdatedRuleSet0000184 { get; protected set; }

        /// <summary>Gets or sets the rule set0000186.</summary>
        /// <value>The rule set0000186.</value>
        public IRuleSet ItemUpdatedRuleSet0000186 { get; protected set; }

        /// <summary>Gets or sets the rule set0000192.</summary>
        /// <value>The rule set0000192.</value>
        public IRuleSet ItemUpdatedRuleSet0000192 { get; protected set; }

        /// <summary>Gets or sets the rule set0000202.</summary>
        /// <value>The rule set0000202.</value>
        public IRuleSet ItemUpdatedRuleSet0000202 { get; protected set; }

        /// <summary>Gets or sets the rule set0000204.</summary>
        /// <value>The rule set0000204.</value>
        public IRuleSet ItemUpdatedRuleSet0000204 { get; protected set; }

        /// <summary>Gets or sets the rule set0000206.</summary>
        /// <value>The rule set0000206.</value>
        public IRuleSet ItemUpdatedRuleSet0000206 { get; protected set; }

        /// <summary>Gets or sets the rule set0000209.</summary>
        /// <value>The rule set0000209.</value>
        public IRuleSet ItemUpdatedRuleSet0000209 { get; protected set; }

        /// <summary>Gets or sets the rule set0000211.</summary>
        /// <value>The rule set0000211.</value>
        public IRuleSet ItemUpdatedRuleSet0000211 { get; protected set; }

        /// <summary>Gets or sets the rule set0000215.</summary>
        /// <value>The rule set0000215.</value>
        public IRuleSet ItemUpdatedRuleSet0000215 { get; protected set; }

        /// <summary>Gets or sets the rule set0000216.</summary>
        /// <value>The rule set0000216.</value>
        public IRuleSet ItemUpdatedRuleSet0000216 { get; protected set; }

        /// <summary> Gets or sets the skip demographics section. </summary>
        /// <value> The skip demographics section. </value>
        public IItemSkippingRule SkipDemographicsSection { get; protected set; }

        /// <summary>Gets or sets the should clear0000004.</summary>
        /// <value>The should clear0000004.</value>
        public IItemSkippingRule SkipItem0000004 { get; protected set; }

        /// <summary>Gets or sets the should clear0000020.</summary>
        /// <value>The should clear0000020.</value>
        public IItemSkippingRule SkipItem0000020 { get; protected set; }

        /// <summary>Gets or sets the should clear0000034.</summary>
        /// <value>The should clear0000034.</value>
        public IItemSkippingRule SkipItem0000034 { get; protected set; }

        /// <summary>Gets or sets the should clear0000044.</summary>
        /// <value>The should clear0000044.</value>
        public IItemSkippingRule SkipItem0000044 { get; protected set; }

        /// <summary>Gets or sets the should clear0000049.</summary>
        /// <value>The should clear0000049.</value>
        public IItemSkippingRule SkipItem0000049 { get; protected set; }

        /// <summary>Gets or sets the should clear0000056.</summary>
        /// <value>The should clear0000056.</value>
        public IItemSkippingRule SkipItem0000056 { get; protected set; }

        /// <summary>Gets or sets the should clear0000060.</summary>
        /// <value>The should clear0000060.</value>
        public IItemSkippingRule SkipItem0000060 { get; protected set; }

        /// <summary>Gets or sets the should clear0000066.</summary>
        /// <value>The should clear0000066.</value>
        public IItemSkippingRule SkipItem0000066 { get; protected set; }

        /// <summary>Gets or sets the should clear0000068.</summary>
        /// <value>The should clear0000068.</value>
        public IItemSkippingRule SkipItem0000068 { get; protected set; }

        /// <summary>Gets or sets the should clear0000070 to0000077.</summary>
        /// <value>The should clear0000070 to0000077.</value>
        public IItemSkippingRule SkipItem0000070To0000077 { get; protected set; }

        /// <summary>Gets or sets the should clear0000077.</summary>
        /// <value>The should clear0000077.</value>
        public IItemSkippingRule SkipItem0000077 { get; protected set; }

        /// <summary>Gets or sets the should clear0000092.</summary>
        /// <value>The should clear0000092.</value>
        public IItemSkippingRule SkipItem0000092 { get; protected set; }

        /// <summary>Gets or sets the should clear0000094.</summary>
        /// <value>The should clear0000094.</value>
        public IItemSkippingRule SkipItem0000094 { get; protected set; }

        /// <summary>Gets or sets the should clear0000096.</summary>
        /// <value>The should clear0000096.</value>
        public IItemSkippingRule SkipItem0000096 { get; protected set; }

        /// <summary>Gets or sets the should clear0000098.</summary>
        /// <value>The should clear0000098.</value>
        public IItemSkippingRule SkipItem0000098 { get; protected set; }

        /// <summary>Gets or sets the should clear0000100.</summary>
        /// <value>The should clear0000100.</value>
        public IItemSkippingRule SkipItem0000100 { get; protected set; }

        /// <summary>Gets or sets the should clear0000102.</summary>
        /// <value>The should clear0000102.</value>
        public IItemSkippingRule SkipItem0000102 { get; protected set; }

        /// <summary>Gets or sets the should clear0000104.</summary>
        /// <value>The should clear0000104.</value>
        public IItemSkippingRule SkipItem0000104 { get; protected set; }

        /// <summary>Gets or sets the should clear0000106.</summary>
        /// <value>The should clear0000106.</value>
        public IItemSkippingRule SkipItem0000106 { get; protected set; }

        /// <summary>Gets or sets the should clear0000108.</summary>
        /// <value>The should clear0000108.</value>
        public IItemSkippingRule SkipItem0000108 { get; protected set; }

        /// <summary>Gets or sets the should clear0000110.</summary>
        /// <value>The should clear0000110.</value>
        public IItemSkippingRule SkipItem0000110 { get; protected set; }

        /// <summary>Gets or sets the should clear0000112.</summary>
        /// <value>The should clear0000112.</value>
        public IItemSkippingRule SkipItem0000112 { get; protected set; }

        /// <summary>Gets or sets the should clear0000114.</summary>
        /// <value>The should clear0000114.</value>
        public IItemSkippingRule SkipItem0000114 { get; protected set; }

        /// <summary>Gets or sets the should clear0000116.</summary>
        /// <value>The should clear0000116.</value>
        public IItemSkippingRule SkipItem0000116 { get; protected set; }

        /// <summary>Gets or sets the should clear0000118.</summary>
        /// <value>The should clear0000118.</value>
        public IItemSkippingRule SkipItem0000118 { get; protected set; }

        /// <summary>Gets or sets the should clear0000120.</summary>
        /// <value>The should clear0000120.</value>
        public IItemSkippingRule SkipItem0000120 { get; protected set; }

        /// <summary>Gets or sets the should clear0000122.</summary>
        /// <value>The should clear0000122.</value>
        public IItemSkippingRule SkipItem0000122 { get; protected set; }

        /// <summary>Gets or sets the should clear0000124.</summary>
        /// <value>The should clear0000124.</value>
        public IItemSkippingRule SkipItem0000124 { get; protected set; }

        /// <summary>Gets or sets the should clear0000126.</summary>
        /// <value>The should clear0000126.</value>
        public IItemSkippingRule SkipItem0000126 { get; protected set; }

        /// <summary>Gets or sets the should clear0000128.</summary>
        /// <value>The should clear0000128.</value>
        public IItemSkippingRule SkipItem0000128 { get; protected set; }

        /// <summary>Gets or sets the should clear0000130.</summary>
        /// <value>The should clear0000130.</value>
        public IItemSkippingRule SkipItem0000130 { get; protected set; }

        /// <summary>Gets or sets the should clear0000132and0000133.</summary>
        /// <value>The should clear0000132and0000133.</value>
        public IItemSkippingRule SkipItem0000132And0000133 { get; protected set; }

        /// <summary>Gets or sets the should clear0000137.</summary>
        /// <value>The should clear0000137.</value>
        public IItemSkippingRule SkipItem0000137 { get; protected set; }

        /// <summary>Gets or sets the should clear0000138.</summary>
        /// <value>The should clear0000138.</value>
        public IItemSkippingRule SkipItem0000138 { get; protected set; }

        /// <summary>Gets or sets the should clear0000149.</summary>
        /// <value>The should clear0000149.</value>
        public IItemSkippingRule SkipItem0000149 { get; protected set; }

        /// <summary>Gets or sets the should clear0000152.</summary>
        /// <value>The should clear0000152.</value>
        public IItemSkippingRule SkipItem0000152 { get; protected set; }

        /// <summary>Gets or sets the should clear0000160.</summary>
        /// <value>The should clear0000160.</value>
        public IItemSkippingRule SkipItem0000160 { get; protected set; }

        /// <summary>Gets or sets the should clear0000162.</summary>
        /// <value>The should clear0000162.</value>
        public IItemSkippingRule SkipItem0000162 { get; protected set; }

        /// <summary>Gets or sets the should clear0000169.</summary>
        /// <value>The should clear0000169.</value>
        public IItemSkippingRule SkipItem0000169 { get; protected set; }

        /// <summary>Gets or sets the should clear0000171.</summary>
        /// <value>The should clear0000171.</value>
        public IItemSkippingRule SkipItem0000171 { get; protected set; }

        /// <summary>Gets or sets the should clear0000173.</summary>
        /// <value>The should clear0000173.</value>
        public IItemSkippingRule SkipItem0000173 { get; protected set; }

        /// <summary>Gets or sets the should clear0000175.</summary>
        /// <value>The should clear0000175.</value>
        public IItemSkippingRule SkipItem0000175 { get; protected set; }

        /// <summary>Gets or sets the should clear0000177.</summary>
        /// <value>The should clear0000177.</value>
        public IItemSkippingRule SkipItem0000177 { get; protected set; }

        /// <summary>Gets or sets the should clear0000179.</summary>
        /// <value>The should clear0000179.</value>
        public IItemSkippingRule SkipItem0000179 { get; protected set; }

        /// <summary>Gets or sets the should clear0000181.</summary>
        /// <value>The should clear0000181.</value>
        public IItemSkippingRule SkipItem0000181 { get; protected set; }

        /// <summary>Gets or sets the should clear0000183.</summary>
        /// <value>The should clear0000183.</value>
        public IItemSkippingRule SkipItem0000183 { get; protected set; }

        /// <summary>Gets or sets the should clear0000185.</summary>
        /// <value>The should clear0000185.</value>
        public IItemSkippingRule SkipItem0000185 { get; protected set; }

        /// <summary>Gets or sets the should clear0000187 to0000191.</summary>
        /// <value>The should clear0000187 to0000191.</value>
        public IItemSkippingRule SkipItem0000187To0000191 { get; protected set; }

        /// <summary>Gets or sets the should clear0000193.</summary>
        /// <value>The should clear0000193.</value>
        public IItemSkippingRule SkipItem0000193 { get; protected set; }

        /// <summary>Gets or sets the should clear0000203.</summary>
        /// <value>The should clear0000203.</value>
        public IItemSkippingRule SkipItem0000203 { get; protected set; }

        /// <summary>Gets or sets the should clear0000205.</summary>
        /// <value>The should clear0000205.</value>
        public IItemSkippingRule SkipItem0000205 { get; protected set; }

        /// <summary>Gets or sets the should clear0000207.</summary>
        /// <value>The should clear0000207.</value>
        public IItemSkippingRule SkipItem0000207 { get; protected set; }

        /// <summary>Gets or sets the should clear0000210.</summary>
        /// <value>The should clear0000210.</value>
        public IItemSkippingRule SkipItem0000210 { get; protected set; }

        /// <summary>Gets or sets the should clear0000212.</summary>
        /// <value>The should clear0000212.</value>
        public IItemSkippingRule SkipItem0000212 { get; protected set; }

        /// <summary>Gets or sets the should clear0000216.</summary>
        /// <value>The should clear0000216.</value>
        public IItemSkippingRule SkipItem0000216 { get; protected set; }

        /// <summary>Gets or sets the should clear0000217.</summary>
        /// <value>The should clear0000217.</value>
        public IItemSkippingRule SkipItem0000217 { get; protected set; }

        #endregion

        #region Methods

        private ItemDefinition GetItemDefinition ( string itemDefinitionCode )
        {
            if ( !_gpraAssessmentDefinitionKey.HasValue )
            {
                _gpraAssessmentDefinitionKey = _assessmentDefinitionRepository.GetKeyByCode ( GpraInterview.AssessmentCodedConcept.Code );
            }

            var assessmentDefinition = _assessmentDefinitionRepository.GetByKey ( _gpraAssessmentDefinitionKey.Value );
            return assessmentDefinition.GetItemDefinitionByCode ( itemDefinitionCode );
        }

        private ItemDefinition[] GetItemDefinition ( params string[] itemDefinitionCode )
        {
            return itemDefinitionCode.Select ( GetItemDefinition ).ToArray ();
        }

        #endregion
    }
}