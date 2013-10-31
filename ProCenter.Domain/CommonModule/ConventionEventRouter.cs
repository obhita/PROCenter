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