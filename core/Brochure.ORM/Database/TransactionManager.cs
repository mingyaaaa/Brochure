using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Brochure.ORM.Database
{
    /// <summary>
    /// The transaction manager.
    /// </summary>
    public interface ITransactionManager
    {
        /// <summary>
        /// Gets a value indicating whether is empty.
        /// </summary>
        bool IsEmpty { get; }

        /// <summary>
        /// Adds the transaction.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        void AddTransaction(ITransaction transaction);

        /// <summary>
        /// Removes the transaction.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        void RemoveTransaction(ITransaction transaction);

        /// <summary>
        /// Gets the db transaction.
        /// </summary>
        /// <returns>An IDbTransaction.</returns>
        Task<DbTransaction> GetDbTransactionAsync();
    }

    public class TransactionManager : ITransactionManager
    {
        private readonly List<ITransaction> transactions;
        private readonly object lockObj = new object();

        /// <summary>
        /// 事物管理类
        /// </summary>
        public TransactionManager()
        {
            transactions = new List<ITransaction>();
        }

        /// <summary>
        /// Adds the transaction.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        public void AddTransaction(ITransaction transaction)
        {
            lock (lockObj)
            {
                transactions.Add(transaction);
            }
        }

        /// <summary>
        /// Gets a value indicating whether is empty.
        /// </summary>
        public bool IsEmpty { get { return transactions.Count == 0; } }

        /// <summary>
        /// Removes the transaction.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        public void RemoveTransaction(ITransaction transaction)
        {
            lock (lockObj)
            {
                transactions.Remove(transaction);
            }
        }

        /// <summary>
        /// Gets the db transaction.
        /// </summary>
        /// <returns>An IDbTransaction.</returns>
        public async Task<DbTransaction> GetDbTransactionAsync()
        {
            DbTransaction dbTransaction = null;
            foreach (var item in transactions.OfType<Transaction>())
            {
                dbTransaction = await item.GetDbTransactionAsync();
                if (dbTransaction != null)
                    break;
            }
            return dbTransaction;
        }
    }
}