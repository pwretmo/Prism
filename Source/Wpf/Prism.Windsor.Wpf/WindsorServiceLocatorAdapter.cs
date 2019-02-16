using Castle.Windsor;
using CommonServiceLocator;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Prism.Windsor
{
    public class WindsorServiceLocatorAdapter : ServiceLocatorImplBase
    {
        /// <summary>Exposes underlying Container for direct operation.</summary>
        public readonly IWindsorContainer _container;

        /// <summary>Creates new locator as adapter for provided container.</summary>
        /// <param name="container">Container to use/adapt.</param>
        public WindsorServiceLocatorAdapter(IWindsorContainer container)
        {
            if (container == null) throw new ArgumentNullException("container");
            _container = container;
        }

        /// <summary>Resolves service from container. Throws if unable to resolve.</summary>
        /// <param name="serviceType">Service type to resolve.</param>
        /// <param name="key">(optional) Service key to resolve.</param>
        /// <returns>Resolved service object.</returns>
        protected override object DoGetInstance(Type serviceType, string key)
        {
            if (serviceType == null) throw new ArgumentNullException("serviceType");

            return key == null ? _container.Resolve(serviceType) : _container.Resolve(key, serviceType);
        }

        /// <summary>Returns enumerable which when enumerated! resolves all default and named 
        /// implementations/registrations of requested service type. 
        /// If no services resolved when enumerable accessed, no exception is thrown - enumerable is empty.</summary>
        /// <param name="serviceType">Service type to resolve.</param>
        /// <returns>Returns enumerable which will return resolved service objects.</returns>
        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            if (serviceType == null) throw new ArgumentNullException("serviceType");
            return this._container.ResolveAll(serviceType).Cast<object>();
        }
    }
}
