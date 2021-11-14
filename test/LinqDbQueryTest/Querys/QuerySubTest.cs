using Brochure.ORM.Querys;
using Brochure.ORMTest;
using LinqDbQueryTest.Datas;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Brochure.ORMTest.Datas;

namespace LinqDbQueryTest.Querys
{
    [TestClass]
    public class QuerySubTest : BaseTest
    {
        private readonly IQueryBuilder queryBuilder;

        public QuerySubTest()
        {
            queryBuilder = base.Provider.GetService<IQueryBuilder>();
        }

        [TestMethod]
        public void TestFromSubQuery()
        {
            var q = Query.From<Students>();
            var q1 = Query.From(q).Select(t => new { t.Id, t.No });
            var r = queryBuilder.Build(q1);

            Assert.AreEqual("select TEMP0.`Id` as Id,TEMP0.`No` as No from (select * from `Students` ) TEMP0", r.SQL);
        }

        [TestMethod("匿名对象")]
        public void TestFromSubQuery2()
        {
            var q = Query.From<Students>().Select(t => new { t.ClassCount, t.ClassId });
            var q1 = Query.From(q).Select(t => t.ClassId);
            var r = queryBuilder.Build(q1);

            Assert.AreEqual("select TEMP0.`ClassId` from (select `Students`.`ClassCount` as ClassCount,`Students`.`ClassId` as ClassId from `Students` ) TEMP0", r.SQL);
        }

        [TestMethod]
        public void TestJoinSubQuery()
        {
            var q = Query.From<Classes>();
            var q2 = Query.From<Students>().Join(q, (s, c) => s.ClassId == c.Id);

            var r = queryBuilder.Build(q2);
            Assert.AreEqual("select * from `Students` join (select * from `Classes` ) TEMP0 on `Students`.`ClassId` = TEMP0.`Id`", r.SQL);
        }
    }
}