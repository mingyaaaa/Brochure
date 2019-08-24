using System;
using System.Linq.Expressions;

namespace LinqDbQuery.Visitors
{
    public class JoinVisitor : ORMVisitor
    {
        private string tableName;
        private IDbProvider _dbPrivoder;
        public JoinVisitor(Type tableType, IDbProvider dbPrivoder)
        {
            _dbPrivoder = dbPrivoder;
            tableName = ReflectedUtli.GetTableName(tableType);
        }
        protected override Expression VisitBinary(BinaryExpression node)
        {
            var left = GetSql(node.Left);
            var exType = node.NodeType;
            var right = GetSql(node.Right);
            sql = _dbPrivoder.GetOperateSymbol(left, exType, right);
            sql = $"join {tableName} on {sql}";

            return node;
        }
    }
}
