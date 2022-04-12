using Brochure.Abstract;
using Brochure.Abstract.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Brochure.Core.Test
{
    public enum TestEnum
    {
        a = 1,
        b = 2
    }

    /// <summary>
    /// The a.
    /// </summary>
    public class A
    {
        /// <summary>
        /// Gets or sets the a str.
        /// </summary>
        public string AStr { get; set; }

        /// <summary>
        /// Gets or sets the b object.
        /// </summary>
        public B BObject { get; set; }

        /// <summary>
        /// Gets or sets the c.
        /// </summary>
        public int C { get; set; }

        /// <summary>
        /// Gets or sets the date time.
        /// </summary>
        public DateTime DateTime { get; set; }
    }

    /// <summary>
    /// The b.
    /// </summary>
    public class B
    {
        /// <summary>
        /// Gets or sets the b str.
        /// </summary>
        public string BStr { get; set; }

        /// <summary>
        /// Gets or sets the c.
        /// </summary>
        public int C { get; set; }

        /// <summary>
        /// Gets or sets the date time.
        /// </summary>
        public DateTime DateTime { get; set; }
    }

    /// <summary>
    /// The c.
    /// </summary>
    public class C
    { }

    /// <summary>
    /// The as test.
    /// </summary>
    [TestClass]
    public class AsTest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AsTest"/> class.
        /// </summary>
        public AsTest()
        {
            var service = new ServiceCollection();
            service.AddLogging();
            service.AddBrochureCore();
        }

        /// <summary>
        /// Strings the to.
        /// </summary>
        [TestMethod]
        public void StringTo()
        {
            var str = "1";
            Assert.AreEqual(1, str.As<int>());
            str = "aaa";
            try
            {
                str.As<int>();

                Assert.IsTrue(false);
            }
            catch (Exception)
            {
                Assert.IsTrue(true);
            }
            str = "1.34";
            Assert.AreEqual(1.34, str.As<double>());
            var date = "1992.3.6 13:6:7";
            date = "1992.3.6";
            Assert.AreEqual(1992, date.As<DateTime>().Year);
            date = "1992.3.6 25:6:7";
            try
            {
                date.As<DateTime>();
                Assert.IsTrue(false);
            }
            catch (Exception)
            {
                Assert.IsTrue(true);
            }
        }

        /// <summary>
        /// Enums the to.
        /// </summary>
        [TestMethod]
        public void EnumTo()
        {
            //测试枚举
            var te = TestEnum.a;
            Assert.AreEqual(1, te.As<int>());
            var i = 1;
            Assert.AreEqual(TestEnum.a, i.As<TestEnum>());
            i = 3;
            try
            {
                Assert.AreEqual(TestEnum.a, i.As<TestEnum>());
                Assert.IsTrue(false);
            }
            catch (Exception)
            {
                Assert.IsTrue(true);
            }
        }

        /// <summary>
        /// Ints the to.
        /// </summary>
        [TestMethod]
        public void IntTo()
        {
            var i = 1;
            Assert.AreEqual(1, i.As<int>());
            Assert.AreEqual(1, i.As<double>());
            Assert.AreEqual("1", i.As<string>());
        }

        /// <summary>
        /// Doubles the to.
        /// </summary>
        [TestMethod]
        public void DoubleTo()
        {
            var d = 1.3;
            Assert.AreEqual(1, (int)d);
            //double 无法直接转int
            try
            {
                d.As<int>();
                Assert.IsTrue(false);
            }
            catch (Exception)
            {
                Assert.IsTrue(true);
            }

            Assert.AreEqual(1.3, d.As<double>());
            Assert.AreEqual("1.3", d.As<string>());
        }

        /// <summary>
        /// Dates the time to.
        /// </summary>
        [TestMethod]
        public void DateTimeTo()
        {
            //Given
            var date = DateTime.Now;
            date.As<string>();
            try
            {
                date.As<int>();
                Assert.IsTrue(false);
            }
            catch (Exception)
            {
                Assert.IsTrue(true);
            }
            //When

            //Then
        }

        /// <summary>
        /// Records the to.
        /// </summary>
        [TestMethod]
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
            Assert.IsTrue(ar.ContainsKey(nameof(a.AStr)));
            Assert.IsTrue(ar.ContainsKey(nameof(a.BObject)));
            Assert.IsTrue(ar.ContainsKey(nameof(a.C)));
            Assert.IsTrue(ar.ContainsKey(nameof(a.DateTime)));
            Assert.AreEqual(a.AStr, ar[nameof(a.AStr)]);
            Assert.AreEqual(a.C, ar[nameof(a.C)]);
            Assert.AreEqual(a.DateTime, ar[nameof(a.DateTime)]);
            Assert.AreEqual(a.AStr, ar[nameof(a.AStr)]);
            var br = ar[nameof(a.BObject)].As<IRecord>();
            Assert.AreEqual(b.BStr, br[nameof(b.BStr)]);
            Assert.AreEqual(b.C, br[nameof(b.C)]);
            Assert.AreEqual(b.DateTime, br[nameof(b.DateTime)]);
        }

        /// <summary>
        /// IS the b conver to.
        /// </summary>
        [TestMethod]
        public void IBConverTo()
        {
            var c = new C();
            var b = c.As<B>();
            Assert.AreEqual("C", b.BStr);
        }
    }
}