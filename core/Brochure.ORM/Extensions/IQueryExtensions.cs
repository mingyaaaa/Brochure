using System.Collections.Generic;

namespace Brochure.ORM.Extensions
{
    /// <summary>
    /// The i query extensions.
    /// </summary>
    public static class IQueryExtensions
    {
        /// <summary>
        /// Continues the.
        /// </summary>
        /// <returns>An IQuery.</returns>
        public static IEnumerable<ISql> Continue(this ISql sql, ISql desSql)
        {
            return new List<ISql>() { sql, desSql };
        }

        /// <summary>
        /// Continues the.
        /// </summary>
        /// <returns>An IQuery.</returns>
        public static IEnumerable<ISql> Continue(this IEnumerable<ISql> sql, ISql desSql)
        {
            return new List<ISql>(sql) { desSql };
        }
    }
}