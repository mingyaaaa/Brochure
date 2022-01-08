using Brochure.Core.Extenstions;
using Brochure.ORM.Querys;
using Brochure.ORM.Visitors;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Brochure.ORM
{
    /// <summary>
    /// The query builder.
    /// </summary>
    public interface ISqlBuilder
    {
        /// <summary>
        /// Builds the.
        /// </summary>
        /// <param name="sqls"></param>
        /// <returns>A ParmsSqlResult.</returns>
        ISqlResult Build(params ISql[] sqls);

        /// <summary>
        /// Builds the.
        /// </summary>
        /// <param name="sqls"></param>
        /// <returns>A ParmsSqlResult.</returns>
        ISqlResult Build(IEnumerable<ISql> sqls);

        /// <summary>
        /// Renames the parameter.
        /// </summary>
        /// <param name="sql">The sql.</param>
        /// <param name="parDic">The par dic.</param>
        /// <returns>An array of IDbDataParameters.</returns>
        IDbDataParameter[] RenameParameter(StringBuilder sql, IEnumerable<IDbDataParameter> parDic);

        /// <summary>
        /// Renames the table type.
        /// </summary>
        /// <param name="str">The str.</param>
        /// <param name="tableType">The table type.</param>
        /// <param name="tempTable">The temp table.</param>
        /// <param name="groupDic">The group dic.</param>
        void RenameTableType(StringBuilder str, IDictionary<int, Type> tableType, IEnumerable<int> tempTable = null, IDictionary<string, string> groupDic = null);

        /// <summary>
        /// Renames the temp table type.
        /// </summary>
        /// <param name="str">The str.</param>
        /// <param name="tempType">The temp type.</param>
        void RenameTempTableType(StringBuilder str, IEnumerable<int> tempType);
    }

    /// <summary>
    /// The query builder.
    /// </summary>
    public partial class SqlBuilder : ISqlBuilder
    {
        /// <summary>
        ///
        /// </summary>
        protected readonly IVisitProvider _visitProvider;

        /// <summary>
        ///
        /// </summary>
        protected readonly IDbProvider _dbProvider;

        /// <summary>
        ///
        /// </summary>
        protected readonly DbOption _dbOption;

        /// <summary>
        ///
        /// </summary>
        protected readonly TypeMap _typeMap;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlBuilder"/> class.
        /// </summary>
        /// <param name="visitProvider">The visit provider.</param>
        /// <param name="dbProvider">The db provider.</param>
        /// <param name="dbOption"></param>
        /// <param name="typeMap"></param>
        public SqlBuilder(IVisitProvider visitProvider, IDbProvider dbProvider,
            DbOption dbOption,
            TypeMap typeMap)
        {
            _visitProvider = visitProvider;
            _dbProvider = dbProvider;
            _dbOption = dbOption;
            _typeMap = typeMap;
        }

        /// <summary>
        /// Builds the.
        /// </summary>
        /// <param name="sqls"></param>
        /// <returns>A ParmsSqlResult.</returns>
        public ISqlResult Build(params ISql[] sqls)
        {
            var result = new ParmsSqlResult();
            var sqlList = new List<string>();
            var sqlParam = new SqlParam();
            foreach (var item in sqls)
            {
                var r = BuildSqlResult(item);
                sqlList.Add(r.SQL);
                result.Parameters.AddRange(r.Parameters);
            }
            result.SQL = string.Join(';', sqlList);
            result.Parameters.AddRange(sqlParam.Params);
            var t_sql = new StringBuilder(result.SQL);

            RenameParameter(t_sql, result.Parameters);
            result.SQL = t_sql.ToString().Trim();
            return result;
        }

        /// <summary>
        /// Builds the.
        /// </summary>
        /// <param name="sqls">The sqls.</param>
        /// <returns>An ISqlResult.</returns>
        public ISqlResult Build(IEnumerable<ISql> sqls)
        {
            return Build(sqls.ToArray());
        }

        /// <summary>
        /// Builds the sql result.
        /// </summary>
        /// <param name="sql">The sql.</param>
        /// <returns>An ISqlResult.</returns>
        public ISqlResult BuildSqlResult(ISql sql)
        {
            return sql switch
            {
                IQuery query => BuildQuery(query),
                CountDatabaseSql countDatabaseSql => BuildCountForDatabase(countDatabaseSql),
                DeleteDatabaseSql deleteDatabaseSql => BuildDeleteDatabase(deleteDatabaseSql),
                AllTableNamesSql allTableNamesSql => BuildAllTableName(allTableNamesSql),
                CreateDatabaseSql createDatabaseSql => BuildCreateDatabase(createDatabaseSql),
                AllDatabaseSql allDatabaseSql => BuildAllDatabase(allDatabaseSql),
                CreateTableSql createTableSql => BuildCreateTable(createTableSql),
                CountTableSql countTableSql => BuildTableCount(countTableSql),
                UpdateTableNameSql updateTableNameSql => BuildUpdateTableName(updateTableNameSql),
                DeleteTableSql deleteTableSql => BuildDeleteTable(deleteTableSql),
                ColumsNamesSql columsNamesSql => BuildColumnNames(columsNamesSql),
                CountColumsSql countColumsSql => BuildColumnsCount(countColumsSql),
                RenameColumnSql renameColumnSql => BuildRenameColumnsName(renameColumnSql),
                UpdateColumnsSql updateColumnsSql => BuildUpdateColumns(updateColumnsSql),
                AddColumnsSql addColumnsSql => BuildAddColumns(addColumnsSql),
                DeleteColumsSql deleteColumsSql => BuildDeleteColumn(deleteColumsSql),
                CreateIndexSql createIndexSql => BuildCreateIndex(createIndexSql),
                DeleteIndexSql deleteIndexSql => BuildDeleteIndex(deleteIndexSql),
                CountIndexSql countIndexSql => BuildIndexCount(countIndexSql),
                RenameIndexSql renameIndexSql => RenameIndexSql(renameIndexSql),
                DeleteSql deleteSql => BuildDeleteSql(deleteSql),
                InsertSql insertSql => BuildInsertSql(insertSql),
                UpdateSql updateSql => BuildUpdateSql(updateSql),
                _ => new ParmsSqlResult()
            };
        }

        /// <summary>
        /// Adds the white space.
        /// </summary>
        /// <param name="str">The str.</param>
        /// <returns>A string.</returns>
        private string AddWhiteSpace(string str)
        {
            if (!string.IsNullOrWhiteSpace(str))
                return str + " ";
            return string.Empty;
        }

        /// <summary>
        /// Renames the parameter.
        /// </summary>
        /// <param name="stringBuilder">The string builder.</param>
        /// <param name="parameters">The par dic.</param>
        /// <returns>An array of IDbDataParameters.</returns>
        public IDbDataParameter[] RenameParameter(StringBuilder stringBuilder, IEnumerable<IDbDataParameter> parameters)
        {
            var name = string.Empty;
            var count = 0;
            foreach (var item in parameters)
            {
                name = $"{_dbProvider.GetParamsSymbol()}p{count}";
                stringBuilder.Replace(item.ParameterName, name);
                item.ParameterName = name;
                count++;
            }
            return parameters.ToArray();
        }

        /// <summary>
        /// Gets the length str.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <param name="code">The code.</param>
        /// <returns>A string.</returns>
        private string GetLengthStr(int length, TypeCode code)
        {
            if (code == TypeCode.Double || code == TypeCode.Single)
            {
                length = length <= 0 ? 15 : length;
                return $"({length},6)";
            }
            else if (code != TypeCode.DateTime &&
                code != TypeCode.Byte &&
                code != TypeCode.Boolean
            )
            {
                length = length <= 0 ? 255 : length;
                return $"({length})";
            }
            return string.Empty;
        }
    }
}