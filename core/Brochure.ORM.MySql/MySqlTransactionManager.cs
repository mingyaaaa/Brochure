using Brochure.ORM.Database;

namespace Brochure.ORM.MySql
{
    /// <summary>
    /// The my sql transaction manager.
    /// </summary>
    public class MySqlTransactionManager : TransactionManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlTransactionManager"/> class.
        /// </summary>
        public MySqlTransactionManager() : base()
        {
        }
    }
}