using System;
using System.Linq.Expressions;
using LinqDbQuery.Database;

namespace LinqDbQueryTest.Datas
{
    public class MySqlDbData : DbData
    {
        private readonly DbSql dbSql;

        public MySqlDbData (LinqDbQuery.DbOption dbOption, DbSql dbSql) : base (dbOption, dbSql)
        {
            this.dbSql = dbSql;
        }

        public string GetNewInsertSql<T> (T obj)
        {
            var tuple = dbSql.GetInsertSql<T> (obj);
            return tuple.Item1;
        }

        public string GetNewUpdateSql<T> (object obj, Expression<Func<T, bool>> whereFun)
        {
            var tuple = dbSql.GetUpdateSql<T> (obj, whereFun);
            return tuple.Item1;
        }

        public string GetNewDeleteSql<T> (Expression<Func<T, bool>> whereFun)
        {
            var tuple = dbSql.GetDeleteSql (whereFun);
            return tuple.Item1;
        }
    }
}