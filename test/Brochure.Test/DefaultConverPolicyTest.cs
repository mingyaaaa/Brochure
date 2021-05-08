using System;
using Brochure.Abstract;
using Brochure.Abstract.Models;
using Brochure.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Brochure.Test
{
    [TestClass]
    public class DefaultConverPolicyTest
    {
        [TestMethod]
        public void TestDefaultConverPolicy()
        {
            var policy = new DefaultConverPolicy();
            var a = new Pa()
            {
                A = "aa",
                B = 1,
            };
            var b = policy.ConverTo<Pa, Pb>(a);
            Assert.AreEqual(b.A, "aa");
            Assert.AreEqual(b.B, 1);
        }

        [TestMethod]
        public void TestRecordConverPolicy()
        {
            var policy = new GetValueConverPolicy();
            var record = new Record()
            {
                [nameof(Pa.A)] = "aa",
                [nameof(Pa.B)] = 1,
            };
            var b = policy.ConverTo<IRecord, Pa>(record);
            Assert.AreEqual(b.A, "aa");
            Assert.AreEqual(b.B, 1);
        }

        [TestMethod]
        public void TestObjectToRecordConverPolicy()
        {
            var policy = new ObjectToRecordConverPolicy<Pa>();
            var a = new Pa()
            {
                A = "aa",
                B = 3
            };
            var b = policy.ConverTo(a);
            Assert.AreEqual(b[nameof(Pa.A)], "aa");
            Assert.AreEqual(b[nameof(Pa.B)], 3);
        }
    }

    public class Pa
    {
        public string A { get; set; }
        public int B { get; set; }
    }

    public class Pb
    {
        public string A { get; set; }
        public int B { get; set; }
    }
}