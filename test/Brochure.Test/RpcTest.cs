using Brochure.Core.RPC;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;

namespace Brochure.Test
{
    /// <summary>
    /// The rpc test.
    /// </summary>
    [TestClass]
    public class RpcTest
    {
        /// <summary>
        /// Creates the proxy.
        /// </summary>
        /// <returns>An AClient.</returns>
        public AClient CreateProxy()
        {
            var factory = new RpcPollyProxyFactory(new PollyOption() { RetryCount = 2 });
            return new Rpc<AClient>(factory).Ins;
        }

        /// <summary>
        /// Creates the memory proxy.
        /// </summary>
        /// <returns>An AClient.</returns>
        public AClient CreateMemoryProxy()
        {
            var factory = new RpcMemoryProxyFactory(new AService());
            return new Rpc<AClient>(factory).Ins;
        }

        /// <summary>
        /// Creates the proxy test.
        /// </summary>
        [TestMethod]
        public void CreateProxyTest()
        {
            CreateProxy();
        }

        /// <summary>
        /// Proxies the test.
        /// </summary>
        [TestMethod]
        public void ProxyTest()
        {
            var alient = CreateProxy();
            const int initData = 4;
            alient.A = initData;
            var r = alient.GetAValue(initData);
            Assert.AreEqual(initData, r);
        }

        /// <summary>
        /// Proxies the exception.
        /// </summary>
        [TestMethod]
        public void ProxyException()
        {
            var alient = CreateProxy();
            const int initData = 4;
            alient.A = initData;
            Assert.ThrowsException<Exception>(() => alient.GetAValue(initData - 1));
        }

        /// <summary>
        /// Proxies the retry count.
        /// </summary>
        [TestMethod]
        public void ProxyRetryCount()
        {
            var alient = CreateProxy();
            var initData = 4;
            alient.A = initData;
            try
            {
                alient.GetAValue(initData - 1);
            }
            catch (System.Exception)
            {
                Trace.TraceInformation(alient.A.ToString());
            }
            //执行一次  重试两次 共三次
            Assert.AreEqual(initData + 3, alient.A);
        }

        /// <summary>
        /// Creates the memory proxy test.
        /// </summary>
        [TestMethod]
        public void CreateMemoryProxyTest()
        {
            CreateMemoryProxy();
        }

        /// <summary>
        /// Memories the proxy excute test.
        /// </summary>
        [TestMethod]
        public void MemoryProxyExcuteTest()
        {
            var ins = CreateMemoryProxy();
            var i = 1;
            var a = ins.GetAValue(i);
            Assert.AreEqual(i + 1, a);
        }
    }

    /// <summary>
    /// The a client.
    /// </summary>
    public class AClient
    {
        /// <summary>
        /// Gets or sets the a.
        /// </summary>
        public int A { get; set; }

        /// <summary>
        /// Gets the a value.
        /// </summary>
        /// <param name="a">The a.</param>
        /// <returns>An int.</returns>
        public virtual int GetAValue(int a)
        {
            if (A != a)
            {
                A++;
                throw new Exception();
            }
            return a;
        }
    }

    /// <summary>
    /// The a base.
    /// </summary>
    public abstract class ABase
    {
        /// <summary>
        /// Gets the a value.
        /// </summary>
        /// <param name="a">The a.</param>
        /// <returns>An int.</returns>
        public virtual int GetAValue(int a)
        {
            throw new Exception();
        }
    }

    /// <summary>
    /// The a service.
    /// </summary>
    public class AService : ABase
    {
        /// <summary>
        /// Gets the a value.
        /// </summary>
        /// <param name="a">The a.</param>
        /// <returns>An int.</returns>
        public override int GetAValue(int a)
        {
            return 1 + a;
        }
    }
}