using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AspectCore.Injector;

namespace LinqDbQuery.Database
{
    public abstract class DbData
    {
        protected DbData ()
        {
            Provider = DI.Ins.ServiceProvider.ResolveRequired<IDbProvider> ();
        }

        protected DbData (IDbProvider provider)
        {
            Provider = provider;
        }

        protected IDbProvider Provider;

        public virtual int Insert<T> (T obj)
        {
            return -1;
        }

        public virtual int InsertMany<T> (IEnumerable<T> objs)
        {
            return -1;
        }

        public virtual T Update<T> (object obj, Expression<Func<T, bool>> Func)
        {
            return (T) (object) null;
        }

        public virtual int Delete<T> (Expression<Func<T, bool>> fun)
        {
            return -1;
        }
    }
}