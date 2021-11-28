using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;

namespace Brochure.ORM.Querys
{
    /// <summary>
    /// The query.
    /// </summary>
    public class Query : IQuery, IWhereQuery
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Query"/> class.
        /// </summary>
        public Query()
        {
            MainTables = new List<BaseSubQueryType>();
            WhereListExpression = new List<(string, Expression)>();
            JoinExpression = new List<(BaseSubQueryType, Expression)>();
            LeftJoinExpress = new List<(BaseSubQueryType, Expression)>();
            IsDistinct = false;
        }

        /// <summary>
        /// Gets or sets the main tables.
        /// </summary>
        public List<BaseSubQueryType> MainTables { get; set; }

        /// <summary>
        /// Gets or sets the select expression.
        /// </summary>
        public Expression SelectExpression { get; set; }

        /// <summary>
        /// Gets or sets the where expression.
        /// </summary>
        public Expression WhereExpression { get; set; }

        /// <summary>
        /// Gets or sets the where list expression.
        /// </summary>
        public IList<(string, Expression)> WhereListExpression { get; set; }

        /// <summary>
        /// Gets or sets the order expression.
        /// </summary>
        public Expression OrderExpression { get; set; }

        /// <summary>
        /// Gets or sets the order desc expression.
        /// </summary>
        public Expression OrderDescExpression { get; set; }

        /// <summary>
        /// Gets or sets the join expression.
        /// </summary>
        public IList<(BaseSubQueryType, Expression)> JoinExpression { get; set; }

        /// <summary>
        /// Gets or sets the left join express.
        /// </summary>
        public IList<(BaseSubQueryType, Expression)> LeftJoinExpress { get; set; }

        /// <summary>
        /// Gets or sets the group express.
        /// </summary>
        public Expression GroupExpress { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is distinct.
        /// </summary>
        public bool IsDistinct { get; set; }

        /// <summary>
        /// Gets or sets the take count.
        /// </summary>
        public int TakeCount { get; set; }

        /// <summary>
        /// Gets or sets the skip count.
        /// </summary>
        public int SkipCount { get; set; }

        public string Database { get; set; }

        /// <summary>
        /// Wheres the.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>An IQuery.</returns>
        public static IWhereQuery<T> Where<T>(Expression<Func<T, bool>> expression)
        {
            var obj = new Query<T>();
            obj.WhereExpression = expression;
            return obj;
        }

        /// <summary>
        /// Froms the.
        /// </summary>
        /// <returns>An IQuery.</returns>
        public static IQuery<T> From<T>()
        {
            var obj = new Query<T>();
            obj.MainTables.Add(new TableNameSubQueryType(typeof(T)));
            return obj;
        }

        /// <summary>
        /// Froms the.
        /// </summary>
        /// <returns>An IQuery.</returns>
        public static IQuery<T> From<T>(IQuery<T> query)
        {
            var obj = new Query<T>();
            obj.MainTables.Add(new QuerySubQueryType(query));
            if (query.IsWhereSql())
            {
                obj.MainTables.Add(new TableNameSubQueryType(typeof(T)));
            }
            return obj;
        }

        /// <summary>
        /// Froms the.
        /// </summary>
        /// <returns>An IQuery.</returns>
        public static IQuery<T1, T2> From<T1, T2>(IQuery<T1> query)
        {
            var obj = new Query<T1, T2>();
            obj.MainTables.Add(new QuerySubQueryType(query));
            return obj;
        }

        /// <summary>
        /// Froms the.
        /// </summary>
        /// <returns>An IQuery.</returns>
        public static IQuery<T1, T2> From<T1, T2>()
        {
            var obj = new Query<T1, T2>();
            obj.MainTables.Add(new TableNameSubQueryType(typeof(T1)));
            obj.MainTables.Add(new TableNameSubQueryType(typeof(T2)));
            return obj;
        }

        /// <summary>
        /// Froms the.
        /// </summary>
        /// <returns>An IQuery.</returns>
        public static IQuery<T1, T2, T3> From<T1, T2, T3>()
        {
            var obj = new Query<T1, T2, T3>();
            obj.MainTables.Add(new TableNameSubQueryType(typeof(T1)));
            obj.MainTables.Add(new TableNameSubQueryType(typeof(T2)));
            obj.MainTables.Add(new TableNameSubQueryType(typeof(T3)));
            return obj;
        }

        /// <summary>
        /// Froms the.
        /// </summary>
        /// <returns>An IQuery.</returns>
        public static IQuery<T1, T2, T3, T4> From<T1, T2, T3, T4>()
        {
            var obj = new Query<T1, T2, T3, T4>();
            obj.MainTables.Add(new TableNameSubQueryType(typeof(T1)));
            obj.MainTables.Add(new TableNameSubQueryType(typeof(T2)));
            obj.MainTables.Add(new TableNameSubQueryType(typeof(T3)));
            obj.MainTables.Add(new TableNameSubQueryType(typeof(T4)));
            return obj;
        }

        /// <summary>
        /// Froms the.
        /// </summary>
        /// <returns>An IQuery.</returns>
        public static IQuery<T1, T2, T3, T4, T5> From<T1, T2, T3, T4, T5>()
        {
            var obj = new Query<T1, T2, T3, T4, T5>();
            obj.MainTables.Add(new TableNameSubQueryType(typeof(T1)));
            obj.MainTables.Add(new TableNameSubQueryType(typeof(T2)));
            obj.MainTables.Add(new TableNameSubQueryType(typeof(T3)));
            obj.MainTables.Add(new TableNameSubQueryType(typeof(T4)));
            obj.MainTables.Add(new TableNameSubQueryType(typeof(T5)));
            return obj;
        }

        /// <summary>
        /// Copies the property.
        /// </summary>
        /// <param name="des">The des.</param>
        protected void CopyProperty(Query des)
        {
            des.MainTables = this.MainTables;
            des.GroupExpress = this.GroupExpress;
            des.JoinExpression = this.JoinExpression;
            des.LeftJoinExpress = this.LeftJoinExpress;
            des.OrderDescExpression = this.OrderDescExpression;
            des.OrderExpression = this.OrderExpression;
            des.SelectExpression = this.SelectExpression;
            des.WhereExpression = this.WhereExpression;
            des.WhereListExpression = this.WhereListExpression;
        }

        /// <summary>
        /// Are the where sql.
        /// </summary>
        /// <returns>A bool.</returns>
        public bool IsWhereSql()
        {
            return this.SelectExpression == null && MainTables.Count == 0;
        }
    }
}