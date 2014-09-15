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

namespace ProCenter.ReadSideService
{
    #region Using Statements

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using NEventStore;
    using NEventStore.Dispatcher;

    using NLog;

    using Pillar.Common.InversionOfControl;

    using ProCenter.Common;
    using ProCenter.Domain.CommonModule;

    using StructureMap;
    using StructureMap.Pipeline;

    using IContainer = Pillar.Common.InversionOfControl.IContainer;

    #endregion

    /// <summary>The read side persistence dispatcher class.</summary>
    public class ReadSidePersistenceDispatcher : IDispatchCommits, IOrderedBootstrapperTask
    {
        #region Fields

        private readonly IContainer _container;

        private readonly Dictionary<Type, List<Action<object>>> _eventUpdaters = new Dictionary<Type, List<Action<object>>> ();

        private readonly Logger _logger = LogManager.GetCurrentClassLogger ();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadSidePersistenceDispatcher"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public ReadSidePersistenceDispatcher ( IContainer container )
        {
            _container = container;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the order.
        /// </summary>
        /// <value>
        /// The order.
        /// </value>
        public int Order { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Dispatches the commit specified to the messaging infrastructure.
        /// </summary>
        /// <param name="commit">The commmit to be dispatched.</param>
        public  void Dispatch ( Commit commit )
        {
            Lifecycles.GetLifecycle ( InstanceScope.Hybrid ).EjectAll ();
            try
            {
                foreach (var @event in commit.Events)
                {
                    var type = @event.Body.GetType();
                    if (_eventUpdaters.ContainsKey(type))
                    {
                        foreach (var action in _eventUpdaters[type])
                        {
                            action(@event.Body);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                _logger.ErrorException("Error when dispatching events.", exception);
                throw;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose ()
        {
            Dispose ( true );
            GC.SuppressFinalize ( this );
        }

        /// <summary>
        /// Executes this instance.
        /// </summary>
        public void Execute ()
        {
            var registerMethod = GetType ().GetMethod ( "Register" );

            _container.RegisterInstance ( typeof(IDispatchCommits), this );

            foreach ( var handler in _container.ResolveAll<IHandleMessage> () )
            {
                foreach (
                    var handlerInterface in
                        handler.GetType ()
                            .GetInterfaces ()
                            .Where ( t => t.IsGenericType && t.GetGenericTypeDefinition () == typeof(IHandleMessages<>) )
                    )
                {
                    var handlerEventType = handlerInterface.GetGenericArguments ().First ();
                    var genericRegisterMethod = registerMethod.MakeGenericMethod ( handlerEventType );
                    genericRegisterMethod.Invoke ( this, new object[] { handler } );
                }
            }
        }

        /// <summary>
        /// Registers the specified handler.
        /// </summary>
        /// <typeparam name="T">Type of message.</typeparam>
        /// <param name="handler">The handler.</param>
        public void Register<T> ( IHandleMessages<T> handler ) where T : class, ICommitEvent
        {
            if ( _eventUpdaters.ContainsKey ( typeof(T) ) )
            {
                _eventUpdaters[typeof(T)].Add ( ( @event => handler.Handle ( @event as T ) ) );
            }
            else
            {
                _eventUpdaters.Add ( typeof(T), new List<Action<object>> { ( @event => handler.Handle ( @event as T ) ) } );
            }
        }

        #endregion

        #region Methods

        /// <summary>Releases unmanaged and - optionally - managed resources.</summary>
        /// <param name="disposing"><c>True</c> to release both managed and unmanaged resources; <c>False</c> to release only unmanaged resources.</param>
        protected virtual void Dispose ( bool disposing )
        {
        }

        #endregion
    }
}