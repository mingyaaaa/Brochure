using System.Diagnostics;
using LinqDbQuery.Querys;
using LinqDbQueryTest.Datas;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LinqDbQueryTest.Querys
{
    [TestClass]
    public class QueryTest
    {
        private readonly MySqlDbProvider provider;

        public QueryTest ()
        {
            provider = new MySqlDbProvider ();
        }

        [TestMethod]
        public void QueryInsert () { }

        [TestMethod]
        public void QueryInsertMany () { }

        [TestMethod]
        public void QueryUpdate () { }

        [TestMethod]
        public void QueryDelete () { }

        [TestMethod]
        public void QuerySelect ()
        {
            var option = new MySqlOption (provider);
            var query = new Query<Students> (option);
            query.Select (t => t.Id);
            var sql = query.GetSql ();
            Assert.AreEqual ("select [Students].[Id] from [Students]", sql);

            var query2 = new Query<Peoples, Students> (option);
            query2.Select ((p, s) => new { p.Id, s.Class });
            sql = query2.GetSql ();
            Assert.AreEqual ("select [Peoples].[Id] as Id,[Students].[Class] as Class from [Peoples],[Students]", sql);

            var query3 = new Query<Peoples, Students, Teachers> (option);
            query3.Select ((p, s, t) => new { p.Id, s.Class, s.School, t.Job });
            sql = query3.GetSql ();
            Assert.AreEqual ("select [Peoples].[Id] as Id,[Students].[Class] as Class,[Students].[School] as School,[Teachers].[Job] as Job from [Peoples],[Students],[Teachers]", sql);
        }

        [TestMethod]
        public void QueryJoin () { }

        [TestMethod]
        public void QueryGroup () { }

        [TestMethod]
        public void QueryOrder () { }

        [TestMethod]
        public void QueryWhere () { }
    }
}