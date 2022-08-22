using Brochure.Abstract;
using Brochure.Abstract.Extensions;
using Brochure.Abstract.Models;
using Brochure.Extensions;
using Brochure.ORM;
using Brochure.ORM.Database;
using Brochure.ORM.MySql;
using LinqDbQueryTest.Datas;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Brochure.ORMTest.Querys
{
    /// <summary>
    /// The db test.
    /// </summary>
    [TestClass]
    public class DbTest : BaseTest
    {
        /// <summary>
        /// Gets the provider.
        /// </summary>
        public IDbProvider provider { get; }

        private readonly DbOption option;
        private ISqlBuilder _sqlBuilder;
        private Mock<TransactionManager> transactionManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbTest"/> class.
        /// </summary>
        public DbTest()
        {
            transactionManager = new Mock<TransactionManager>();
            provider = base.Provider.GetService<IDbProvider>();
            option = base.Provider.GetService<DbOption>();
            _sqlBuilder = Provider.GetService<ISqlBuilder>();
            option.IsUseParamers = false;
        }

        /// <summary>
        /// Tests the db data.
        /// </summary>
        [TestMethod]
        public void TestDbData()
        {
            var obj = new Students()
            {
                ClassCount = 1,
                ClassId = "11",
                School = "dd",
            };
            var sql = Sql.InsertSql(obj);
            var sqlResult = _sqlBuilder.Build(sql);
            Assert.AreEqual("insert into `Students`(`School`,`ClassId`,`ClassCount`,`No`) values('dd','11',1,0)", sqlResult.SQL);
            sql = Sql.DeleteSql<Students>(t => t.ClassId == "1" && t.ClassCount == 2);
            sqlResult = _sqlBuilder.Build(sql);
            Assert.AreEqual("delete from `Students` where `Students`.`ClassId` = '1' and `Students`.`ClassCount` = 2", sqlResult.SQL);
            sql = Sql.UpdateSql<Students>(new
            {
                ClassId = "2"
            }, t => t.Id == "aa");
            sqlResult = _sqlBuilder.Build(sql);
            Trace.TraceInformation(sqlResult.SQL);
        }

        /// <summary>
        /// Tests the insert.
        /// </summary>
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
            var sql = Sql.InsertSql(obj);
            var sqlResult = _sqlBuilder.Build(sql);
            Assert.AreEqual("insert into `Students`(`School`,`ClassId`,`ClassCount`,`No`) values(@p0,@p1,@p2,@p3)", sqlResult.SQL);

            Assert.AreEqual(sqlResult.Parameters.FirstOrDefault(t => t.ParameterName == "@p0").Value, "dd");
            Assert.AreEqual(sqlResult.Parameters.FirstOrDefault(t => t.ParameterName == "@p1").Value, "11");
            Assert.AreEqual(sqlResult.Parameters.FirstOrDefault(t => t.ParameterName == "@p2").Value, 1);
        }

        [TestMethod]
        public void TestInsertManay()
        {
            var list = new List<Students>();
            for (int i = 0; i < 3; i++)
            {
                var obj = new Students()
                {
                    ClassCount = i,
                    ClassId = i.ToString(),
                    School = "dd",
                };
                list.Add(obj);
            }
            option.IsUseParamers = true;
            var sql = new InsertManySql(list);
            var sqlResult = _sqlBuilder.Build(sql);
            Assert.AreEqual("insert into `Students`(`Id`,`School`,`ClassId`,`PeopleId`,`ClassCount`,`No`) values(@p0,@p1,@p2,@p3,@p4,@p5),(@p6,@p7,@p8,@p9,@p10,@p11),(@p12,@p13,@p14,@p15,@p16,@p17)", sqlResult.SQL);

            Assert.AreEqual(sqlResult.Parameters.FirstOrDefault(t => t.ParameterName == "@p0").Value, null);
            Assert.AreEqual(sqlResult.Parameters.FirstOrDefault(t => t.ParameterName == "@p1").Value, "dd");
            Assert.AreEqual(sqlResult.Parameters.FirstOrDefault(t => t.ParameterName == "@p2").Value, "0");
            Assert.AreEqual(sqlResult.Parameters.FirstOrDefault(t => t.ParameterName == "@p3").Value, null);
            Assert.AreEqual(sqlResult.Parameters.FirstOrDefault(t => t.ParameterName == "@p4").Value, 0);

            Assert.AreEqual(sqlResult.Parameters.FirstOrDefault(t => t.ParameterName == "@p6").Value, null);
            Assert.AreEqual(sqlResult.Parameters.FirstOrDefault(t => t.ParameterName == "@p7").Value, "dd");
            Assert.AreEqual(sqlResult.Parameters.FirstOrDefault(t => t.ParameterName == "@p8").Value, "1");
            Assert.AreEqual(sqlResult.Parameters.FirstOrDefault(t => t.ParameterName == "@p9").Value, null);
            Assert.AreEqual(sqlResult.Parameters.FirstOrDefault(t => t.ParameterName == "@p10").Value, 1);
        }

        /// <summary>
        /// Tests the db database.
        /// </summary>
        [TestMethod]
        public void TestDbDatabase()
        {
            var sql = Sql.CreateDatabase("testdb");
            var sqlResult = _sqlBuilder.Build(sql);
            Assert.AreEqual("create database testdb", sqlResult.SQL);
            sql = Sql.DeleteDatabase("testdb");
            sqlResult = _sqlBuilder.Build(sql);
            Assert.AreEqual("drop database testdb", sqlResult.SQL);
            sql = Sql.GetAllDatabase();
            sqlResult = _sqlBuilder.Build(sql);
            Assert.AreEqual("select schema_name from information_schema.schemata", sqlResult.SQL);
            sql = Sql.DatabaseCount("testdb");
            sqlResult = _sqlBuilder.Build(sql);
            Assert.AreEqual("SELECT count(1) FROM information_schema.SCHEMATA where SCHEMA_NAME='testdb'", sqlResult.SQL);
        }

        /// <summary>
        /// Tests the db table.
        /// </summary>
        [TestMethod]
        public void TestDbTable()
        {
            var sql = Sql.GetCountTable("testTable", "testdb");
            sql.Database = "testdb";
            var sqlResult = _sqlBuilder.Build(sql);
            Assert.AreEqual("SELECT count(1) FROM information_schema.TABLES WHERE table_name ='testTable' and TABLE_SCHEMA ='testdb'", sqlResult.SQL);
            sql = Sql.GetAllTableName("testdb");
            sqlResult = _sqlBuilder.Build(sql);
            Assert.AreEqual("select table_name from information_schema.tables where table_schema='testdb'", sqlResult.SQL);
            sql = Sql.CreateTable<Students>();
            sqlResult = _sqlBuilder.Build(sql);
            Assert.AreEqual("create table if not exists Students(Id nvarchar(36),School nvarchar(255),ClassId nvarchar(255),PeopleId nvarchar(255),ClassCount decimal not null,No decimal not null,PRIMARY KEY ( Id ))", sqlResult.SQL);
            sql = Sql.DeleteTable("testTable");
            sqlResult = _sqlBuilder.Build(sql);
            Assert.AreEqual("drop table if exists testTable", sqlResult.SQL);
            sql = Sql.UpdateTableName("testTable", "newTable");
            sqlResult = _sqlBuilder.Build(sql);
            Assert.AreEqual("alter table testTable rename newTable", sqlResult.SQL);
        }

        /// <summary>
        /// Tests the my sql db type table.
        /// </summary>
        [TestMethod("测试创建MySql表类型数据")]
        public void TestMySqlDbTypeTable()
        {
            var sql = Sql.CreateTable<DbTypeEntiry>();
            var sqlResult = _sqlBuilder.Build(sql);
            Assert.AreEqual("create table if not exists DbTypeEntiry(Id nvarchar(36),DInt decimal not null,DDouble decimal(15,6) not null,DFloat decimal(15,6) not null,DDateTime datetime not null,DString nvarchar(255),DGuid nvarchar(36) not null,DByte tinyint not null,DNInt decimal,DNDouble decimal(15,6),DNFloat decimal(15,6),DNDateTime datetime,DNString nvarchar(255) not null,DNGuid nvarchar(36),DNByte tinyint,PRIMARY KEY ( Id ))", sqlResult.SQL);
        }

        /// <summary>
        /// Tests the db column.
        /// </summary>
        [TestMethod]
        public void TestDbColumn()
        {
            var sql = Sql.GetAddColumnSql("testTable", "testColumn", System.TypeCode.String, true, 200);
            var sqlResult = _sqlBuilder.Build(sql);
            Assert.AreEqual("alter table testTable add column testColumn nvarchar(200) not null", sqlResult.SQL);
            sql = Sql.GetColumnsCount("testTable", "testColumn", "testdb");
            sql.Database = "testdb";
            sqlResult = _sqlBuilder.Build(sql);
            Assert.AreEqual("select COUNT(1) from information_schema.columns WHERE table_schema='testdb' and table_name = 'testTable' and column_name = 'testColumn'", sqlResult.SQL);
            sql = Sql.GetColumsNames("testTable");
            sql.Database = "testdb";
            sqlResult = _sqlBuilder.Build(sql);
            Assert.AreEqual("select column_name from information_schema.columns where table_schema='testdb' and table_name='testTable'", sqlResult.SQL);
            sql = Sql.GetDeleteColumnSql("testTable", "testColumn");
            sqlResult = _sqlBuilder.Build(sql);
            Assert.AreEqual("alter table testTable drop column testColumn", sqlResult.SQL);
            sql = Sql.GetRenameColumnNameSql("testTable", "testColumn", "newColumn", TypeCode.Boolean);
            sqlResult = _sqlBuilder.Build(sql);
            Assert.AreEqual("alter table testTable change column testColumn newColumn tinyint", sqlResult.SQL);
            sql = Sql.GetUpdateColumnSql("testTable", "testColumn", TypeCode.String, true, 200);
            sqlResult = _sqlBuilder.Build(sql);
            Assert.AreEqual("alter table testTable modify testColumn nvarchar(200) not null", sqlResult.SQL);
        }

        /// <summary>
        /// Tests the db index.
        /// </summary>
        [TestMethod]
        public void TestDbIndex()
        {
            var sql = Sql.CreateIndex("test", new string[] { "column1", "column2" }, "column1_column2", "index");
            var sqlResult = _sqlBuilder.Build(sql);
            Assert.AreEqual("create index column1_column2 on test(column1,column2)", sqlResult.SQL);
            sql = Sql.DeleteIndex("test", "column1_column2");
            sqlResult = _sqlBuilder.Build(sql);
            Assert.AreEqual("drop index column1_column2 on test", sqlResult.SQL);

            sql = Sql.CountIndex("test", "index1", "testDb");
            sqlResult = _sqlBuilder.Build(sql);
            Assert.AreEqual("select COUNT(1) from information_schema.columns WHERE table_schema='testDb' and table_name = 'test' and index_name = 'index1'", sqlResult.SQL);

            sql = Sql.RenameIndex("test", "index1", "newIndex");
            sqlResult = _sqlBuilder.Build(sql);
            Assert.AreEqual("ALTER TABLE test RENAME INDEX index1 TO newIndex", sqlResult.SQL);
        }

        //        [TestMethod]
        //        public void TestDbIfExist()
        //        {
        //            var sql = Sql.If(new ExistSql(Sql.GetCountTable("test", "testDb")), Sql.DeleteTable("Students"), null);
        //            var sqlResult = _sqlBuilder.Build(sql);
        //            Assert.AreEqual(@"if exists (SELECT count(1) FROM information_schema.TABLES WHERE table_name ='test' and TABLE_SCHEMA ='testDb')
        //drop table Students", sqlResult.SQL);
        //        }

        //        [TestMethod]
        //        public void TestDbNotExist()
        //        {
        //            var sql = Sql.If(new ExistSql(Sql.GetCountTable("test", "testDb"), true), Sql.CreateTable<Students>(), null);
        //            var sqlResult = _sqlBuilder.Build(sql);
        //            Assert.AreEqual(@"if not exists (SELECT count(1) FROM information_schema.TABLES WHERE table_name ='test' and TABLE_SCHEMA ='testDb')
        //create table Students(Id nvarchar(36),School nvarchar(255),ClassId nvarchar(255),PeopleId nvarchar(255),ClassCount decimal not null,No decimal not null,PRIMARY KEY ( Id ))", sqlResult.SQL);
        //        }

        //        [TestMethod]
        //        public void TestElse()
        //        {
        //            var sql = Sql.If(new StringSql("1=2"), Sql.CreateTable<Students>(), Sql.DeleteTable("Students"));
        //            var sqlResult = _sqlBuilder.Build(sql);
        //            Assert.AreEqual(@"if 1=2
        //create table Students(Id nvarchar(36),School nvarchar(255),ClassId nvarchar(255),PeopleId nvarchar(255),ClassCount decimal not null,No decimal not null,PRIMARY KEY ( Id ))
        //else
        //drop table Students", sqlResult.SQL);
        //        }

        #region 类

        //public class Students
        //{
        //    /// <summary>
        //    /// Gets or sets the id.
        //    /// </summary>
        //    public string Id { get; set; }

        //    /// <summary>
        //    /// Gets or sets the school.
        //    /// </summary>
        //    public string School { get; set; }

        //    /// <summary>
        //    /// Gets or sets the class id.
        //    /// </summary>
        //    public string ClassId { set; get; }

        //    /// <summary>
        //    /// Gets or sets the class count.
        //    /// </summary>
        //    public int ClassCount { get; set; }

        //    /// <summary>
        //    /// Gets or sets the no.
        //    /// </summary>
        //    public decimal No { get; set; }
        //}

        #endregion 类
    }
}