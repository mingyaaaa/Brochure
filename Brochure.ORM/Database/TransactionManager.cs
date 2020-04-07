using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml.Linq;
namespace LinqDbQuery.Database
{
    public interface ITransactionManager
    {
        bool IsEmpty { get; }
        void AddTransaction (ITransaction transaction);

        void RemoveTransaction (ITransaction transaction);

        IDbTransaction GetDbTransaction ();
    }

    public class TransactionManager : ITransactionManager
    {
        private readonly List<ITransaction> transactions;
        private readonly object lockObj = new object ();

        /// <summary>
        /// 事物管理类
        /// </summary>
        public TransactionManager ()
        {
            transactions = new List<ITransaction> ();
        }

        public void AddTransaction (ITransaction transaction)
        {
            lock (lockObj)
            {
                transactions.Add (transaction);
            }
        }

        public bool IsEmpty { get { return transactions.Count == 0; } }

        public void RemoveTransaction (ITransaction transaction)
        {
            lock (lockObj)
            {
                transactions.Remove (transaction);
            }
        }

        public IDbTransaction GetDbTransaction ()
        {
            IDbTransaction dbTransaction = null;
            foreach (var item in transactions)
            {
                dbTransaction = item.GetDbTransaction ();
                if (dbTransaction != null)
                    break;
            }
            return dbTransaction;
        }
    }
}