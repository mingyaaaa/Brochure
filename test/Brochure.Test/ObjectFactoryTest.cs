using AutoFixture;
using Brochure.Abstract;
using Brochure.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Brochure.Test
{
    [TestClass]
    public class ObjectFactoryTest : BaseTest
    {
        [TestMethod]
        public void TestNullable()
        {
            Action<TestA, TestB> action = (t, o) => t.A = (int)(object)o.A;
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


        public interface ITestA
        {

        }
        public class TestA : ITestA, IConverPolicy<TestB>
        {
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
            public int C { get; set; }

            public string Str { get; set; }

            public TestA TestA { get; set; }
        }
    }
}




