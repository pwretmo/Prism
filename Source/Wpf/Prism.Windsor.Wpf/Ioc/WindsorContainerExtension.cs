using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Prism.Windsor.Ioc
{
    public class WindsorContainerExtension : IContainerExtension<IWindsorContainer>
    {
        private Installer installer = new Installer();
        private ISet<IWindsorInstaller> installers = new HashSet<IWindsorInstaller>();

        public WindsorContainerExtension(IWindsorContainer container)
        {
            Instance = container;
            installers.Add(installer);
        }

        public IWindsorContainer Instance { get; }

        public bool SupportsModules => true;

        private void AddRegistration(IRegistration registration)
        {
            if (installer != null)
            {
                installer.Registrations.Add(registration);
            }
            else
            {
                Instance.Register(registration);
            }
        }

        public void AddInstaller(IWindsorInstaller installer)
        {
            if (installers == null)
            {
                throw new InvalidOperationException("Windsor installers can only be added before FinalizeExtension() has been executed.");
            }

            if (installer == null)
            {
                throw new ArgumentNullException(nameof(installer));
            }

            this.installers.Add(installer);
        }

        public void FinalizeExtension()
        {
            if (installer != null)
            {
                Instance.Install(this.installers.ToArray());
                installer = null;
                installers = null;
            }
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

        private class Installer : IWindsorInstaller
        {
            public ISet<IRegistration> Registrations { get; } = new HashSet<IRegistration>();

            public void Install(IWindsorContainer container, IConfigurationStore store)
            {
                container.Register(Registrations.ToArray());
            }
        }
    }
}
