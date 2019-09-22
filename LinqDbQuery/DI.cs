using System;
using AspectCore.Injector;

namespace LinqDbQuery
{
    public class DI
    {

        public static DI Ins { get; } = new DI ();
        public IServiceResolver ServiceProvider { get; }
    }
}