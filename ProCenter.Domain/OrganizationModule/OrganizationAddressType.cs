using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProCenter.Domain.OrganizationModule
{
    using CommonModule;
    using CommonModule.Lookups;

    public class OrganizationAddressType : Lookup
    {

        private static readonly CodeSystem CodeSystem = CodeSystems.Obhita;

        /// <summary>
        ///     Office = 2.
        /// </summary>
        public static readonly OrganizationAddressType Office = new OrganizationAddressType
        {
            CodedConcept = new CodedConcept(code: "Office", codeSystem: CodeSystem, name: "Office"),
            SortOrder = 1,
            Value = 0
        };
    }
}
