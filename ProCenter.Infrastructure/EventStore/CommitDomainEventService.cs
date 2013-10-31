namespace ProCenter.Infrastructure.EventStore
{
    #region Using Statements

    using System;
    using System.Collections.Generic;
    using Pillar.Common.InversionOfControl;
    using Pillar.Domain.Event;
    using ProCenter.Domain.CommonModule;

    #endregion

    /// <summary>
    ///     Domain event service that handles commiting of events to event store.
    /// </summary>
    public class CommitDomainEventService : ICommitDomainEventService
    {
        #region Fields

        private readonly IContainer _container;
        private readonly IUnitOfWorkProvider _unitOfWorkProvider;
        private List<Delegate> _actions;
        private List<Action<IDomainEvent>> _allEventActions;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CommitDomainEventService" /> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="unitOfWorkProvider">The unit of work provider.</param>
        public CommitDomainEventService ( IContainer container, IUnitOfWorkProvider unitOfWorkProvider )
        {
            _container = container;
            _unitOfWorkProvider = unitOfWorkProvider;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Raises the specified @event.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="event">The @event.</param>
        public void Raise<TEvent> ( TEvent @event ) where TEvent : IDomainEvent
        {
            if ( _container != null )
            {
                foreach ( var domainEventHandler in _container.ResolveAll<IDomainEventHandler<TEvent>> () )
                    domainEventHandler.Handle ( @event );
            }
            if (_actions != null)
            {
                foreach (var @delegate in _actions)
                {
                    if (@delegate is Action<TEvent>)
                        ((Action<TEvent>)@delegate)(@event);
                }
            }
            if (_allEventActions != null)
            {
                foreach (var action in _allEventActions)
                {
                    action(@event);
                }
            }
        }

        /// <summary>
        ///     Raises the commit event.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="aggregateRoot">The aggregate root.</param>
        /// <param name="commitEvent">The commit event.</param>
        public void RaiseCommit<TEvent> ( IAggregateRoot aggregateRoot, TEvent commitEvent ) where TEvent : ICommitEvent
        {
            var unitOfWork = _unitOfWorkProvider.GetCurrentUnitOfWork ();
            unitOfWork.Register ( aggregateRoot, commitEvent );
            Raise ( commitEvent );
        }

        /// <summary>
        ///     Registers the specified callback.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="callback">The callback.</param>
        public void Register<TEvent> ( Action<TEvent> callback ) where TEvent : IDomainEvent
        {
            if ( _actions == null )
                _actions = new List<Delegate> ();
            _actions.Add ( callback );
        }

        /// <summary>
        /// Registers for all events.
        /// </summary>
        /// <param name="callback">The callback.</param>
        public void RegisterAll(Action<IDomainEvent> callback)
        {
            if (_allEventActions == null)
            {
                _allEventActions = new List<Action<IDomainEvent>>();
            }
            _allEventActions.Add(callback);
        }

        #endregion
    }
}