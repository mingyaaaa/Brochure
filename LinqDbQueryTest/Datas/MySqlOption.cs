using LinqDbQuery;

namespace LinqDbQueryTest.Datas
{
    public class MySqlOption : DbOption
    {
        public MySqlOption () { }

        public MySqlOption (IDbProvider dbProvider) : base (dbProvider) { }
    }
}