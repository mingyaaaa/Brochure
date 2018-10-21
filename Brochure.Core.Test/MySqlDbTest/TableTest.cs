﻿using Brochure.Core.Server;
using Brochure.Core.Server.core;
using Brochure.Core.Server.Enums.sql;
using Brochure.Server.MySql;
using Brochure.Server.MySql.Implements;
using System.Threading.Tasks;
using Xunit;
using System;
using Xunit.Abstractions;

namespace Brochure.Core.Test.MySqlDbTest
{
    [Table("aaa")]
    public class TestTable : EntityBase
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
    public class TableTest
    {
        private readonly ITestOutputHelper _output;

        private static DbFactory _factory = new MySqlDbFactory("10.0.0.18", "root", "123456", "3306");
        private IClient client;
        public TableTest(ITestOutputHelper output)
        {
            DbConnectPool.RegistFactory(_factory, DatabaseType.MySql);
            client = new MySqlClient();
            _output = output;
        }
        [Fact]
        public async void CreateAndDropDataBase()
        {
            var databaseHub = await client.GetDatabaseHubAsync();
            var rr = await databaseHub.CreateDataBaseAsync("test1");
            Assert.Equal(1, rr);
            rr = 0;
            var isExist = await databaseHub.IsExistDataBaseAsync("test1");
            Assert.True(isExist);
            rr = 0;
            rr = await databaseHub.DeleteDataBaseAsync("test1");
            Assert.Equal(1, rr);

        }

        [Fact]
        public async void CreateOrDropTable1()
        {
            await CreateDatabase();
            try
            {
                var cc = await client.GetDataTableHubAsync();
                Assert.True(false);
            }
            catch (System.Exception)
            {
                Assert.True(true);
            }
            client.SetDatabase("test");
            var tableHub = await client.GetDataTableHubAsync();
            var tableName = DbUtil.GetTableName<TestTable>();
            var isExist = await tableHub.IsExistTableAsync(tableName);
            if (isExist)
            {
                var rr = await tableHub.DeleteTableAsync(tableName);
                Assert.Equal(1, rr);
            }
            var r = await tableHub.CreateTableAsync<TestTable>();
            //Given
            Assert.Equal(1, r);
            //When
            r = await tableHub.DeleteTableAsync(tableName);
            Assert.Equal(1, r);
            //Then
        }

        [Fact]
        public async void CreateOrDropTable2()
        {
            await CreateDatabase();
            var tableHub = await client.GetDataTableHubAsync("test");
            var tableName = DbUtil.GetTableName<TestTable>();
            var isExist = await tableHub.IsExistTableAsync(tableName);
            if (isExist)
            {
                var rr = await tableHub.DeleteTableAsync(tableName);
                Assert.Equal(1, rr);
            }
            var r = await tableHub.RegistTableAsync<TestTable>();
            //Given
            Assert.Equal(1, r);
            //When
            r = await tableHub.DeleteTableAsync(tableName);
            Assert.Equal(1, r);
            //Then
        }

        [Fact]
        public async void UpdateTableName()
        {
            await CreateDatabase();
            var tableHub = await client.GetDataTableHubAsync("test");
            var tableName = DbUtil.GetTableName<TestTable>();
            var isExist = await tableHub.IsExistTableAsync(tableName);
            if (!isExist)
            {
                await tableHub.CreateTableAsync<TestTable>();
            }
            var nerTableName = "aaa1";
            var rr = await tableHub.UpdateTableNameAsync(tableName, nerTableName);
            Assert.Equal(1, rr);
            rr = await tableHub.DeleteTableAsync(nerTableName);
            Assert.Equal(1, rr);
        }

        [Fact]
        public async void CreatetorIndex()
        {
            await CreateDatabase();
            //Given
            var tableHub = await client.GetDataTableHubAsync("test");
            var tableName = DbUtil.GetTableName<TestTable>();
            await tableHub.CreateTableAsync<TestTable>();
            //When
            var dataHub = await client.GetDataHubAsync(tableName);
            var r = await dataHub.CreateIndexAsync(new string[] { "Name" }, "aaahhh", SQLIndex.Unique);
            Assert.Equal(1, r);
            await tableHub.DeleteTableAsync(tableName);
            //Then
        }

