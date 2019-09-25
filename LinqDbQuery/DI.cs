using System;
using AspectCore.Injector;

namespace LinqDbQuery
{
    public class DI
    {

        /// <summary>
        /// 测试使用
        /// </summary>
        public DI ()
        {
            var container = new ServiceContainer ();
            this.ServiceProvider = container.Build ();
        }
        public DI (IServiceResolver serviceResolver)
        {
            this.ServiceProvider = serviceResolver;
        }
        public static DI Ins { get; } = new DI ();
        public IServiceResolver ServiceProvider { get; }
    }
}