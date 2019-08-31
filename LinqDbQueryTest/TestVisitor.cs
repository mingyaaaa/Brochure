using System;
using System.Linq;
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

            int[] array = new int[] { 1, 10, 2 };

            ex = t => array.Contains (t.Age);
            visitor.Visit (ex);
            sql = visitor.GetSql ().ToString ().Trim ();
            Assert.AreEqual ("where [Peoples].[Age] in (1,10,2)", sql);

            var name = "aaa";
            ex = t => t.Name.Contains (name);
            visitor.Visit (ex);
            sql = visitor.GetSql ().ToString ().Trim ();
            Assert.AreEqual ("where [Peoples].[Name] like '%aaa%'", sql);

            ex = t => t.Name.StartsWith (name);
            visitor.Visit (ex);
            sql = visitor.GetSql ().ToString ().Trim ();
            Assert.AreEqual ("where [Peoples].[Name] like '%aaa'", sql);

            ex = t => t.Name.EndsWith (name);
            visitor.Visit (ex);
            sql = visitor.GetSql ().ToString ().Trim ();
            Assert.AreEqual ("where [Peoples].[Name] like 'aaa%'", sql);

            ex = t => t.Name == null;
            visitor.Visit (ex);
            sql = visitor.GetSql ().ToString ().Trim ();
            Assert.AreEqual ("where [Peoples].[Name] is null", sql);

            ex = t => t.Name != null;

            visitor.Visit (ex);
            sql = visitor.GetSql ().ToString ().Trim ();
            Assert.AreEqual ("where [Peoples].[Name] is not null", sql);
            ex = t => t.Age != 1;
            visitor.Visit (ex);
            sql = visitor.GetSql ().ToString ().Trim ();
            Assert.AreEqual ("where [Peoples].[Age] < 1 and [Peoples].[Age] > 1", sql);
        }

        [TestMethod]
        public void TestSelectVisitor ()
        {
            visitor = new SelectVisitor (new MySqlDbProvider ());
            Expression<Func<Peoples, object>> ex = t => new { NewName = t.Name, NewAge = t.Age };
            var a = visitor.Visit (ex);
            var sql = visitor.GetSql ().ToString ().Trim ();
            Assert.AreEqual ("select [Peoples].[Name] as NewName,[Peoples].[Age] as NewAge from", sql);

        }

        [TestMethod]
        public void TestJoinVisitor ()
        {
            visitor = new JoinVisitor (typeof (Students), new MySqlDbProvider ());
            Expression<Func<Peoples, Students, bool>> ex = (p, s) => s.PeopleId == p.Id;
            visitor.Visit (ex);
            var sql = visitor.GetSql ().ToString ().Trim ();
            Assert.AreEqual ("join [Students] on [Students].[PeopleId] = [Peoples].[Id]", sql);
        }
    }
}