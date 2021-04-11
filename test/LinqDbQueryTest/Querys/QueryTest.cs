using System.Diagnostics;
using System.Linq;
using Brochure.LinqDbQuery.MySql;
using Brochure.ORM;
using Brochure.ORM.Database;
using Brochure.ORM.Querys;
using Brochure.ORMTest.Datas;
using LinqDbQueryTest.Datas;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

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

            var query = queryBuilder.Build<Students>();

            var sql = query.GetSql();
            Assert.AreEqual("select * from `Students`", sql);

            var q = query.Select(t => t.Id);
            sql = q.GetSql();
            Assert.AreEqual("select `Students`.`Id` from `Students`", sql);

            var query2 = queryBuilder.Build<Peoples, Students>();
            var q2 = query2.Select((p, s) => new { p.Id, s.ClassId });
            sql = q2.GetSql();
            Assert.AreEqual("select `Peoples`.`Id` as Id,`Students`.`ClassId` as ClassId from `Peoples`,`Students`", sql);

            var query3 = queryBuilder.Build<Peoples, Students, Teachers>();
            var q3 = query3.Select((p, s, t) => new { p.Id, s.ClassId, s.School, t.Job });
            sql = q3.GetSql();
            Assert.AreEqual("select `Peoples`.`Id` as Id,`Students`.`ClassId` as ClassId,`Students`.`School` as School,`Teachers`.`Job` as Job from `Peoples`,`Students`,`Teachers`", sql);
        }

        [TestMethod]
        public void QueryJoin()
        {
            var option = new MySqlOption();
            var query = queryBuilder.Build<Students>();
            var q = query.Join<Peoples>((s, p) => s.PeopleId == p.Id).Select((s, p) => new { s.ClassId, StudentId = s.Id, PeopleId = p.Id, p.Name });
            var sql = q.GetSql();
            Assert.AreEqual("select `Students`.`ClassId` as ClassId,`Students`.`Id` as StudentId,`Peoples`.`Id` as PeopleId,`Peoples`.`Name` as Name from `Students` join `Peoples` on `Students`.`PeopleId` = `Peoples`.`Id`", sql);

            var q2 = query.Join<Peoples>((s, p) => s.PeopleId == p.Id).Join<Classes>((s, _, c) => s.ClassId == c.Id).Select((_, __, c) => new { c.ClassName });
            sql = q2.GetSql();
            Assert.AreEqual("select `Classes`.`ClassName` as ClassName from `Students` join `Peoples` on `Students`.`PeopleId` = `Peoples`.`Id` join `Classes` on `Students`.`ClassId` = `Classes`.`Id`", sql);

            var query2 = queryBuilder.Build<Students, Peoples>();
            var q3 = query2.Join<Classes>((s, _, c) => s.ClassId == c.Id).Select((_, __, c) => new { c.ClassName });
            sql = q3.GetSql();
            Assert.AreEqual("select `Classes`.`ClassName` as ClassName from `Students`,`Peoples` join `Classes` on `Students`.`ClassId` = `Classes`.`Id`", sql);
        }

        [TestMethod]
        public void QueryGroup()
        {
            var option = new MySqlOption();
            var query = queryBuilder.Build<Students>();
            var q = query.Groupby(t => t.School).Select(t => new { School = t.Key, Count = t.Count() });
            var sql = q.GetSql();
            Assert.AreEqual("select `Students`.`School` as School,count(`Students`.`School`) as Count from `Students` group by `Students`.`School`", sql);

            var q1 = query.Groupby(t => new { t.School, t.ClassId }).Select(t => new { t.Key.School, t.Key.ClassId, Count = t.Count() });
            sql = q1.GetSql();
            Assert.AreEqual("select `Students`.`School` as School,`Students`.`ClassId` as ClassId,count(`Students`.`ClassId`) as Count from `Students` group by `Students`.`School`,`Students`.`ClassId`", sql);

            var q2 = query.Groupby(t => new { t.School, t.ClassId }).Select(t => new { t.Key.School, t.Key.ClassId, Min = t.Min(p => p.ClassCount) });
            sql = q2.GetSql();
            Assert.AreEqual("select `Students`.`School` as School,`Students`.`ClassId` as ClassId,min(`Students`.`ClassCount`) as Min from `Students` group by `Students`.`School`,`Students`.`ClassId`", sql);
            //Trace.TraceInformation (sql);
        }

        [TestMethod]
        public void QueryOrder()
        {
            var option = new MySqlOption();
            var query = queryBuilder.Build<Students>();
            var q = query.OrderBy(t => t.ClassCount);
            var sql = q.GetSql();
            Assert.AreEqual("select * from `Students` order by `Students`.`ClassCount`", sql);
            var query2 = queryBuilder.Build<Students, Peoples>();
            var q2 = query2.OrderBy((t, _) => t.ClassCount);
            sql = q2.GetSql();
            Assert.AreEqual("select * from `Students`,`Peoples` order by `Students`.`ClassCount`", sql);
            var q3 = query2.OrderBy((t, p) => new { t.ClassCount, p.Age });
            sql = q3.GetSql();
            Assert.AreEqual("select * from `Students`,`Peoples` order by `Students`.`ClassCount`,`Peoples`.`Age`", sql);
            //Trace.TraceInformation (sql);
        }

        [TestMethod]
        public void QueryWhereAnd()
        {
            var option = new MySqlOption();
            var query = queryBuilder.Build<Students>();
            var q = query.WhereAnd(t => t.ClassCount == 1 && t.ClassId == "a");
            var sql = q.GetSql();
            var paramss = q.GetDbDataParameters();
            Assert.AreEqual("select * from `Students` where `Students`.`ClassCount` = @p0 and `Students`.`ClassId` = @p1", sql);
            Assert.AreEqual(2, paramss.Count);

            const int a = 1;
            const string astr = "a";
            var q2 = query.WhereAnd(t => t.ClassCount == a && t.ClassId == astr);
            sql = q2.GetSql();
            paramss = q2.GetDbDataParameters();
            Assert.AreEqual("select * from `Students` where `Students`.`ClassCount` = @p0 and `Students`.`ClassId` = @p1", sql);
            Assert.AreEqual(2, paramss.Count);

            var q3 = q2.WhereAnd(t => t.Id == "c");
            sql = q3.GetSql();
            paramss = q3.GetDbDataParameters();
            Trace.TraceInformation(sql);
            Assert.AreEqual("select * from `Students` where `Students`.`ClassCount` = @p0 and `Students`.`ClassId` = @p1 and (`Students`.`Id` = @p2)", sql);
            Assert.AreEqual(3, paramss.Count);
        }

        [TestMethod]
        public void QueryWhereOr()
        {
            var option = new MySqlOption();
            var query = queryBuilder.Build<Students>();
            var q = query.WhereOr(t => t.ClassCount == 1 || t.ClassId == "a");
            var sql = q.GetSql();
            var paramss = q.GetDbDataParameters();
            Assert.AreEqual("select * from `Students` where `Students`.`ClassCount` = @p0 or `Students`.`ClassId` = @p1", sql);
            Assert.AreEqual(2, paramss.Count);

            const int a = 1;
            const string astr = "a";
            var q2 = query.WhereOr(t => t.ClassCount == a || t.ClassId == astr);
            sql = q2.GetSql();
            paramss = q2.GetDbDataParameters();
            Assert.AreEqual("select * from `Students` where `Students`.`ClassCount` = @p0 or `Students`.`ClassId` = @p1", sql);
            Assert.AreEqual(2, paramss.Count);

            var q3 = q2.WhereOr(t => t.Id == "c");
            sql = q3.GetSql();
            paramss = q3.GetDbDataParameters();
            Trace.TraceInformation(sql);
            Assert.AreEqual("select * from `Students` where `Students`.`ClassCount` = @p0 or `Students`.`ClassId` = @p1 or (`Students`.`Id` = @p2)", sql);
            Assert.AreEqual(3, paramss.Count);

            var q4 = q2.WhereAnd(t => t.Id == "c");
            sql = q4.GetSql();
            paramss = q4.GetDbDataParameters();
            Trace.TraceInformation(sql);
            Assert.AreEqual("select * from `Students` where `Students`.`ClassCount` = @p0 or `Students`.`ClassId` = @p1 and (`Students`.`Id` = @p2)", sql);
            Assert.AreEqual(3, paramss.Count);

        }
    }
}