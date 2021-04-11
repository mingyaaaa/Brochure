using AutoFixture;
using Brochure.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public class TestA
        {
            public int? A { get; set; }
        }

        public class TestB
        {
            public int A { get; set; }
        }
    }
}




