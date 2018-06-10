using Xunit;
using System;
using Brochure.Core.Server.SQLMap;
using Brochure.Core.Abstracts;
using Brochure.Core.Server.Abstracts;
using Brochure.Core.Interfaces;

namespace Brochure.Core.Test.server
{
    public class S1 : AbSingleton, IS1
    {

    }
    public interface IS1
    {

    }
    public class ServerTest
    {
        [Fact]
        public void TypeMapTest()
        {
            var aa = new SqlTypeMap();
            var bb = AbSingleton.GetInstance<AbTypeMap>();
            Assert.True(aa == bb);
            var cc = AbSingleton.GetInstance<SqlTypeMap>();
            Assert.True(aa == cc);
            Assert.True(bb == cc);
            try
            {
                var dd = new SqlTypeMap();
                Assert.False(false);
            }
            catch (Exception)
            {
                Assert.True(true);
            }
            //测试接口
            var s1 = AbSingleton.GetInstance<S1>();
            var s2 = AbSingleton.GetInstance<IS1>();
            Assert.True(s1 == s2);

        }
    }
}