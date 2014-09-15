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

namespace ProCenter.Mvc.Infrastructure.Service
{
    #region Using Statements

    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using Agatha.Common;
    using Agatha.Common.Caching;

    #endregion

    /// <summary>The asynchronous request dispatcher class.</summary>
    public class AsyncRequestDispatcher : RequestDispatcher, IAsyncRequestDispatcher
    {
        #region Fields

        private readonly HttpContext _httpContext;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncRequestDispatcher"/> class.
        /// </summary>
        /// <param name="requestProcessor">The request processor.</param>
        /// <param name="cacheManager">The cache manager.</param>
        public AsyncRequestDispatcher ( IRequestProcessor requestProcessor, ICacheManager cacheManager )
            : base ( requestProcessor, cacheManager )
        {
            _httpContext = HttpContext.Current;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>Gets all asynchronous.</summary>
        /// <returns>A <see cref="Task"/>.</returns>
        public Task GetAllAsync ()
        {
            return Task.Run ( () =>
            {
                HttpContext.Current = _httpContext;

                //This Triggers dispatcher to get responses
                HasResponse<Response> ();
            } );
        }

        /// <summary>
        /// Gets the asynchronous.
        /// </summary>
        /// <typeparam name="T">The type of response to get.</typeparam>
        /// <returns>A <see cref="Task"/> that returns the response of type <typeparam name="T"></typeparam>.</returns>
        public async Task<T> GetAsync<T> ()
            where T : Response
        {
            T response = null;
            await Task.Run ( () =>
            {
                HttpContext.Current = _httpContext;
                response = base.Get<T> ();
            } );
            return response;
        }

        /// <summary>
        /// Gets the asynchronous.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>A <see cref="Task"/> that returns a response object.</returns>
        public async Task<object> GetAsync ( Type type )
        {
            object response = null;
            await Task.Run ( () =>
            {
                HttpContext.Current = _httpContext;
                response = Responses.First ( f => f.GetType () == type );
            } );
            return response;
        }

        #endregion
    }
}