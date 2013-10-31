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
namespace ProCenter.Domain.CommonModule
{
    #region Using Statements

    using System;
    using System.Collections.Generic;
    using Lookups;

    #endregion

    /// <summary>
    ///     Interface for lookup provider
    /// </summary>
    public interface ILookupProvider
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Finds the specified code.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        T Find<T> ( string code ) where T : Lookup;

        /// <summary>
        ///     Finds the specified lookup type.
        /// </summary>
        /// <param name="lookupType">Type of the lookup.</param>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        Lookup Find ( string lookupType, string code );

        /// <summary>
        ///     Gets all.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IEnumerable<T> GetAll<T> () where T : Lookup;

        /// <summary>
        ///     Gets all.
        /// </summary>
        /// <param name="lookupType">Type of the lookup.</param>
        /// <returns></returns>
        IEnumerable<Lookup> GetAll ( string lookupType );

        /// <summary>
        ///     Registers this instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void Register<T> () where T : Lookup;

        /// <summary>
        ///     Registers the lookup based on the specified register type.
        /// </summary>
        /// <typeparam name="T">The actual type of the lookup.</typeparam>
        /// <param name="registerType">The type to register the lookup on behalve of.</param>
        /// <exception cref="System.InvalidOperationException"> Cannot register a type that does not inherit from lookup type.</exception>
        void Register<T> ( Type registerType ) where T : Lookup;

        /// <summary>
        ///     Registers the lookup based on the specified register type.
        /// </summary>
        /// <param name="actualType">The actual type to pull lookups from.</param>
        /// <param name="registerType">The type to register the lookup on behalve of.</param>
        /// <exception cref="System.InvalidOperationException">Cannot register a type that does not inherit from lookup type.</exception>
        void Register ( Type actualType, Type registerType );

        /// <summary>
        ///     Registers the lookup based on the specified register type.
        /// </summary>
        /// <param name="lookupType">The lookup type.</param>
        /// <exception cref="System.InvalidOperationException"> Cannot register a type that does not inherit from lookup type.</exception>
        void Register ( Type lookupType );

        #endregion
    }
}