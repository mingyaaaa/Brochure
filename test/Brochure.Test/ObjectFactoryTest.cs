using AutoFixture;
using Brochure.Abstract;
using Brochure.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Brochure.Test
{
    /// <summary>
    /// The object factory test.
    /// </summary>
    [TestClass]
    public class ObjectFactoryTest : BaseTest
    {
        /// <summary>
        /// Tests the nullable.
        /// </summary>
        [TestMethod]
        public void TestNullable()
        {
            Action<TestA, TestB> action = (t, o) => t.A = (int?)(object)o.A;
            var obj = new ObjectFactory();
            var testa = Fixture.Create<TestB>();
            action(new TestA(), testa);
            //   var a = obj.Create<TestB, TestA>(testa);
            //    Assert.AreEqual(testa.A, a.A);
        }

        /// <summary>
        /// Tests the create t.
        /// </summary>
        [TestMethod]
        public void TestCreateT()
        {
            var factory = new ObjectFactory();
            var testa = factory.Create<TestA>();
            Assert.IsNull(testa.A);
            var testb = factory.Create<TestB>();
            Assert.AreEqual(0, testb.A);
        }

        /// <summary>
        /// Tests the create t with params.
        /// </summary>
        [TestMethod]
        public void TestCreateTWithParams()
        {
            var factory = new ObjectFactory();
            var testa = factory.Create<TestC>(1, "a", new TestA());
            Assert.AreEqual(1, testa.C);
            Assert.AreEqual("a", testa.Str);
            Assert.AreEqual(null, testa.TestA.A);
        }

        /// <summary>
        /// Tests the create obj return interface.
        /// </summary>
        [TestMethod]
        public void TestCreateObjReturnInterface()
        {
            var factory = new ObjectFactory();
            //     var a = factory.Create<ITestA, TestA>();
            //   Assert.IsInstanceOfType(a, typeof(TestA));

            //      var c = factory.Create<ITestC, TestC>();
            //      Assert.IsInstanceOfType(c, typeof(TestC));
        }

        /// <summary>
        /// Tests the create obj.
        /// </summary>
        [TestMethod]
        public void TestCreateObjIConverPolicy()
        {
            var factory = new ObjectFactory();
            var testa = Fixture.Create<TestA>();
            //     var a = factory.Create<TestA, TestB>(testa);
            //     Assert.AreEqual(-1, a.A);
        }

        /// <summary>
        /// Tests the create obj.
        /// </summary>
        [TestMethod]
        public void TestCreateObj()
        {
            // IObjectFactory factory = new ObjectFactory();
            //var a = factory.Create<TestB>(new GetValueC(new Dictionary<string, object>()
            //{
            //    [nameof(TestB.A)] = 1
            //}));
            //Assert.AreEqual(1, a.A);
        }

        /// <summary>
        /// Tests the create genier.
        /// </summary>
        [TestMethod]
        public void TestCreateGenier()
        {
            IObjectFactory factory = new ObjectFactory();
            //    Genet f2 = new Genet(factory);
            //var a = f2.Create<TestB>(new GetValueC(new Dictionary<string, object>()
            //{
            //    [nameof(TestB.A)] = 1
            //}));
            //Assert.AreEqual(1, a.A);
        }

        /// <summary>
        /// Tests the create i record.
        /// </summary>
        [TestMethod]
        public void TestCreateIRecord()
        {
            IObjectFactory factory = new ObjectFactory();
            //var a = factory.Create<IRecord>(new GetValueC(new Dictionary<string, object>()
            //{
            //    [nameof(TestB.A)] = 1
            //}));
            //   Assert.AreEqual(1, a[nameof(TestB.A)]);
        }

        /// <summary>
        /// The test a.
        /// </summary>
        public interface ITestA
        {
        }

        /// <summary>
        /// The test a.
        /// </summary>
        public class TestA : ITestA
        {
            /// <summary>
            /// Gets or sets the a.
            /// </summary>
            public int? A { get; set; }

            /// <summary>
            /// Convers the to.
            /// </summary>
            /// <param name="obj">The obj.</param>
            /// <returns>A TestB.</returns>
            public TestB ConverTo(TestB obj = null)
            {
                if (obj != null)
                {
                    obj.A = -1;
                }
                return obj;
            }
        }

        /// <summary>
        /// The test b.
        /// </summary>
        public class TestB
        {
            /// <summary>
            /// Gets or sets the a.
            /// </summary>
            public int A { get; set; }
        }

        /// <summary>
        /// The test c.
        /// </summary>
        public interface ITestC
        {
        }

        /// <summary>
        /// The test c.
        /// </summary>
        public class TestC : ITestC
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="TestC"/> class.
            /// </summary>
            public TestC()
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="TestC"/> class.
            /// </summary>
            /// <param name="a">The a.</param>
            /// <param name="str">The str.</param>
            /// <param name="testA">The test a.</param>
            public TestC(int a, string str, TestA testA)
            {
                C = a;
                this.Str = str;
                TestA = testA;
            }

            /// <summary>
            /// Gets or sets the c.
            /// </summary>
            public int C { get; set; }

            /// <summary>
            /// Gets or sets the str.
            /// </summary>
            public string Str { get; set; }

            /// <summary>
            /// Gets or sets the test a.
            /// </summary>
            public TestA TestA { get; set; }
        }
    }
}