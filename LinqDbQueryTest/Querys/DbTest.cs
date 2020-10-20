using System;
using System.Diagnostics;
using Brochure.Abstract;
using Brochure.Abstract.Models;
using Brochure.Extensions;
using Brochure.LinqDbQuery.MySql;
using Brochure.ORM;
using Brochure.ORM.Database;
using LinqDbQueryTest.Datas;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
namespace Brochure.ORMTest.Querys
{
    [TestClass]
    public class DbTest : BaseTest
    {
        public IDbProvider provider { get; }
        private readonly DbOption option;

        private readonly DbSql dbSql;
        private Mock<TransactionManager> transactionManager;
        public DbTest ()
        {
            transactionManager = new Mock<TransactionManager> ();
            provider = base.Provider.GetService<IDbProvider> ();
            option = base.Provider.GetService<DbOption> ();
            option.IsUseParamers = false;
            ObjectConverCollection.RegistObjectConver<IRecord> (t => new Record (t.AsDictionary ()));
            dbSql = base.Provider.GetService<DbSql> ();
        }

        [TestMethod]
        public void TestDbData ()
        {
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

        [TestMethod]
        public void TestDbDatabase ()
        {
            var sql = dbSql.GetCreateDatabaseSql ("testdb");
            Assert.AreEqual ("create database testdb", sql);
            sql = dbSql.GetDeleteDatabaseSql ("testdb");
            Assert.AreEqual ("drop database testdb", sql);
            sql = dbSql.GetAllDatabaseNameSql ();
            Assert.AreEqual ("select schema_name from information_schema.schemata", sql);
            sql = dbSql.GetDataBaseNameCountSql ("testdb");
            Assert.AreEqual ("SELECT count(1) FROM information_schema.SCHEMATA where SCHEMA_NAME='testdb'", sql);
            Trace.TraceInformation (sql);
        }

        [TestMethod]
        public void TestDbTable ()
        {
            var sql = dbSql.GetTableNameCountSql ("testTable");
            Assert.AreEqual ("SELECT count(1) FROM information_schema.TABLES WHERE table_name ='testTable'", sql);
            sql = dbSql.GetAllTableName ("testdb");
            Assert.AreEqual ("select table_name from information_schema.tables where table_schema='testdb'", sql);
            sql = dbSql.GetCreateTableSql<Students> ();
            Assert.AreEqual ("create table Students(Id nvarchar(36),School nvarchar(255),ClassId nvarchar(255),PeopleId nvarchar(255),ClassCount decimal,PRIMARY KEY ( Id ))", sql);
            sql = dbSql.GetDeleteTableSql ("testTable");
            Assert.AreEqual ("drop table testTable", sql);
            sql = dbSql.GetUpdateTableNameSql ("testTable", "newTable");
            Assert.AreEqual ("alter table testTable rename newTable", sql);
            Trace.TraceInformation (sql);
        }

        [TestMethod]
        public void TestDbColumn ()
        {
            var sql = dbSql.GetAddllColumnSql ("testTable", "testColumn", System.TypeCode.String, true, 200);
            Assert.AreEqual ("alter table testTable add column testColumn nvarchar(200) not null", sql);
            sql = dbSql.GetColumsNameCountSql ("testdb", "testTable", "testColumn");
            Assert.AreEqual ("select COUNT(1) from information_schema.columns WHERE table_schema='testdb' and table_name = 'testTable' and column_name = 'testColumn'", sql);
            sql = dbSql.GetColumsSql ("testdb", "testTable");
            Assert.AreEqual ("select column_name from information_schema.columns where table_schema='testdb' and table_name='testTable'", sql);
            sql = dbSql.GetDeleteColumnSql ("testTable", "testColumn");
            Assert.AreEqual ("alter table testTable drop column testColumn", sql);
            sql = dbSql.GetRenameColumnNameSql ("testTable", "testColumn", "newColumn", TypeCode.Boolean);
            Assert.AreEqual ("alter table testTable change column testColumn newColumn decimal(1)", sql);
            sql = dbSql.GetUpdateColumnSql ("testTable", "testColumn", TypeCode.String, true, 200);
            Assert.AreEqual ("alter table testTable modify testColumn nvarchar(200) not null", sql);
            Trace.TraceInformation (sql);
        }

        [TestMethod]
        public void TestDbIndex ()
        {
            var sql = dbSql.GetCreateIndexSql ("test", new string[] { "column1", "column2" }, "column1_column2", "index");
            Assert.AreEqual ("create index column1_column2 on test(column1,column2)", sql);
            sql = dbSql.GetDeleteIndexSql ("test", "column1_column2");
            Assert.AreEqual ("drop index column1_column2 on test", sql);
            Trace.TraceInformation (sql);
        }
    }
}