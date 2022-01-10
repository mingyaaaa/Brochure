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
            var condition = BuildSqlResult(ifSql.ConditionSql);
            var then = BuildSqlResult(ifSql.ThenSql);
            var elsesql = BuildSqlResult(ifSql.ElseSql);
            var sql = new ParmsSqlResult();
            var builder = new StringBuilder();
            builder.AppendLine($"if {condition.SQL}");
            builder.AppendLine(then.SQL);
            if (!string.IsNullOrWhiteSpace(elsesql.SQL))
            {
                builder.AppendLine("else");
                builder.AppendLine(elsesql.SQL);
            }
            sql.SQL = builder.ToString();
            sql.Parameters.AddRange(condition.Parameters);
            sql.Parameters.AddRange(then.Parameters);
            sql.Parameters.AddRange(elsesql.Parameters);
            return sql;
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
            var subSql = BuildSqlResult(existSql.SubSql);
            var notString = existSql.IsNot ? "not " : "";
            subSql.SQL = $"{notString}exists ({subSql.SQL})";
            return subSql;
        }
    }
}