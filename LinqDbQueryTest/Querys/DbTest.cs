using System.Diagnostics;
using System.Runtime.Loader;
using Brochure.Abstract;
using Brochure.Core;
using Brochure.LinqDbQuery.MySql;
using LinqDbQueryTest.Datas;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace LinqDbQueryTest.Querys
{
    [TestClass]
    public class DbTest
    {
        public MySqlDbProvider provider { get; }
        private MySqlOption option;

        private MySqlDbSql dbSql;
        public DbTest ()
        {
            provider = new MySqlDbProvider () { IsUseParamers = false };
            option = new MySqlOption (provider);
            ObjectConverCollection.RegistObjectConver<Record> ();
            dbSql = new MySqlDbSql (option);
        }

        [TestMethod]
        public void TestDbData ()
        {
            var dbData = new MySqlDbData (option, new MySqlDbSql (option));
            var obj = new Students ()
            {
                ClassCount = 1,
                ClassId = "11",
                School = "dd",
            };
            var sql = dbSql.GetInsertSql (obj);
            Assert.AreEqual ("insert into [Students]([School],[ClassId],[ClassCount]) values('dd','11',1)", sql.Item1);

            sql = dbSql.GetDeleteSql<Students> (t => t.ClassId == "1" && t.ClassCount == 2);
            Assert.AreEqual ("delete from [Students] where [Students].[ClassId] = '1' and [Students].[ClassCount] = 2", sql.Item1);
            sql = dbSql.GetUpdateSql<Students> (new
            {
                ClassId = "2"
            }, t => t.Id == "aa");
            Trace.TraceInformation (sql.Item1);
        }

    }
}