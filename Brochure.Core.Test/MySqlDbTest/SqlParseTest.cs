using System.Linq;
using Brochure.Core.Querys;
using Brochure.Core.Server.core;
using Brochure.Server.MySql.Utils;
using Xunit;
using Xunit.Abstractions;

namespace Brochure.Core.Test.MySqlDbTest
{
    public class SqlParseTest
    {
        ITestOutputHelper output;
        public SqlParseTest (ITestOutputHelper up)
        {
            output = up;
        }

        [Fact]
        public void TestOperationExp ()
        {
            var mysqlParse = new MySqlParse ();
            var parse = new QueryParse (mysqlParse);
            //Given
            var query = Query.Eq ("aa", 11);
            var param = parse.Parse (query);
            //When
            Assert.Equal ("aa = @aa", param.Sql);
            Assert.Equal ("@aa", param.Params.Keys.FirstOrDefault ());
            Assert.Equal (11, param.Params["@aa"]);

            query = Query.Gt ("bb", 11);
            param = parse.Parse (query);
            Assert.Equal ("bb > @bb", param.Sql);
            Assert.Equal ("@bb", param.Params.Keys.FirstOrDefault ());
            Assert.Equal (11, param.Params["@bb"]);

            query = Query.Gte ("cc", 11);
            param = parse.Parse (query);
            Assert.Equal ("cc >= @cc", param.Sql);
            Assert.Equal ("@cc", param.Params.Keys.FirstOrDefault ());
            Assert.Equal (11, param.Params["@cc"]);

            query = Query.Lt ("bb", 11);
            param = parse.Parse (query);
            Assert.Equal ("bb < @bb", param.Sql);
            Assert.Equal ("@bb", param.Params.Keys.FirstOrDefault ());
            Assert.Equal (11, param.Params["@bb"]);

            query = Query.Lte ("cc", 11);
            param = parse.Parse (query);
            Assert.Equal ("cc <= @cc", param.Sql);
            Assert.Equal ("@cc", param.Params.Keys.FirstOrDefault ());
            Assert.Equal (11, param.Params["@cc"]);
            //Then
        }

        [Fact]
        public void TestNull ()
        {
            var mysqlParse = new MySqlParse ();
            var parse = new QueryParse (mysqlParse);
            //Given
            var query = Query.Eq ("aaa", null);
            //When
            var param = parse.Parse (query);
            Assert.Equal ("aaa is null", param.Sql);
            Assert.Empty (param.Params.Keys);

            query = Query.IsNull ("aaa");
            param = parse.Parse (query);
            Assert.Equal ("aaa is null", param.Sql);
            Assert.Empty (param.Params.Keys);
            query = Query.IsNotNull ("aaa");
            param = parse.Parse (query);
            Assert.Equal ("aaa is not null", param.Sql);
            Assert.Empty (param.Params.Keys);
            //Then
        }

        [Fact]
        public void TestIn ()
        {
            var mysqlParse = new MySqlParse ();
            var parse = new QueryParse (mysqlParse);
            //Given
            var aaa = new int[] { 1, 2, 3 };
            var query = Query.In ("aaa", aaa);
            var param = parse.Parse (query);
            Assert.Equal ("aaa in (@aaa1,@aaa2,@aaa3)", param.Sql);
            Assert.Equal (1, param.Params["@aaa1"]);
            Assert.Equal (2, param.Params["@aaa2"]);
            Assert.Equal (3, param.Params["@aaa3"]);
            //When
            var bbb = new string[] { "1", "2", "3" };
            query = Query.In ("bbb", bbb);
            param = parse.Parse (query);
            Assert.Equal ("bbb in (@bbb1,@bbb2,@bbb3)", param.Sql);
            Assert.Equal ("1", param.Params["@bbb1"]);
            Assert.Equal ("2", param.Params["@bbb2"]);
            Assert.Equal ("3", param.Params["@bbb3"]);
            //Then
        }
    }
}