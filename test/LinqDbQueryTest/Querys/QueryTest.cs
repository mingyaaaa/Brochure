using Brochure.ORM;
using Brochure.ORM.Database;
using Brochure.ORM.MySql;
using Brochure.ORM.Querys;
using Brochure.ORMTest.Datas;
using LinqDbQueryTest.Datas;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;

namespace Brochure.ORMTest.Querys
{
    [TestClass]
    public class QueryTest : BaseTest
    {
        private readonly IDbProvider provider;
        private DbOption option;
        private Mock<TransactionManager> transactionManager;
        private IQueryBuilder queryBuilder;

        public QueryTest()
        {
            provider = base.Provider.GetService<IDbProvider>();
            transactionManager = new Mock<TransactionManager>();
            queryBuilder = base.Provider.GetService<IQueryBuilder>();
        }

        [TestMethod]
        public void QuerySelect()
        {
            var option = new MySqlOption();

            var query = queryBuilder.Build(Query.From<Students>());

            Assert.AreEqual("select * from `Students`", query.SQL);

            var q = queryBuilder.Build(Query.From<Students>().Select(t => t.Id));
            Assert.AreEqual("select `Students`.`Id` from `Students`", q.SQL);

            var query2 = queryBuilder.Build(Query.From<Peoples, Students>().Select((p, s) => new { p.Id, s.ClassId }));
            Assert.AreEqual("select `Peoples`.`Id` as Id,`Students`.`ClassId` as ClassId from `Peoples`,`Students`", query2.SQL);

            var query3 = queryBuilder.Build(Query.From<Peoples, Students, Teachers>().Select((p, s, t) => new { p.Id, s.ClassId, s.School, t.Job }));
            Assert.AreEqual("select `Peoples`.`Id` as Id,`Students`.`ClassId` as ClassId,`Students`.`School` as School,`Teachers`.`Job` as Job from `Peoples`,`Students`,`Teachers`", query3.SQL);
        }

        [TestMethod]
        public void QueryJoin()
        {
            var query = Query.From<Students>().Join<Peoples>((s, p) => s.PeopleId == p.Id).Select((s, p) => new { s.ClassId, StudentId = s.Id, PeopleId = p.Id, p.Name });
            var result = queryBuilder.Build(query);

            Assert.AreEqual("select `Students`.`ClassId` as ClassId,`Students`.`Id` as StudentId,`Peoples`.`Id` as PeopleId,`Peoples`.`Name` as Name from `Students` join `Peoples` on `Students`.`PeopleId` = `Peoples`.`Id`", result.SQL);

            var query2 = Query.From<Students>().Join<Peoples>((s, p) => s.PeopleId == p.Id).Join<Classes>((s, _, c) => s.ClassId == c.Id).Select((_, __, c) => new { c.ClassName });

            result = queryBuilder.Build(query2);
            Assert.AreEqual("select `Classes`.`ClassName` as ClassName from `Students` join `Peoples` on `Students`.`PeopleId` = `Peoples`.`Id` join `Classes` on `Students`.`ClassId` = `Classes`.`Id`", result.SQL);

            var q3 = Query.From<Students, Peoples>().Join<Classes>((s, _, c) => s.ClassId == c.Id).Select((_, __, c) => new { c.ClassName });
            result = queryBuilder.Build(q3);
            Assert.AreEqual("select `Classes`.`ClassName` as ClassName from `Students`,`Peoples` join `Classes` on `Students`.`ClassId` = `Classes`.`Id`", result.SQL);
        }

