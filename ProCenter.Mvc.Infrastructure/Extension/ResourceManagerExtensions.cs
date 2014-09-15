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

namespace ProCenter.Mvc.Infrastructure.Extension
{
    #region Using Statements

    using System;
    using System.Resources;

    #endregion

    /// <summary>Extensions for <see cref="ResourceManager" /></summary>
    public static class ResourceManagerExtensions
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Gets the resource string by concatenating the type name of the <paramref name="obj" /> and the string
        ///     representation of
        ///     <paramref
        ///         name="obj" />
        ///     in the format "typename_string".
        /// </summary>
        /// <param name="resourceManager">The resource manager.</param>
        /// <param name="obj">The obj.</param>
        /// <returns>Resource string.</returns>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="obj" /> cannot be null.
        /// </exception>
        public static string GetTypedString ( this ResourceManager resourceManager, object obj )
        {
            if ( obj == null )
            {
                throw new ArgumentNullException ( paramName: "obj" );
            }
            return resourceManager.GetString ( string.Format ( "{0}_{1}", obj.GetType ().Name, obj.ToString () ) );
        }

        #endregion
    }
}