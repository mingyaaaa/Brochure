using Brochure.ORM.Querys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brochure.ORM
{
    /// <summary>
    /// The sql builder.
    /// </summary>
    public partial class SqlBuilder
    {
        /// <summary>
        /// Builders the if sql.
        /// </summary>
        /// <param name="ifSql">The if sql.</param>
        /// <returns>An ISqlResult.</returns>
        protected virtual ISqlResult BuilderIfSql(IfSql ifSql)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Builders the string sql.
        /// </summary>
        /// <param name="stringsql">The stringsql.</param>
        /// <returns>An ISqlResult.</returns>
        protected virtual ISqlResult BuilderStringSql(StringSql stringsql)
        {
            var sqlResult = new ParmsSqlResult();
            sqlResult.SQL = stringsql.ToString();
            return sqlResult;
        }

        /// <summary>
        /// Builders the exist.
        /// </summary>
        /// <param name="existSql">The exist sql.</param>
        /// <param name="isNot">If true, is not.</param>
        /// <returns>An ISqlResult.</returns>
        protected virtual ISqlResult BuilderExist(ExistSql existSql)
        {
            throw new NotSupportedException();
        }
    }
}