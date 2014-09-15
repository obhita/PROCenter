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

namespace ProCenter.Mvc.Infrastructure.Binder
{
    #region Using Statements

    using System.Web.Mvc;

    #endregion

    /// <summary>The nullable handling value provider wrapper class.</summary>
    public class NullableHandlingValueProviderWrapper : IValueProvider, INullableHandling
    {
        #region Fields

        private readonly IValueProvider _backingProvider;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NullableHandlingValueProviderWrapper"/> class.
        /// </summary>
        /// <param name="backingProvider">The backing provider.</param>
        public NullableHandlingValueProviderWrapper ( IValueProvider backingProvider )
        {
            this._backingProvider = backingProvider;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Determines whether the collection contains the specified prefix.
        /// </summary>
        /// <param name="prefix">The prefix to search for.</param>
        /// <returns>
        /// True if the collection contains the specified prefix; otherwise, false.
        /// </returns>
        public bool ContainsPrefix ( string prefix )
        {
            return this._backingProvider.ContainsPrefix ( prefix );
        }

        /// <summary>
        /// Retrieves a value object using the specified key.
        /// </summary>
        /// <param name="key">The key of the value object to retrieve.</param>
        /// <returns>
        /// The value object for the specified key.
        /// </returns>
        public ValueProviderResult GetValue ( string key )
        {
            var result = this._backingProvider.GetValue ( key );
            return result == null ? null : new NullableHandlingValueProviderResult ( result );
        }

        #endregion
    }
}