        [TestMethod]
        public void QueryGroup()
        {
            var query = Query.From<Students>();
            var q = query.Groupby(t => t.School).Select(t => new { School = t.Key, Count = t.Count() });
            var sql = queryBuilder.Build(q);
            Assert.AreEqual("select `Students`.`School` as School,count(`Students`.`School`) as Count from `Students` group by `Students`.`School`", sql.SQL);

            var q1 = query.Groupby(t => new { t.School, t.ClassId }).Select(t => new { t.Key.School, t.Key.ClassId, Count = t.Count() });
            sql = queryBuilder.Build(q1);
            Assert.AreEqual("select `Students`.`School` as School,`Students`.`ClassId` as ClassId,count(`Students`.`ClassId`) as Count from `Students` group by `Students`.`School`,`Students`.`ClassId`", sql.SQL);

            var q2 = query.Groupby(t => new { t.School, t.ClassId }).Select(t => new { t.Key.School, t.Key.ClassId, Min = t.Min(p => p.ClassCount) });
            sql = queryBuilder.Build(q2);
            Assert.AreEqual("select `Students`.`School` as School,`Students`.`ClassId` as ClassId,min(`Students`.`ClassCount`) as Min from `Students` group by `Students`.`School`,`Students`.`ClassId`", sql.SQL);
            //Trace.TraceInformation (sql);
        }

        [TestMethod]
        public void QueryOrder()
        {
            var query = Query.From<Students>();
            var q = query.OrderBy(t => t.ClassCount);
            var sql = queryBuilder.Build(query);
            Assert.AreEqual("select * from `Students` order by `Students`.`ClassCount`", sql.SQL);
            var query2 = Query.From<Students, Peoples>();
            var q2 = query2.OrderBy((t, _) => t.ClassCount);
            sql = queryBuilder.Build(q2);
            Assert.AreEqual("select * from `Students`,`Peoples` order by `Students`.`ClassCount`", sql.SQL);
            var q3 = query2.OrderBy((t, p) => new { t.ClassCount, p.Age });
            sql = queryBuilder.Build(q3);
            Assert.AreEqual("select * from `Students`,`Peoples` order by `Students`.`ClassCount`,`Peoples`.`Age`", sql.SQL);
        }

        [TestMethod]
        public void QueryOrderDesc()
        {
            var query = Query.From<Students>();
            var q = query.OrderByDesc(t => t.ClassCount);
            var sql = queryBuilder.Build(query);
            Assert.AreEqual("select * from `Students` order by `Students`.`ClassCount` desc", sql.SQL);
            var query2 = Query.From<Students, Peoples>();
            var q2 = query2.OrderByDesc((t, _) => t.ClassCount);
            sql = queryBuilder.Build(q2);
            Assert.AreEqual("select * from `Students`,`Peoples` order by `Students`.`ClassCount` desc", sql.SQL);
            var q3 = query2.OrderByDesc((t, p) => new { t.ClassCount, p.Age });
            sql = queryBuilder.Build(q3);
            Assert.AreEqual("select * from `Students`,`Peoples` order by `Students`.`ClassCount`,`Peoples`.`Age` desc", sql.SQL);
        }

        [TestMethod]
        public void QueryWhereAnd()
        {
            var query = Query.From<Students>();
            var q = query.WhereAnd(t => t.ClassCount == 1 && t.ClassId == "a");
            var sql = queryBuilder.Build(query);
            Assert.AreEqual("select * from `Students` where `Students`.`ClassCount` = @p0 and `Students`.`ClassId` = @p1", sql.SQL);
            Assert.AreEqual(2, sql.Parameters.Count);

            const int a = 1;
            const string astr = "a";
            query = Query.From<Students>();
            var q2 = query.WhereAnd(t => t.ClassCount == a && t.ClassId == astr);
            sql = queryBuilder.Build(query);
            Assert.AreEqual("select * from `Students` where `Students`.`ClassCount` = @p0 and `Students`.`ClassId` = @p1", sql.SQL);
            Assert.AreEqual(2, sql.Parameters.Count);

            var q3 = q2.WhereAnd(t => t.Id == "c");
            sql = queryBuilder.Build(query);
            Assert.AreEqual("select * from `Students` where `Students`.`ClassCount` = @p0 and `Students`.`ClassId` = @p1 and (`Students`.`Id` = @p2)", sql.SQL);
            Assert.AreEqual(3, sql.Parameters.Count);
        }

