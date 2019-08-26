using System;
using System.Linq.Expressions;
using LinqDbQuery;
using LinqDbQuery.Visitors;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LinqDbQueryTest
{
    [TestClass]
    public class TestVisitor
    {

        private ORMVisitor visitor;
        public TestVisitor ()
        {

        }

        [TestMethod]
        public void TestWhereVisitor ()
        {
            visitor = new WhereVisitor (new MySqlDbProvider ());
            Expression<Func<Peoples, bool>> ex = t => t.Id == "1";
            var a = visitor.Visit (ex);
            var sql = visitor.GetSql ().ToString ().Trim ();
            Assert.AreEqual ("where [Peoples].[Id] = '1'", sql);

            ex = t => t.Age == 1;
            visitor.Visit (ex);
            sql = visitor.GetSql ().ToString ().Trim ();
            Assert.AreEqual ("where [Peoples].[Age] = 1", sql);

        }
    }
}