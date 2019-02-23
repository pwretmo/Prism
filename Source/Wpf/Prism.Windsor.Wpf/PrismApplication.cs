using CommonServiceLocator;
using Prism.Windsor.Ioc;
using Prism.Ioc;
using Prism.Regions;
using System;
using Castle.Windsor;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Prism.Regions.Behaviors;

namespace Prism.Windsor
{
    public abstract class PrismApplication : PrismApplicationBase
    {
        ///// <summary>
        ///// Create <see cref="Rules" /> to alter behavior of <see cref="IContainer" />
        ///// </summary>
        ///// <returns>An instance of <see cref="Rules" /></returns>
        //protected virtual Rules CreateContainerRules() => Rules.Default.WithAutoConcreteTypeResolution()
        //                                                               .With(Made.Of(FactoryMethod.ConstructorWithResolvableArguments))
        //                                                               .WithDefaultIfAlreadyRegistered(IfAlreadyRegistered.Replace);

        protected override IContainerExtension CreateContainerExtension()
        {
            return new WindsorContainerExtension(new WindsorContainer());
        }

        protected override void RegisterRequiredTypes(IContainerRegistry containerRegistry)
        {
            base.RegisterRequiredTypes(containerRegistry);
            containerRegistry.RegisterSingleton<IRegionNavigationContentLoader, RegionNavigationContentLoader>();
            containerRegistry.RegisterInstance<IServiceLocator>(new WindsorServiceLocatorAdapter(containerRegistry.GetContainer()));

            containerRegistry.RegisterSingleton<DelayedRegionCreationBehavior>();
            containerRegistry.GetContainer().Register(Classes.FromAssemblyContaining<IRegionBehavior>().BasedOn<IRegionBehavior>().WithServiceSelf());

            containerRegistry.GetContainer().Register(Classes.FromAssemblyContaining<IRegionAdapter>().BasedOn<IRegionAdapter>().WithServiceSelf());
        }

        protected override void RegisterFrameworkExceptionTypes()
        {
            base.RegisterFrameworkExceptionTypes();

            ExceptionExtensions.RegisterFrameworkExceptionType(typeof(ComponentResolutionException));
            ExceptionExtensions.RegisterFrameworkExceptionType(typeof(ComponentNotFoundException));
            ExceptionExtensions.RegisterFrameworkExceptionType(typeof(ComponentRegistrationException));
            ExceptionExtensions.RegisterFrameworkExceptionType(typeof(CircularDependencyException));
        }
    }
}
