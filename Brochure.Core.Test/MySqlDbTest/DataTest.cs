using Brochure.Core.Server;
using Brochure.Server.MySql;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Brochure.Core.Test.MySqlDbTest
{
    [Table("User")]
    public class UserTable : EntityBase
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
    public class DataTest
    {
        private static DbFactoryAbstract _factory = new MySqlDbFactory("10.0.0.18", "root", "123456", "3306");
        private static IDbConnect connect;
        public DataTest()
        {
            if (connect == null)
                connect = _factory.GetDbConnect();
        }

        [Fact]
        public async void InsertData()
        {

        }

        private async Task Insert(UserTable usertable)
        {
            var datahub = connect.GetDataHub<UserTable>();
            await datahub.InserOneAsync(usertable);
        }
        private async Task InsertMany(List<UserTable> usertables)
        {
            var datahub = connect.GetDataHub<UserTable>();
            await datahub.InserManyAsync(usertables);
        }

        public async Task Update()
        {
            var datahub = connect.GetDataHub<UserTable>();
            // await datahub.UpdateAsync()
        }
    }
}
