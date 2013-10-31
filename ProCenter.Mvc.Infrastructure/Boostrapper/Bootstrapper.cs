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
namespace ProCenter.Mvc.Infrastructure.Boostrapper
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Reflection;
    using System.Resources;
    using System.Web.Configuration;
    using Agatha.Common;
    using Agatha.Common.InversionOfControl;
    using Agatha.Common.WCF;
    using Agatha.ServiceLayer;
    using Agatha.ServiceLayer.Conventions;
    using Common;
    using Domain.AssessmentModule;
    using Domain.CommonModule;
    using Domain.CommonModule.Lookups;
    using Domain.MessageModule;
    using Domain.Nida;
    using NLog;
    using Pillar.Common.Bootstrapper;
    using Pillar.Common.Configuration;
    using Pillar.Domain.Event;
    using Pillar.Domain.FluentRuleEngine;
    using Pillar.FluentRuleEngine;
    using Pillar.Security.AccessControl;
    using ProCenter.Infrastructure;
    using ProCenter.Infrastructure.Domain;
    using ProCenter.Infrastructure.EventStore;
    using ProCenter.Infrastructure.Service;
    using ProCenter.Infrastructure.Service.Completeness;
    using ProCenter.Infrastructure.Service.ReadSideService;
    using ProCenter.Service.Handler.Common.Lookups;
    using ProCenter.Service.Message.Common.Lookups;
    using Security;
    using Service;
    using StructureMap;
    using AsyncRequestDispatcher = Service.AsyncRequestDispatcher;
    using Container = Agatha.StructureMap.Container;
    using IContainer = StructureMap.IContainer;

    #endregion

    /// <summary>
    ///     Defines the bootstrapper process needs to run.
    /// </summary>
    public class Bootstrapper
    {
        #region Fields

        /// <summary>
        ///     Gets the logger.
        /// </summary>
        protected readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Initializes the specified app state.
        /// </summary>
        public virtual void Run()
        {
            Logger.Info("Initializing StructureMap IoC Container");
            var appContainer = CreateAndConfigureApplicationDiContainer();

            Logger.Info("Initializing Pillar IoC");
            ConfigurePillarIoC(appContainer);

            RegisterDomainEventService(appContainer);

            Logger.Info("Initializing Agatha");
            ConfigureAgatha(appContainer);

            Logger.Info("Initializing Security");
            ConfigureSecurity(appContainer);

            Logger.Info("Initializing Unit Of Work");
            ConfigureEventStore(appContainer);

            appContainer.Configure(x => x.For<IRuleViolationReporter>().Add<DomainRuleViolationCollection>());

            Logger.Info("Running Bootstrapper Tasks");
            RunBootstrapperTasks(appContainer);

            Logger.Debug(appContainer.WhatDoIHave());
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Configures the security.
        /// </summary>
        /// <param name="appContainer">The app container.</param>
        protected virtual void ConfigureSecurity(IContainer appContainer)
        {
            appContainer.Configure(c => c.For<ICurrentUserPermissionService>().Use<CurrentUserPermissionService>());
            appContainer.Configure(c => c.For<ICurrentClaimsPrincipalService>().Use<CurrentClaimsPrincipleService>());
            appContainer.Configure(c => c.For<IAccessControlManager>().Singleton().Use<ProCenterAccessControlManager>());
            appContainer.Configure(c => c.For<IProvidePermissions>().Use(conf => conf.GetInstance<IAccessControlManager>() as IProvidePermissions));
            var accessCtrlMgr = appContainer.GetInstance<IAccessControlManager>();
            var permissionDescriptors = appContainer.GetAllInstances<IPermissionDescriptor>();
            accessCtrlMgr.RegisterPermissionDescriptor(permissionDescriptors.ToArray());
        }

        /// <summary>
        ///     Configures the agatha.
        /// </summary>
        /// <param name="appContainer">The app container.</param>
        protected virtual void ConfigureAgatha(IContainer appContainer)
        {
            var structureMapContainer = new Container(appContainer);
            IoC.Container = structureMapContainer;

            var serviceLayerAndClientConfiguration =
                new ServiceLayerAndClientConfiguration(typeof (GetLookupsByCategoryRequestHandler).Assembly,
                                                       typeof (GetLookupsByCategoryRequest).Assembly,
                                                       structureMapContainer);
            // register agatha conventions
            serviceLayerAndClientConfiguration.Use<RequestHandlerBasedConventions>();
            serviceLayerAndClientConfiguration.RegisterRequestHandlerInterceptor<RuleViolationEventInterceptor>();

            serviceLayerAndClientConfiguration.RequestDispatcherImplementation = typeof (AsyncRequestDispatcher);

            serviceLayerAndClientConfiguration.RequestProcessorImplementation = typeof (UnitOfWorkRequestProcessor);

            serviceLayerAndClientConfiguration.Initialize();
            //KnownTypeProvider.ClearAllKnownTypes();
            //KnownTypesHelper.RegisterRequestsAndResponses(typeof(SaveDtoRequest<>).Assembly);
        }

        /// <summary>
        ///     Configures the pillar IoC.
        /// </summary>
        /// <param name="container">The container.</param>
        protected virtual void ConfigurePillarIoC(IContainer container)
        {
            Pillar.Common.InversionOfControl.IoC.SetContainerProvider(() => new Pillar.IoC.StructureMap.Container(container));
            Pillar.Common.InversionOfControl.IoC.Bootstrap();
        }

        /// <summary>
        ///     Creates the and configure application di container.
        /// </summary>
        /// <returns>A StructureMap Container.</returns>
        protected virtual IContainer CreateAndConfigureApplicationDiContainer()
        {
            var nidaAssessFurtherType = typeof(NidaAssessFurther); // todo: better way to copy dll to mvc project
            

            ObjectFactory.Configure(x =>
                                    x.Scan(scanner =>
                                        {
                                            scanner.AssembliesFromApplicationBaseDirectory(assembly => assembly.GetName().Name.StartsWith("ProCenter."));
                                            scanner.AddAllTypesOf<IOrderedBootstrapperTask>();
                                            scanner.AddAllTypesOf<IPermissionDescriptor>();
                                            scanner.AddAllTypesOf<IHandleMessage>();
                                            scanner.AddAllTypesOf<Lookup>();
                                            scanner.AddAllTypesOf<ICompletenessRuleCollection<AssessmentInstance>>().NameBy(type => type.Name.Replace("ReportCompletenessRuleCollection", ""));

                                            scanner.WithDefaultConventions();

                                            scanner.LookForRegistries();

                                            scanner.Convention<LookupPrimitiveResourceConvention>();
                                            scanner.Convention<WorkflowEngineConvention>();
                                            scanner.Convention<WorkflowReportEngineConvention>();

                                            scanner.AddAllTypesOf<IScoringEngine>().NameBy(type => type.Name.Replace("ScoringEngine", ""));
                                            scanner.AddAllTypesOf<IAssessmentRuleCollection>().NameBy(type => type.Name.Replace("RuleCollection", ""));

                                            scanner.ConnectImplementationsToTypesClosing(typeof (IDomainEventHandler<>));
                                            scanner.ConnectImplementationsToTypesClosing(typeof (IRuleCollection<>));
                                        }));

           //Note: scan default conventions first to avoid constructing singleton twice
            ObjectFactory.Configure(c => c.For<IMessageCollector>().HybridHttpOrThreadLocalScoped().Use<MessageCollector>());
            ObjectFactory.Configure(c => c.For<ILookupProvider>().Singleton().Use<LookupProvider>());
            ObjectFactory.Configure(c => c.For<IAssessmentCompletenessManager>().Singleton().Use<AssessmentCompletenessManager>());
            ObjectFactory.Configure(c => c.For<IConnectionStringConfigurationProvider>().Singleton().Use(new ConnectionStringConfigurationProvider(WebConfigurationManager.ConnectionStrings)));
            ObjectFactory.Configure(c => c.For<IConfigurationPropertiesProvider>().Singleton().Use(new AppSettingsConfiguration(WebConfigurationManager.AppSettings)));
            ObjectFactory.Configure(c => c.For<IDbConnectionFactory>().Singleton().Use<SqlConnectionFactory>());
            ObjectFactory.Configure(c => c.For<ICompletenessRuleCollection<AssessmentInstance>>().MissingNamedInstanceIs.IsThis(new EmptyCompletenessRuleCollection()));
            ObjectFactory.Configure(c => c.For<ResourceManager>().MissingNamedInstanceIs.IsThis(EmptyResources.ResourceManager));

            return ObjectFactory.Container;
        }

        /// <summary>
        ///     Registers the domain event service.
        /// </summary>
        /// <param name="container">The container.</param>
        protected virtual void RegisterDomainEventService(IContainer container)
        {
            //Domain Event Service - per request scope
            container.Configure(c => c.For<ICommitDomainEventService>().Singleton().Use<CommitDomainEventService>());
            container.Configure(c => c.For<IDomainEventService>().Use(context => context.GetInstance<ICommitDomainEventService>()));
        }

        /// <summary>
        ///     Runs the bootstrapper tasks.
        /// </summary>
        /// <param name="container">The container.</param>
        protected virtual void RunBootstrapperTasks(IContainer container)
        {
            var tasks = container.GetAllInstances<IOrderedBootstrapperTask>();
            foreach (var bootstrapperTask in tasks.OrderBy ( t => t.Order ))
            {
                bootstrapperTask.Execute();
            }
        }

        /// <summary>
        ///     Configures services needed by event store.
        /// </summary>
        /// <param name="container">The container.</param>
        protected virtual void ConfigureEventStore(IContainer container)
        {
            container.Configure(x => x.For<IUnitOfWork>().HybridHttpOrThreadLocalScoped().Use<UnitOfWork>());
            container.Configure(x => x.For<IUnitOfWorkProvider>().Singleton().Use<UnitOfWorkProvider>());

            container.Configure(x => x.For<IEventStoreFactory>().Singleton().Use<EventStoreFactory>());
            container.Configure(x => x.For<IEventStoreRepository>().HybridHttpOrThreadLocalScoped().Use<EventStoreRepository>());
            container.Configure(x => x.For<IDetectConflicts>().Use<EmptyConflictDetector>());
        }

        #endregion

        /// <summary>
        ///     Static class for helping registering known types
        /// </summary>
        public static class KnownTypesHelper
        {
            #region Public Methods and Operators

            /// <summary>
            ///     Gets the generic requests and responses.
            /// </summary>
            /// <param name="assembly">The assembly.</param>
            /// <returns></returns>
            /// <exception cref="System.NotImplementedException"></exception>
            /// <exception cref="System.InvalidOperationException">generic parameter must be constraint to implement some marker interface in order to build closed generic types for every marker implementor</exception>
            public static IEnumerable<Type> GetGenericRequestsAndResponses(Assembly assembly)
            {
                var genericTypes = from t in assembly.GetTypes()
                                   where t.IsGenericTypeDefinition && t.IsAbstract == false
                                   select t;

                foreach (var genericType in genericTypes)
                {
                    if (genericType.GetGenericArguments().Length > 1)
                    {
                        throw new NotImplementedException();
                    }

                    var argument = genericType.GetGenericArguments().Single();

                    var markerType = argument.GetGenericParameterConstraints().FirstOrDefault();
                    if (markerType == null)
                    {
                        throw new InvalidOperationException(
                            "generic parameter must be constraint to implement some marker interface in order to build closed generic types for every marker implementor");
                    }

                    var genericType1 = genericType;
                    var typesToRegister = from t in assembly.GetTypes()
                                          where markerType.IsAssignableFrom(t) && t.IsAbstract == false
                                          select genericType1.MakeGenericType(t);
                    foreach (var type in typesToRegister)
                    {
                        yield return type;
                    }
                }
            }

            /// <summary>
            ///     Registers the requests and responses.
            /// </summary>
            /// <param name="assembly">The assembly.</param>
            public static void RegisterRequestsAndResponses(Assembly assembly)
            {
                var list = new List<Type>();
                list.AddRange(GetNonGenericRequestsAndResponses(assembly));
                list.AddRange(GetGenericRequestsAndResponses(assembly));

                foreach (var type in list)
                {
                    KnownTypeProvider.Register(type);
                }
            }

            #endregion

            #region Methods

            private static IEnumerable<Type> GetNonGenericRequestsAndResponses(Assembly assembly)
            {
                return (from t in assembly.GetTypes()
                        where
                            t.IsAbstract == false && t.IsGenericTypeDefinition == false
                            && (t.IsSubclassOf(typeof (Request)) || t.IsSubclassOf(typeof (Response)))
                        select t).ToArray();
            }

            #endregion
        }
    }
}