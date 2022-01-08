using AspectCore.Abstractions.DependencyInjection;
using Brochure.Abstract;
using Brochure.ORM.Database;
using Brochure.ORM.Visitors;
using Microsoft.Extensions.DependencyInjection;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlDbContext"/> class.
        /// </summary>
        /// <param name="objectFactory">The object factory.</param>
        /// <param name="connectFactory">The connect factory.</param>
        /// <param name="transactionManager">The transaction manager.</param>
        /// <param name="sqlBuilder">The sql builder.</param>
        /// <param name="serviceScopeFactory"></param>
        [InjectConstructor]
        public MySqlDbContext(IObjectFactory objectFactory, IConnectFactory connectFactory, ITransactionManager transactionManager, ISqlBuilder sqlBuilder, IServiceScopeFactory serviceScopeFactory)
            : base(objectFactory, connectFactory, transactionManager, sqlBuilder, serviceScopeFactory)
        {
        }
    }
}