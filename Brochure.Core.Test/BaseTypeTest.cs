using System;
using Brochure.Core.System;
using Xunit;

namespace Brochure.Core.Test
{
    public class BaseTypeTest
    {
        [Fact]
        public void TestName ()
        {
            //Given
            var a = 1;
            var b = 0.1f;
            var c = 0.1;
            var d = DateTime.Now;
            byte e = 1;
            long f = 1;
            var type = a.GetType ();
            Assert.Equal (BaseType.Int, type.Name);

            type = b.GetType ();
            Assert.Equal (BaseType.Float, type.Name);

            type = c.GetType ();
            Assert.Equal (BaseType.Double, type.Name);

            type = d.GetType ();
            Assert.Equal (BaseType.DateTime, type.Name);

            type = e.GetType ();
            Assert.Equal (BaseType.Byte, type.Name);

            type = f.GetType ();
            Assert.Equal (BaseType.Long, type.Name);
            //When

            //Then
        }
    }
}