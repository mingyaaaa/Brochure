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
            const int i = 1;
            const string str = "aaa";
            Assert.IsFalse (ObjectTypeUtils.IsIEnumerable (i));
            Assert.IsFalse (ObjectTypeUtils.IsIEnumerable (str));
            var a = new int[] { 1, 2, 3 };
            Assert.IsTrue (ObjectTypeUtils.IsIEnumerable (a));
            var list = new List<int>
            {
                i
            };
            Assert.IsTrue (ObjectTypeUtils.IsIEnumerable (list));
        }
    }
}