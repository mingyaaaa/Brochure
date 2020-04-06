using System;
using System.Data;
using System.Reflection;
using System.Threading.Tasks;
using AspectCore.DynamicProxy;
using LinqDbQuery.Database;
using Microsoft.Extensions.DependencyInjection;
namespace LinqDbQuery.Atrributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class TransactionAttribute : AspectCore.DynamicProxy.AbstractInterceptorAttribute
    {
        public bool IsDisable { get; set; }
        public TransactionAttribute() { }

        public override Task Invoke(AspectContext context, AspectDelegate next)
        {
            if (IsDisable)
            {
                next(context);
            }
            else
            {
                var transactionManager = context.ServiceProvider.GetService<ITransactionManager>();
                var factory = context.ServiceProvider.GetService<ITransactionFactory>();
                //此处如果transactionManager.IsEmpty为空则 返回Transaction 否则返回InnerTransaction
                ITransaction transaction = factory.GetTransaction();
                transactionManager.AddTransaction(transaction);
                try
                {
                    next(context);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }

                transaction.Commit();
                transactionManager.RemoveTransaction(transaction);
            }
            return Task.CompletedTask;
        }
    }
}