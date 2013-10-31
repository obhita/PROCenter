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
namespace ProCenter.Infrastructure.Service
{
    #region Using Statements

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Common;
    using Pillar.Common.InversionOfControl;
    using ProCenter.Domain.CommonModule;
    using ProCenter.Domain.CommonModule.Lookups;

    #endregion

    /// <summary>
    ///     Lookup provider.
    /// </summary>
    public class LookupProvider : ILookupProvider, IOrderedBootstrapperTask
    {
        private readonly IContainer _container;

        #region Fields

        private readonly Dictionary<string, IEnumerable<Lookup>> _lookupsByCategory = new Dictionary<string, IEnumerable<Lookup>> ();

        #endregion

        public int Order { get; private set; }

        public LookupProvider (IContainer container)
        {
            _container = container;
        }

        #region Public Methods and Operators

        /// <summary>
        ///     Finds the specified code.
        /// </summary>
        /// <typeparam name="T">The Lookup.</typeparam>
        /// <param name="code">The code.</param>
        /// <returns>A lookup.</returns>
        public T Find<T> ( string code ) where T : Lookup
        {
            var lookup = GetAll<T> ().FirstOrDefault ( l => l.CodedConcept.Code == code );

            return lookup;
        }

        /// <summary>
        ///     Finds the specified lookup type.
        /// </summary>
        /// <param name="lookupType">Type of the lookup.</param>
        /// <param name="code">The code.</param>
        /// <returns>A lookup.</returns>
        public Lookup Find ( string lookupType, string code )
        {
            var lookup = GetAll ( lookupType ).FirstOrDefault ( l => l.CodedConcept.Code == code );

            return lookup;
        }

        /// <summary>
        ///     Gets all.
        /// </summary>
        /// <typeparam name="T">The Lookup.</typeparam>
        /// <returns>IEnumerable of Lookup.</returns>
        public IEnumerable<T> GetAll<T> () where T : Lookup
        {
            return GetAll ( typeof(T).Name ).OfType<T> ();
        }

        /// <summary>
        ///     Gets all.
        /// </summary>
        /// <param name="lookupType">Type of the lookup.</param>
        /// <returns>IEnumerable of Lookup.</returns>
        public IEnumerable<Lookup> GetAll ( string lookupType )
        {
            return _lookupsByCategory.ContainsKey ( lookupType ) ? _lookupsByCategory[lookupType] : Enumerable.Empty<Lookup> ();
        }

        /// <summary>
        ///     Registers the lookup type.
        /// </summary>
        /// <typeparam name="T">
        ///     Type of <see cref="Lookup" />.
        /// </typeparam>
        /// <exception cref="System.InvalidOperationException"> Cannot register a type that does not inherit from lookup type.</exception>
        public void Register<T> () where T : Lookup
        {
            Register ( typeof(T), GetRegistrationType ( typeof(T) ) );
        }

        /// <summary>
        ///     Registers the lookup based on the specified register type.
        /// </summary>
        /// <typeparam name="T">The actual type of the lookup.</typeparam>
        /// <param name="registerType">The type to register the lookup on behalve of.</param>
        /// <exception cref="System.InvalidOperationException"> Cannot register a type that does not inherit from lookup type.</exception>
        public void Register<T> ( Type registerType ) where T : Lookup
        {
            Register ( typeof(T), registerType );
        }

        /// <summary>
        ///     Registers the lookup based on the specified register type.
        /// </summary>
        /// <param name="lookupType">The lookup type.</param>
        /// <exception cref="System.InvalidOperationException"> Cannot register a type that does not inherit from lookup type.</exception>
        public void Register ( Type lookupType )
        {
            Register ( lookupType, GetRegistrationType ( lookupType ) );
        }

        /// <summary>
        ///     Registers the lookup based on the specified register type.
        /// </summary>
        /// <param name="actualType">The actual type to pull lookups from.</param>
        /// <param name="registerType">The type to register the lookup on behalve of.</param>
        /// <exception cref="System.InvalidOperationException">Cannot register a type that does not inherit from lookup type.</exception>
        public void Register ( Type actualType, Type registerType )
        {
            if ( !typeof(Lookup).IsAssignableFrom ( registerType ) )
            {
                throw new InvalidOperationException ( string.Format ( "Cannot register type {0} because it does not inherit from type Lookup.", registerType ) );
            }
            if ( !typeof(Lookup).IsAssignableFrom ( actualType ) )
            {
                throw new InvalidOperationException ( string.Format ( "Cannot use actual type {0} because it does not inherit from type Lookup.", actualType ) );
            }
            var fields =
                actualType.GetFields ( BindingFlags.Static | BindingFlags.Public ).Where ( f => f.FieldType == actualType );

            var lookups = from f in fields
                          let looupItem = (Lookup) f.GetValue ( null )
                          orderby looupItem.SortOrder
                          select looupItem;

            if ( _lookupsByCategory.ContainsKey ( registerType.Name ) )
            {
                _lookupsByCategory[registerType.Name] = lookups;
            }
            else
            {
                _lookupsByCategory.Add ( registerType.Name, lookups );
            }
        }

        #endregion

        #region Methods

        private static Type GetRegistrationType ( Type lookupType )
        {
            var lookupAttribute = lookupType.GetCustomAttribute<LookupRegistration> ();
            return lookupAttribute != null ? lookupAttribute.LookupType : lookupType;
        }

        #endregion

        public void Execute ()
        {
            var lookups = _container.ResolveAll<Lookup> ();
            foreach ( var lookupType in lookups.Select ( l => l.GetType () ) )
            {
                Register ( lookupType );
            }

            _container.RegisterInstance ( typeof(ILookupProvider), this );
        }
    }
}