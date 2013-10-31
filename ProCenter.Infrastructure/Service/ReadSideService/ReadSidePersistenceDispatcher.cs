namespace ProCenter.Infrastructure.Service.ReadSideService
{
    #region Using Statements

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Common;
    using Pillar.Common.Bootstrapper;
    using Pillar.Common.InversionOfControl;
    using ProCenter.Domain.CommonModule;
    using global::EventStore;
    using global::EventStore.Dispatcher;

    #endregion

    public class ReadSidePersistenceDispatcher : IDispatchCommits, IOrderedBootstrapperTask
    {
        private readonly IContainer _container;
        private readonly Dictionary<Type, List<Action<object>>> _eventUpdaters = new Dictionary<Type, List<Action<object>>>();

        public int Order { get; private set; }

        public ReadSidePersistenceDispatcher(IContainer container)
        {
            _container = container;
        }

        public void Execute()
        {
            MethodInfo registerMethod = GetType().GetMethod("Register");

            _container.RegisterInstance(typeof(IDispatchCommits), this);

            foreach (IHandleMessage handler in _container.ResolveAll<IHandleMessage>())
            {
                foreach (
                    Type handlerInterface in
                        handler.GetType()
                               .GetInterfaces()
                               .Where(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof (IHandleMessages<>))
                    )
                {
                    Type handlerEventType = handlerInterface.GetGenericArguments().First();
                    MethodInfo genericRegisterMethod = registerMethod.MakeGenericMethod(handlerEventType);
                    genericRegisterMethod.Invoke(this, new object[] {handler});
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispatch(Commit commit)
        {
            foreach (EventMessage @event in commit.Events)
            {
                Type type = @event.Body.GetType();
                if (_eventUpdaters.ContainsKey(type))
                {
                    foreach ( var action in _eventUpdaters[type] )
                    {
                        action ( @event.Body );
                    }
                }
            }
        }

        public void Register<T>(IHandleMessages<T> handler) where T : class, ICommitEvent
        {
            if ( _eventUpdaters.ContainsKey ( typeof(T) ) )
            {
                _eventUpdaters[typeof(T)].Add((@event => handler.Handle(@event as T)));
            }
            else
            {
                _eventUpdaters.Add ( typeof(T), new List<Action<object>> { (@event => handler.Handle ( @event as T )) });
            }
        }

        protected virtual void Dispose(bool disposing)
        {
        }
    }
}