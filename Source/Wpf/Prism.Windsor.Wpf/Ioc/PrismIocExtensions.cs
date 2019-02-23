using Castle.Windsor;
using Prism.Ioc;
using Prism.Windsor.Ioc;

namespace Prism.Windsor
{
    public static class PrismIocExtensions
    {
        public static IWindsorContainer GetContainer(this IContainerProvider containerProvider)
        {
            return ((IContainerExtension<IWindsorContainer>)containerProvider).Instance;
        }

        public static IWindsorContainer GetContainer(this IContainerRegistry containerRegistry)
        {
            return ((IContainerExtension<IWindsorContainer>)containerRegistry).Instance;
        }

        public static WindsorContainerExtension GetWindsorContainerExtension(this IContainerProvider containerProvider)
        {
            return (WindsorContainerExtension)containerProvider;
        }

        public static WindsorContainerExtension GetWindsorContainerExtension(this IContainerRegistry containerRegistry)
        {
            return (WindsorContainerExtension)containerRegistry;
        }
    }
}
