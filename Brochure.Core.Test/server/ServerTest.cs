using System;
using System.Collections.Generic;
using Brochure.Core.Abstracts;
using Brochure.Core.Interfaces;
using Brochure.Core.Server.Abstracts;
using Xunit;

namespace Brochure.Core.Test.server
{
    public class S1 : AbSingleton, IS1
    {

    }
    public interface IS1
    {

    }
    public class T1 : EntityBase
    {

    }
    public abstract class abc : AbSingleton
    {

    }
    public class abcA : abc
    {

    }
    public class ServerTest
    {
        [Fact]
        public void TypeMapTest ()
        {
            var aa = new abcA ();
            var bb = AbSingleton.GetInstance<abc> ();
            Assert.True (aa == bb);
            var cc = AbSingleton.GetInstance<abcA> ();
            Assert.True (aa == cc);
            Assert.True (bb == cc);
            try
            {
                var dd = new abcA ();
                Assert.False (false);
            }
            catch (Exception)
            {
                Assert.True (true);
            }
            //测试接口
            try
            {
                var s11 = AbSingleton.GetInstance<S1> ();
                Assert.False (false);
            }
            catch (Exception)
            {
                Assert.True (true);
            }
            AbSingleton.Regist<S1> ();
            var s1 = AbSingleton.GetInstance<S1> ();
            var s2 = AbSingleton.GetInstance<IS1> ();
            Assert.True (s1 == s2);
        }

        [Fact]
        public void TypeInstance ()
        {
            RegistTable ();
        }
        private void RegistTable (params EntityBase[] tables)
        {

        }
    }
}