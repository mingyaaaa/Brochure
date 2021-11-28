using System;
using System.Data;
using System.Reflection;
using System.Threading.Tasks;
using AspectCore.DynamicProxy;
using Brochure.ORM.Database;
using Microsoft.Extensions.DependencyInjection;

namespace Brochure.ORM.Atrributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class TransactionAttribute : AspectCore.DynamicProxy.AbstractInterceptorAttribute
    {
        /// <summary>
        /// Gets or sets a value indicating whether is disable.
        /// </summary>
        public bool IsDisable { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionAttribute"/> class.
        /// </summary>
        public TransactionAttribute()
        {
        }

        /// <summary>
        /// Invokes the.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="next">The next.</param>
        /// <returns>A Task.</returns>
        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            if (IsDisable)
            {
                await next(context);
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
                    await next(context);
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
                await transaction.CommitAsync();
                transactionManager.RemoveTransaction(transaction);
            }
        }
    }
}