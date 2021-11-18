using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Brochure.ORM.Extensions
{
    public static class DbContextExtensions
    {
        #region DbData

        public static int Insert<T>(this DbContext dbContext, T data) where T : class
        {
            var dbData = dbContext.Datas;
            return dbData.Insert(data);
        }

        public static int InsertMany<T>(this DbContext dbContext, IEnumerable<T> datas) where T : class
        {
            var dbData = dbContext.Datas;
            return dbData.InsertMany(datas);
        }

        public static int Update<T>(this DbContext dbContext, object obj, Expression<Func<T, bool>> where)
        {
            var dbData = dbContext.Datas;
            return dbData.Update(obj, where);
        }

        public static int Update<T>(this DbContext dbContext, object obj, IQuery query)
        {
            var dbData = dbContext.Datas;
            return dbData.Update<T>(obj, query);
        }

        public static int Delete<T>(this DbContext dbContext, Expression<Func<T, bool>> where)
        {
            var dbData = dbContext.Datas;
            return dbData.Delete(where);
        }

        public static int Delete<T>(this DbContext dbContext, IQuery query)
        {
            var dbData = dbContext.Datas;
            return dbData.Delete<T>(query);
        }

        public static IEnumerable<T> Query<T>(this DbContext dbContext, IQuery query) where T : class, new()
        {
            var dbData = dbContext.Datas;
            return dbData.Query<T>(query);
        }

        #endregion DbData
    }
}