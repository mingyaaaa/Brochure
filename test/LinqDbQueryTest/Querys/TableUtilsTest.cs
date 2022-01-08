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
        /// <summary>
        /// Tests the get table name by class.
        /// </summary>
        [TestMethod]
        public void TestGetTableNameByClass()
        {
            var name = TableUtlis.GetTableName<A>();
            Assert.AreEqual("A", name);
        }

        /// <summary>
        /// Tests the get table name by table attribute.
        /// </summary>
        [TestMethod]
        public void TestGetTableNameByTableAttribute()
        {
            var name = TableUtlis.GetTableName<B>();
            Assert.AreEqual("aaa", name);
        }

        /// <summary>
        /// Tests the get table name by type.
        /// </summary>
        [TestMethod]
        public void TestGetTableNameByType()
        {
            var name = TableUtlis.GetTableName(typeof(B));
            Assert.AreEqual("aaa", name);
        }


        /// <summary>
        /// The a.
        /// </summary>
        private class A
        {


        }
        /// <summary>
        /// The b.
        /// </summary>
        [Table("aaa")]
        public class B
        {

        }
    }
}
