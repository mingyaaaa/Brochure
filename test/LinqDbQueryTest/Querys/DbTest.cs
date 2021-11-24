using System;
using System.Diagnostics;
using System.Linq;
using Brochure.Abstract;
using Brochure.Abstract.Extensions;
using Brochure.Abstract.Models;
using Brochure.Extensions;
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
        /// <summary>
        /// Gets the provider.
        /// </summary>
        public IDbProvider provider { get; }
        private readonly DbOption option;

        private readonly DbSql dbSql;
        private Mock<TransactionManager> transactionManager;

        public DbTest()
        {
            transactionManager = new Mock<TransactionManager>();
            provider = base.Provider.GetService<IDbProvider>();
            option = base.Provider.GetService<DbOption>();
            option.IsUseParamers = false;
            ObjectConverCollection.RegistObjectConver<IRecord>(t => new Record(t.AsDictionary()));
            dbSql = base.Provider.GetService<DbSql>();
        }

        [TestMethod]
        public void TestDbData()
        {
            var obj = new Students()
            {
                ClassCount = 1,
                ClassId = "11",
                School = "dd",
            };
            var sql = dbSql.GetInsertSql(obj);
            Assert.AreEqual("insert into `Students`(`School`,`ClassId`,`ClassCount`,`No`) values('dd','11',1,0)", sql.SQL);
            sql = dbSql.GetDeleteSql<Students>(t => t.ClassId == "1" && t.ClassCount == 2);
            Assert.AreEqual("delete from `Students` where `Students`.`ClassId` = '1' and `Students`.`ClassCount` = 2", sql.SQL);
            sql = dbSql.GetUpdateSql<Students>(new
            {
                ClassId = "2"
            }, t => t.Id == "aa");
            Trace.TraceInformation(sql.SQL);
        }

        [TestMethod]
        public void TestInsert()
        {
            var obj = new Students()
            {
                ClassCount = 1,
                ClassId = "11",
                School = "dd",
            };
            option.IsUseParamers = true;
            var sql = dbSql.GetInsertSql(obj);
            Assert.AreEqual("insert into `Students`(`School`,`ClassId`,`ClassCount`,`No`) values(@School,@ClassId,@ClassCount,@No)", sql.SQL);

            Assert.AreEqual(sql.Parameters.FirstOrDefault(t => t.ParameterName == "@School").Value, "dd");
            Assert.AreEqual(sql.Parameters.FirstOrDefault(t => t.ParameterName == "@ClassId").Value, "11");
            Assert.AreEqual(sql.Parameters.FirstOrDefault(t => t.ParameterName == "@ClassCount").Value, 1);
        }

        [TestMethod]
        public void TestDbDatabase()
        {
            var sql = dbSql.GetCreateDatabaseSql("testdb").SQL;
            Assert.AreEqual("create database testdb", sql);
            sql = dbSql.GetDeleteDatabaseSql("testdb").SQL;
            Assert.AreEqual("drop database testdb", sql);
            sql = dbSql.GetAllDatabaseNameSql().SQL;
            Assert.AreEqual("select schema_name from information_schema.schemata", sql);
            sql = dbSql.GetDataBaseNameCountSql("testdb").SQL;
            Assert.AreEqual("SELECT count(1) FROM information_schema.SCHEMATA where SCHEMA_NAME='testdb'", sql);
            Trace.TraceInformation(sql);
        }

        [TestMethod]
        public void TestDbTable()
        {
            option.DatabaseName = "testdb";
            var sql = dbSql.GetTableNameCountSql("testTable").SQL;
            Assert.AreEqual("SELECT count(1) FROM information_schema.TABLES WHERE table_name ='testTable' and TABLE_SCHEMA ='testdb'", sql);
            sql = dbSql.GetAllTableName("testdb").SQL;
            Assert.AreEqual("select table_name from information_schema.tables where table_schema='testdb'", sql);
            sql = dbSql.GetCreateTableSql<Students>().SQL;
            Assert.AreEqual("create table Students(Id nvarchar(36),School nvarchar(255),ClassId nvarchar(255),PeopleId nvarchar(255),ClassCount decimal not null,No decimal not null,PRIMARY KEY ( Id ))", sql);
            sql = dbSql.GetDeleteTableSql("testTable").SQL;
            Assert.AreEqual("drop table testTable", sql);
            sql = dbSql.GetUpdateTableNameSql("testTable", "newTable").SQL;
            Assert.AreEqual("alter table testTable rename newTable", sql);
            Trace.TraceInformation(sql);
        }

        [TestMethod("测试创建MySql表类型数据")]
        public void TestMySqlDbTypeTable()
        {
            var sql = dbSql.GetCreateTableSql<DbTypeEntiry>().SQL;
            Assert.AreEqual("create table DbTypeEntiry(Id nvarchar(36),DInt decimal not null,DDouble decimal(15,6) not null,DFloat decimal(15,6) not null,DDateTime datetime not null,DString nvarchar(255),DGuid nvarchar(36) not null,DByte tinyint not null,DNInt decimal,DNDouble decimal(15,6),DNFloat decimal(15,6),DNDateTime datetime,DNString nvarchar(255) not null,DNGuid nvarchar(36),DNByte tinyint,PRIMARY KEY ( Id ))", sql);
            Trace.TraceInformation(sql);
        }

        [TestMethod]
        public void TestDbColumn()
        {
            var sql = dbSql.GetAddllColumnSql("testTable", "testColumn", System.TypeCode.String, true, 200).SQL;
            Assert.AreEqual("alter table testTable add column testColumn nvarchar(200) not null", sql);
            sql = dbSql.GetColumsNameCountSql("testdb", "testTable", "testColumn").SQL;
            Assert.AreEqual("select COUNT(1) from information_schema.columns WHERE table_schema='testdb' and table_name = 'testTable' and column_name = 'testColumn'", sql);
            sql = dbSql.GetColumsSql("testdb", "testTable").SQL;
            Assert.AreEqual("select column_name from information_schema.columns where table_schema='testdb' and table_name='testTable'", sql);
            sql = dbSql.GetDeleteColumnSql("testTable", "testColumn").SQL;
            Assert.AreEqual("alter table testTable drop column testColumn", sql);
            sql = dbSql.GetRenameColumnNameSql("testTable", "testColumn", "newColumn", TypeCode.Boolean).SQL;
            Assert.AreEqual("alter table testTable change column testColumn newColumn tinyint", sql);
            sql = dbSql.GetUpdateColumnSql("testTable", "testColumn", TypeCode.String, true, 200).SQL;
            Assert.AreEqual("alter table testTable modify testColumn nvarchar(200) not null", sql);
            Trace.TraceInformation(sql);
        }

        [TestMethod]
        public void TestDbIndex()
        {
            var sql = dbSql.GetCreateIndexSql("test", new string[] { "column1", "column2" }, "column1_column2", "index").SQL;
            Assert.AreEqual("create index column1_column2 on test(column1,column2)", sql);
            sql = dbSql.GetDeleteIndexSql("test", "column1_column2").SQL;
            Assert.AreEqual("drop index column1_column2 on test", sql);
            Trace.TraceInformation(sql);
        }
    }
}