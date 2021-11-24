using AutoFixture;
using Brochure.Abstract;
using Brochure.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Brochure.Test
{
    [TestClass]
    public class ObjectFactoryTest : BaseTest
    {
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

        public interface ITestA
        {
        }

        public class TestA : ITestA, IConverPolicy<TestB>
        {
            /// <summary>
            /// Gets or sets the a.
            /// </summary>
            public int? A { get; set; }

            public TestB ConverTo(TestB obj = null)
            {
                if (obj != null)
                {
                    obj.A = -1;
                }
                return obj;
            }
        }

        public class TestB
        {
            /// <summary>
            /// Gets or sets the a.
            /// </summary>
            public int A { get; set; }
        }

        public interface ITestC
        {
        }

        public class TestC : ITestC
        {
            public TestC()
            {
            }

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

        public class GetValueC : IGetValue
        {
            private readonly IDictionary<string, object> _dic;

            public GetValueC(IDictionary<string, object> dic)
            {
                _dic = dic;
            }

            public T GetValue<T>(string propertyName)
            {
                return (T)GetValue(propertyName);
            }

            public object GetValue(string propertyName)
            {
                _dic.TryGetValue(propertyName, out var obj);
                return obj;
            }
        }

        public class Genet
        {
            private readonly IObjectFactory _objectFactory;

            public Genet(IObjectFactory objectFactory)
            {
                _objectFactory = objectFactory;
            }

            public T Create<T>(IGetValue getValue) where T : class, new()
            {
                return _objectFactory.Create<T>(getValue);
            }
        }
    }
}