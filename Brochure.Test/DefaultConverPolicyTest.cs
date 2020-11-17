using System;
using Brochure.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Brochure.Test
{
    [TestClass]
    public class DefaultConverPolicyTest
    {
        [TestMethod]
        public void TestDefaultConverPolicy ()
        {
            var policy = new DefaultConverPolicy ();
            var a = new Pa ()
            {
                A = "aa",
                B = 1,
            };
            var b = policy.ConverTo<Pa, Pb> (a);
            Assert.AreEqual (b.A, "aa");
            Assert.AreEqual (b.B, 1);
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