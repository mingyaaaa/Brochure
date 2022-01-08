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

        /// <summary>
        /// Renames the index sql.
        /// </summary>
        /// <param name="renameIndexSql">The rename index sql.</param>
        /// <returns>An ISqlResult.</returns>
        protected virtual ISqlResult RenameIndexSql(RenameIndexSql renameIndexSql)
        {
            var r = new ParmsSqlResult();
            r.SQL = $"ALTER TABLE {renameIndexSql.TableName} RENAME INDEX {renameIndexSql.OldIndexName} TO {renameIndexSql.NewIndexName}";
            return r;
        }

        /// <summary>
        /// Builds the index count.
        /// </summary>
        /// <param name="countIndexSql">The count index sql.</param>
        /// <returns>An ISqlResult.</returns>
        protected virtual ISqlResult BuildIndexCount(CountIndexSql countIndexSql)
        {
            var r = new ParmsSqlResult();
            r.SQL = $"select COUNT(1) from information_schema.columns WHERE table_schema='{countIndexSql.Database}' and table_name = '{countIndexSql.TableName}' and index_name = '{countIndexSql.IndexName}'";
            return r;
        }
    }
}