        [Fact]
        public async void DropIndex()
        {
            await CreateDatabase();
            //Given
            var tableHub = await client.GetDataTableHubAsync("test");

            var tableName = DbUtil.GetTableName<TestTable>();
            await tableHub.CreateTableAsync<TestTable>();
            //When
            var dataHub = await client.GetDataHubAsync(tableName);

            var index = "aaahhh";
            await dataHub.CreateIndexAsync(new string[] { "Name" }, index, SQLIndex.Unique);
            var r = await dataHub.DeleteIndexAsync(index);
            Assert.Equal(1, r);
            await tableHub.DeleteTableAsync(tableName);
            //Then
        }

        [Fact]
        public async void AddColumn()
        {
            var tableHub = await client.GetDataTableHubAsync("test");
            var tableName = DbUtil.GetTableName<TestTable>();
            var dataHub = await client.GetDataHubAsync(tableName);
            await tableHub.CreateTableAsync<TestTable>();
            var r = await dataHub.AddColumnsAsync("ccc", "nvarchar(255)", true);
            Assert.Equal(1, r);
            await tableHub.DeleteTableAsync(tableName);
            //Then
        }

        [Fact]
        public async void DeleteColumn()
        {
            await CreateDatabase();
            //Given
            var tableHub = await client.GetDataTableHubAsync("test");
            var tableName = DbUtil.GetTableName<TestTable>();
            await tableHub.CreateTableAsync<TestTable>();
            //When
            var columnName = "ccc";
            var dataHub = await client.GetDataHubAsync(tableName);
            var isExist = await dataHub.IsExistColumnAsync(columnName);
            if (!isExist)
                await dataHub.AddColumnsAsync(columnName, "nvarchar(255)", true);
            var r = await dataHub.DeleteColumnAsync(columnName);
            Assert.Equal(1, r);
            await tableHub.DeleteTableAsync(tableName);
            //Then
        }

        [Fact]
        public async void UpdateColumn()
        {
            await CreateDatabase();
            //Given
            var tableHub = await client.GetDataTableHubAsync("test");
            var tableName = DbUtil.GetTableName<TestTable>();
            await tableHub.CreateTableAsync<TestTable>();
            //When
            var columnName = "ccc";
            var columnHub = await client.GetDataHubAsync(tableName);
            var isExist = await columnHub.IsExistColumnAsync(columnName);
            if (!isExist)
                await columnHub.AddColumnsAsync(columnName, "nvarchar(255)", true);
            var r = await columnHub.UpdateColumnAsync(columnName, "DECIMAL", false);
            Assert.Equal(1, r);
            await tableHub.DeleteTableAsync(tableName);
            //Then
        }

        [Fact]
        public async void RenameColumn()
        {
            await CreateDatabase();
            client.SetDatabase("test");
            //Given
            var tableHub = await client.GetDataTableHubAsync();

            var tableName = DbUtil.GetTableName<TestTable>();
            await tableHub.CreateTableAsync<TestTable>();
            //When
            var columnName = "ccc";
            var columnHub = await client.GetDataHubAsync(tableName);
            var isExist = await columnHub.IsExistColumnAsync(columnName);
            if (!isExist)
                await columnHub.AddColumnsAsync(columnName, "nvarchar(255)", true);
            var newName = "cccc33333";
            var r = await columnHub.RenameColumnAsync(columnName, newName, "nvarchar(255)");
            Assert.Equal(1, r);
            isExist = await columnHub.IsExistColumnAsync(newName);
            Assert.True(isExist);
            await tableHub.DeleteTableAsync(tableName);

            //Then
        }
        [Fact]
        private async void Pool()
        {
            var connect = await client.GetDatabaseHubAsync();
            await connect.CreateDataBaseAsync("test2");
            for (int i = 0; i < 100; i++)
            {
                var tableHub = await client.GetDataTableHubAsync("test");
                tableHub.Dispose();
            }
        }


        private async Task CreateDatabase()
        {
            var connect = await client.GetDatabaseHubAsync();
            await connect.CreateDataBaseAsync("test");
        }
    }

}
