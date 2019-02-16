using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Prism.Ioc;
using System;
using System.Collections.Generic;

namespace Prism.Windsor.Ioc
{
    public class WindsorContainerExtension : IContainerExtension<IWindsorContainer>, IWindsorInstaller
    {
        private List<IRegistration> registrations = new List<IRegistration>();

        public IWindsorContainer Instance { get; }

        public bool SupportsModules => true;

        public WindsorContainerExtension(IWindsorContainer container)
        {
            Instance = container;
        }

        private void AddRegistration(IRegistration registration)
        {
            if (registrations != null)
            {
                registrations.Add(registration);
            }
            else
            {
                Instance.Register(registration);
            }
        }

        public void FinalizeExtension()
        {
            Instance.Install(this);
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(registrations.ToArray());

            registrations.Clear();
            registrations = null;
        }

        public void RegisterInstance(Type type, object instance)
        {
            this.AddRegistration(Component.For(type).Instance(instance));
        }

        public void RegisterSingleton(Type from, Type to)
        {
            this.AddRegistration(Component.For(from).ImplementedBy(to));
        }

        public void Register(Type from, Type to)
        {
            this.AddRegistration(Component.For(from).ImplementedBy(to).LifestyleTransient());
        }

        public void Register(Type from, Type to, string name)
        {
            this.AddRegistration(Component.For(from).ImplementedBy(to).LifestyleTransient().Named(name));
        }

        public object Resolve(Type type)
        {
            return Instance.Resolve(type);
        }

        public object Resolve(Type type, string name)
        {
            return Instance.Resolve(name, type);
        }

        public object ResolveViewModelForView(object view, Type viewModelType)
        {
            return Instance.Resolve(viewModelType);
        }
    }
}
