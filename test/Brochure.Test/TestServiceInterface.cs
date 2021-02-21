using System;
using System.Linq;
using Brochure.Abstract;
using Brochure.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Brochure.Test
{
    [TestClass]
    public class ServiceInterfaceTest
    {
        private ServiceCollection service;

        public ServiceInterfaceTest()
        {
            service = new ServiceCollection();
        }

        [TestMethod]
        public void TestAddSingleService()
        {
            service.InitService(this.GetType().Assembly);
            var a = service.GetServiceInstance<IA>();
            Assert.IsNotNull(a);
            var count = service.GetServiceInstances<IA>().Count();
            Assert.AreEqual(1, count);
        }
        [TestMethod]
        public void TestAddMutiService()
        {
            service.InitService(this.GetType().Assembly);
            var count = service.GetServiceInstances<IMA>().Count();
            Assert.AreEqual(2, count);
        }
    }

    public interface IA
    {

    }
    public class A : IA, ISingleton
    {
    }
    public class A2 : IA, ISingleton
    {
    }

    public interface IB
    {

    }
    public class B : IB, ISingleton
    {
    }
    public interface IC
    {

    }
    public class C : IC, ISingleton
    {
    }
    public interface IMA
    {
    }
    public class MA : IMA, IMutiSingleton
    {
    };
    public class MA2 : IMA, IMutiSingleton
    {
    };
}
