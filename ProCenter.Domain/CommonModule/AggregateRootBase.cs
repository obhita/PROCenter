namespace ProCenter.Domain.CommonModule
{
    #region Using Statements

    using System;
    using Pillar.Domain;
    using Pillar.Domain.Event;

    #endregion

    /// <summary>
    ///     Base class for aggregate roots.
    /// </summary>
    public abstract class AggregateRootBase : Entity<Guid>, IAggregateRoot
    {
        #region Fields

        private IRouteEvents _eventRouter;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="AggregateRootBase" /> class.
        /// </summary>
        /// <param name="eventRouter">The event router.</param>
        protected AggregateRootBase ( IRouteEvents eventRouter = null )
        {
            _eventRouter = eventRouter ?? new ConventionEventRouter ( this );
            //_eventRouter.Register ( this );
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the registered routes.
        /// </summary>
        /// <value>
        ///     The registered routes.
        /// </value>
        /// <exception cref="System.InvalidOperationException">AggregateRootBase must have an event router to function</exception>
        protected IRouteEvents RegisteredRoutes
        {
            get { return _eventRouter ?? ( _eventRouter = new ConventionEventRouter ( this ) ); }
            set
            {
                if ( value == null )
                    throw new InvalidOperationException ( "AggregateRootBase must have an event router to function" );

                _eventRouter = value;
            }
        }

        #endregion

        #region Explicit Interface Methods

        /// <summary>
        ///     Applies the event.
        /// </summary>
        /// <param name="event">The event.</param>
        void IAggregateRoot.ApplyEvent ( object @event )
        {
            RegisteredRoutes.Dispatch ( @event );
            Version++;
        }

        /// <summary>
        ///     Gets the snapshot.
        /// </summary>
        /// <returns></returns>
        IMemento IAggregateRoot.GetSnapshot ()
        {
            var snapshot = GetSnapshot ();
            snapshot.Key = Key;
            snapshot.Version = Version;
            return snapshot;
        }

        void IAggregateRoot.RestoreSnapshot(IMemento memento)
        {
            RestoreSnapshot(memento);

            Key = memento.Key;
            Version = memento.Version;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Gets the snapshot.
        /// </summary>
        /// <returns></returns>
        protected virtual IMemento GetSnapshot ()
        {
            return null;
        }

        /// <summary>
        /// Restores the snapshot.
        /// </summary>
        /// <param name="memento">The memento.</param>
        protected virtual void RestoreSnapshot ( IMemento memento )
        {
            
        }

        /// <summary>
        ///     Raises the event.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="event">The event.</param>
        protected void RaiseEvent<TEvent> ( TEvent @event )
            where TEvent : ICommitEvent
        {
            ( (IAggregateRoot) this ).ApplyEvent ( @event );
            CommitEvent.RaiseCommitEvent ( this, @event );
        }

        /// <summary>
        ///     Registers the specified route.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="route">The route.</param>
        protected void Register<TEvent> ( Action<TEvent> route )
            where TEvent : IDomainEvent
        {
            RegisteredRoutes.Register ( route );
        }

        #endregion
    }
}