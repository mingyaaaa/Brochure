using Brochure.ORM;
using Brochure.ORM.Database;

namespace Brochure.ORM.MySql
{
    public class MySqlDbIndex : DbIndex
    {
        public MySqlDbIndex(DbContext dbContext) : base(dbContext)
        {
        }
    }
}