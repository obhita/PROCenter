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

namespace ProCenter.Domain.Nida
{
    #region Using Statements

    using ProCenter.Domain.AssessmentModule;
    using ProCenter.Domain.AssessmentModule.Attributes;
    using ProCenter.Domain.CommonModule;

    #endregion

    /// <summary>The drug abuse screening test class.</summary>
    [CodeSystem ( CodeSystems.NciCode )]
    [Code("3254100")]
    [ScoreType(ScoreTypeEnum.ScoreTypeInt)]
    public class DrugAbuseScreeningTest : Assessment
    {
        #region Constructors and Destructors

        /// <summary>Initializes static members of the <see cref="DrugAbuseScreeningTest" /> class.</summary>
        static DrugAbuseScreeningTest ()
        {
            AssessmentCodedConcept = GetCodedConcept<DrugAbuseScreeningTest>();
        }

        /// <summary>Initializes a new instance of the <see cref="DrugAbuseScreeningTest"/> class.</summary>
        public DrugAbuseScreeningTest()
            : this ( null )
        {
        }

        /// <summary>Initializes a new instance of the <see cref="DrugAbuseScreeningTest" /> class.</summary>
        /// <param name="assessmentInstance">The assessment instance.</param>
        public DrugAbuseScreeningTest ( AssessmentInstance assessmentInstance )
            : base ( assessmentInstance )
        {
        }

        #endregion

        #region Public Properties

        /// <summary>Gets the assessment coded concept.</summary>
        /// <value>The assessment coded concept.</value>
        public static CodedConcept AssessmentCodedConcept { get; private set; }

        /// <summary>
        ///     Gets a value indicating whether [substance abuse prescription illicit
        ///     substance overthe counter product associated diseaseor disorder personal medical history ind2].
        /// </summary>
        /// <value>
        ///     <c>True</c> if [substance abuse prescription illicit substance overthe counter product associated diseaseor
        ///     disorder personal medical history ind2]; otherwise, <c>False</c>.
        /// </value>
        [Code ( "3254072" )]
        [ValueType ( typeof(NidaValueType), NidaValueType.YesOrNoResponseCode )]
        [DisplayOrder ( 9 )]
        public bool SubstanceAbusePrescriptionIllicitSubstanceOvertheCounterProductAssociatedDiseaseorDisorderPersonalMedicalHistoryInd2 { get; private set; }

        /// <summary>
        ///     Gets a value indicating whether [substance abuse prescription illicit
        ///     substance overthe counter product blackout flashbacks personal medical history ind2].
        /// </summary>
        /// <value>
        ///     <c>True</c> if [substance abuse prescription illicit substance overthe counter product blackout flashbacks personal
        ///     medical history ind2]; otherwise, <c>False</c>.
        /// </value>
        [Code ( "3254061" )]
        [ValueType ( typeof(NidaValueType), NidaValueType.YesOrNoResponseCode )]
        [DisplayOrder ( 3 )]
        public bool SubstanceAbusePrescriptionIllicitSubstanceOvertheCounterProductBlackoutFlashbacksPersonalMedicalHistoryInd2 { get; private set; }

        /// <summary>
        ///     Gets a value indicating whether [substance abuse prescription illicit
        ///     substance overthe counter product cessation ability personal medical history ind2].
        /// </summary>
        /// <value>
        ///     <c>True</c> if [substance abuse prescription illicit substance overthe counter product cessation ability personal
        ///     medical history ind2]; otherwise, <c>False</c>.
        /// </value>
        [Code ( "3254058" )]
        [ValueType ( typeof(NidaValueType), NidaValueType.YesOrNoResponseCode )]
        [DisplayOrder ( 2 )]
        public bool SubstanceAbusePrescriptionIllicitSubstanceOvertheCounterProductCessationAbilityPersonalMedicalHistoryInd2 { get; private set; }

        /// <summary>
        ///     Gets a value indicating whether [substance abuse prescription illicit
        ///     substance overthe counter product concurrent use personal medical history ind2].
        /// </summary>
        /// <value>
        ///     <c>True</c> if [substance abuse prescription illicit substance overthe counter product concurrent use personal
        ///     medical history ind2]; otherwise, <c>False</c>.
        /// </value>
        [Code ( "3254057" )]
        [ValueType ( typeof(NidaValueType), NidaValueType.YesOrNoResponseCode )]
        [DisplayOrder ( 1 )]
        public bool SubstanceAbusePrescriptionIllicitSubstanceOvertheCounterProductConcurrentUsePersonalMedicalHistoryInd2 { get; private set; }

