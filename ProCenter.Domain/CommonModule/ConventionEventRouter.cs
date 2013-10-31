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
namespace ProCenter.Domain.CommonModule
{
    #region Using Statements

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    #endregion

    /// <summary>
    ///     Convention based event router.
    /// </summary>
    /// <remarks>
    ///     Will automatically register event handlers for events by looking at the aggregate and registering methods named Apply
    ///     that return void and take only one parameter that is an event.
    /// </remarks>
    public class ConventionEventRouter : IRouteEvents
    {
        #region Fields

        private readonly IDictionary<Type, Action<object>> _handlers = new Dictionary<Type, Action<object>> ();
        private readonly bool _throwOnApplyNotFound;
        private IAggregateRoot _registered;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ConventionEventRouter" /> class.
        /// </summary>
        public ConventionEventRouter ()
            : this ( true )
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ConventionEventRouter" /> class.
        /// </summary>
        /// <param name="throwOnApplyNotFound">
        ///     if set to <c>true</c> throws exception if apply method not found.
        /// </param>
        public ConventionEventRouter ( bool throwOnApplyNotFound )
        {
            _throwOnApplyNotFound = throwOnApplyNotFound;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ConventionEventRouter" /> class.
        /// </summary>
        /// <param name="aggregate">The aggregate.</param>
        /// <param name="throwOnApplyNotFound">
        ///     if set to <c>true</c> throws exception if apply method not found.
        /// </param>
        public ConventionEventRouter ( IAggregateRoot aggregate, bool throwOnApplyNotFound = true )
            : this ( throwOnApplyNotFound )
        {
            Register ( aggregate );
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Dispatches the specified event message.
        /// </summary>
        /// <param name="eventMessage">The event message.</param>
        /// <exception cref="System.ArgumentNullException">eventMessage</exception>
        /// <exception cref="System.InvalidOperationException"></exception>
        public virtual void Dispatch ( object eventMessage )
        {
            if ( eventMessage == null )
                throw new ArgumentNullException ( "eventMessage" );

            var eventType = eventMessage.GetType ();

            Action<object> handler;
            if ( _handlers.TryGetValue ( eventType, out handler ) )
                handler ( eventMessage );
            else if ( _throwOnApplyNotFound )
                throw new InvalidOperationException ( string.Format ( "Handler not found for event {0} on {1} aggregate", eventType, _registered.GetType () ) );
        }

        /// <summary>
        ///     Registers the specified handler.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handler">The handler.</param>
        /// <exception cref="System.ArgumentNullException">handler</exception>
        public virtual void Register<T> ( Action<T> handler )
        {
            if ( handler == null )
                throw new ArgumentNullException ( "handler" );

            Register ( typeof(T), @event => handler ( (T) @event ) );
        }

        /// <summary>
        ///     Registers the specified aggregate.
        /// </summary>
        /// <param name="aggregate">The aggregate.</param>
        /// <exception cref="System.ArgumentNullException">aggregate</exception>
        public virtual void Register ( IAggregateRoot aggregate )
        {
            if ( aggregate == null )
                throw new ArgumentNullException ( "aggregate" );

            _registered = aggregate;

            // Get instance methods named Apply with one parameter returning void
            var applyMethods = aggregate.GetType ()
                                        .GetMethods ( BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance )
                                        .Where ( m => m.Name == "Apply" && m.GetParameters ().Length == 1 && m.ReturnType == typeof(void) )
                                        .Select ( m => new
                                            {
                                                Method = m,
                                                MessageType = m.GetParameters ().Single ().ParameterType
                                            } );

            foreach ( var apply in applyMethods )
            {
                var applyMethod = apply.Method;
                _handlers.Add ( apply.MessageType, m => applyMethod.Invoke ( aggregate, new[] {m} ) );
            }
        }

        #endregion

        #region Methods

        private void Register ( Type messageType, Action<object> handler )
        {
            _handlers[messageType] = handler;
        }

        #endregion
    }
}