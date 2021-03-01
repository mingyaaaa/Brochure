using System;
using System.Collections.Generic;
using System.Linq.Expressions;
namespace Brochure.ORM.Extensions
{
    public static class DbContextExtensions
    {
        #region DbData
        public static int Insert<T>(this DbContext dbContext, T data)
        {
            var dbData = dbContext.GetDbData();
            return dbData.Insert(data);
        }

        public static int InsertMany<T>(this DbContext dbContext, IEnumerable<T> datas)
        {
            var dbData = dbContext.GetDbData();
            return dbData.InsertMany(datas);
        }

        public static int Update<T>(this DbContext dbContext, object obj, Expression<Func<T, bool>> where)
        {
            var dbData = dbContext.GetDbData();
            return dbData.Update(obj, where);
        }

        public static int Update<T>(this DbContext dbContext, object obj, IWhereQuery query)
        {
            var dbData = dbContext.GetDbData();
            return dbData.Update<T>(obj, query);
        }

        public static int Delete<T>(this DbContext dbContext, Expression<Func<T, bool>> where)
        {
            var dbData = dbContext.GetDbData();
            return dbData.Delete(where);
        }

        public static int Delete<T>(this DbContext dbContext, IWhereQuery query)
        {
            var dbData = dbContext.GetDbData();
            return dbData.Delete<T>(query);
        }

        public static IEnumerable<T> Query<T>(this DbContext dbContext, IQuery query)
        {
            var dbData = dbContext.GetDbData();
            return dbData.Query<T>(query);
        }
        #endregion
    }
}