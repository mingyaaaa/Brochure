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
            query2.Select ((p, s) => new { p.Id, s.ClassId });
            sql = query2.GetSql ();
            Assert.AreEqual ("select [Peoples].[Id] as Id,[Students].[Class] as Class from [Peoples],[Students]", sql);

            var query3 = new Query<Peoples, Students, Teachers> (option);
            query3.Select ((p, s, t) => new { p.Id, s.ClassId, s.School, t.Job });
            sql = query3.GetSql ();
            Assert.AreEqual ("select [Peoples].[Id] as Id,[Students].[Class] as Class,[Students].[School] as School,[Teachers].[Job] as Job from [Peoples],[Students],[Teachers]", sql);
        }

        [TestMethod]
        public void QueryJoin ()
        {
            var option = new MySqlOption (provider);
            var query = new Query<Students> (option);
            var q = query.Join<Peoples> ((s, p) => s.PeopleId == p.Id).Select ((s, p) => new { s.ClassId, StudentId = s.Id, PeopleId = p.Id, p.Name });
            var sql = q.GetSql ();
            Assert.AreEqual ("select [Students].[ClassId] as ClassId,[Students].[Id] as StudentId,[Peoples].[Id] as PeopleId,[Peoples].[Name] as Name from [Students] join [Peoples] on [Students].[PeopleId] = [Peoples].[Id]", sql);

            var q2 = query.Join<Peoples> ((s, p) => s.PeopleId == p.Id).Join<Classes> ((s, p, c) => s.ClassId == c.Id).Select ((s, p, c) => new { c.ClassName });
            sql = q2.GetSql ();
            Assert.AreEqual (@"select [Classes].[ClassName] as ClassName from [Students] join [Peoples] on [Students].[PeopleId] = [Peoples].[Id] join [Peoples] on [Students].[PeopleId] = [Peoples].[Id] join [Classes] on [Students].[ClassId] = [Classes].[Id]", sql);

            var query2 = new Query<Students, Peoples> (option);
            var q3 = query2.Join<Classes> ((s, p, c) => s.ClassId == c.Id).Select ((s, p, c) => new { c.ClassName });
            sql = q3.GetSql ();
            Assert.AreEqual ("select [Classes].[ClassName] as ClassName from [Students],[Peoples] join [Classes] on [Students].[ClassId] = [Classes].[Id]", sql);
        }

        [TestMethod]
        public void QueryGroup () { }

        [TestMethod]
        public void QueryOrder () { }

        [TestMethod]
        public void QueryWhere () { }
    }
}