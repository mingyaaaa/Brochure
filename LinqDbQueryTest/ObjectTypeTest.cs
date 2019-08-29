using System.Collections.Generic;
using LinqDbQuery;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LinqDbQueryTest
{
    [TestClass]
    public class ObjectTypeTest
    {
        [TestMethod]
        public void TestIEnumerable ()
        {
            var i = 1;
            var str = "aaa";
            Assert.IsFalse (ObjectTypeUtils.IsIEnumerable (i));
            Assert.IsFalse (ObjectTypeUtils.IsIEnumerable (str));
            var a = new int[] { 1, 2, 3 };
            Assert.IsTrue (ObjectTypeUtils.IsIEnumerable (a));
            var list = new List<int> ();
            list.Add (i);
            Assert.IsTrue (ObjectTypeUtils.IsIEnumerable (list));
        }
    }
}