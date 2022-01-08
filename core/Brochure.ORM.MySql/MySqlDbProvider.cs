using System;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using Brochure.ORM;
using Brochure.ORM.Visitors;
using MySql.Data.MySqlClient;

namespace Brochure.ORM.MySql
{
    /// <summary>
    /// The my sql db provider.
    /// </summary>
    public class MySqlDbProvider : IDbProvider
    {
        private readonly DbOption dbOption;

        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlDbProvider"/> class.
        /// </summary>
        /// <param name="dbOption">The db option.</param>
        public MySqlDbProvider(DbOption dbOption)
        {
            this.dbOption = dbOption;
        }

        /// <summary>
        /// Formats the field name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>A string.</returns>
        public string FormatFieldName(string name)
        {
            return $"`{name}`";
        }

        /// <summary>
        /// Gets the db connection.
        /// </summary>
        /// <returns>An DbConnection.</returns>
        public DbConnection GetDbConnection()
        {
            return new MySqlConnection();
        }

        /// <summary>
        /// Gets the db data parameter.
        /// </summary>
        /// <returns>An IDbDataParameter.</returns>
        public IDbDataParameter GetDbDataParameter()
        {
            return new MySqlParameter();
        }

        /// <summary>
        /// Gets the object type.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>A string.</returns>
        public string GetObjectType(object obj)
        {
            if (obj == null)
                return null;
            string str = string.Empty;
            if (dbOption.IsUseParamers)
            {
                str = obj.ToString();
            }
            else
            {
                if (obj is string)
                    str = $"'{obj}'";
                else if (obj is int || obj is double || obj is float || obj is decimal)
                    str = obj.ToString();
            }
            return str;
        }

        /// <summary>
        /// Gets the operate symbol.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="expressionType">The expression type.</param>
        /// <param name="right">The right.</param>
        /// <returns>A string.</returns>
        public string GetOperateSymbol(object left, ExpressionType expressionType, object right)
        {
            var str = string.Empty;
            string leftStr = left.ToString();
            var rightObject = GetObjectType(right);
            if (expressionType == ExpressionType.Equal)
            {
                if (right == null)
                    return $"{leftStr} is null";
                else
                    return $"{leftStr} = {rightObject}";
            }
            else if (expressionType == ExpressionType.NotEqual)
            {
                if (right == null)
                    return $"{leftStr} is not null";
                else
                    return $"{leftStr} != {rightObject}";
            }
            else if (expressionType == ExpressionType.AndAlso)
            {
                return $"{left} and {right}";
            }
            else if (expressionType == ExpressionType.OrElse)
            {
                return $"{left} or {right}";
            }
            return str;
        }

        /// <summary>
        /// Gets the params symbol.
        /// </summary>
        /// <returns>A string.</returns>
        public string GetParamsSymbol()
        {
            return "@";
        }

        /// <summary>
        /// Gets the type map.
        /// </summary>
        /// <returns>A TypeMap.</returns>
        public TypeMap GetTypeMap()
        {
            return new MySqlTypeMap();
        }
    }
}