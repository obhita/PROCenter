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

namespace ProCenter.Domain.GainShortScreener
{
    #region Using Statements

    using ProCenter.Common;

    #endregion

    /// <summary>
    ///     Bootstrapper task for registering resource managers.
    /// </summary>
    public class RegisterResourcesBootstrapperTask : IOrderedBootstrapperTask
    {
        #region Fields

        private readonly IResourcesManager _resourcesManager;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="RegisterResourcesBootstrapperTask" /> class.
        /// </summary>
        /// <param name="resourcesManager">The resource manager provider.</param>
        public RegisterResourcesBootstrapperTask ( IResourcesManager resourcesManager )
        {
            _resourcesManager = resourcesManager;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the order.
        /// </summary>
        /// <value>
        /// The order.
        /// </value>
        public int Order { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Executes this instance.
        /// </summary>
        public void Execute ()
        {
            _resourcesManager.Register<GainShortScreener> ( GainShortScreener.AssessmentCodedConcept.Code );
        }

        #endregion
    }
}