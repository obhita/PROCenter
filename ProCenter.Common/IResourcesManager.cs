#region Licence Header
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
namespace ProCenter.Common
{
    #region Using Statements

    using System;
    using System.Reflection;
    using System.Resources;

    #endregion

    /// <summary>
    ///     Interface for managing resources.
    /// </summary>
    public interface IResourcesManager
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Gets the resource manager by name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>
        ///     A <see cref="ResourceManager" />.
        /// </returns>
        ResourceManager GetResourceManagerByName ( string name );

        /// <summary>
        /// Registers a resource manager for the type.
        /// </summary>
        /// <typeparam name="TResource">The type of the resource.</typeparam>
        void Register<TResource> (string code = null);

        /// <summary>
        /// Registers a resource manager for the type.
        /// </summary>
        /// <param name="resourceType">Type of the resource.</param>
        void Register(Type resourceType, string code = null);

        /// <summary>
        /// Registers a resource manager.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="fullName">The full name.</param>
        /// <param name="assembly">The assembly.</param>
        void Register(string name, string fullName, Assembly assembly, string code = null);

        #endregion
    }
}