using System;
using System.Threading.Tasks;
using Brochure.ORM.Atrributes;

namespace Brochure.ORM.Database
{
    public abstract class DbColumns
    {
        protected DbOption Option;
        private readonly DbSql _dbSql;

        protected DbColumns (DbOption option, DbSql dbSql)
        {
            Option = option;
            this._dbSql = dbSql;
        }

        public Task<bool> IsExistColumnAsync (string tableName, string columnName)
        {
            return Task.Run (() => IsExistColumn (tableName, columnName));
        }

        [Transaction]
        public virtual bool IsExistColumn (string tableName, string columnName)
        {
            var connection = Option.GetDbConnection ();
            var command = connection.CreateCommand ();
            command.CommandText = _dbSql.GetColumsNameCountSql (Option.DatabaseName, tableName, columnName);
            var r = (int) command.ExecuteScalar ();
            return r >= 1;
        }

        [Transaction]
        public Task<long> RenameColumnAsync (string tableName, string columnName, string newcolumnName, TypeCode typeName)
        {
            return Task.Run (() => RenameColumn (tableName, columnName, newcolumnName, typeName));
        }

        [Transaction]
        public virtual long RenameColumn (string tableName, string columnName, string newcolumnName, TypeCode typeCode)
        {
            var connection = Option.GetDbConnection ();
            var command = connection.CreateCommand ();
            command.CommandText = _dbSql.GetRenameColumnNameSql (tableName, columnName, newcolumnName, typeCode);
            return command.ExecuteNonQuery ();
        }

        [Transaction]
        public Task<long> UpdateColumnAsync (string tableName, string columnName, TypeCode typeCode, bool isNotNull)
        {
            return Task.Run (() => UpdateColumn (tableName, columnName, typeCode, isNotNull));
        }

        [Transaction]
        public virtual long UpdateColumn (string tableName, string columnName, TypeCode typeCode, bool isNotNull)
        {
            var connection = Option.GetDbConnection ();
            var command = connection.CreateCommand ();
            command.CommandText = _dbSql.GetUpdateColumnSql (tableName, columnName, typeCode, isNotNull);
            return command.ExecuteNonQuery ();
        }

        public Task<long> DeleteColumnAsync (string tableName, string columnName)
        {
            return Task.Run (() => DeleteColumn (tableName, columnName));
        }

        public virtual long DeleteColumn (string tableName, string columnName)
        {
            var connection = Option.GetDbConnection ();
            var command = connection.CreateCommand ();
            command.CommandText = _dbSql.GetDeleteColumnSql (tableName, columnName);
            return command.ExecuteNonQuery ();
        }

        public Task<long> AddColumnsAsync (string tableName, string columnName, TypeCode typeCode, bool isNotNull)
        {
            return Task.Run (() => AddColumns (tableName, columnName, typeCode, isNotNull));
        }

        public virtual long AddColumns (string tableName, string columnName, TypeCode typeCode, bool isNotNull)
        {
            var connection = Option.GetDbConnection ();
            var command = connection.CreateCommand ();
            command.CommandText = _dbSql.GetAddllColumnSql (tableName, columnName, typeCode, isNotNull);
            return command.ExecuteNonQuery ();
        }
    }
}