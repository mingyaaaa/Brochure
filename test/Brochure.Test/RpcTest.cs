using Brochure.Core.RPC;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;

namespace Brochure.Test
{
    [TestClass]
    public class RpcTest
    {

        public AClient CreateProxy()
        {
            var factory = new RpcPollyProxyFactory(new PollyOption() { RetryCount = 2 });
            return new Rpc<AClient>(factory).Ins;
        }

        public AClient CreateMemoryProxy()
        {
            var factory = new RpcMemoryProxyFactory(new AService());
            return new Rpc<AClient>(factory).Ins;
        }

        [TestMethod]
        public void CreateProxyTest()
        {
            CreateProxy();
        }

        [TestMethod]
        public void ProxyTest()
        {
            var alient = CreateProxy();
            const int initData = 4;
            alient.A = initData;
            var r = alient.GetAValue(initData);
            Assert.AreEqual(initData, r);
        }

        [TestMethod]
        public void ProxyException()
        {
            var alient = CreateProxy();
            const int initData = 4;
            alient.A = initData;
            Assert.ThrowsException<Exception>(() => alient.GetAValue(initData - 1));
        }

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

        [TestMethod]
        public void CreateMemoryProxyTest()
        {
            CreateMemoryProxy();
        }

        [TestMethod]
        public void MemoryProxyExcuteTest()
        {
            var ins = CreateMemoryProxy();
            var i = 1;
            var a = ins.GetAValue(i);
            Assert.AreEqual(i + 1, a);
        }
    }

    public class AClient
    {
        /// <summary>
        /// Gets or sets the a.
        /// </summary>
        public int A { get; set; }
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
    public abstract class ABase
    {
        public virtual int GetAValue(int a)
        {
            throw new Exception();
        }
    }
    public class AService : ABase
    {
        public override int GetAValue(int a)
        {
            return 1 + a;
        }
    }
}