using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;

namespace Brochure.Test
{
    /// <summary>
    /// The proxy test.
    /// </summary>
    [TestClass]
    public class ProxyTest
    {
        /// <summary>
        /// Tests the proxy.
        /// </summary>
        [TestMethod]
        public void TestProxy()
        {
            //创建代理类，并把SampleProxy作为拦截器注入
            var sampleProxy = (targetInterface)SampleProxy.Create<targetInterface, SampleProxy>();
            var inter = sampleProxy.GetType().GetInterfaces();
            //执行接口方法
            sampleProxy.Write("here is invoke by proxy");
            sampleProxy.Write2(1);
        }
    }

    //需要被生成代理实例的接口
    /// <summary>
    /// The target interface.
    /// </summary>
    public interface targetInterface
    {
        //这个方法会被代理类实现
        /// <summary>
        /// Writes the.
        /// </summary>
        /// <param name="writesomeshing">The writesomeshing.</param>
        void Write(string writesomeshing);

        /// <summary>
        /// Write2S the.
        /// </summary>
        /// <param name="a">The a.</param>
        void Write2(int a);
    }

    //需要被生成代理实例的接口
    /// <summary>
    /// The imp interface.
    /// </summary>
    public class ImpInterface : targetInterface
    {
        /// <summary>
        /// Writes the.
        /// </summary>
        /// <param name="writesomeshing">The writesomeshing.</param>
        public virtual void Write(string writesomeshing)
        {
            Console.WriteLine(writesomeshing);
        }

        /// <summary>
        /// Write2S the.
        /// </summary>
        /// <param name="a">The a.</param>
        public void Write2(int a)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// The sample proxy.
    /// </summary>
    public class SampleProxy : DispatchProxy
    {
        /// <summary>
        /// 拦截调用
        /// </summary>
        /// <param name="method">所拦截的方法信息</param>
        /// <param name="parameters">所拦截方法被传入的参数指</param>
        /// <returns></returns>
        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            var name = targetMethod.Name;
            Console.WriteLine(args[0]);
            Console.WriteLine(name);
            return null;
        }
    }
}