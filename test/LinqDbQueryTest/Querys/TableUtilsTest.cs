using Brochure.ORM;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinqDbQueryTest.Querys
{
    /// <summary>
    /// The table utils test.
    /// </summary>
    [TestClass]
    public class TableUtilsTest
    {
        [TestMethod]
        public void TestGetTableNameByClass()
        {
            var name = TableUtlis.GetTableName<A>();
            Assert.AreEqual("A", name);
        }

        [TestMethod]
        public void TestGetTableNameByTableAttribute()
        {
            var name = TableUtlis.GetTableName<B>();
            Assert.AreEqual("aaa", name);
        }

        [TestMethod]
        public void TestGetTableNameByType()
        {
            var name = TableUtlis.GetTableName(typeof(B));
            Assert.AreEqual("aaa", name);
        }


        private class A
        {


        }
        [Table("aaa")]
        public class B
        {

        }
    }
}
