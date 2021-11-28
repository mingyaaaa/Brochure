using Brochure.ORM;
using Brochure.ORM.Database;

namespace Brochure.ORM.MySql
{
    public class MySqlDbColumns : DbColumns
    {
        public MySqlDbColumns(DbContext dbContext) : base(dbContext)
        {
        }
    }
}