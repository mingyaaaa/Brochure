namespace Brochure.ORM.Querys
{
    public interface IQueryBuilder
    {
        Query Build<T> ();
        Query Build<T1, T2> ();
        Query Build<T1, T2, T3> ();
        Query Build<T1, T2, T3, T4> ();
        Query Build<T1, T2, T3, T4, T5> ();
    }
    public class QueryBuilder : IQueryBuilder
    {
        private readonly IDbProvider dbProvider;

        public QueryBuilder (IDbProvider dbProvider)
        {
            this.dbProvider = dbProvider;
        }
        public Query Build<T> ()
        {
            return new Query<T> (dbProvider);
        }

        public Query Build<T1, T2> ()
        {
            return new Query<T1, T2> (dbProvider);
        }

        public Query Build<T1, T2, T3> ()
        {
            return new Query<T1, T2, T3> (dbProvider);
        }

        public Query Build<T1, T2, T3, T4> ()
        {
            return new Query<T1, T2, T3, T4> (dbProvider);
        }

        public Query Build<T1, T2, T3, T4, T5> ()
        {
            return new Query<T1, T2, T3, T4, T5> (dbProvider);
        }
    }
}