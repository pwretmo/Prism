using CommonServiceLocator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prism.IocContainer.Wpf.Tests.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prism.Windsor.Wpf.Tests
{
    [TestClass]
    public class WindsorBootstrapperRegisterForNavigationFixture : BootstrapperFixtureBase
    {
        [TestMethod]
        public void RunCheckIfViewRegisteredForNavigationCanBeResolvedTroughServiceLocatorWithObjectServiceType()
        {
            var bootstrapper = new RegisterForNavigationBootstrapper();
            bootstrapper.Run();
            IServiceLocator serviceLocator = bootstrapper.Container.Resolve<IServiceLocator>();
            object viewInstance = serviceLocator.GetInstance<object>(nameof(NavigateView));

            Assert.IsNotNull(viewInstance);
            Assert.IsInstanceOfType(viewInstance, typeof(NavigateView));
        }

        private class RegisterForNavigationBootstrapper : WindsorBootstrapper
        {
            protected override void ConfigureContainer()
            {
                base.ConfigureContainer();
                Container.RegisterTypeForNavigation<NavigateView>();
            }
        }

        public class NavigateView
        {

        }
    }
}
