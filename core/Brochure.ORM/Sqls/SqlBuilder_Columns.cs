using System;

namespace Brochure.ORM
{
    public partial class SqlBuilder
    {
        /// <summary>
        /// Builds the column names.
        /// </summary>
        /// <param name="columsNamesSql">The colums names sql.</param>
        /// <returns>An ISqlResult.</returns>
        protected virtual ISqlResult BuildColumnNames(ColumsNamesSql columsNamesSql)
        {
            var r = new ParmsSqlResult();
            r.SQL = $"select column_name from information_schema.columns where table_schema='{columsNamesSql.DatabaseName}' and table_name='{columsNamesSql.TableName}'";
            return r;
        }

        /// <summary>
        /// Builds the colums count.
        /// </summary>
        /// <param name="countColumsSql">The count colums sql.</param>
        /// <returns>An ISqlResult.</returns>
        protected virtual ISqlResult BuildColumnsCount(CountColumsSql countColumsSql)
        {
            var r = new ParmsSqlResult();
            r.SQL = $"select COUNT(1) from information_schema.columns WHERE table_schema='{countColumsSql.Database}' and table_name = '{countColumsSql.TableName}' and column_name = '{countColumsSql.ColumnsName}'";
            return r;
        }

        /// <summary>
        /// Builds the rebane columns name.
        /// </summary>
        /// <param name="renameColumnSql">The rename column sql.</param>
        /// <returns>An ISqlResult.</returns>
        protected virtual ISqlResult BuildRenameColumnsName(RenameColumnSql renameColumnSql)
        {
            var r = new ParmsSqlResult();
            var sqlType = _typeMap.GetSqlType(renameColumnSql.TypeCode.ToString());
            if (renameColumnSql.Lentgh < 0)
                throw new ArgumentException("长度不能为小于0");
            var lengthStr = GetLengthStr(renameColumnSql.Lentgh, renameColumnSql.TypeCode);
            r.SQL = $"alter table {renameColumnSql.TableName} change column {renameColumnSql.OldName} {renameColumnSql.NewName} {sqlType}{lengthStr}";
            return r;
        }

        /// <summary>
        /// Builds the update columns.
        /// </summary>
        /// <param name="updateColumnsSql">The update columns sql.</param>
        /// <returns>An ISqlResult.</returns>
        protected virtual ISqlResult BuildUpdateColumns(UpdateColumnsSql updateColumnsSql)
        {
            var r = new ParmsSqlResult();
            var sqlType = _typeMap.GetSqlType(updateColumnsSql.TypeCode.ToString());
            var sql = $"alter table {updateColumnsSql.TableName} modify {updateColumnsSql.ColumnName} {sqlType}";
            if (updateColumnsSql.Lentgh < 0)
                throw new ArgumentException("长度不能为小于0");
            var lengthStr = GetLengthStr(updateColumnsSql.Lentgh, updateColumnsSql.TypeCode);
            sql = $"{sql}{lengthStr}";
            if (updateColumnsSql.IsNotNull)
                sql = $"{sql} not null";
            r.SQL = sql;
            return r;
        }

        /// <summary>
        /// Builds the add colums.
        /// </summary>
        /// <param name="addColumnsSql">The add columns sql.</param>
        /// <returns>An ISqlResult.</returns>
        protected virtual ISqlResult BuildAddColumns(AddColumnsSql addColumnsSql)
        {
            var r = new ParmsSqlResult();
            var sqlType = _typeMap.GetSqlType(addColumnsSql.TypeCode.ToString());
            var sql = $"alter table {addColumnsSql.TableName} add column {addColumnsSql.ColumnName} {sqlType}";
            if (addColumnsSql.Lentgh < 0)
                throw new ArgumentException("长度不能为小于0");
            var lengthStr = GetLengthStr(addColumnsSql.Lentgh, addColumnsSql.TypeCode);
            sql = $"{sql}{lengthStr}";
            if (addColumnsSql.IsNotNull)
            {
                sql = $"{sql} not null";
            }
            r.SQL = sql;
            return r;
        }

        /// <summary>
        /// Builds the delete column.
        /// </summary>
        /// <param name="deleteColumsSql">The delete colums sql.</param>
        /// <returns>An ISqlResult.</returns>
        protected virtual ISqlResult BuildDeleteColumn(DeleteColumsSql deleteColumsSql)
        {
            var r = new ParmsSqlResult();
            r.SQL = $"alter table {deleteColumsSql.TableName} drop column {deleteColumsSql.ColumsName}";
            return r;
        }
    }
}