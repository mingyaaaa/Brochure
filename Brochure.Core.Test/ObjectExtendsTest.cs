using System;
using Brochure.Core.Extends;
using Xunit;
namespace Brochure.Core.Test
{
    public enum TestEnum
    {
        a = 1,
        b = 2
    }
    public class UnitTest1
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
    }
}