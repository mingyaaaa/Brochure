using System;
using System.Data;
using Brochure.ORM.Database;

namespace Brochure.ORM
{
    /// <summary>
    /// The db option.
    /// </summary>
    public abstract class DbOption
    {
        /// <summary>
        /// Gets or sets the transaction level.
        /// </summary>
        public IsolationLevel TransactionLevel { get; set; } = IsolationLevel.ReadUncommitted;
        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        public string ConnectionString { get; set; }
        /// <summary>
        /// Gets or sets the timeout.
        /// </summary>
        public int Timeout { get; set; }
        /// <summary>
        /// Gets or sets the database name.
        /// </summary>
        public string DatabaseName { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether use is paramers.
        /// </summary>
        public bool IsUseParamers { get; set; } = true;
    }
}