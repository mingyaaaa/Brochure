using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Brochure.ORM.Extensions
{
    public static class DbContextExtensions
    {
        #region DbData

        /// <summary>
        /// Inserts the.
        /// </summary>
        /// <param name="dbContext">The db context.</param>
        /// <param name="data">The data.</param>
        /// <returns>An int.</returns>
        public static int Insert<T>(this DbContext dbContext, T data) where T : class
        {
            var dbData = dbContext.Datas;
            return dbData.Insert(data);
        }

        /// <summary>
        /// Inserts the many.
        /// </summary>
        /// <param name="dbContext">The db context.</param>
        /// <param name="datas">The datas.</param>
        /// <returns>An int.</returns>
        public static int InsertMany<T>(this DbContext dbContext, IEnumerable<T> datas) where T : class
        {
            var dbData = dbContext.Datas;
            return dbData.InsertMany(datas);
        }

        /// <summary>
        /// Updates the.
        /// </summary>
        /// <param name="dbContext">The db context.</param>
        /// <param name="obj">The obj.</param>
        /// <param name="where">The where.</param>
        /// <returns>An int.</returns>
        public static int Update<T>(this DbContext dbContext, object obj, Expression<Func<T, bool>> where)
        {
            var dbData = dbContext.Datas;
            return dbData.Update(obj, where);
        }

        /// <summary>
        /// Updates the.
        /// </summary>
        /// <param name="dbContext">The db context.</param>
        /// <param name="obj">The obj.</param>
        /// <param name="query">The query.</param>
        /// <returns>An int.</returns>
        public static int Update<T>(this DbContext dbContext, object obj, IQuery query)
        {
            var dbData = dbContext.Datas;
            return dbData.Update<T>(obj, query);
        }

        /// <summary>
        /// Deletes the.
        /// </summary>
        /// <param name="dbContext">The db context.</param>
        /// <param name="where">The where.</param>
        /// <returns>An int.</returns>
        public static int Delete<T>(this DbContext dbContext, Expression<Func<T, bool>> where)
        {
            var dbData = dbContext.Datas;
            return dbData.Delete(where);
        }

        /// <summary>
        /// Deletes the.
        /// </summary>
        /// <param name="dbContext">The db context.</param>
        /// <param name="query">The query.</param>
        /// <returns>An int.</returns>
        public static int Delete<T>(this DbContext dbContext, IQuery query)
        {
            var dbData = dbContext.Datas;
            return dbData.Delete<T>(query);
        }

        /// <summary>
        /// Queries the.
        /// </summary>
        /// <param name="dbContext">The db context.</param>
        /// <param name="query">The query.</param>
        /// <returns>A list of TS.</returns>
        public static IEnumerable<T> Find<T>(this DbContext dbContext, IQuery query) where T : class, new()
        {
            var dbData = dbContext.Datas;
            return dbData.Find<T>(query);
        }

        #endregion DbData
    }
}