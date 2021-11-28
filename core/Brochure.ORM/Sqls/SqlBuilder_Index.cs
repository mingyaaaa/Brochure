using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brochure.ORM
{
    public partial class SqlBuilder
    {
        /// <summary>
        /// Creates the index.
        /// </summary>
        /// <param name="createIndexSql">The create index sql.</param>
        /// <returns>An ISqlResult.</returns>
        protected virtual ISqlResult BuildCreateIndex(CreateIndexSql createIndexSql)
        {
            var r = new ParmsSqlResult();
            r.SQL = $"create {createIndexSql.SqlIndex} {createIndexSql.IndexName} on {createIndexSql.TableName}({string.Join(",", createIndexSql.ColumnNames)})";
            return r;
        }

        /// <summary>
        /// Deletes the index.
        /// </summary>
        /// <param name="deleteIndexSql">The delete index sql.</param>
        /// <returns>An ISqlResult.</returns>
        protected virtual ISqlResult BuildDeleteIndex(DeleteIndexSql deleteIndexSql)
        {
            var r = new ParmsSqlResult();
            r.SQL = $"drop index {deleteIndexSql.IndexName} on {deleteIndexSql.TableName}";
            return r;
        }
    }
}