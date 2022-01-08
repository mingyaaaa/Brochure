using System;
using Brochure.Abstract;
using Brochure.Abstract.Models;
using Brochure.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Brochure.Test
{
    /// <summary>
    /// The default conver policy test.
    /// </summary>
    [TestClass]
    public class DefaultConverPolicyTest
    {
        /// <summary>
        /// Tests the default conver policy.
        /// </summary>
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

            var policy1 = new GetValueConverPolicy();
            var record = new Record()
            {
                [nameof(Pa.A)] = "aa",
                [nameof(Pa.B)] = 1,
                [nameof(Pa.C)] = (decimal)22
            };
            var bb = policy1.ConverTo<IRecord, Pa>(record);
        }

        /// <summary>
        /// Tests the record conver policy.
        /// </summary>
        [TestMethod]
        public void TestRecordConverPolicy()
        {
            var policy = new GetValueConverPolicy();
            var record = new Record()
            {
                [nameof(Pa.A)] = "aa",
                [nameof(Pa.B)] = 1,
                [nameof(Pa.C)] = (decimal)22
            };
            var b = policy.ConverTo<IRecord, Pa>(record);
            Assert.AreEqual(b.A, "aa");
            Assert.AreEqual(b.B, 1);
        }

        /// <summary>
        /// Tests the object to record conver policy.
        /// </summary>
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

    /// <summary>
    /// The pa.
    /// </summary>
    public class Pa
    {
        /// <summary>
        /// Gets or sets the a.
        /// </summary>
        public string A { get; set; }

        /// <summary>
        /// Gets or sets the b.
        /// </summary>
        public int B { get; set; }

        /// <summary>
        /// Gets or sets the c.
        /// </summary>
        public long C { get; set; }
    }

    /// <summary>
    /// The pb.
    /// </summary>
    public class Pb
    {
        /// <summary>
        /// Gets or sets the a.
        /// </summary>
        public string A { get; set; }

        /// <summary>
        /// Gets or sets the b.
        /// </summary>
        public int B { get; set; }
    }
}