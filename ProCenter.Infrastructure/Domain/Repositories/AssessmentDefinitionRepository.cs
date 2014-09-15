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

namespace ProCenter.Infrastructure.Domain.Repositories
{
    #region Using Statements

    using System;
    using System.Linq;

    using Dapper;

    using ProCenter.Common;
    using ProCenter.Domain.AssessmentModule;

    #endregion

    /// <summary>The assessment definition repository class.</summary>
    public class AssessmentDefinitionRepository : RepositoryBase<AssessmentDefinition>, IAssessmentDefinitionRepository
    {
        #region Fields

        private readonly IDbConnectionFactory _connectionFactory;

        #endregion

        #region Constructors and Destructors

        /// <summary>Initializes a new instance of the <see cref="AssessmentDefinitionRepository" /> class.</summary>
        /// <param name="connectionFactory">The connection factory.</param>
        /// <param name="unitOfWorkProvider">The unit of work provider.</param>
        public AssessmentDefinitionRepository ( IDbConnectionFactory connectionFactory, IUnitOfWorkProvider unitOfWorkProvider )
            : base ( unitOfWorkProvider )
        {
            _connectionFactory = connectionFactory;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>Gets the key by code.</summary>
        /// <param name="assessmentDefinitionCode">The assessment definition code.</param>
        /// <returns>A <see cref="Guid" />.</returns>
        /// <exception cref="System.ArgumentException">The assessmentDefinitionCode is not defined.</exception>
        public Guid GetKeyByCode ( string assessmentDefinitionCode )
        {
            using ( var connection = _connectionFactory.CreateConnection () )
            {
                var key = connection.Query<Guid?> (
                                                   "Select AssessmentDefinitionKey from [AssessmentModule].[AssessmentDefinition] Where AssessmentCode = @code",
                    new { code = assessmentDefinitionCode } ).SingleOrDefault ();
                if ( !key.HasValue )
                {
                    throw new ArgumentException (
                        string.Format ( "There is no assessment definition with the code {0}", assessmentDefinitionCode ),
                        "assessmentDefinitionCode" );
                }
                return key.Value;
            }
        }

        #endregion
    }
}