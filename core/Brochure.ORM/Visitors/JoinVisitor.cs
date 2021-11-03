using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Brochure.ORM.Visitors
{
    /// <summary>
    /// The join visitor.
    /// </summary>
    public class JoinVisitor : ORMVisitor
    {
        private string tableName;

        /// <summary>
        /// Initializes a new instance of the <see cref="JoinVisitor"/> class.
        /// </summary>
        /// <param name="dbPrivoder">The db privoder.</param>
        /// <param name="dbOption">The db option.</param>
        /// <param name="funcVisits">The func visits.</param>
        public JoinVisitor(IDbProvider dbPrivoder, DbOption dbOption, IEnumerable<IFuncVisit> funcVisits) : base(dbPrivoder, dbOption, funcVisits) { }

        /// <summary>
        /// Sets the table name.
        /// </summary>
        /// <param name="tableType">The table type.</param>
        public void SetTableName(Type tableType)
        {
            tableName = TableUtlis.GetTableName(tableType);
        }
        /// <summary>
        /// Visits the binary.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>An Expression.</returns>
        protected override Expression VisitBinary(BinaryExpression node)
        {
            var left = base.GetSql(node.Left);
            var right = base.GetSql(node.Right);
            sql = $"join {_dbPrivoder.FormatFieldName(tableName)} on {left} = {right}";
            return node;
        }
    }
}