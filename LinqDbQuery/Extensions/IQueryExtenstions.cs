using System;
using System.Collections.Generic;
using System.Linq.Expressions;
namespace LinqDbQuery.Extensions
{
    public static class IQueryExtenstions
    {
        public static int Insert<T> (this IQuery<T> query, T obj)
        {
            return -1;
        }
        public static int InsertMany<T> (this IQuery<T> query, IEnumerable<T> objs)
        {
            return -1;
        }
        public static T Update<T> (this IQuery<T> query, object obj, Expression<Func<T, bool>> Func)
        {
            return (T) (object) null;
        }
        public static int Delete<T> (this IQuery<T> query, Expression<Func<T, bool>> fun)
        {
            return -1;
        }

    }
}