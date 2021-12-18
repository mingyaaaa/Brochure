using AspectCore.Abstractions.DependencyInjection;
using Brochure.ORM;
using Brochure.ORM.Database;

namespace Brochure.ORM.MySql
{
    public class MySqlDbTable : DbTable
    {
        [InjectConstructor]
        public MySqlDbTable(DbContext dbContext) : base(dbContext)
        {
        }

        public MySqlDbTable(bool isBeginTransaction = false) : base(new MySqlDbContext(isBeginTransaction))
        {
        }
    }
}