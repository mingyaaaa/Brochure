using Brochure.ORM;
using Brochure.ORM.Database;

namespace Brochure.ORM.MySql
{
    public class MySqlDbTable : DbTable
    {
        public MySqlDbTable(DbContext dbContext) : base(dbContext)
        {
        }
    }
}