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
            var a = obj.Create<TestB, TestA>(testa);
            Assert.AreEqual(testa.A, a.A);
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
            var a = factory.Create<ITestA, TestA>();
            Assert.IsInstanceOfType(a, typeof(TestA));

            var c = factory.Create<ITestC, TestC>();
            Assert.IsInstanceOfType(c, typeof(TestC));
        }

        /// <summary>
        /// Tests the create obj.
        /// </summary>
        [TestMethod]
        public void TestCreateObjIConverPolicy()
        {
            var factory = new ObjectFactory();
            var testa = Fixture.Create<TestA>();
            var a = factory.Create<TestA, TestB>(testa);
            Assert.AreEqual(-1, a.A);
        }

        /// <summary>
        /// Tests the create obj.
        /// </summary>
        [TestMethod]
        public void TestCreateObj()
        {
            IObjectFactory factory = new ObjectFactory();
            var a = factory.Create<TestB>(new GetValueC(new Dictionary<string, object>()
            {
                [nameof(TestB.A)] = 1
            }));
            Assert.AreEqual(1, a.A);
        }

        /// <summary>
        /// Tests the create genier.
        /// </summary>
        [TestMethod]
        public void TestCreateGenier()
        {
            IObjectFactory factory = new ObjectFactory();
            Genet f2 = new Genet(factory);
            var a = f2.Create<TestB>(new GetValueC(new Dictionary<string, object>()
            {
                [nameof(TestB.A)] = 1
            }));
            Assert.AreEqual(1, a.A);
        }

        /// <summary>
        /// Tests the create i record.
        /// </summary>
        [TestMethod]
        public void TestCreateIRecord()
        {
            IObjectFactory factory = new ObjectFactory();
            var a = factory.Create<IRecord>(new GetValueC(new Dictionary<string, object>()
            {
                [nameof(TestB.A)] = 1
            }));
            Assert.AreEqual(1, a[nameof(TestB.A)]);
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
        public class TestA : ITestA, IConverPolicy<TestB>
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

        /// <summary>
        /// The get value c.
        /// </summary>
        public class GetValueC : IGetValue
        {
            private readonly IDictionary<string, object> _dic;

            /// <summary>
            /// Initializes a new instance of the <see cref="GetValueC"/> class.
            /// </summary>
            /// <param name="dic">The dic.</param>
            public GetValueC(IDictionary<string, object> dic)
            {
                _dic = dic;
            }

            /// <summary>
            /// Gets the properties.
            /// </summary>
            public IEnumerable<string> Properties => _dic.Keys;

            /// <summary>
            /// Gets the value.
            /// </summary>
            /// <param name="propertyName">The property name.</param>
            /// <returns>A T.</returns>
            public T GetValue<T>(string propertyName)
            {
                return (T)GetValue(propertyName);
            }

            /// <summary>
            /// Gets the value.
            /// </summary>
            /// <param name="propertyName">The property name.</param>
            /// <returns>An object.</returns>
            public object GetValue(string propertyName)
            {
                _dic.TryGetValue(propertyName, out var obj);
                return obj;
            }
        }

        /// <summary>
        /// The genet.
        /// </summary>
        public class Genet
        {
            private readonly IObjectFactory _objectFactory;

            /// <summary>
            /// Initializes a new instance of the <see cref="Genet"/> class.
            /// </summary>
            /// <param name="objectFactory">The object factory.</param>
            public Genet(IObjectFactory objectFactory)
            {
                _objectFactory = objectFactory;
            }

            /// <summary>
            /// Creates the.
            /// </summary>
            /// <param name="getValue">The get value.</param>
            /// <returns>A T.</returns>
            public T Create<T>(IGetValue getValue) where T : class, new()
            {
                return _objectFactory.Create<T>(getValue);
            }
        }
    }
}