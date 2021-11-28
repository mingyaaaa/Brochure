using Brochure.ORM.Querys;
using Brochure.ORMTest;
using LinqDbQueryTest.Datas;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.DependencyInjection;
using Brochure.ORM;

namespace LinqDbQueryTest.QueryExpressionTest
{
    [TestClass]
    public class TestQueryExp : BaseTest
    {
        private ISqlBuilder builder;

        public TestQueryExp()
        {
            builder = Provider.GetService<ISqlBuilder>();
        }

        [TestMethod]
        public void TestSelectSql()
        {
            var query = Query.From<Peoples>().Select(t => new
            {
                t.Age
            });
            var r = builder.Build(query);
            Assert.AreEqual("select `Peoples`.`Age` as Age from `Peoples`", r.SQL);
        }
    }
}