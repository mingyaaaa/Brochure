using System;
using System.Data.Common;
using Brochure.Core.Server;
namespace Brochure.Server.MySql.Implements
{
    public class DbTransaction : IDbTransaction
    {

        public DbTransaction(System.Data.Common.DbTransaction dbTransaction)
        {
            Transaction = dbTransaction;
        }

        public System.Data.Common.DbTransaction Transaction { get; }

        public void Commit()
        {
            Transaction.Commit();
            Transaction.Connection.Close();
        }

        public void Rollback()
        {
            Transaction.Rollback();
            Transaction.Connection.Close();
        }
    }
}
