using LinqDbQuery;

namespace LinqDbQueryTest.Datas
{
    public class MySqlOption : DbQueryOption
    {
        public MySqlOption ()
        { }

        public MySqlOption (IDbProvider dbProvider) : base (dbProvider) { }
    }
}