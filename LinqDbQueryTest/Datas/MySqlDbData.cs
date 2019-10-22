using System;
using System.Linq.Expressions;
using LinqDbQuery.Database;

namespace LinqDbQueryTest.Datas
{
    public class MySqlDbData : DbData
    {
        public MySqlDbData (LinqDbQuery.DbQueryOption dbOption) : base (dbOption) { }

        public string GetNewInsertSql<T> (T obj)
        {
            var tuple = base.GetInsertSql<T> (obj);
            return tuple.Item1;
        }
        public string GetNewUpdateSql<T> (object obj, Expression<Func<T, bool>> whereFun)
        {
            var tuple = base.GetUpdateSql<T> (obj, whereFun);
            return tuple.Item1;
        }

        public string GetNewDeleteSql<T> (Expression<Func<T, bool>> whereFun)
        {
            var tuple = base.GetDeleteSql (whereFun);
            return tuple.Item1;
        }
    }
}