namespace ProCenter.Mvc.Infrastructure.Service
{
    #region Using Statements

    using System;
    using System.Resources;
    using Domain.CommonModule;
    using Domain.CommonModule.Lookups;
    using Pillar.Common.InversionOfControl;
    using Primitive;
    using StructureMap.Configuration.DSL;
    using StructureMap.Graph;
    using StructureMap.Pipeline;

    #endregion

    /// <summary>
    ///     Convention to automatically register Lookup Resources
    /// </summary>
    public class LookupPrimitiveResourceConvention : IRegistrationConvention
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Processes the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="registry">The registry.</param>
        public void Process ( Type type, Registry registry )
        {
            if ( typeof(Lookup).IsAssignableFrom ( type ) && type != typeof(Lookup) || typeof(IPrimitive).IsAssignableFrom ( type ) )
            {
                registry.For<ResourceManager> ()
                        .Use ( () => new ResourceManager ( type ) )
                        .Named ( type.Name );
            }
        }

        #endregion
    }
}