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

namespace ProCenter.Common.Extension
{
    #region Using Statements

    using System;
    using System.Linq;
    using System.Security.Claims;

    #endregion

    /// <summary>
    /// <see cref="ClaimsPrincipal"/> extensions.
    /// </summary>
    public static class ClaimPrincipalExtensions
    {
        #region Public Methods and Operators

        /// <summary>Gets the claim.</summary>
        /// <typeparam name="T">The type of claim.</typeparam>
        /// <param name="claimsPrincipal">The claims principal.</param>
        /// <param name="claimType">Type of the claim.</param>
        /// <returns>A claim of type <typeparam name="T"></typeparam> or null.</returns>
        public static T GetClaim<T> ( this ClaimsPrincipal claimsPrincipal, string claimType )
        {
            var claim = claimsPrincipal.Claims.FirstOrDefault ( c => c.Type == claimType );
            if ( claim != null )
            {
                var type = typeof(T);
                var baseType = Nullable.GetUnderlyingType ( type ) ?? type;
                object value = claim.Value;
                if ( baseType == typeof(Guid) )
                {
                    value = Guid.Parse ( claim.Value );
                }
                return (T) Convert.ChangeType ( value, baseType );
            }
            return default( T );
        }

        #endregion
    }
}