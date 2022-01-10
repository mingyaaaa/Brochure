using Brochure.ORM.Querys;

namespace Brochure.ORM
{
    /// <summary>
    /// The sql.
    /// </summary>
    public interface ISql
    {
        /// <summary>
        /// Gets or sets the database.
        /// </summary>
        public string Database { get; set; }
    }

    /// <summary>
    /// The sql.
    /// </summary>
    public partial class Sql
    {
        /// <summary>
        /// Ifs the.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <param name="then">The then.</param>
        /// <param name="elseSql">The else sql.</param>
        /// <param name="database">The database.</param>
        /// <returns>An ISql.</returns>
        public static ISql If(ISql condition, ISql then, ISql elseSql, string database = "")
        {
            return new IfSql(then, condition, elseSql, database);
        }
    }
}