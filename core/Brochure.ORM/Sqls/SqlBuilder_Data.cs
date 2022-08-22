using Brochure.Abstract;
using Brochure.Abstract.Extensions;
using Brochure.Extensions;
using Brochure.ORM.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Brochure.ORM
{
    /// <summary>
    /// The sql builder.
    /// </summary>
    public partial class SqlBuilder
    {
        /// <summary>
        /// Builds the delete sql.
        /// </summary>
        /// <param name="deleteSql">The delete sql.</param>
        /// <returns>An ISqlResult.</returns>
        protected virtual ISqlResult BuildDeleteSql(DeleteSql deleteSql)
        {
            var query = deleteSql.WhereQuery;
            var result = new ParmsSqlResult();
            var whereSqlResult = query != null ? BuildQuery(query) : new ParmsSqlResult();
            var tableName = TableUtlis.GetTableName(deleteSql.Table);
            var sql = $"delete from {_dbProvider.FormatFieldName(tableName)} {whereSqlResult.SQL}";
            result.SQL = sql;
            result.Parameters.AddRange(whereSqlResult.Parameters);
            return result;
        }

        /// <summary>
        /// Builds the insert sql.
        /// </summary>
        /// <param name="insertSql">The insert sql.</param>
        /// <returns>An ISqlResult.</returns>
        protected virtual ISqlResult BuildInsertSql(InsertSql insertSql)
        {
            var result = new ParmsSqlResult();
            var doc = EntityUtil.AsTableRecord(insertSql.Table);
            var tableName = TableUtlis.GetTableName(insertSql.Table.GetType());
            var sql = $"insert into {_dbProvider.FormatFieldName(tableName)}";
            var pams = new List<IDbDataParameter>();
            var fields = new StringJoin(",");
            var valueList = new StringJoin(",");
            foreach (var item in doc.Keys.ToList())
            {
                if (_dbOption.IsUseParamers)
                {
                    var param = _dbProvider.GetDbDataParameter();
                    param.ParameterName = Guid.NewGuid().ToString();
                    param.Value = doc[item];
                    if (param.Value != null)
                    {
                        fields.Join($"{_dbProvider.FormatFieldName(item)}");
                        pams.Add(param);
                    }
                }
                else
                {
                    var t_value = _dbProvider.GetObjectType(doc[item]);
                    if (t_value != null)
                    {
                        valueList.Join(t_value);
                        fields.Join($"{_dbProvider.FormatFieldName(item)}");
                    }
                }
            }
            if (_dbOption.IsUseParamers)
                sql = $"{sql}({fields}) values({pams.Join(",", t => t.ParameterName)})";
            else
                sql = $"{sql}({fields}) values({valueList})";
            result.SQL = sql;
            result.Parameters.AddRange(pams);
            return result;
        }

        /// <summary>
        /// Builds the update sql.
        /// </summary>
        /// <param name="updateSql">The update sql.</param>
        /// <returns>An ISqlResult.</returns>
        protected virtual ISqlResult BuildUpdateSql(UpdateSql updateSql)
        {
            var query = updateSql.WhereQuery;
            var result = new ParmsSqlResult();
            var whereSqlResult = query != null ? BuildQuery(query) : new ParmsSqlResult();
            var tableName = TableUtlis.GetTableName(updateSql.Table);
            var doc = updateSql.UpdateObj.As<IRecord>();
            var sql = $"update {_dbProvider.FormatFieldName(tableName)} set ";
            var fieldList = new StringJoin(",");
            var parms = new List<IDbDataParameter>(whereSqlResult.Parameters);
            foreach (var item in doc.Keys.ToList())
            {
                var fieldStr = string.Empty;
                if (_dbOption.IsUseParamers)
                {
                    var param = _dbProvider.GetDbDataParameter();
                    param.ParameterName = $"{_dbProvider.GetParamsSymbol()}{item}";
                    param.Value = doc[item];
                    fieldStr = $"{_dbProvider.FormatFieldName(item)}={param.ParameterName}";
                    parms.Add(param);
                }
                else
                {
                    fieldStr = $"{_dbProvider.FormatFieldName(item)}={_dbProvider.GetObjectType(doc[item])}";
                }
                fieldList.Join(fieldStr);
            }
            sql = $"{sql}{fieldList} {whereSqlResult.SQL}";
            result.SQL = sql;
            result.Parameters.AddRange(parms);
            return result;
        }
    }
}