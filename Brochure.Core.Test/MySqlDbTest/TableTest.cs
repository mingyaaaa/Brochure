using System;
using Brochure.Core.Abstracts;
using Brochure.Core.Server.Abstracts;
using Brochure.Core.Server.Attributes;
using Brochure.Core.Server.Enums;
using Brochure.Core.Server.Interfaces;
using Brochure.Server.MySql;
using Brochure.Server.MySql.Implements;
using Brochure.Server.MySql.Utils;
using Xunit;

namespace Brochure.Core.Test.MySqlDbTest
{
    [Table ("aaa")]
    public class TestTable : EntityBase
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
    public class TableTest
    {
        private static DbFactoryAbstract _factory = new MySqlDbFactory ("10.0.0.18", "root", "123456", "3306");
        [Fact]
        public async void CreateAndDropDataBase ()
        {
            //var factory = new MySqlDbFactory ("10.0.0.18", "root", "123456", "3306");
            var connect = GetconncetNoDatabase ();
            var databaseHub = connect.GetBatabaseHub ();
            var rr = await databaseHub.CreateDataBaseAsync ("test1");
            Assert.Equal (1, rr);
            rr = 0;
            var isExist = await databaseHub.IsExistDataBaseAsync ("test1");
            Assert.True (isExist);
            rr = 0;
            rr = await databaseHub.DeleteDataBaseAsync ("test1");
            Assert.Equal (1, rr);
        }

        [Fact]
        public async void CreateOrDropTable ()
        {
            var connect = Getconncet ();
            var tableHub = connect.GetTableHub ();
            var tableName = DbUtil.GetTableName<TestTable> ();
            var isExist = await tableHub.IsExistTableAsync (tableName);
            if (isExist)
            {
                var rr = await tableHub.DeleteTableAsync (tableName);
                Assert.Equal (1, rr);
            }
            var r = await tableHub.CreateTable<TestTable> ();
            //Given
            Assert.Equal (1, r);
            //When
            r = await tableHub.DeleteTableAsync (tableName);
            Assert.Equal (1, r);
            //Then
        }

        [Fact]
        public async void UpdateTableName ()
        {
            var connect = Getconncet ();
            var tableHub = connect.GetTableHub ();
            var tableName = DbUtil.GetTableName<TestTable> ();
            var isExist = await tableHub.IsExistTableAsync (tableName);
            if (!isExist)
            {
                await tableHub.CreateTable<TestTable> ();
            }
            var nerTableName = "aaa1";
            var rr = await tableHub.UpdateTableName (tableName, nerTableName);
            Assert.Equal (1, rr);
            rr = await tableHub.DeleteTableAsync (nerTableName);
            Assert.Equal (1, rr);
        }

        [Fact]
        public async void CreatetorIndex ()
        {
            //Given
            var connect = Getconncet ();
            var tableHub = connect.GetTableHub ();
            var tableName = DbUtil.GetTableName<TestTable> ();
            await tableHub.CreateTable<TestTable> ();
            //When
            var dataHub = connect.GetDataHub (tableName);
            var r = await dataHub.CreateIndexAsync (new string[] { "Name" }, "aaahhh", SQLIndex.Unique);
            Assert.Equal (1, r);
            await tableHub.DeleteTableAsync (tableName);
            //Then
        }

        [Fact]
        public async void DropIndex ()
        {
            //Given
            var connect = Getconncet ();
            var tableHub = connect.GetTableHub ();
            var tableName = DbUtil.GetTableName<TestTable> ();
            await tableHub.CreateTable<TestTable> ();
            //When
            var dataHub = connect.GetDataHub (tableName);
            var index = "aaahhh";
            await dataHub.CreateIndexAsync (new string[] { "Name" }, index, SQLIndex.Unique);
            var r = await dataHub.DeleteIndexAsync (index);
            Assert.Equal (1, r);
            await tableHub.DeleteTableAsync (tableName);
            //Then
        }

        [Fact]
        public async void AddColumn ()
        {
            //Given
            var connect = Getconncet ();
            var tableHub = connect.GetTableHub ();
            var tableName = DbUtil.GetTableName<TestTable> ();
            await tableHub.CreateTable<TestTable> ();
            //When
            var dataHub = connect.GetDataHub (tableName);
            var r = await dataHub.AddColumnsAsync ("ccc", "nvarchar(255)", true);
            Assert.Equal (1, r);
            await tableHub.DeleteTableAsync (tableName);
            //Then
        }

        [Fact]
        public async void DeleteColumn ()
        {
            //Given
            var connect = Getconncet ();
            var tableHub = connect.GetTableHub ();
            var tableName = DbUtil.GetTableName<TestTable> ();
            await tableHub.CreateTable<TestTable> ();
            //When
            var dataHub = connect.GetDataHub (tableName);
            var columnName = "ccc";
            var isExist = await dataHub.IsExistColmnAsync (columnName);
            if (!isExist)
                await dataHub.AddColumnsAsync (columnName, "nvarchar(255)", true);
            var r = await dataHub.DeleteColumnAsync (columnName);
            Assert.Equal (1, r);
            await tableHub.DeleteTableAsync (tableName);
            //Then
        }

        [Fact]
        public async void UpdateColumn ()
        {
            //Given
            var connect = Getconncet ();
            var tableHub = connect.GetTableHub ();
            var tableName = DbUtil.GetTableName<TestTable> ();
            await tableHub.CreateTable<TestTable> ();
            //When
            var dataHub = connect.GetDataHub (tableName);
            var columnName = "ccc";
            var isExist = await dataHub.IsExistColmnAsync (columnName);
            if (!isExist)
                await dataHub.AddColumnsAsync (columnName, "nvarchar(255)", true);
            var r = await dataHub.UpdateColumnAsync (columnName, "DECIMAL", false);
            Assert.Equal (1, r);
            await tableHub.DeleteTableAsync (tableName);
            //Then
        }

        [Fact]
        public async void RenameColumn ()
        {
            //Given
            var connect = Getconncet ();
            var tableHub = connect.GetTableHub ();
            var tableName = DbUtil.GetTableName<TestTable> ();
            await tableHub.CreateTable<TestTable> ();
            //When
            var dataHub = connect.GetDataHub (tableName);
            var columnName = "ccc";
            var isExist = await dataHub.IsExistColmnAsync (columnName);
            if (!isExist)
                await dataHub.AddColumnsAsync (columnName, "nvarchar(255)", true);
            var newName = "cccc33333";
            var r = await dataHub.RenameColumnAsync (columnName, newName, "nvarchar(255)");
            Assert.Equal (1, r);
            isExist = await dataHub.IsExistColmnAsync (newName);
            Assert.True (isExist);
            await tableHub.DeleteTableAsync (tableName);
            //Then
        }
        private IDbConnect Getconncet ()
        {
            _factory.DatabaseName = "test";
            return _factory.GetDbConnect ();
        }
        private IDbConnect GetconncetNoDatabase ()
        {
            return _factory.GetDbConnect ();
        }
    }

}