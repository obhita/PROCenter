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

namespace ProCenter.Domain.AssessmentModule
{
    /// <summary>The assessment class.</summary>
    public abstract class Assessment : AssessmentPart
    {
        #region Constructors and Destructors

        /// <summary>Initializes a new instance of the <see cref="Assessment" /> class.</summary>
        /// <param name="assessmentInstance">The assessment instance.</param>
        protected Assessment ( AssessmentInstance assessmentInstance )
            : base (assessmentInstance)
        {
            var type = GetType();
            Name = type.Name;
            AssessmentInstance = assessmentInstance;
        }

        #endregion

        #region Public Properties

        /// <summary>Gets or sets the name.</summary>
        /// <value>The name.</value>
        public string Name { get; protected set; }

        /// <summary>
        /// Gets or sets the assessment instance.
        /// </summary>
        /// <value>
        /// The assessment instance.
        /// </value>
        public AssessmentInstance AssessmentInstance { get; protected set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>Creates the assessment definition.</summary>
        /// <returns>A <see cref="AssessmentDefinition"/>.</returns>
        public AssessmentDefinition CreateAssessmentDefinition ()
        {
            var type = GetType ();
            var assessmentDefinition = new AssessmentDefinition ( GetCodedConcept ( type ), GetScoreType ( type ) );
            foreach (var propertyInfo in GetOrderedProperties ())
            {
                if ( typeof(IItemDefinitionProvider).IsAssignableFrom ( propertyInfo.PropertyType ) )
                {
                    var itemDefinitionProvider = propertyInfo.GetValue ( this ) as IItemDefinitionProvider;
                    assessmentDefinition.AddItemDefinition (
                                                            itemDefinitionProvider.CreateItemDefinition (
                                                                                                         GetCodedConcept (
                                                                                                                          propertyInfo,
                                                                                                             assessmentDefinition.CodedConcept.CodeSystem ) ) );
                }
                else
                {
                    assessmentDefinition.AddItemDefinition ( CreateItemDefinitionForQuestionProperty ( propertyInfo, assessmentDefinition.CodedConcept.CodeSystem ) );
                }
            }
            return assessmentDefinition;
        }

        #endregion
    }
}