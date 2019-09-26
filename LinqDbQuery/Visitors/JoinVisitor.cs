using System;
using System.Linq.Expressions;

namespace LinqDbQuery.Visitors
{
    public class JoinVisitor : ORMVisitor
    {
        private string tableName;
        public JoinVisitor (Type tableType, IDbProvider dbPrivoder) : base (dbPrivoder)
        {
            tableName = TableUtlis.GetTableName (tableType);
        }
        protected override Expression VisitBinary (BinaryExpression node)
        {
            var left = GetSql (node.Left);
            var exType = node.NodeType;
            var right = GetSql (node.Right);
            sql = $"join [{tableName}] on {left} = {right}";
            return node;
        }
    }
}