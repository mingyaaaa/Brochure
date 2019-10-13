using System;
using System.Collections.Generic;
using System.Linq.Expressions;
namespace LinqDbQuery.Extensions
{
    public static class IQueryExtenstions
    {
        public static int Insert<T> (T obj)
        {

            return -1;
        }

        public static int InsertMany<T> (IEnumerable<T> objs)
        {
            return -1;
        }

        public static T Update<T> (object obj, Expression<Func<T, bool>> Func)
        {
            return (T) (object) null;
        }

        public static int Delete<T> (Expression<Func<T, bool>> fun)
        {
            return -1;
        }
    }
}