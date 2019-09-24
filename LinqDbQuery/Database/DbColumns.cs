using System;
using System.Threading.Tasks;
namespace LinqDbQuery.Database
{
    public abstract class DbColumns
    {
        protected DbQueryOption Option;
        public DbColumns (DbQueryOption option)
        {
            Option = option;
        }
        public Task<bool> IsExistColumnAsync (string columnName)
        {
            return Task.Run (() =>
            {
                return IsExistColumn (columnName);
            });
        }
        public abstract bool IsExistColumn (string columnName);

        public Task<long> RenameColumnAsync (string columnName, string newcolumnName, string typeName)
        {
            return Task.Run (() =>
            {
                return RenameColumn (columnName, newcolumnName, typeName);
            });

        }
        public abstract long RenameColumn (string columnName, string newcolumnName, string typeName);

        public Task<long> UpdateColumnAsync (string columnName, string typeName, bool isNotNull)
        {
            return Task.Run (() =>
            {
                return UpdateColumn (columnName, typeName, isNotNull);
            });
        }
        public abstract long UpdateColumn (string columnName, string typeName, bool isNotNull);

        public Task<long> DeleteColumnAsync (string columnName)
        {
            return Task.Run (() =>
            {
                return DeleteColumn (columnName);
            });
        }
        public abstract long DeleteColumn (string columnName);

        public Task<long> AddColumnsAsync (string columnName, string typeName, bool isNotNull)
        {
            return Task.Run (() =>
            {
                return AddColumns (columnName, typeName, isNotNull);
            });
        }
        public abstract long AddColumns (string columnName, string typeName, bool isNotNull);
    }
}