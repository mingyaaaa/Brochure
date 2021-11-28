using Brochure.ORM.Querys;
using System;
using System.Linq.Expressions;

namespace Brochure.ORM
{
    public partial class Sql
    {
        /// <summary>
        /// Inserts the sql.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>An ISql.</returns>
        public static ISql InsertSql<T>(T obj)
        {
            return new InsertSql(obj);
        }

        /// <summary>
        /// Deletes the.
        /// </summary>
        /// <param name="whereQuery">The where query.</param>
        /// <returns>An ISql.</returns>
        public static ISql DeleteSql<T>(Expression<Func<T, bool>> expression)
        {
            return new DeleteSql(typeof(T), Query.Where(expression));
        }

        /// <summary>
        /// Deletes the.
        /// </summary>
        /// <param name="whereQuery">The where query.</param>
        /// <returns>An ISql.</returns>
        public static ISql DeleteSql<T>(IWhereQuery whereQuery = null)
        {
            return new DeleteSql(typeof(T), whereQuery);
        }

        /// <summary>
        /// Updates the.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="whereQuery">The where query.</param>
        /// <returns>An ISql.</returns>
        public static ISql UpdateSql<T>(object obj, Expression<Func<T, bool>> expression)
        {
            return UpdateSql(typeof(T), obj, Query.Where(expression));
        }

        /// <summary>
        /// Updates the.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="whereQuery">The where query.</param>
        /// <returns>An ISql.</returns>
        public static ISql UpdateSql<T>(object obj, IWhereQuery whereQuery)
        {
            return UpdateSql(typeof(T), obj, whereQuery);
        }

        /// <summary>
        /// Updates the.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="whereQuery">The where query.</param>
        /// <returns>An ISql.</returns>
        public static ISql UpdateSql(Type type, object obj, IWhereQuery whereQuery)
        {
            return new UpdateSql(type, obj, whereQuery);
        }
    }

    /// <summary>
    /// The delete sql.
    /// </summary>
    public class DeleteSql : ISql
    {
        /// <summary>
        /// Gets or sets the table.
        /// </summary>
        public Type Table { get; set; }

        /// <summary>
        /// Gets or sets the where query.
        /// </summary>
        public IWhereQuery WhereQuery { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteSql"/> class.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="whereQuery">The where query.</param>
        public DeleteSql(Type table, IWhereQuery whereQuery)
        {
            Table = table;
            WhereQuery = whereQuery;
        }
    }

    /// <summary>
    /// The insert sql.
    /// </summary>
    public class InsertSql : ISql
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InsertSql"/> class.
        /// </summary>
        /// <param name="table">The table.</param>
        public InsertSql(object table)
        {
            Table = table;
        }

        /// <summary>
        /// Gets or sets the table.
        /// </summary>
        public object Table { get; set; }
    }

    /// <summary>
    /// The update sql.
    /// </summary>
    public class UpdateSql : ISql
    {
        /// <summary>
        /// Gets or sets the table.
        /// </summary>
        public Type Table { get; set; }

        /// <summary>
        /// Gets or sets the update obj.
        /// </summary>
        public object UpdateObj { get; set; }

        /// <summary>
        /// Gets or sets the where query.
        /// </summary>
        public IWhereQuery WhereQuery { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateSql"/> class.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="updateObj">The update obj.</param>
        /// <param name="whereQuery">The where query.</param>
        public UpdateSql(Type table, object updateObj, IWhereQuery whereQuery)
        {
            Table = table;
            UpdateObj = updateObj;
            WhereQuery = whereQuery;
        }
    }
}