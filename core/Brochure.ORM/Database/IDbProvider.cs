using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Brochure.ORM
{
    /// <summary>
    /// The db provider.
    /// </summary>
    public interface IDbProvider
    {
        string GetParamsSymbol();
        /// <summary>
        /// Gets the db data parameter.
        /// </summary>
        /// <returns>An IDbDataParameter.</returns>
        IDbDataParameter GetDbDataParameter();
        /// <summary>
        /// Gets the operate symbol.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="expressionType">The expression type.</param>
        /// <param name="right">The right.</param>
        /// <returns>A string.</returns>
        string GetOperateSymbol(object left, ExpressionType expressionType, object right);
        /// <summary>
        /// Gets the object type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>A string.</returns>
        string GetObjectType(object type);
        /// <summary>
        /// Gets the db connection.
        /// </summary>
        /// <returns>An IDbConnection.</returns>
        IDbConnection GetDbConnection();
        /// <summary>
        /// Gets the type map.
        /// </summary>
        /// <returns>A TypeMap.</returns>
        TypeMap GetTypeMap();

        /// <summary>
        /// Formats the field name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>A string.</returns>
        string FormatFieldName(string name);
    }
}