        /// <summary>
        ///     Gets a value indicating whether [substance abuse prescription illicit
        ///     substance overthe counter product crime obtain personal medical history ind2].
        /// </summary>
        /// <value>
        ///     <c>True</c> if [substance abuse prescription illicit substance overthe counter product crime obtain personal
        ///     medical history ind2]; otherwise, <c>False</c>.
        /// </value>
        [Code ( "3254067" )]
        [ValueType ( typeof(NidaValueType), NidaValueType.YesOrNoResponseCode )]
        [DisplayOrder ( 7 )]
        public bool SubstanceAbusePrescriptionIllicitSubstanceOvertheCounterProductCrimeObtainPersonalMedicalHistoryInd2 { get; private set; }

        /// <summary>
        ///     Gets a value indicating whether [substance abuse prescription illicit
        ///     substance overthe counter product domestic partnership spouse complain personal medical history ind2].
        /// </summary>
        /// <value>
        ///     <c>True</c> if [substance abuse prescription illicit substance overthe counter product domestic partnership spouse
        ///     complain personal medical history ind2]; otherwise, <c>False</c>.
        /// </value>
        [Code ( "3254065" )]
        [ValueType ( typeof(NidaValueType), NidaValueType.YesOrNoResponseCode )]
        [DisplayOrder ( 5 )]
        public bool SubstanceAbusePrescriptionIllicitSubstanceOvertheCounterProductDomesticPartnershipSpouseComplainPersonalMedicalHistoryInd2 { get; private set; }

        /// <summary>
        ///     Gets a value indicating whether [substance abuse prescription illicit
        ///     substance overthe counter product family neglect personal medical history ind2].
        /// </summary>
        /// <value>
        ///     <c>True</c> if [substance abuse prescription illicit substance overthe counter product family neglect personal
        ///     medical history ind2]; otherwise, <c>False</c>.
        /// </value>
        [Code ( "3254066" )]
        [ValueType ( typeof(NidaValueType), NidaValueType.YesOrNoResponseCode )]
        [DisplayOrder ( 6 )]
        public bool SubstanceAbusePrescriptionIllicitSubstanceOvertheCounterProductFamilyNeglectPersonalMedicalHistoryInd2 { get; private set; }

        /// <summary>
        ///     Gets a value indicating whether [substance abuse prescription illicit
        ///     substance overthe counter product guilt regret personal medical history ind2].
        /// </summary>
        /// <value>
        ///     <c>True</c> if [substance abuse prescription illicit substance overthe counter product guilt regret personal
        ///     medical history ind2]; otherwise, <c>False</c>.
        /// </value>
        [Code ( "3254063" )]
        [ValueType ( typeof(NidaValueType), NidaValueType.YesOrNoResponseCode )]
        [DisplayOrder ( 4 )]
        public bool SubstanceAbusePrescriptionIllicitSubstanceOvertheCounterProductGuiltRegretPersonalMedicalHistoryInd2 { get; private set; }

        /// <summary>
        ///     Gets a value indicating whether [substance abuse prescription illicit
        ///     substance overthe counter product personal medical history ind2].
        /// </summary>
        /// <value>
        ///     <c>True</c> if [substance abuse prescription illicit substance overthe counter product personal medical history
        ///     ind2]; otherwise, <c>False</c>.
        /// </value>
        [Code ( "3254039" )]
        [ValueType ( typeof(NidaValueType), NidaValueType.YesOrNoResponseCode )]
        [DisplayOrder ( 0 )]
        public bool SubstanceAbusePrescriptionIllicitSubstanceOvertheCounterProductPersonalMedicalHistoryInd2 { get; private set; }

        /// <summary>
        ///     Gets a value indicating whether [substance abuse prescription illicit
        ///     substance overthe counter product substance withdrawal syndrome personal medical history ind2].
        /// </summary>
        /// <value>
        ///     <c>True</c> if [substance abuse prescription illicit substance overthe counter product substance withdrawal
        ///     syndrome personal medical history ind2]; otherwise, <c>False</c>.
        /// </value>
        [Code ( "3254070" )]
        [ValueType ( typeof(NidaValueType), NidaValueType.YesOrNoResponseCode )]
        [DisplayOrder ( 8 )]
        public bool SubstanceAbusePrescriptionIllicitSubstanceOvertheCounterProductSubstanceWithdrawalSyndromePersonalMedicalHistoryInd2 { get; private set; }

        #endregion
    }
}