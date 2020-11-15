using System;
using Brochure.Extensions;
using Brochure.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Brochure.Test
{
    [TestClass]
    public class ReflectorUtilTest
    {
        [TestMethod]
        public void GetSetPropertyValueFunTest ()
        {

            var obj = new TestA ();
            var fun = ReflectorUtil.Instance.GetSetPropertyValueFun<TestA, string> ("Ap");
            fun.Invoke (obj, "a");
            Assert.AreEqual (obj.Ap, "a");

            var fun1 = ReflectorUtil.Instance.GetSetPropertyValueFun<TestA> (typeof (string), "Ap");
            fun1.Invoke (obj, "a");
            Assert.AreEqual (obj.Ap, "a");

            var fun2 = ReflectorUtil.Instance.GetSetPropertyValueFun<TestA> (typeof (int), "Ab");
            fun2.Invoke (obj, 1);
            Assert.AreEqual (obj.Ab, 1);

        }

        [TestMethod]
        public void GetPropertyValueFunTest ()
        {
            var obj = new TestA ()
            {
                Ap = "a",
                Ab = 1,
            };
            var fun = ReflectorUtil.Instance.GetPropertyValueFun<TestA, string> ("Ap");
            var value = fun.Invoke (obj);
            Assert.AreEqual (value, "a");
            var fun1 = ReflectorUtil.Instance.GetPropertyValueFun<TestA> ("Ap");
            Assert.IsNotNull (fun1);
            var value1 = fun1.Invoke (obj);
            Assert.AreEqual (value1, "a");

            var fun2 = ReflectorUtil.Instance.GetPropertyValueFun<TestA> ("Ab");
            Assert.IsNotNull (fun2);
            var value2 = fun2.Invoke (obj);
            Assert.AreEqual (value2, 1);
        }
    }
    public class TestA
    {
        public string Ap { get; set; }

        public int Ab { get; set; }
    }
}