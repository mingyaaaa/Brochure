using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Brochure.ORM.Querys
{
    public class Query : IQuery
    {
        public Query()
        {
            MainTables = new List<Type>();
            WhereListExpression = new List<(string, Expression)>();
            JoinExpression = new List<(Type, Expression)>();
            LeftJoinExpress = new List<(Type, Expression)>();
        }

        public List<Type> MainTables { get; set; }

        public Expression SelectExpression { get; set; }

        public Expression WhereExpression { get; set; }

        public IList<(string, Expression)> WhereListExpression { get; set; }

        public Expression OrderExpression { get; set; }

        public Expression OrderDescExpression { get; set; }

        public IList<(Type, Expression)> JoinExpression { get; set; }

        public IList<(Type, Expression)> LeftJoinExpress { get; set; }

        public Expression GroupExpress { get; set; }


        public static IQuery<T> Where<T>(Expression<Func<T, bool>> expression)
        {
            var obj = new Query<T>();
            obj.WhereExpression = expression;
            return obj;
        }

        public static IQuery<T> From<T>()
        {
            var obj = new Query<T>();
            obj.MainTables.Add(typeof(T));
            return obj;
        }

        public static IQuery<T1, T2> From<T1, T2>()
        {
            var obj = new Query<T1, T2>();
            obj.MainTables.Add(typeof(T1));
            obj.MainTables.Add(typeof(T2));
            return obj;
        }

        public static IQuery<T1, T2, T3> From<T1, T2, T3>()
        {
            var obj = new Query<T1, T2, T3>();
            obj.MainTables.Add(typeof(T1));
            obj.MainTables.Add(typeof(T2));
            obj.MainTables.Add(typeof(T3));
            return obj;
        }

        public static IQuery<T1, T2, T3, T4> From<T1, T2, T3, T4>()
        {
            var obj = new Query<T1, T2, T3, T4>();
            obj.MainTables.Add(typeof(T1));
            obj.MainTables.Add(typeof(T2));
            obj.MainTables.Add(typeof(T3));
            obj.MainTables.Add(typeof(T4));
            return obj;
        }

        public static IQuery<T1, T2, T3, T4, T5> From<T1, T2, T3, T4, T5>()
        {
            var obj = new Query<T1, T2, T3, T4, T5>();
            obj.MainTables.Add(typeof(T1));
            obj.MainTables.Add(typeof(T2));
            obj.MainTables.Add(typeof(T3));
            obj.MainTables.Add(typeof(T4));
            obj.MainTables.Add(typeof(T5));
            return obj;
        }

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
    }
}