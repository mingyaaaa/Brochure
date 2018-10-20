﻿using Brochure.Core.Server;
using Brochure.Server.MySql;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Brochure.Server.MySql.Implements;
using Brochure.Core.Server.core;
using Brochure.Core.Server.Enums.sql;

namespace Brochure.Core.Test.MySqlDbTest
{
    [Table("User")]
    public class UserTable : EntityBase
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
    public class DataTest : IDisposable
    {
        private static DbFactory _factory = new MySqlDbFactory("10.0.0.18", "root", "123456", "3306", "test");
        private static IClient client;
        public DataTest()
        {
            DbConnectPool.RegistFactory(_factory, DatabaseType.MySql);
            client = new MySqlClient();
        }

        [Fact]
        public async void InsertData()
        {
            await CreateTable();

            var datahub = await client.GetDataHubAsync<UserTable>();
            var user = new UserTable()
            {
                Name = "User1",
                Age = 12
            };
            var r = await datahub.InserOneAsync(user);
            Assert.Equal(1, r);
            var users = new List<UserTable>();
            for (int i = 0; i < 5; i++)
            {
                var user1 = new UserTable()
                {
                    Name = "User" + i,
                    Age = 12 + i
                };
                users.Add(user1);
            }
            r = await datahub.InserManyAsync(users);
            Assert.Equal(5, r);
            var record = await GetUserTable(user.Id);
            var rUser = record.As<UserTable>();
            Assert.Equal(user.Name, rUser.Name);
            Assert.Equal(user.Id, rUser.Id);

            var records = await GetUserList(new SearchParams(Query.In("Id", new Guid[]
            {
                users[0].Id, users[1].Id
            })));
            Assert.Equal(2, records.Count);
        }

        [Fact]
        public async void UpdateData()
        {
            await CreateTable();
            var datahub = await client.GetDataHubAsync<UserTable>();
            var user = new UserTable();
            user.Name = Guid.NewGuid().ToString();
            user.Age = 32;
            await Insert(user);
            var updateUser = user;
            updateUser.Age = 1;
            var r = await datahub.UpdateAsync(user.Id, updateUser.As<IRecord>());
            Assert.Equal(1, r);
            user.Age = 5;
            r = await datahub.UpdateAsync(Query.Eq(nameof(user.Name), user.Name), updateUser.As<IRecord>());
            Assert.Equal(1, r);
        }

        [Fact]
        public async void DeleteData()
        {
            //Given
            await CreateTable();
            var datahub = await client.GetDataHubAsync<UserTable>();
            //When
            var user = new UserTable();
            user.Name = "UserName";
            user.Age = 1;
            await Insert(user);

            var r = await datahub.DeleteAsync(user.Id);
            Assert.Equal(1, r);

            var users = new List<UserTable>();
            for (int i = 0; i < 5; i++)
            {
                var user1 = new UserTable()
                {
                    Name = "User" + i,
                    Age = 12 + i
                };
                users.Add(user1);
            }
            await InsertMany(users);
            r = await datahub.DeleteAsync(users.Where(t => t.Age <= 13).Select(t => t.Id));
            Assert.Equal(2, r);

            r = await datahub.DeleteAsync(Query.Eq("Name", "User3"));
            Assert.Equal(1, r);
        }


        [Fact]
        public void Test()
        {
            var _connectStr = "Data Source={0};User Id={1};Password={2};pooling=false;CharSet=utf8;port={3};SslMode = none;";
            var connectString = string.Format(_connectStr, "10.0.0.18", "root", "123456", "3306", "test");

            var aaa = new List<MySqlTransaction>();
            for (int i = 0; i <= 1; i++)
            {
                var connect = new MySqlConnection(connectString);
                connect.Open();
                var aa = connect.BeginTransaction();
                aaa.Add(aa);
            }
            Assert.Same(aaa[0], aaa[1]);
        }


        private async Task Insert(UserTable usertable)
        {
            var datahub = await client.GetDataHubAsync<UserTable>();
            var r = await datahub.InserOneAsync(usertable);
            if (r < -1)
                throw new Exception();
        }
        private async Task InsertMany(List<UserTable> usertables)
        {
            var datahub = await client.GetDataHubAsync<UserTable>();
            var r = await datahub.InserManyAsync(usertables);
            if (r < -1)
                throw new Exception();
        }

        public async Task Update(Guid id, IRecord data)
        {
            var datahub = await client.GetDataHubAsync<UserTable>();
            var r = await datahub.UpdateAsync(id, data);
            if (r < -1)
                throw new Exception();
        }
        public async Task Update(Query query, IRecord data)
        {
            var datahub = await client.GetDataHubAsync<UserTable>();
            var r = await datahub.UpdateAsync(query, data);
            if (r < -1)
                throw new Exception();
        }
        public async Task Delete(Guid id)
        {
            var datahub = await client.GetDataHubAsync<UserTable>();
            var r = await datahub.DeleteAsync(id);
            if (r < -1)
                throw new Exception();
        }
        public async Task Delete(Guid[] ids)
        {
            var datahub = await client.GetDataHubAsync<UserTable>();
            var r = await datahub.DeleteAsync(ids);
            if (r < -1)
                throw new Exception();
        }
        public async Task Delete(Query query)
        {
            var datahub = await client.GetDataHubAsync<UserTable>();
            var r = await datahub.DeleteAsync(query);
            if (r < -1)
                throw new Exception();
        }

        public async Task<IRecord> GetUserTable(Guid id)
        {
            var datahub = await client.GetDataHubAsync<UserTable>();
            var r = await datahub.GetInfoAsync(id);
            return r;
        }
        public async Task<List<IRecord>> GetUserList(SearchParams searchParams)
        {
            var datahub = await client.GetDataHubAsync<UserTable>();
            var r = await datahub.GetListAsync(searchParams);
            return r;
        }
        public async Task<List<IRecord>> GetUserListGroup(List<Aggregate> aggregates, SearchParams searchParams, params string[] strs)
        {
            var datahub = await client.GetDataHubAsync<UserTable>();
            var r = await datahub.GetListGroupByAsync(aggregates, searchParams, strs);
            return r;
        }
        public async Task CreateTable()
        {
            var tablehub = await client.GetDataTableHubAsync();
            await tablehub.RegistTableAsync<UserTable>();
        }

        public void Dispose()
        {

        }
    }
}
