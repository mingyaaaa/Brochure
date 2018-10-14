using Brochure.Core.Server;
using Brochure.Server.MySql;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Brochure.Core.Test.MySqlDbTest
{
    public class SqlParseTest
    {
        ITestOutputHelper output;
        public SqlParseTest(ITestOutputHelper up)
        {
            output = up;
        }

        [Fact]
        public void TestOperationExp()
        {
            var mysqlParse = new MySqlParse();
            var parse = new QueryParse(mysqlParse);

            var query = Query.Eq("aa", 11);
            var param = parse.Parse(query);
            Assert.Equal("aa = @aa", param.Sql);
            Assert.Equal("@aa", param.Params.Keys.FirstOrDefault());
            Assert.Equal(11, param.Params["@aa"]);

            var guid = Guid.NewGuid();
            query = Query.Eq("a", guid);
            param = parse.Parse(query);
            Assert.Equal("a = @a", param.Sql);


            var time = new DateTime(2011, 1, 1);
            query = Query.Eq("aa", time);
            param = parse.Parse(query);
            Assert.Equal("aa = @aa", param.Sql);
            Assert.Equal("@aa", param.Params.Keys.FirstOrDefault());
            Assert.Equal(time, param.Params["@aa"]);

            query = Query.NotEq("aa", 11);
            param = parse.Parse(query);
            Assert.Equal("aa != @aa", param.Sql);
            Assert.Equal("@aa", param.Params.Keys.FirstOrDefault());
            Assert.Equal(11, param.Params["@aa"]);

            query = Query.Gt("bb", 11);
            param = parse.Parse(query);
            Assert.Equal("bb > @bb", param.Sql);
            Assert.Equal("@bb", param.Params.Keys.FirstOrDefault());
            Assert.Equal(11, param.Params["@bb"]);

            query = Query.Gte("cc", 11);
            param = parse.Parse(query);
            Assert.Equal("cc >= @cc", param.Sql);
            Assert.Equal("@cc", param.Params.Keys.FirstOrDefault());
            Assert.Equal(11, param.Params["@cc"]);

            query = Query.Lt("bb", 11);
            param = parse.Parse(query);
            Assert.Equal("bb < @bb", param.Sql);
            Assert.Equal("@bb", param.Params.Keys.FirstOrDefault());
            Assert.Equal(11, param.Params["@bb"]);

            query = Query.Lte("cc", 11);
            param = parse.Parse(query);
            Assert.Equal("cc <= @cc", param.Sql);
            Assert.Equal("@cc", param.Params.Keys.FirstOrDefault());
            Assert.Equal(11, param.Params["@cc"]);
            //Then
        }

        [Fact]
        public void TestNull()
        {
            var mysqlParse = new MySqlParse();
            var parse = new QueryParse(mysqlParse);
            //Given
            var query = Query.Eq("aaa", null);
            //When
            var param = parse.Parse(query);
            Assert.Equal("aaa is null", param.Sql);
            Assert.Empty(param.Params.Keys);

            query = Query.IsNull("aaa");
            param = parse.Parse(query);
            Assert.Equal("aaa is null", param.Sql);
            Assert.Empty(param.Params.Keys);
            query = Query.IsNotNull("aaa");
            param = parse.Parse(query);
            Assert.Equal("aaa is not null", param.Sql);
            Assert.Empty(param.Params.Keys);
            //Then
        }

        [Fact]
        public void TestIn()
        {
            var mysqlParse = new MySqlParse();
            var parse = new QueryParse(mysqlParse);
            //Given
            var aaa = new int[] { 1, 2, 3 };
            var query = Query.In("aaa", aaa);
            var param = parse.Parse(query);
            Assert.Equal("aaa in (@aaa1,@aaa2,@aaa3)", param.Sql);
            Assert.Equal(1, param.Params["@aaa1"]);
            Assert.Equal(2, param.Params["@aaa2"]);
            Assert.Equal(3, param.Params["@aaa3"]);

            var guidList = new List<Guid>();
            for (int i = 0; i < 3; i++)
                guidList.Add(Guid.NewGuid());
            query = Query.In("aaa", guidList);
            param = parse.Parse(query);
            Assert.Equal("aaa in (@aaa1,@aaa2,@aaa3)", param.Sql);
            //When
            var bbb = new string[] { "1", "2", "3" };
            query = Query.In("bbb", bbb);
            param = parse.Parse(query);
            Assert.Equal("bbb in (@bbb1,@bbb2,@bbb3)", param.Sql);
            Assert.Equal("1", param.Params["@bbb1"]);
            Assert.Equal("2", param.Params["@bbb2"]);
            Assert.Equal("3", param.Params["@bbb3"]);
            //Then

            var ccc = new string[] { "1", "2", "3" };
            query = Query.NotIn("bbb", ccc);
            param = parse.Parse(query);
            Assert.Equal("bbb not in (@bbb1,@bbb2,@bbb3)", param.Sql);
            Assert.Equal("1", param.Params["@bbb1"]);
            Assert.Equal("2", param.Params["@bbb2"]);
            Assert.Equal("3", param.Params["@bbb3"]);




        }

        [Fact]
        public void TestLike()
        {
            var mysqlParse = new MySqlParse();
            var parse = new QueryParse(mysqlParse);
            //Given
            var query = Query.Like("aaa", "aaaaa");
            var param = parse.Parse(query);
            Assert.Equal("aaa like '%@aaa%'", param.Sql);
            Assert.Equal("aaaaa", param.Params["@aaa"]);
            //When

            query = Query.NotLike("aaa", "aaaa");
            param = parse.Parse(query);
            Assert.Equal("aaa not like '%@aaa%'", param.Sql);
            Assert.Equal("aaaa", param.Params["@aaa"]);
            //Then
        }

        [Fact]
        public void TestBetween()
        {
            //Given
            var mysqlParse = new MySqlParse();
            var parse = new QueryParse(mysqlParse);
            //Given
            var query = Query.Between("aaa", 1, 2);
            var param = parse.Parse(query);
            Assert.Equal("aaa between @aaa1 and @aaa2", param.Sql);
            Assert.Equal(1, param.Params["@aaa1"]);
            Assert.Equal(2, param.Params["@aaa2"]);
            //When

            query = Query.NotBetween("aaa", 1, 2);
            param = parse.Parse(query);
            Assert.Equal("aaa not between @aaa1 and @aaa2", param.Sql);
            Assert.Equal(1, param.Params["@aaa1"]);
            Assert.Equal(2, param.Params["@aaa2"]);
            //When

            var time1 = new DateTime(2011, 1, 1);
            var time2 = new DateTime(2011, 1, 2);
            query = Query.NotBetween("aaa", time1, time2);
            param = parse.Parse(query);
            Assert.Equal("aaa not between @aaa1 and @aaa2", param.Sql);
            Assert.Equal(time1, param.Params["@aaa1"]);
            Assert.Equal(time2, param.Params["@aaa2"]);
            //Then
        }

        [Fact]
        public void TestAnd()
        {
            var mysqlParse = new MySqlParse();
            var parse = new QueryParse(mysqlParse);

            var query = Query.Eq("aa", 11).And(Query.Eq("bb", 2));
            var param = parse.Parse(query);
            Assert.Equal("aa = @aa and (bb = @bb)", param.Sql);

            query = Query.Eq("aa", 11).And(Query.Eq("bb", 2).Or(Query.Eq("cc", 3)));
            param = parse.Parse(query);
            Assert.Equal("aa = @aa and (bb = @bb or (cc = @cc))", param.Sql);

        }
    }
}