        [TestMethod]
        public void QueryWhereOr()
        {
            var query = Query.From<Students>();
            var q = query.WhereOr(t => t.ClassCount == 1 || t.ClassId == "a");
            var sql = queryBuilder.Build(query);
            Assert.AreEqual("select * from `Students` where `Students`.`ClassCount` = @p0 or `Students`.`ClassId` = @p1", sql.SQL);
            Assert.AreEqual(2, sql.Parameters.Count);

            const int a = 1;
            const string astr = "a";
            query = Query.From<Students>();
            var q2 = query.WhereOr(t => t.ClassCount == a || t.ClassId == astr);
            sql = queryBuilder.Build(query);
            Assert.AreEqual("select * from `Students` where `Students`.`ClassCount` = @p0 or `Students`.`ClassId` = @p1", sql.SQL);
            Assert.AreEqual(2, sql.Parameters.Count);

            var q3 = q2.WhereOr(t => t.Id == "c");
            sql = queryBuilder.Build(query);
            Assert.AreEqual("select * from `Students` where `Students`.`ClassCount` = @p0 or `Students`.`ClassId` = @p1 or (`Students`.`Id` = @p2)", sql.SQL);
            Assert.AreEqual(3, sql.Parameters.Count);
        }

        [TestMethod]
        public void QueryWhereAndOr()
        {
            var query = Query.From<Students>();
            var q = query.WhereAnd(t => t.ClassCount == 1 && t.ClassId == "a").WhereOr(t => t.Id == "c");
            var r = queryBuilder.Build(q);
            Assert.AreEqual("select * from `Students` where `Students`.`ClassCount` = @p0 and `Students`.`ClassId` = @p1 or (`Students`.`Id` = @p2)", r.SQL);
            Assert.AreEqual(3, r.Parameters.Count);
        }

        [TestMethod]
        public void QueryWhere()
        {
            var query = Query.From<Students>();
            var q = query.Where(t => t.ClassCount == 1 && t.ClassId == "a");
            var r = queryBuilder.Build(q);
            Assert.AreEqual("select * from `Students` where `Students`.`ClassCount` = @p0 and `Students`.`ClassId` = @p1", r.SQL);
            Assert.AreEqual(2, r.Parameters.Count);

            var query1 = Query.Where<Students>(t => t.ClassId == "a");
            r = queryBuilder.Build(query1);
            Assert.AreEqual("where `Students`.`ClassId` = @p0", r.SQL);
            Assert.AreEqual(1, r.Parameters.Count);
        }

        [TestMethod]
        public void QueryWithIEntityKey()
        {
            IEntityKey<string> teacher = new Teachers();
            var query = Query.From<Teachers>().Where(t => t.Id == teacher.Id);
            var r = queryBuilder.Build(query);

            Assert.AreEqual("select * from `Teachers` where `Teachers`.`Id` = @p0", r.SQL);
            Assert.AreEqual(teacher.Id, r.Parameters[0].Value);

            var teacher1 = new FTeachers();
            query = Query.From<Teachers>().Where(t => t.Id == teacher1.Teachers.Id);
            r = queryBuilder.Build(query);

            Assert.AreEqual("select * from `Teachers` where `Teachers`.`Id` = @p0", r.SQL);

            IEntityKey<string> tea = new Teachers();
            IQuery q = Query.Where<Teachers>(t => t.Id == tea.Id);
            r = queryBuilder.Build(q);

            Assert.AreEqual("where `Teachers`.`Id` = @p0", r.SQL);
        }

        [TestMethod]
        public void QueryTakeAndSkip()
        {
            var query = Query.From<Teachers>().Take(1);
            var r = queryBuilder.Build(query);
            Assert.AreEqual("select * from `Teachers` limit @p0", r.SQL);
            Assert.AreEqual(1, r.Parameters.Count);
        }

        [TestMethod]
        public void QueryGenericType()
        {
            var teacher = new Teachers();
            var repository = new QueryGeneric<Teachers>(queryBuilder);
            var sql = repository.GetGenericSql(teacher.Id);
            Assert.AreEqual("where `Teachers`.`Id` = @p0", sql);
        }

        private class QueryGeneric<T> where T : IEntityKey<string>
        {
            private readonly IQueryBuilder _queryBuilder;

            public QueryGeneric(IQueryBuilder queryBuilder)
            {
                _queryBuilder = queryBuilder;
            }

            public string GetGenericSql(string id)
            {
                return _queryBuilder.Build(Query.Where<T>((T t) => t.Id == id)).SQL;
            }
        }
    }
}