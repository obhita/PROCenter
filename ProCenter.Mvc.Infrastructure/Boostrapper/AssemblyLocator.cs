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

#region Using Statements



#endregion

namespace ProCenter.Mvc.Infrastructure.Boostrapper
{
    #region Using Statements

    using System.Collections.Generic;
    using System.Reflection;
    using Domain.PatientModule;
    using Pillar.Common.Bootstrapper;
    using Primitive;
    using ProCenter.Infrastructure;

    #endregion

    /// <summary>Assembly Locator for ProCenter.</summary>
    public class AssemblyLocator : IAssemblyLocator
    {
        #region Fields

        private readonly IEnumerable<Assembly> _domainAssemblies;
        private readonly IEnumerable<Assembly> _infrastructureAssemblies;
        private readonly IEnumerable<Assembly> _webServiceAssemblies;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="AssemblyLocator" /> class.
        /// </summary>
        public AssemblyLocator ()
        {
            _domainAssemblies = new List<Assembly>
            {
                Assembly.GetAssembly ( typeof(Patient) ),
                Assembly.GetAssembly ( typeof(PersonName) )
            };
            _infrastructureAssemblies = new List<Assembly>
            {
                GetType ().Assembly,
                Assembly.GetAssembly ( typeof(IUnitOfWork) )
            };
            _webServiceAssemblies = new List<Assembly> ();
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Locates the domain assemblies.
        /// </summary>
        /// <returns>
        ///     An <see cref="T:System.Collections.Generic.IEnumerable`1" />
        /// </returns>
        public IEnumerable<Assembly> LocateDomainAssemblies ()
        {
            return _domainAssemblies;
        }

        /// <summary>
        ///     Locates the infrastructure assemblies.
        /// </summary>
        /// <returns>
        ///     An <see cref="T:System.Collections.Generic.IEnumerable`1" />
        /// </returns>
        public IEnumerable<Assembly> LocateInfrastructureAssemblies ()
        {
            return _infrastructureAssemblies;
        }

        /// <summary>
        ///     Locates the web service assemblies.
        /// </summary>
        /// <returns>
        ///     An <see cref="T:System.Collections.Generic.IEnumerable`1" />
        /// </returns>
        public IEnumerable<Assembly> LocateWebServiceAssemblies ()
        {
            return _webServiceAssemblies;
        }

        #endregion
    }
}