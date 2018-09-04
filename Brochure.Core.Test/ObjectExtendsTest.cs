using System;
using Xunit;
namespace Brochure.Core.Test
{
    public enum TestEnum
    {
        a = 1,
        b = 2
    }

    public class A
    {
        public string AStr { get; set; }
        public B BObject { get; set; }
        public int C { get; set; }
        public DateTime DateTime { get; set; }
    }
    public class B
    {
        public string BStr { get; set; }
        public int C { get; set; }
        public DateTime DateTime { get; set; }
    }
    public class AsTest
    {
        [Fact]
        public void StringTo()
        {
            var str = "1";
            Assert.Equal(1, str.As<int>());
            str = "aaa";
            try
            {
                str.As<int>(new Exception("aaaa"));

                Assert.True(false);
            }
            catch (Exception)
            {
                Assert.True(true);
            }
            str = "1.34";
            Assert.Equal(0, str.As<int>());
            Assert.Equal(1.34, str.As<double>());
            var date = "1992.3.6 13:6:7";
            date = "1992.3.6";
            Assert.Equal(1992, date.As<DateTime>().Year);
            date = "1992.3.6 25:6:7";
            try
            {
                date.As<DateTime>();
                Assert.True(false);
            }
            catch (Exception)
            {
                Assert.True(true);
            }
        }

        [Fact]
        public void EnumTo()
        {
            //测试枚举
            var te = TestEnum.a;
            Assert.Equal(1, te.As<int>());
            var i = 1;
            Assert.Equal(TestEnum.a, i.As<TestEnum>());
            i = 3;
            try
            {
                Assert.Equal(TestEnum.a, i.As<TestEnum>());
                Assert.True(false);
            }
            catch (Exception)
            {
                Assert.True(true);
            }
        }

        [Fact]
        public void IntTo()
        {
            var i = 1;
            Assert.Equal(1, i.As<int>());
            Assert.Equal(1, i.As<double>());
            Assert.Equal("1", i.As<string>());
        }

        [Fact]
        public void DoubleTo()
        {
            var d = 1.3;
            Assert.Equal(1, (int)d);
            //double 无法直接转int
            try
            {
                d.As<int>();
                Assert.True(false);
            }
            catch (Exception)
            {
                Assert.True(true);
            }

            Assert.Equal(1.3, d.As<double>());
            Assert.Equal("1.3", d.As<string>());
        }

        [Fact]
        public void DateTimeTo()
        {
            //Given
            var date = DateTime.Now;
            date.As<string>();
            try
            {
                date.As<int>();
                Assert.True(false);
            }
            catch (Exception)
            {
                Assert.True(true);
            }
            //When

            //Then
        }

        [Fact]
        public void NUllTo()
        {
            object obj = null;
            Assert.Equal(0, obj.As<int>());
            Assert.Equal(0, obj.As<double>());
            Assert.Null(obj.As<string>());
            Assert.Equal(default(DateTime), obj.As<DateTime>());

        }

        [Fact]
        public void RecordTo()
        {
            var a = new A();
            a.AStr = "AStr";
            a.C = 0;
            a.DateTime = DateTime.Now;
            var b = new B();
            b.BStr = "BStr";
            b.C = 1;
            b.DateTime = DateTime.Now.AddDays(-1);
            a.BObject = b;
            var ar = a.As<IRecord>();
            Assert.True(ar.ContainsKey(nameof(a.AStr)));
            Assert.True(ar.ContainsKey(nameof(a.BObject)));
            Assert.True(ar.ContainsKey(nameof(a.C)));
            Assert.True(ar.ContainsKey(nameof(a.DateTime)));
            Assert.Equal(a.AStr, ar[nameof(a.AStr)]);
            Assert.Equal(a.C, ar[nameof(a.C)]);
            Assert.Equal(a.DateTime, ar[nameof(a.DateTime)]);
            Assert.Equal(a.AStr, ar[nameof(a.AStr)]);
            var br = ar[nameof(a.BObject)].As<IRecord>();
            Assert.Equal(b.BStr, br[nameof(b.BStr)]);
            Assert.Equal(b.C, br[nameof(b.C)]);
            Assert.Equal(b.DateTime, br[nameof(b.DateTime)]);
        }
    }
}
