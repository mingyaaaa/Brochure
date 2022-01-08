using System;
using System.Linq;
using Brochure.Abstract;
using Brochure.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Brochure.Test
{
    /// <summary>
    /// The service interface test.
    /// </summary>
    [TestClass]
    public class ServiceInterfaceTest
    {
        private ServiceCollection service;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceInterfaceTest"/> class.
        /// </summary>
        public ServiceInterfaceTest()
        {
            service = new ServiceCollection();
        }

        /// <summary>
        /// Tests the add single service.
        /// </summary>
        [TestMethod]
        public void TestAddSingleService()
        {
            service.InitService(this.GetType().Assembly);
            var a = service.GetServiceInstance<IA>();
            Assert.IsNotNull(a);
            var count = service.GetServiceInstances<IA>().Count();
            Assert.AreEqual(1, count);
        }
        /// <summary>
        /// Tests the add muti service.
        /// </summary>
        [TestMethod]
        public void TestAddMutiService()
        {
            service.InitService(this.GetType().Assembly);
            var count = service.GetServiceInstances<IMA>().Count();
            Assert.AreEqual(2, count);
        }



        /// <summary>
        /// The a.
        /// </summary>
        public interface IA
        {

        }
        /// <summary>
        /// The a.
        /// </summary>
        public class A : IA, ISingleton
        {
        }
        /// <summary>
        /// The a2.
        /// </summary>
        public class A2 : IA, ISingleton
        {
        }

        /// <summary>
        /// The b.
        /// </summary>
        public interface IB
        {

        }
        /// <summary>
        /// The b.
        /// </summary>
        public class B : IB, ISingleton
        {
        }
        /// <summary>
        /// The c.
        /// </summary>
        public interface IC
        {

        }
        /// <summary>
        /// The c.
        /// </summary>
        public class C : IC, ISingleton
        {
        }
        /// <summary>
        /// The m a.
        /// </summary>
        public interface IMA
        {
        }
        /// <summary>
        /// The m a.
        /// </summary>
        public class MA : IMA, IMutiSingleton
        {
        };
        /// <summary>
        /// The m a2.
        /// </summary>
        public class MA2 : IMA, IMutiSingleton
        {
        };
    }


}
