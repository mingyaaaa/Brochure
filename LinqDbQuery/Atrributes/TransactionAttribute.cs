using System;
using System.Data;
using System.Reflection;
using System.Threading.Tasks;
using AspectCore.DynamicProxy;
using LinqDbQuery.Database;
using Microsoft.Extensions.DependencyInjection;
namespace LinqDbQuery.Atrributes
{
    [AttributeUsage (AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class TransactionAttribute : AspectCore.DynamicProxy.AbstractInterceptorAttribute
    {
        public bool IsDisable { get; set; }
        public TransactionAttribute () { }

        public override Task Invoke (AspectContext context, AspectDelegate next)
        {
            if (IsDisable)
            {
                next (context);
            }
            else
            {
                var transactionManager = context.ServiceProvider.GetService<TransactionManager> ();
                var dbOption = context.ServiceProvider.GetService<DbOption> ();
                ITransaction transaction;
                if (transactionManager.IsEmpty)
                    transaction = new Transaction (dbOption);
                else
                    transaction = new InnerTransaction (dbOption);
                transactionManager.AddTransaction (transaction);
                next (context);
                transaction.Commit ();
                transactionManager.RemoveTransaction (transaction);
            }
            return Task.CompletedTask;
        }
    }
}