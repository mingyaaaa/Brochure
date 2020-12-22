using System;
using System.Linq.Expressions;
using Brochure.ORM.Visitors;

namespace Brochure.ORM.Querys
{
    public interface IQueryBuilder
    {
        Query<T1, T2> From<T1, T2> ();
        Query<T1, T2, T3> From<T1, T2, T3> ();
        Query<T1, T2, T3, T4> From<T1, T2, T3, T4> ();
        Query<T1, T2, T3, T4, T5> From<T1, T2, T3, T4, T5> ();
        Query<T> From<T> ();
    }
    public class QueryBuilder : IQueryBuilder
    {
        private readonly IDbProvider dbProvider;
        private readonly DbOption dbOption;
        private readonly IVisitProvider visitProvider;

        public QueryBuilder (IDbProvider dbProvider, DbOption dbOption, IVisitProvider visitProvider)
        {
            this.dbProvider = dbProvider;
            this.dbOption = dbOption;
            this.visitProvider = visitProvider;
        }

        public Query<T1, T2> From<T1, T2> ()
        {
            return new Query<T1, T2> (dbProvider, dbOption, visitProvider);
        }

        public Query<T1, T2, T3> From<T1, T2, T3> ()
        {
            return new Query<T1, T2, T3> (dbProvider, dbOption, visitProvider);
        }

        public Query<T1, T2, T3, T4> From<T1, T2, T3, T4> ()
        {
            return new Query<T1, T2, T3, T4> (dbProvider, dbOption, visitProvider);
        }

        public Query<T1, T2, T3, T4, T5> From<T1, T2, T3, T4, T5> ()
        {
            return new Query<T1, T2, T3, T4, T5> (dbProvider, dbOption, visitProvider);
        }

        public Query<T> From<T> ()
        {
            return new Query<T> (dbProvider, dbOption, visitProvider);
        }
    }
}