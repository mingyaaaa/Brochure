using System;
using System.Diagnostics;
using AspectCore.DynamicProxy;
using Brochure.Core.RPC;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Brochure.Test
{
    [TestClass]
    public class RpcTest
    {

        public AClient CreateProxy()
        {
            var facory = new RpcPollyProxyFactory(new PollyOption() { RetryCount = 2 });
            return new Rpc<AClient>(facory).Ins;
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
            Assert.ThrowsException<AspectInvocationException>(() => alient.GetAValue(initData - 1));
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
    }

    public class AClient
    {
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