using Brochure.ORM;
using Brochure.ORM.Database;

namespace Brochure.ORM.MySql
{
    public class MySqlDbIndex : DbIndex
    {
        public MySqlDbIndex(DbContext dbContext) : base(dbContext)
        {
        }

        public MySqlDbIndex(bool isBeginTransaction = false) : base(new MySqlDbContext(isBeginTransaction))
        {
        }
    }
}