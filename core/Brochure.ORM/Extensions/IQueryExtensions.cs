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
        /// <param name="thisQuery">The this query.</param>
        /// <param name="query1">The query1.</param>
        /// <returns>An IQuery.</returns>
        public static IEnumerable<ISql> Continue(this ISql sql, ISql desSql)
        {
            return new List<ISql>() { sql, desSql };
        }
    }
}