using System;
using System.Threading.Tasks;
namespace LinqDbQuery.Database
{
    public abstract class DbColumns
    {
        protected DbQueryOption Option;

        protected DbColumns (DbQueryOption option)
        {
            Option = option;
        }

        public Task<bool> IsExistColumnAsync (string columnName)
        {
            return Task.Run (() => IsExistColumn (columnName));
        }

        public abstract bool IsExistColumn (string columnName);

        public Task<long> RenameColumnAsync (string columnName, string newcolumnName, string typeName)
        {
            return Task.Run (() => RenameColumn (columnName, newcolumnName, typeName));
        }

        public abstract long RenameColumn (string columnName, string newcolumnName, string typeName);

        public Task<long> UpdateColumnAsync (string columnName, string typeName, bool isNotNull)
        {
            return Task.Run (() => UpdateColumn (columnName, typeName, isNotNull));
        }

        public abstract long UpdateColumn (string columnName, string typeName, bool isNotNull);

        public Task<long> DeleteColumnAsync (string columnName)
        {
            return Task.Run (() => DeleteColumn (columnName));
        }

        public abstract long DeleteColumn (string columnName);

        public Task<long> AddColumnsAsync (string columnName, string typeName, bool isNotNull)
        {
            return Task.Run (() => AddColumns (columnName, typeName, isNotNull));
        }

        public abstract long AddColumns (string columnName, string typeName, bool isNotNull);
    }
}