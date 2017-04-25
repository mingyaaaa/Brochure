using System.Data.SqlClient;

namespace Brochure.Core.implement
{
    public class SqlServerTransaction:ITransaction
    {
        private SqlTransaction _transaction;
        public SqlServerTransaction(SqlTransaction transaction1)
        {
            this._transaction = transaction1;
        }

        public void Dispose()
        {
            _transaction.Dispose();
        }

        public void Rollback()
        {
            _transaction.Rollback();
        }

        public void Commit()
        {
           _transaction.Commit();
        }
    }
}