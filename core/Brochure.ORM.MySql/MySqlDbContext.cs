using Brochure.Abstract;
using Brochure.ORM.Database;
using Brochure.ORM.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brochure.ORM.MySql
{
    /// <summary>
    /// The my sql db context.
    /// </summary>
    public class MySqlDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlDbContext"/> class.
        /// </summary>
        public MySqlDbContext(bool isBeginTransaction = false) : base(isBeginTransaction)
        {
        }

        public MySqlDbContext(IObjectFactory objectFactory, IConnectFactory connectFactory, ITransactionManager transactionManager, ISqlBuilder sqlBuilder) : base(objectFactory, connectFactory, transactionManager, sqlBuilder)
        {
        }
    }
}