#region Using Statements

using System.Collections.Generic;
using System.Reflection;
using Pillar.Common.Bootstrapper;
using ProCenter.Domain.PatientModule;
using ProCenter.Infrastructure;
using ProCenter.Primitive;

#endregion

namespace ProCenter.Mvc.Infrastructure.Boostrapper
{
    /// <summary>
    ///     Assembly Locator for ProCenter
    /// </summary>
    public class AssemblyLocator : IAssemblyLocator
    {
        private readonly IEnumerable<Assembly> _domainAssemblies;
        private readonly IEnumerable<Assembly> _infrastructureAssemblies;
        private readonly IEnumerable<Assembly> _webServiceAssemblies;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AssemblyLocator" /> class.
        /// </summary>
        public AssemblyLocator()
        {
            _domainAssemblies = new List<Assembly>
                {
                    Assembly.GetAssembly(typeof (Patient)),
                    Assembly.GetAssembly(typeof (PersonName))
                };
            _infrastructureAssemblies = new List<Assembly>
                {
                    GetType().Assembly,
                    Assembly.GetAssembly(typeof (IUnitOfWork))
                };
            _webServiceAssemblies = new List<Assembly>();
        }

        /// <summary>
        ///     Locates the domain assemblies.
        /// </summary>
        /// <returns>
        ///     An <see cref="T:System.Collections.Generic.IEnumerable`1" />
        /// </returns>
        public IEnumerable<Assembly> LocateDomainAssemblies()
        {
            return _domainAssemblies;
        }

        /// <summary>
        ///     Locates the infrastructure assemblies.
        /// </summary>
        /// <returns>
        ///     An <see cref="T:System.Collections.Generic.IEnumerable`1" />
        /// </returns>
        public IEnumerable<Assembly> LocateInfrastructureAssemblies()
        {
            return _infrastructureAssemblies;
        }

        /// <summary>
        ///     Locates the web service assemblies.
        /// </summary>
        /// <returns>
        ///     An <see cref="T:System.Collections.Generic.IEnumerable`1" />
        /// </returns>
        public IEnumerable<Assembly> LocateWebServiceAssemblies()
        {
            return _webServiceAssemblies;
        }
    }
}