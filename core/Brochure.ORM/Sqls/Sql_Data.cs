using Brochure.ORM.Querys;
using System;
using System.Linq.Expressions;

namespace Brochure.ORM
{
    /// <summary>
    /// The sql.
    /// </summary>
    public partial class Sql
    {
        /// <summary>
        /// Inserts the sql.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="database"></param>
        /// <returns>An ISql.</returns>
        public static ISql InsertSql<T>(T obj, string database = "")
        {
            return new InsertSql(obj, database);
        }

        /// <summary>
        /// Deletes the.
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="database"></param>
        /// <returns>An ISql.</returns>
        public static ISql DeleteSql<T>(Expression<Func<T, bool>> expression, string database = "")
        {
            return new DeleteSql(typeof(T), Query.Where(expression), database);
        }

        /// <summary>
        /// Deletes the.
        /// </summary>
        /// <param name="whereQuery">The where query.</param>
        /// <param name="database"></param>
        /// <returns>An ISql.</returns>
        public static ISql DeleteSql<T>(IWhereQuery whereQuery = null, string database = "")
        {
            return new DeleteSql(typeof(T), whereQuery, database);
        }

        /// <summary>
        /// Updates the.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="expression"></param>
        /// <param name="database"></param>
        /// <returns>An ISql.</returns>
        public static ISql UpdateSql<T>(object obj, Expression<Func<T, bool>> expression, string database = "")
        {
            return UpdateSql(typeof(T), obj, Query.Where(expression), database);
        }

        /// <summary>
        /// Updates the.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="whereQuery">The where query.</param>
        /// <param name="database"></param>
        /// <returns>An ISql.</returns>
        public static ISql UpdateSql<T>(object obj, IWhereQuery whereQuery, string database = "")
        {
            return UpdateSql(typeof(T), obj, whereQuery, database);
        }

        /// <summary>
        /// Updates the.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="obj">The obj.</param>
        /// <param name="whereQuery">The where query.</param>
        /// <param name="database"></param>
        /// <returns>An ISql.</returns>
        public static ISql UpdateSql(Type type, object obj, IWhereQuery whereQuery, string database = "")
        {
            return new UpdateSql(type, obj, whereQuery, database);
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
        /// Gets or sets the database.
        /// </summary>
        public string Database { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteSql"/> class.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="whereQuery">The where query.</param>
        /// <param name="database"></param>
        public DeleteSql(Type table, IWhereQuery whereQuery, string database)
        {
            Table = table;
            WhereQuery = whereQuery;
            Database = database;
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
        /// <param name="database"></param>
        public InsertSql(object table, string database)
        {
            Table = table;
            Database = database;
        }

        /// <summary>
        /// Gets or sets the table.
        /// </summary>
        public object Table { get; set; }

        /// <summary>
        /// Gets or sets the database.
        /// </summary>
        public string Database { get; set; }
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
        /// Gets or sets the database.
        /// </summary>
        public string Database { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateSql"/> class.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="updateObj">The update obj.</param>
        /// <param name="whereQuery">The where query.</param>
        /// <param name="database"></param>
        public UpdateSql(Type table, object updateObj, IWhereQuery whereQuery, string database)
        {
            Table = table;
            UpdateObj = updateObj;
            WhereQuery = whereQuery;
            Database = database;
        }
    }
}